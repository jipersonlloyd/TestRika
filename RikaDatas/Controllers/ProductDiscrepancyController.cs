using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using RikaDatas.Models;

namespace RikaDatas.Controllers
{
    public class ProductDiscrepancyController : Controller
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public async Task<IActionResult> Index()
        {

            var workbook = new XLWorkbook();
            var disworkbook = new XLWorkbook();

            //             List<string> products = [
            // "1201987",
            // "1012216",
            // "1006724",
            // "1003654",
            // "12023162",
            // "1012715",
            // "1012371",
            //             ];    

            var path = "C:\\Users\\Owner\\Desktop\\C# TestRika\\RikaDatas\\Controllers\\jrborjaproducts.json";
            var jsonString = System.IO.File.ReadAllText(path);
            var products = JsonSerializer.Deserialize<List<Product>>(jsonString);

            HttpClient httpClient = new HttpClient();

            httpClient.Timeout = TimeSpan.FromMinutes(100);

            var throttler = new SemaphoreSlim(10); // maxConcurrency
var tasks = new List<Task<ProductDiscrepancyModel>>();

foreach (var product in products)
{
    tasks.Add(Task.Run(async ()  =>
    {
        await throttler.WaitAsync();
        try
        {
            string productId = product.fproductid;
            string apiUrl = $"http://rika-replica.alliancewebpos.net/appserv/app/batch/CTest.php?fproductid={product.fproductid}";
            
            var response = await _httpClient.GetAsync(apiUrl);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"[{productId}] Response: {jsonResponse}");

            if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(jsonResponse))
            {
                Console.WriteLine($"[{productId}] Invalid or empty response.");
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<ProductDiscrepancyModel>(jsonResponse);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[{productId}] JSON error: {ex.Message}");
                return null;
            }
        }
        finally
        {
            throttler.Release();
        }
    }));
}

var results = await Task.WhenAll(tasks);

                var productlist = results.Where(r => r != null).ToList();
                var discrepancies = productlist.Where(p => p.HasDiscrepancy == "yes").ToList();

            for (int i = 0; i < productlist.Count; i++)
            {
                Console.WriteLine($"Writing Products in Excel {i + 1} out of {productlist.Count}");
                createExcelFile(workbook, i + 2, productlist[i].Productid, productlist[i].HasDiscrepancy, productlist.Count, "JrBorjaProducts");
                if (i + 1 == productlist.Count)
                {
                    workbook.SaveAs($"C:\\Users\\Owner\\Desktop\\RikaScript\\JrBorjaProducts.xlsx");
                }
            }

            for (int i = 0; i < discrepancies.Count; i++)
            {
                Console.WriteLine($"Writing Discrepancy in Excel {i + 1} out of {discrepancies.Count}");
                createAnotherExcelFile(disworkbook, i + 2, discrepancies[i].Productid, discrepancies[i].HasDiscrepancy, discrepancies.Count, "JrBorjaDiscrepancies");
                if (i + 1 == discrepancies.Count)
                {
                    disworkbook.SaveAs($"C:\\Users\\Owner\\Desktop\\RikaScript\\JrBorjaDiscrepancies.xlsx");
                }
            }

            return View(productlist);
        }

        public void createExcelFile(XLWorkbook workbook, int row, string productID, string HasDiscrepancy, int total, string title)
        {

            // var worksheet = workbook.Worksheets.Add(title);
            var worksheet = workbook.Worksheets.Contains(title)
                ? workbook.Worksheet(title)
                : workbook.AddWorksheet(title);


            // Set headers
            worksheet.Cell(1, 1).Value = "Product ID";
            worksheet.Cell(1, 3).Value = "HasDiscrepancy";

            worksheet.Cell(1, 5).Value = "Total";
            worksheet.Cell(2, 5).Value = total;

            // Make header row bold using method style (parentheses)
            worksheet.Range(1, 1, 1, 5).Style.Font.SetBold();

            // Optional: Autofit the columns
            worksheet.Columns().AdjustToContents();

            worksheet.Cell(row, 1).Value = productID;
            worksheet.Cell(row, 3).Value = HasDiscrepancy;
        }

        public void createAnotherExcelFile(XLWorkbook workbook, int row, string productID, string HasDiscrepancy, int total, string title)
        {

            // var worksheet = workbook.Worksheets.Add(title);
            var worksheet = workbook.Worksheets.Contains(title)
                ? workbook.Worksheet(title)
                : workbook.AddWorksheet(title);


            // Set headers
            worksheet.Cell(1, 1).Value = "Product ID";
            worksheet.Cell(1, 3).Value = "HasDiscrepancy";

            worksheet.Cell(1, 5).Value = "Total";
            worksheet.Cell(2, 5).Value = total;

            // Make header row bold using method style (parentheses)
            worksheet.Range(1, 1, 1, 5).Style.Font.SetBold();

            // Optional: Autofit the columns
            worksheet.Columns().AdjustToContents();

            worksheet.Cell(row, 1).Value = productID;
            worksheet.Cell(row, 3).Value = HasDiscrepancy;
        }
    }

    public class Product
    {
        public string fproductid { get; set; }

    }
}

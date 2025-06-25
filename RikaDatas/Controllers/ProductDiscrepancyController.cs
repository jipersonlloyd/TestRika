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

            var path = "C:\\Users\\Owner\\Desktop\\C# TestRika\\RikaDatas\\Controllers\\agoraproducts.json";
            var jsonString = System.IO.File.ReadAllText(path);
            var products = JsonSerializer.Deserialize<List<Product>>(jsonString);

            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(100);

            int maxConcurrency = 10;
            var throttler = new SemaphoreSlim(maxConcurrency);

            var tasks = products.Select(async product =>
                {
                    await throttler.WaitAsync();
                    try
                    {
                        string apiUrl = $"http://rika-replica.alliancewebpos.net/appserv/app/batch/CTest.php?fproductid={product.fproductid}";
                        var response = await _httpClient.GetAsync(apiUrl);
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(jsonResponse);
                        var data = JsonSerializer.Deserialize<ProductDiscrepancyModel>(jsonResponse);

                        return data;
                    }
                    finally
                    {
                        throttler.Release();
                    }
                });

                var results = await Task.WhenAll(tasks);

                var productlist = results.Where(r => r != null).ToList();
                var discrepancies = productlist.Where(p => p.HasDiscrepancy == "yes").ToList();

            // for (int i = 0; i < products.Count; i++)
            // {

            //     Console.WriteLine($"Checking current index {i + 1} out of {products.Count}");
            //     string apiUrl = $"http://rika-replica.alliancewebpos.net/appserv/app/batch/CTest.php?fproductid={products[i].fproductid}";
            //     // string apiUrl = $"http://jacob:8888/appserv/app/batch/CTest.php?fproductid={products[i]}";
            //     using (var httpClient = new HttpClient())
            //     {
            //         httpClient.Timeout = TimeSpan.FromMinutes(100);

            //         var response = await httpClient.GetAsync(apiUrl);
            //         var jsonResponse = await response.Content.ReadAsStringAsync();
            //         Console.WriteLine(jsonResponse);
            //         var data = JsonSerializer.Deserialize<ProductDiscrepancyModel>(jsonResponse);

            //         if (response.IsSuccessStatusCode)
            //         {
            //             ProductDiscrepancyModel prod = new ProductDiscrepancyModel
            //             {
            //                 Productid = data?.Productid,
            //                 Inv_Ledger = data?.Inv_Ledger,
            //                 Inv_Daily_Sum = data?.Inv_Daily_Sum,
            //                 HasDiscrepancy = data?.HasDiscrepancy
            //             };
            //             productlist.Add(prod);

            //             if (data?.HasDiscrepancy == "yes")
            //             {
            //                 discrepancies.Add(prod);
            //             }
            //         }
            //     }


            // }

            for (int i = 0; i < productlist.Count; i++)
            {
                Console.WriteLine($"Writing Products in Excel {i + 1} out of {productlist.Count}");
                createExcelFile(workbook, i + 2, productlist[i].Productid, productlist[i].HasDiscrepancy, productlist.Count, "AgoraProducts");
                if (i + 1 == productlist.Count)
                {
                    workbook.SaveAs($"C:\\Users\\Owner\\Desktop\\RikaScript\\AgoraProducts.xlsx");
                }
            }

            for (int i = 0; i < discrepancies.Count; i++)
            {
                Console.WriteLine($"Writing Discrepancy in Excel {i + 1} out of {discrepancies.Count}");
                createAnotherExcelFile(disworkbook, i + 2, discrepancies[i].Productid, discrepancies[i].HasDiscrepancy, discrepancies.Count, "AgoraDiscrepancies");
                if (i + 1 == discrepancies.Count)
                {
                    disworkbook.SaveAs($"C:\\Users\\Owner\\Desktop\\RikaScript\\AgoraDiscrepancies.xlsx");
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

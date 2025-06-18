using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using RikaDatas.Models;

namespace RikaDatas.Controllers
{
    public class ProductDiscrepancyController : Controller
    {
        public async Task<IActionResult> Index()
        {

            List<ProductDiscrepancyModel> productlist = new List<ProductDiscrepancyModel>();
            List<ProductDiscrepancyModel> discrepancies = new List<ProductDiscrepancyModel>();

            var workbook = new XLWorkbook();
            var disworkbook = new XLWorkbook();
            string[] products = ["1003356"];

            // string[] products = [
            // "10006744",
            // "10001046",
            // "ALU00003467",
            // "10005845",
            // "1003033",
            // "10005163",
            // "10008637",
            // "ALU00003979",
            // "ALU00003981",
            // "ALU00003984",
            // "ALU00003725",
            // "10001133",
            // "10001131",
            // "10001153",
            // "10001150",
            // "100011541",
            // "100011543",
            // "67481269",
            // "120187634",
            // "1202218",
            // "64314336",
            // "10008124",
            // "10007949",
            // "10007938",
            // "10006386",
            // "ALU00003768",
            // "10008128",
            // "10004599",
            // "10004600",
            // "10004608",
            // "ALU00003913",
            // "10007922",
            // "10007087",
            // "10004654",
            // "10006121",
            // "SURBAR",
            // "10007754",
            // "ALU00003912",
            // "10004691",
            // "ALU00003911",
            // "SURFPWDR",
            // "10004711",
            // "10004715",
            // "100201861",
            // "100011542",
            // "100011544"
            // ];

            // var path = "C:\\Users\\Owner\\Desktop\\C# TestRika\\RikaDatas\\Controllers\\jrborjaproducts.json";
            // var jsonString = System.IO.File.ReadAllText(path);
            // var products = JsonSerializer.Deserialize<List<Product>>(jsonString);

            for (int i = 0; i < products.Length; i++)
            {

                // Console.WriteLine($"Checking current index {i + 1} out of {products.Count}");
                string apiUrl = $"http://rika-replica.alliancewebpos.net/appserv/app/batch/CTest.php?fproductid={products[i]}";
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMinutes(100);

                    var response = await httpClient.GetAsync(apiUrl);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(jsonResponse);
                    var data = JsonSerializer.Deserialize<ProductDiscrepancyModel>(jsonResponse);

                    if (response.IsSuccessStatusCode)
                    {
                        ProductDiscrepancyModel prod = new ProductDiscrepancyModel
                        {
                            Productid = data?.Productid,
                            Inv_Ledger = data?.Inv_Ledger,
                            Inv_Daily_Sum = data?.Inv_Daily_Sum,
                            HasDiscrepancy = data?.HasDiscrepancy
                        };
                        productlist.Add(prod);

                        // createExcelFile(workbook, i + 2, prod.Productid, prod.HasDiscrepancy, products.Count, "JrBorjaProducts");

                        // if (data?.HasDiscrepancy == "yes")
                        // {
                        //     discrepancies.Add(prod);
                        // }
                    }
                }

                
            }
            // workbook.SaveAs($"C:\\Users\\Owner\\Desktop\\RikaScript\\JrBorjaProducts.xlsx");

            // for (int i = 0; i < discrepancies.Count; i++)
            // {
            //     Console.WriteLine($"Writing Discrepancy in Excel {i + 1} out of {discrepancies.Count}");
            //     createAnotherExcelFile(disworkbook, i + 2, discrepancies[i].Productid, discrepancies[i].HasDiscrepancy, discrepancies.Count, "JrBorjaDiscrepancies");
            // }

            // disworkbook.SaveAs($"C:\\Users\\Owner\\Desktop\\RikaScript\\JrBorjaDiscrepancies.xlsx");

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

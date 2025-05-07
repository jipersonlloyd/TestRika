using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RikaDatas.Models;

namespace RikaDatas.Controllers
{
    public class ProductDiscrepancyController : Controller
    {
        public async Task<IActionResult> Index()
        {
            //Error need to fix by queries
            // "2001281008",

            
            List<ProductDiscrepancyModel> productlist = new List<ProductDiscrepancyModel>();
            string[] products = [
                "1006674",
                "201301-11",
                "1007224",
                "1009176",
                "1008520",
                "20130423",
                "201212-44",
                
                "1009020",
                "201305-21",
                "1009126",
                "1009151",
                "1009153",
                "1009161",
                "1009530",
                "1010684",
                "1010456",
                "10007450",
                "1011090",
                "1011415",
                "1011583",
                "1012505",
                "12023135",
                "10007192",
                "10006312",
                "10000615",
                "20129-5656",
                "1002414",
                "1008060",
                "20128-1020",
                "20128-1021",
                "20128-1022",
            ];

            for(int i = 0; i < products.Length; i++) {

                Console.WriteLine($"Fetching Data Product #{products[i]}");
                string apiUrl = $"http://strika5.alliancewebpos.net/appserv/app/batch/CTest.php?fproductid={products[i]}";
                using (var httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromMinutes(10);

                        var response = await httpClient.GetAsync(apiUrl);
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(jsonResponse);
                        var data = JsonSerializer.Deserialize<ProductDiscrepancyModel>(jsonResponse);

                        if (response.IsSuccessStatusCode)
                        {
                            ProductDiscrepancyModel prod = new ProductDiscrepancyModel{
                                Productid = data?.Productid,
                                Inv_Ledger = data?.Inv_Ledger,
                                Inv_Daily_Sum = data?.Inv_Daily_Sum,
                                IsMatch = data?.IsMatch
                            };
                            productlist.Add(prod);
                        }
                    }
            }
            return View(productlist);
        }
    }
}

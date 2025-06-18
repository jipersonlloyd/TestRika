using Microsoft.AspNetCore.Mvc;
using RikaDatas.Models;
using Newtonsoft.Json;

namespace RikaDatas.Controllers
{
    public class InvDailySummaryController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<InvDailySummary> list = new List<InvDailySummary>();

            string[] products = ["10000417", "10000480", "10000507", "10000914"];

            for (int i = 0; i < products.Length; i++)
            {
                string apiUrl = $"http://jacob:8888/appserv/app/batch/Cids.php?fproductid={products[i]}";
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMinutes(100);

                    var response = await httpClient.GetAsync(apiUrl);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<InvDailySummary>>(jsonResponse);
                    if (response.IsSuccessStatusCode && data != null)
                    {
                        for (int j = 0; j < data.Count; j++) 
                        {
                            decimal newfqty = data[j].fqty + data[j].fadjusted_qty;
                            Console.WriteLine(newfqty);
                        }
                        return View(data);
                    }
                }
            }

            return View(list);
        }
    }
}

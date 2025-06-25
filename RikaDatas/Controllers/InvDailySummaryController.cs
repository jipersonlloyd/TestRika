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

            string filePath = "C:\\Users\\Owner\\Desktop\\RikaScript\\QueryJrBorja.txt";
            string query = "";

            List<string> products = [
         "1201987",
"1012216",
"1006724",
"1003654",
"12023162",
"1012715",
"1012371",
            ];     

            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine($"Checking current index {i + 1} out of {products.Count}");
                string apiUrl = $"http://rika-replica.alliancewebpos.net/appserv/app/batch/Cids.php?fproductid={products[i]}";
                // string apiUrl = $"http://jacob:8888/appserv/app/batch/Cids.php?fproductid={products[i]}";
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
                            list.Add(data[j]);
                            string newflotno = data[j].flotno ?? "";
                            decimal newfqty = data[j].fqty + data[j].fadjusted_qty;
                            query += $"UPDATE inv_daily_summary SET fqty = {newfqty} WHERE fcompanyid = 'RIKA-12020182' and fsiteid = 'JRBORJA2019' and fproductid = '{data[j].fproductid}' and ftrxdate = '{data[j].ftrxdate}' and flotno = '{newflotno}';\n\n";

                        }


                    }
                }
            }
            System.IO.File.WriteAllText(filePath, query);
            return View(list);
        }
    }
}

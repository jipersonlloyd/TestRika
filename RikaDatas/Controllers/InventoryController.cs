using Microsoft.AspNetCore.Mvc;

namespace RikaDatas.Controllers
{
    public class InventoryController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string text)
        {

            string apiUrl = text;
            Console.WriteLine($"Executing: {apiUrl}");
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && jsonResponse.Contains("Proceeded"))
                {
                    Console.WriteLine("Execution Finished...");
                    ViewBag.Message = "Execution Finished";
                }
                else
                {
                    Console.WriteLine("Error...");
                    ViewBag.Message = "Error";
                }
            }
            
            return View();
        }

    }
}

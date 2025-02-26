using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RikaDatas.Models;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RikaDatas.Controllers
{
    public class RikaSalesController : Controller
    {

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var pagedData = PagedList.CreateLedgerPagination(page, pageSize);
            return View(pagedData);
        }


        public async Task<IActionResult> ApplyFix()
        {
            //int count = 0;
            string filePath = "C:\\Users\\Owner\\Desktop\\RikaScript\\Script.txt";
            string jsonContent = System.IO.File.ReadAllText(filePath);
            List<string> links = jsonContent.Split(",").ToList();
            Console.WriteLine($"Total Links: {links.Count}");

            for (int i = 0; i < links.Count; i++)
                {
                    if(links[i] == "" || links[i] == null) {
                    break;
                    }
                
                    string apiUrl = links[i];
                    Console.WriteLine($"Executing: {links[i]}");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetAsync(apiUrl);
                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode && jsonResponse.Contains("Proceeded"))
                        {
                            Console.WriteLine("Execution Finished...");
                            continue;
                        }
                        else
                        {
                            string path = "C:\\Users\\Owner\\Desktop\\RikaScript\\ErrorLinks.txt";
                            string path1 = "C:\\Users\\Owner\\Desktop\\RikaScript\\ErrorLinks1.txt";
                            using (var writer = new StreamWriter(path, append: true))
                            {
                                string content = $"{apiUrl},";
                                writer.WriteLine(content);
                                writer.Flush(); // Ensure data is immediately written to the file
                            }

                            using (var writer = new StreamWriter(path1, append: true))
                            {
                                string content = $"{apiUrl},\n";
                                writer.WriteLine(content);
                                writer.Flush(); // Ensure data is immediately written to the file
                            }
                            continue;
                        }
                    }
                }
            
            return View();
        }
    }
}

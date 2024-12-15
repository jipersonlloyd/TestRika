using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RikaDatas.Models;
using System.Globalization;

namespace RikaDatas.Controllers
{
    public class RikaSalesController : Controller
    {
        private static List<RikaSales> sales = new List<RikaSales>();
        private static List<RikaInventory> inventories = new List<RikaInventory>();
        public IActionResult Index()
        {
            List<RikaSales> newSales = new List<RikaSales>();
            sales = Sales();
            inventories = Inventories();
            
            for (int i = 0; i < inventories.Count; i++) 
            {
                for (int j = 0; j < sales.Count; j++) 
                {
                    if (newSales.Contains(sales[j])) 
                    {
                        continue;
                    }

                    if (sales[j].fproductid == inventories[i].fproductid && sales[j].fsale_date == inventories[i].ftrxdate && (sales[j].total_qty != inventories[i].fsold_qty || sales[j].total_qty < inventories[i].fsold_qty))
                    {
                        newSales.Add(sales[j]);
                    }
                    else 
                    {
                        continue;
                    }
                }
            }

            return View(newSales);
        }

        public List<RikaInventory> Inventories()
        {
            List<RikaInventory> inventories = new List<RikaInventory>();
            string filePath = "C:\\Users\\Lloyd Jiperson Diaz\\Downloads\\rikaproductinv09to1018.json";
            string jsonContent = System.IO.File.ReadAllText(filePath);
            JArray array = JArray.Parse(jsonContent);

            for (int i = 0; i < array.Count; i++)
            {
                JObject obj = JObject.FromObject(array[i]);
                RikaInventory inv = JsonConvert.DeserializeObject<RikaInventory>(obj.ToString());
                inventories.Add(inv);
            }

            inventories.Sort((record1, record2) =>
            {
                // Convert the date strings to DateTime for comparison
                DateTime date1 = DateTime.ParseExact(record1.ftrxdate, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime date2 = DateTime.ParseExact(record2.ftrxdate, "yyyyMMdd", CultureInfo.InvariantCulture);

                return date1.CompareTo(date2); // Compare the two DateTime objects
            });

            return inventories;
        }

        public List<RikaSales> Sales()
        {
            List<RikaSales> sales = new List<RikaSales>();
            string filePath = "C:\\Users\\Lloyd Jiperson Diaz\\Downloads\\rikaproduct09to1018.json";
            string jsonContent = System.IO.File.ReadAllText(filePath);
            JArray array = JArray.Parse(jsonContent);

            for (int i = 0; i < array.Count; i++)
            {
                JObject obj = JObject.FromObject(array[i]);
                RikaSales sale = JsonConvert.DeserializeObject<RikaSales>(obj.ToString());
                sales.Add(sale);
            }

            sales.Sort((record1, record2) =>
            {
                // Convert the date strings to DateTime for comparison
                DateTime date1 = DateTime.ParseExact(record1.fsale_date, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime date2 = DateTime.ParseExact(record2.fsale_date, "yyyyMMdd", CultureInfo.InvariantCulture);

                return date1.CompareTo(date2); // Compare the two DateTime objects
            });

            return sales;
        }
    }
}

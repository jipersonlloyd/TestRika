using Newtonsoft.Json;
using System.Globalization;

namespace RikaDatas.Models
{
    public class PagedList
{
    public List<RikaSales> Items { get; set; }
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }

    public PagedList(List<RikaSales> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = items;
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static PagedList Create(int pageIndex, int pageSize)
    {
        List<RikaSales> source = NewSales();
        var count = source.Count;
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PagedList(items, count, pageIndex, pageSize);
    }

        public static List<RikaInventory> Inventories()
        {
            string filePath = "C:\\Users\\Owner\\Documents\\ADMIN-RIDS\\rikainvtagoloan01to10.json";
            string jsonContent = System.IO.File.ReadAllText(filePath);
            List<RikaInventory> inventories = JsonConvert.DeserializeObject<List<RikaInventory>>(jsonContent);


            inventories.Sort((record1, record2) =>
            {
                // Convert the date strings to DateTime for comparison
                DateTime date1 = DateTime.ParseExact(record1.ftrxdate, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime date2 = DateTime.ParseExact(record2.ftrxdate, "yyyyMMdd", CultureInfo.InvariantCulture);

                return date1.CompareTo(date2); // Compare the two DateTime objects
            });

            return inventories;
        }

        public static List<RikaSales> Sales()
        {
            string filePath = "C:\\Users\\Owner\\Documents\\ADMIN-RIDS\\rikasalestagoloan01to10.json";
            string jsonContent = System.IO.File.ReadAllText(filePath);
            List<RikaSales> sales = JsonConvert.DeserializeObject<List<RikaSales>>(jsonContent);

            sales.Sort((record1, record2) =>
            {
                // Convert the date strings to DateTime for comparison
                DateTime date1 = DateTime.ParseExact(record1.fsale_date, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime date2 = DateTime.ParseExact(record2.fsale_date, "yyyyMMdd", CultureInfo.InvariantCulture);

                return date1.CompareTo(date2); // Compare the two DateTime objects
            });

            return sales;
        }

        public static List<RikaSales> NewSales()
        {
            List<RikaSales> newSales = new List<RikaSales>();
            List<RikaSales> sales = Sales();
            List<RikaInventory> inventories = Inventories();
            Console.WriteLine($"Sales: {sales.Count}");
            Console.WriteLine($"Inventories: {inventories.Count}");


            for (int i = 0; i < inventories.Count; i++)
            {
                for (int j = 0; j < sales.Count; j++)
                {
                    if (newSales.Contains(sales[j]))
                    {
                        continue;
                    }

                    if (sales[j].fproductid == inventories[i].fproductid && sales[j].fsale_date == inventories[i].ftrxdate && sales[j].total_qty != inventories[i].fsold_qty)
                    {
                        newSales.Add(sales[j]);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            Console.WriteLine($"NewSales: {newSales.Count}");
            List<RikaSales> distincted = newSales.DistinctBy(x => x.fproductid).ToList();
            string filePath = "C:\\Users\\Owner\\Desktop\\RikaScript\\Script.txt";
            string filePath1 = "C:\\Users\\Owner\\Desktop\\RikaScript\\Script1.txt"; // with new line
            string content = "";
            string content1 = "";
            for(int i = 0; i < distincted.Count; i++) 
            {
                content += $"http://strika5.alliancewebpos.net/appserv/app/batch/fix/rebuild_inv_daily_summary.php?fcompanyid=STRIK5-12020182&fsale_date={distincted[i].fsale_date}&fend_date={distincted[i].fsale_date}&fpassword=5678efgh&fsiteid={distincted[i].fsiteid}&fproductid={distincted[i].fproductid},";
                content1 += $"http://strika5.alliancewebpos.net/appserv/app/batch/fix/rebuild_inv_daily_summary.php?fcompanyid=STRIK5-12020182&fsale_date={distincted[i].fsale_date}&fend_date={distincted[i].fsale_date}&fpassword=5678efgh&fsiteid={distincted[i].fsiteid}&fproductid={distincted[i].fproductid},\n";
            }

            System.IO.File.WriteAllText(filePath, content);
            System.IO.File.WriteAllText(filePath1, content1);

            return distincted;
        }
    }
}

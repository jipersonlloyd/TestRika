using Newtonsoft.Json;
using System.Globalization;

namespace RikaDatas.Models
{
    public class PagedList
{
    public List<InvLedgerDistribution> Items { get; set; }
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }

    public PagedList(List<InvLedgerDistribution> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = items;
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    /* public static PagedList Create(int pageIndex, int pageSize)
    {
        List<RikaSales> source = NewSales();
        var count = source.Count;
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PagedList(items, count, pageIndex, pageSize);
    } */

        public static PagedList CreateLedgerPagination(int pageIndex, int pageSize)
        {
            List<InvLedgerDistribution> ledgerDistributions = NewLedger();
            var count = ledgerDistributions.Count;
            var items = ledgerDistributions.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList(items, count, pageIndex, pageSize);
        }

        public static List<InvLedgerDistribution> InvLedgerDistributions()
        {
            int count = 0;
            string filePath = "C:\\Users\\Owner\\Documents\\ADMIN-RIDS\\invledgerdistribution01to1020.json";
            string jsonContent = System.IO.File.ReadAllText(filePath);
            List<InvLedgerDistribution> ledgerDistributions = JsonConvert.DeserializeObject<List<InvLedgerDistribution>>(jsonContent);
            List<InvLedgerDistribution> newLedger = new List<InvLedgerDistribution>();
            var duplicates = ledgerDistributions.GroupBy(x => new { x.fproductid, x.fcompanyid, x.fsiteid, x.finv_date, x.fdst_ledgerid }).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
            foreach (var item in duplicates)
            {
                InvLedgerDistribution invLedgerDistribution = new InvLedgerDistribution();
                invLedgerDistribution.fdistid = count++;
                invLedgerDistribution.fdst_ledgerid = item.fdst_ledgerid;
                invLedgerDistribution.finv_date = item.finv_date;
                invLedgerDistribution.fqty = 0;
                invLedgerDistribution.fcompanyid = item.fcompanyid;
                invLedgerDistribution.fproductid = item.fproductid;
                invLedgerDistribution.fsiteid = item.fsiteid;
                newLedger.Add(invLedgerDistribution);
            }

            return newLedger;
        }


        public static List<RikaInventory> Inventories()
        {
            string filePath = "C:\\Users\\Owner\\Documents\\ADMIN-RIDS\\rikainvosmena01to10.json";
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

        /* public static List<RikaSales> Sales()
        {
            string filePath = "C:\\Users\\Owner\\Documents\\ADMIN-RIDS\\rikasalesosmena01to10.json";
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
        } */

        public static List<InvLedgerDistribution> NewLedger()
        {
            List<InvLedgerDistribution> newledgerDistributions = new List<InvLedgerDistribution>();
            List<InvLedgerDistribution> ledgerDistributions = InvLedgerDistributions();
            List<RikaInventory> inventories = Inventories();
            Console.WriteLine($"Sales: {ledgerDistributions.Count}");
            Console.WriteLine($"Inventories: {inventories.Count}");
            //string prodPath = "C:\\Users\\Owner\\Desktop\\RikaScript\\INV_PRODUCTS.txt";
            //string content2 = "";


            for (int i = 0; i < inventories.Count; i++)
            {
                //content2 += $"{inventories[i].fproductid},\n";
                for (int j = 0; j < ledgerDistributions.Count; j++)
                {
                    if(inventories[i].fproductid == ledgerDistributions[j].fproductid && inventories[i].ftrxdate == ledgerDistributions[j].finv_date) {
                        newledgerDistributions.Add(ledgerDistributions[j]);
                    }
                    else{
                        continue;
                    } 
                    
                }
            }
            //System.IO.File.WriteAllText(prodPath, content2);

            return newledgerDistributions;
        } 
    }
}

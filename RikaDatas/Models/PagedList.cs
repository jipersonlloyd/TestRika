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
            string filePath = "C:\\Users\\Owner\\Documents\\ADMIN-RIDS\\invledgerdistributionosmena01to1020.json";
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

        public static List<InvLedgerDistribution> NewLedger()
        {
            // List<InvLedgerDistribution> newledgerDistributions = new List<InvLedgerDistribution>();
            // List<InvLedgerDistribution> ledgerDistributions = InvLedgerDistributions();
            // List<RikaInventory> inventories = Inventories();
            // Console.WriteLine($"LedgerCount: {ledgerDistributions.Count}");
            // Console.WriteLine($"Inventories: {inventories.Count}");
            // string prodPath = "C:\\Users\\Owner\\Desktop\\RikaScript\\INV_PRODUCTS.txt";
            // string content2 = "";
            // string filePath = "C:\\Users\\Owner\\Desktop\\RikaScript\\Script.txt";
            // string filePath1 = "C:\\Users\\Owner\\Desktop\\RikaScript\\Script1.txt"; // with new line
            // string content = "";
            // string content1 = "";


            // for (int i = 0; i < inventories.Count; i++)
            // {
            //     for (int j = 0; j < ledgerDistributions.Count; j++)
            //     {
            //         if(inventories[i].fproductid == ledgerDistributions[j].fproductid && inventories[i].ftrxdate == ledgerDistributions[j].finv_date) {
            //             newledgerDistributions.Add(ledgerDistributions[j]);
            //             content += $"http://strika5.alliancewebpos.net/appserv/app/batch/fix/rebuild_inv_daily_summary.php?fcompanyid=STRIK5-12020182&fsale_date={ledgerDistributions[i].finv_date}&fend_date={ledgerDistributions[i].finv_date}&fpassword=5678efgh&fsiteid={ledgerDistributions[i].fsiteid}&fproductid={ledgerDistributions[i].fproductid},";
            //             content1 += $"http://strika5.alliancewebpos.net/appserv/app/batch/fix/rebuild_inv_daily_summary.php?fcompanyid=STRIK5-12020182&fsale_date={ledgerDistributions[i].finv_date}&fend_date={ledgerDistributions[i].finv_date}&fpassword=5678efgh&fsiteid={ledgerDistributions[i].fsiteid}&fproductid={ledgerDistributions[i].fproductid},\n";
            //         }
            //         else{
            //             continue;
            //         } 

            //     }
            // }
            // System.IO.File.WriteAllText(filePath, content);
            // System.IO.File.WriteAllText(filePath1, content1);

            // List<InvLedgerDistribution> distincted = newledgerDistributions.DistinctBy(x => x.fproductid).ToList();
            // for(int i = 0; i < distincted.Count; i++){
            //     content2 += $"{distincted[i].fproductid},\n";
            // }

            // System.IO.File.WriteAllText(prodPath, content2);
            // Console.WriteLine($"NewLedgerCount: {newledgerDistributions.Count}");
            // return newledgerDistributions;

            List<Dictionary<string, string>> dateList = new List<Dictionary<string, string>>();

            // Create the month names and dates
            List<string> months = [
                "01", "02", "03", "04", "05", "06",
            "07", "08", "09", "10", "11", "12"
            ];

            string[] products = [
                "10006744"
            ];


            for (int i = 0; i < months.Count; i++)
            {
                //change 01-28 to 29-31

                string yearMonth = $"2024{months[i]}29"; // Fixed year (2024) and month
                string endDate = "2024" + months[i] + "31"; // Append 28th for the end date

                var dateDict = new Dictionary<string, string>
            {
                { "finv_date", yearMonth },  // "YYYYMM"
                { "fend_date", endDate }     // "YYYYMM28"
            };
                dateList.Add(dateDict);
            }



            string filePath = "C:\\Users\\Owner\\Desktop\\RikaScript\\Script.txt";
            string filePath1 = "C:\\Users\\Owner\\Desktop\\RikaScript\\Script1.txt"; // with new line
            string content = "";
            string content1 = "";

            List<InvLedgerDistribution> newledgerDistributions = new List<InvLedgerDistribution>();
            for (int i = 0; i < products.Length; i++)
            {
                for (int j = 0; j < dateList.Count; j++)
                {
                    if (j == 12)
                    {
                        continue;
                    }
                    InvLedgerDistribution invLedger = new InvLedgerDistribution();
                    invLedger.fproductid = products[i];
                    invLedger.finv_date = dateList[j]["finv_date"];
                    invLedger.fend_date = dateList[j]["fend_date"];
                    newledgerDistributions.Add(invLedger);

                    content += $"https://rika.alliancewebpos.com/appserv/app/batch/fix/rebuild_inv_daily_summary.php?fcompanyid=RIKA5-12020182&fsale_date={invLedger.finv_date}&fend_date={invLedger.fend_date}&fpassword=5678efgh&fsiteid=AGORA2019&fproductid={invLedger.fproductid},";
                    content1 += $"https://rika.alliancewebpos.com/appserv/app/batch/fix/rebuild_inv_daily_summary.php?fcompanyid=RIKA5-12020182&fsale_date={invLedger.finv_date}&fend_date={invLedger.fend_date}&fpassword=5678efgh&fsiteid=AGORA2019&fproductid={invLedger.fproductid},\n";
                }
            }

            System.IO.File.WriteAllText(filePath, content);
            System.IO.File.WriteAllText(filePath1, content1);
            return newledgerDistributions;
        }
    }
}

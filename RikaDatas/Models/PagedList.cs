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



            //UPTOWN 2019 - DONE v1 DONE RELEASED IN PROD
            /*  string[] products = [
                 "2014046", //done
                 "10008504", //done
                 "10001715F", //done
                 "10007633", //done
                 "2014020028", //done
                 "10004867", //done
                 "10001650", //done
                 "10007219", //done
                 "12018663", //done
                 "1008970", //done
                 "1008941", //no issue done
                 "1009232", // done
                 "10004867", // done
                 "10001650", // done
                  "10007219", // done
             ]; */

            //UPTOWN 2019 - v2 DONE RELEASED IN PROD
            /* string[] products = [
                "10008504",
                "201402-09",
                "1201865",
                "10003717",
                "10006744",
                "10003017",
                "10008136",
                "2014020028",
                "1202453",
                "10001178",
                "10001187",
                "10002655",
                "10004939",
                "12018663",
                "10003658"
            ]; */

            //UPTOWN v3
            // string[] products = [
            // "10001715F",
            // "10005353",
            // "10007633",
            // "10008504",
            // "1202199",
            // "201402-09",
            // "10002946",
            // "10007028",
            // "1009232", - fixed using query
            // "025",
            // "10004002",
            // "10007213",
            // "10007671",
            // "10008503",
            // "1201865",
            // "014809",
            // "1002388",
            // "1008971",
            // "1202198",
            // "1202332",
            // "2014046", 
            // "10001044",
            // "10001153",
            // "10002667",
            // "10003671",
            // "10003685",
            // "10003717",
            // "10004867",
            // "10005355",
            // "10006744",
            // "10008124",
            // "12018782",
            // "12021200",
            // "G201312-1",
            // "10003017",
            // "10003681",
            // "10003710",
            // "10008136",
            // "1202424",
            // "201402-06",
            // "2014020028",
            // "10006109",
            // "1202425",
            // "1202453",
            // "415048",
            // "435046",
            // "10001650",
            // "10002662",
            // "10007219",
            // "G201401-18",
            // "10001178",
            // "10001185",
            // "10001187",
            // "10001462",
            // "10002655",
            // "10003290",
            // "10004939",
            // "10005358",
            // "10007014",
            // "12018663",
            // "1201882",
            // "10003658",
            // "10005156",
            // "10007029",
            // "1008051", - fixed using query
            // "1202331",
            // "6022258"
            // ];

            //OSMENA 2019 - 

            /* string[] products = [
                "1012368",
                "100201846",
                "1013261",
                "1008539",
                "23287406",
                "1202229",
                "1202064",
                "1009241",
                "1003350F",
                "1003350",
                "1201987",
            ]; */

            //OSMENA 2019 - v2 

            // "1006724" -- fixed using query

            /* string[] products = [
                "1007410", 
                "1005876", 
                "1012640",
                "1202130",
                "1014417",
                "1006588",
                "156132-S30"
            ]; */

            // string[] products = [
            //     "1012368",
            //     "100201846",
            //     "1013261",
            //     "1008539",
            //     "23287406",
            //     "1202229",
            //     "1202064",
            //     "1009241",
            //     "1003350F",
            //     "1003350",
            //     "1201987",
            //     "1007410", 
            //     "1005876", 
            //     "1012640",
            //     "1202130",
            //     "1014417",
            //     "1006588",
            //     "156132-S30"
            // ];

            // string[] products = [
            //     "1007410",
            //     "1005876",
            //     "1012640",
            //     "1014417",
            //     "1006588"
            // ];




            //ALUBIJID2019 - BATCH 1 - DONE RELEASED IN PROD

            //fixed by queries dont include in rebuild inv daily summaries
            // "10007107", - fixed with query
            // "1002816",  - fixed with query
            // "1003326", - fixed with query
            // "1005391", - fixed with query
            // "1005394", - fixed with query 
            // "1005396", - fixed with query 
            // "1005401", - fixed using query
            // "1005876", - fixed using query
            // "1006587G", - fixed using query 
            // "1006588", - fixed using query
            // "1006593", - fixed using query
            // "1007334", - fixed using query
            // "1007448", - fixed using query
            // "1007450", - fixed using query
            // "1007452", - fixed using query
            // "1007453", - fixed using query
            // "1007461", - fixed using query
            // "1007462", - fixed using query
            // "1009077", - fixed using query
            // "1009197", - fixed using query
            // "1010442", - fixed using query
            // "1010444", - fixed using query
            // "1011168", - fixed using query
            // "1011669", - fixed using query
            // "1014418", - fixed using query
            // "132945", - fixed using query
            // "201310-17", - fixed using query
            // "201310-18", - fixed using query
            // "ALU00003063", - fixed using query
            // "ALU00005856", - fixed using query
            // "ALU0000017385", - fixed using query
            // "10003585", - fixed using query
            // "ALU00001308", - fixed using query
            // "1006485", - fixed using query
            // "1006494", - fixed using query
            // "10003458", - fixed using query
            // "10003711", - fixed using query
            // "1006593", - fixed using query

            //string[] products = [
            //"10001325", - fixed
            //"10006291", - fixed
            //"1003331", - fixed
            //"1005397", - fixed
            //"1005399", - fixed
            //"1005400", - fixed
            //"1005402", - fixed
            //"1005872", - fixed
            //"1006724", - fixed
            //"1007407", - fixed
            //"1007449", - fixed 
            //"1007827", - fixed 
            //"1008051", - fixed
            //"1008227", - fixed
            //"1008678", - fixed
            //"1009043", - fixed
            //"1009044", - fixed
            //"1009217", - fixed
            //"1009232", - fixed
            //"1009781", - fixed
            //"1009782", - fixed
            //"1009783", - fixed
            //"1010439", - fixed
            //"1010440", - fixed
            //"1010441", - fixed
            //"1012054", - fixed
            //"1012055", - fixed
            //"1012056", - fixed
            //"1012640", - fixed
            //"1013171", - fixed
            //"1013279", - fixed
            //"1013520", - fixed
            //"1013523", - fixed
            //"1014148", - fixed
            //"1014149", - fixed
            //"1014415", - fixed
            //"1014417", - fixed
            //"1014684", - fixed
            //"1014685", - fixed
            //"12018171", - fixed
            //"12020103", - fixed
            //"12024168", - fixed
            //"1202482", - fixed
            //"1202483", - fixed
            //"201303-19", - fixed
            //"201312-07", - fixed
            //"201401-155615", - fixed
            //"G201310-205", - fixed
            //"R006" - fixed
            //"ALU00003053", - fixed
            //"04", - fixed
            //"10004160", - fixed
            //"10008125",
            //"10001319", 
            //"10003054F", 
            //"ALU00001723",
            //"1201942",
            //"ALU00002912", - fixed
            //"ALU00000066", - fixed
            //  "10003017",
            //  "ALU00003764",
            //  "ALU00001768",
            //  "ALU00003730",
            //  "12018190",
            //  "1202362",
            //  "10002922",
            //  "10003591",
            //  "10003015",
            //  "10002366",
            //  "10007027",
            //  "ALU00001074", 
            //  "10008310",
            //  "12018201",
            //  "ALU00001724",
            //  "120242",
            //  "10008126",
            //  "10003681",
            //  "10001044",
            //  "809518",
            //  "10000911",
            //  "10003687",
            //  "ALU00000894",
            //  "ALU000001238954",
            //  "10008341",
            //  "ALU00003729",
            //  "10004646",
            //  "2014060",
            //  "ALU00000069",
            //  "10002173",
            //  "10001324",
            //  "10004162",
            //  "ALU00003769",
            //  "10005659",
            //  "1003429",
            //  "ALU00000064",
            //  "10001479",
            //  "10004875",
            //  "1202368",
            //  "1202364",
            //  "ALU00003337",
            //  "10002103",
            //  "ALU00000855",
            //  "10004867",
            //  "ALU00000068",
            //  "10004152",
            //  "10004164",
            //  ];

            //ALUBIJID2019 BATCH 2 - DONE RELEASED IN PROD
            // string[] products = [
            // "06", - fixed using query
            // "10004160", - fixed using query
            // "ALU00005856", - fixed using query
            // "120241" - fixed using query
            // ];



            // AGORA2019 Products

            //"1012315", - done with query
            //"1003946", - done with query
            //"1006724", - done with query
            //"201312-06", - done with query
            //"10005815", - error (possible)
            //"1012079", - done with query
            //"1014417", - done with query
            //"1007410", - done with query
            //"12018171", - done with query
            //"1007461", - done with query
            //"10007107", - done with query
            //"1009782", - done with query
            //"1010440", - done with query
            //"1012640", - dpne with query
            //"120247", - done
            //"102018112", - done

            /* string[] products = [
                "12018709", - done
                "1002461", - done
                "1002746", - done
                "1002750", - done
                "180595", - done
                "162926", - done
                "1003729", - done
                "1003876", - done with query
                "10002046", - done with query
                "1005023", - done
                "170606", - done
                "1010688", - done
                "201212-154465", - done
                "1006789", - done
                "1007195", - done
                "201311-55", - done
                "1008044", - done
                "1008356", - done
                "1008913", - done
                "201401-154512", - done
                "171244", - done
                "1009872", - done
                "171245", - done
                "1010300", - done with query
                "1010320", - done
                "1010322", - done
                "1011072", - done
                "1011967", - done
                "121401-155131", - done
                "1012305", - done
                "1012315", - error
                "1012317", - done
                "1012346", - done
                "1013573", - done
                "1014281", - done
                "1014074", - done
                "1013068", - done
                "1202475", - done
                "1003946", - error
                "1003946", - done
                "1004235", - done
                "1006545", - done
                "1007749", - done
                "1008799", - done
                "1009581", - done
                "1010739", - done
                "12018291", - done
                "1011331", - done
                "23287406", - done 04/28



                "1011574", - done
                "1012921", - done
                "23256874", - done
                "23272039", - done
                "201409-IS4561", - done
                "12018372", - done
                "20128-1012", - done
                "1003254", - done
                "1003654", - done
                "11016910", - done
                "11016901", - done
                "1003994", - done
                "1005045", - done
                "1006334", - done
                "1006338", - done
                "R077", - done
                "R078", - done
                "1007523", - done
                "1007985", - done
                "1009189", - done
                "1009192", - done
                "12018586", - done
                "1009845", -  done
                "1011223", - done
                "1011401", - done   
                "82190", - done
                "1013261", - done
                "82280", - done
                "1014326", - done 04/29

                "25", - invalid product
                "45", - invalid product
                
                "1006588", - done
                "1202481", - done
                "1202480", - done
                "1006724", - error
                "201312-06", - error
                "1007292", - done
                "12018469", - done
                "12021218", - done
                "1008004", - done
                "100634", - done
                "1008903", - done
                "1009057", - done
                "12023111", - done
                "10005815", - error
                "R0215", - done
                "1202047", - done
                "12018499", - done
                "10001388", - done
                "1011191", - done
                "1011193", - done
                "1202038", - done
                "1012079", - error
                "1013171", - done
                "1013366", - done
                "1013759", - done
                "12018315", - done
                "1014417", - error
                "1014418", - done
                "12018645", - done 04/30

                "1002530", - done
                "20128-1025", - done
                "1003712", - done
                "1003714", - done
                "1004042", - done
                "20128-100008", - done
                "1005035", - done
                "126548", - done
                "201301-11", - done
                "201212-44", - done
                "2001281008", - done
                "1009020", - done
                "1012511", - done
                "201301-9", - done
                "120234", - done
                "12018727", - done
                "1003736", - done
                "1004239", - done
                "12018226", - done
                "12018227", - done
                "1202373", - done
                "1009396", - done
                "12018739", - done
                "1012524", - done
                "100201845", - done
                "100201843", - done
                "1013829", - done
                "1015140", - done
                "201312-O5", - done 
                "201312-06", - need to check again
                "1007410", - error
                "1008904", - done 05/02
                
                "1013244", - done
                "1014057", - done
                "1007410", - need to check again
                "10000331", - done
                "1003062", - done
                "120231", - done
                "12018170", - done
                "10000783", - done
                "10000789", - done
                "121291", - done
                "1003838", - done
                "1003850", - done
                "12018171", - error
                "1007410", - need to check again
                "1007461", - error
                "10007107", - error
                 "1008971", - done
                "1008941", - done
                "1009782", - error
                "1010440", - error
                "1010441", - done
                "1012640", - error
                "1013366", - need to check again
                "10002823", - done
                "10001737", - done
                "10001734", - done
                "1007808", - done 05/03

                "1007803", - done
                "1007804", - done
                "10006744", - done
                "10008500", - done
                "10003272", - done
                "10003273", - done
                "120248", - done
                "120247", - error
                "12018145", - done
                "ALU00001351", - done
                "10006949", - done
                "10001539", - done
                "2014397", - done
                "10002151", - done
                "10001845", - done
                "10000002", - done
                "10006829", - done
                "102018112", - error
                "10008838", - done
                "12018723", - done
                "10000475", - done
                "1006281", - done
                "10005347", - done
                "10003668", - done
                "10006327", - done
                "10008303", - done
                "100011542", - done
                "67462921", - done
                "10008385", - done
                "1011273", - done
                "10008125", - done
                "10000458", - done
            ]; */

            string[] products = ["1006674"];



            //JRBORJA PRODUCTS

            //string[] products = [
                // "1006674",
                // "201301-11",
                // "1007224",
                // "1009176",
                // "1008520",
                // "20130423",
                // "201212-44",
                // "2001281008",
                // "1009020",
                // "201305-21",
                // "1009126",
                // "1009151",
                // "1009153",
                // "1009161",
                // "1009530",
                // "1010684",
                // "1010456",
                // "10007450",
                // "1011090",
                // "1011415",
                // "1011583",
                // "1012505",
                // "12023135",
                // "10007192",
                // "10006312",
                // "10000615",
                // "20129-5656",
                // "1002414",
                // "1008060",
                // "20128-1020",
                // "20128-1021",
                // "20128-1022",
                // "1202186",
                // "1003213",
                // "12018733",
                // "1202429",
                // "102018245",
                // "1202410",
                // "1004041",
                // "12018155",
                // "102018275",
                // "1005386",
                // "1005387",
                // "1202333",
                // "12018121",
                // "1002864",
                // "100201760",
                // "1003798",
                // "100201769",
                // "1006593",
                // "1006697",
                // "1007051",
                // "1007318",
                // "12018249",
                // "1007344",
                // "240915",
                // "1008244",
                // "1008246",
                // "1202416",
                // "1008546",
                // "1202255",
                // "1202061",
                // "12020124",
                // "120233",
                // "1009394",
                // "1009396",
                // "1006393",
                // "201308-18",
                // "1010358",
                // "102018279",
                // "1011118",
                // "1011245",
                // "1011243",
                // "1011246",
                // "12018737",
                // "25030",
                // "100201736",
                // "1000246",
                // "1012513",
                // "1012514",
                // "1012524",
                // "102018244",
                // "10007525",
                // "1201822",
                // "100201845",
                // "100201848",
                // "100201846",
                // "100201843",
                // "100201844",
                // "1014100",
                // "1014225",
                // "201302-48",
                // "201302-49",
                // "79620262",
                // "1014859",
                // "120227",
                // "1015202",
                // "1003689",
                // "201310-17",
                // "1006588",
                // "12024171",
                // "1006724",
                // "1007827",
                // "1008903",
                // "1008904",
                // "20130516",
                // "1013279",
                // "1013513",
                // "1013523",
                // "1013759",
                // "1014057",
                // "10000332",
                // "10000331",
                // "10000372",
                // "1003063",
                // "1003061",
                // "100332622:22B2012:212:20",
                // "1003326",
                // "1003331",
                // "201403-32",
                // "10000522",
                // "10000537",
                // "201403-33",
                // "10000530",
                // "10000531",
                // "1003793",
                // "1003794",
                // "10000789",
                // "121291",
                // "1003824",
                // "1003830",
                // "1003840",
                // "20129-75",
                // "1003853",
                // "103385",
                // "1005391",
                // "1005394",
                // "1005400",
                // "1005401",
                // "1005402",
                // "12018171",
                // "120225",
                // "100201745",
                // "1007450",
                // "1007452",
                // "201311-01",
                // "12018594",
                // "10005429",
                // "1008971",
                // "1008940",
                // "1008959",
                // "1009076",
                // "1009077",
                // "1010439",
                // "1010440",
                // "1010441",
                // "1010442",
                // "1012079",
                // "1011669",
                // "1012639",
                // "10001400",
                // "1014417",
                // "1014420",
                // "1014685",
                // "10003168",
                // "415048",
                // "435046",
                // "10005898",
                // "10005169",
                // "10005163",
                // "10001769",
                // "10005164",
                // "1002431",
                // "1002646",
                // "1006234",
                // "2013110009",
                // "10006498",
                // "ALU00001143",
                // "10004160",
                // "10004152",
                // "10002986",
                // "10004973",
                // "10000087",
                // "1006975",
                // "10002595",
                // "10007853",
                // "1010024",
                // "12018109",
                // "10007747",
                // "10008500",
                // "10000458",
                // "1201882",
                // "1003893",
                // "10008136",
                // "10003004",
                // "10003017",
                // "1201942",
                // "10005260",
                // "10008380",
                // "10008245",
                // "10005623",
                // "10005625",
                // "10003047",
                // "10003044",
                // "10003046",
                // "10003054F",
                // "10003066",
                // "10003067",
                // "2014189",
                // "10003216",
                // "1010021",
                // "10000752",
                // "10000764",
                // "1007588",
                // "2014398",
                // "10008730",
                // "10002131",
                // "10002140",
                // "10002141",
                // "10002142",
                // "10002173",
                // "10002186",
                // "10002198",
                // "10002205",
                // "10007670",
                // "1008420",
                // "10001509",
                // "10007027",
                // "10002366",
                // "1014841",
                // "10002368",
                // "6002983",
                // "ALU00002067",
                // "ALU00004227",
                // "681095",
                // "1006215",
                // "1006167",
                // "2014010068",
                // "1008580",
                // "1003373",
                // "G201310-71",
                // "1005416",
                // "1005417",
                // "10001737",
                // "10001734",
                // "10005835",
                // "10005834",
                // "10002314",
                // "10003791",
                // "10004346",
                // "10006744",
                // "10001044",
                // "10001047",
                // "1202245",
                // "10007647",
                // "10006176",
                // "10005845",
                // "1003033",
                // "10003996",
                // "10004916",
                // "10002922",
                // "1201816",
                // "1009693",
                // "10005037",
                // "10001845",
                // "10003945",
                // "G201310-17",
                // "10008189",
                // "10008748",
                // "10007072",
                // "10008756",
                // "1202331",
                // "G201310-27",
                // "10005835G",
                // "10006145G",
                // "G201310-19",
                // "1007127",
                // "10005841",
                // "10005842",
                // "10001253",
                // "10001283",
                // "10001272",
                // "12020111",
                // "12020112",
                // "10001279",
                // "1006139",
                // "1006133",
                // "1006141",
                // "1006132",
                // "10007606",
                // "10006628",
                // "10004003",
                // "10004004",
                // "10004005",
                // "10004006",
                // "10004013",
                // "10004034",
                // "10004036",
                // "10004038",
                // "10004044",
                // "10004047",
                // "10004048",
                // "10004049",
                // "10004051",
                // "10004052",
                // "10002815",
                // "10002816",
                // "10002821",
                // "10002823",
                // "10002873",
                // "10004192",
                // "10004193",
                // "120201",
                // "12024179",
                // "10001069",
                // "10001073",
                // "10008322",
                // "10008324",
                // "10008325",
                // "102018334",
                // "10005995",
                // "1006481",
                // "1006494",
                // "10003480",
                // "10006341",
                // "10003385",
                // "1010379",
                // "10007329",
                // "10007441",
                // "10007311",
                // "ALU002941",
                // "10001864",
                // "10001867",
                // "10001868",
                // "10001869",
                // "ALU00002647",
                // "10007951",
                // "10007923",
                // "ALU00000029",
                // "ALU00000031",
                // "10000194",
                // "1003428",
                // "12018261",
                // "10000839",
                // "10001733",
                // "1202372",
                // "10006934",
                // "1002388",
                // "10007630",
                // "10008341",
                // "10007629",
                // "10000911",
                // "10000915",
                // "10000927",
                // "10001038",
                // "1007447",
                // "2014020028",
                // "2014010071",
                // "ALU00000955",
                // "1007361",
                // "10001126",
                // "10001128",
                // "10006126G",
                // "10001976",
                // "10001979",
                // "10002711",
                // "10002710",
                // "10005160",
                // "10008446",
                // "10002714",
                // "10002718",
                // "10003585",
                // "10003591",
                // "10003655",
                // "10003656",
                // "10003672",
                // "10003671",
                // "10003681",
                // "10003682",
                // "10003685",
                // "10003687",
                // "10003688",
                // "10003701",
                // "10007633",
                // "10003705G",
                // "10003705",
                // "10003710",
                // "10003712",
                // "ALU00003325",
                // "ALU00003333",
                // "10003721",
                // "1202449",
                // "1202199",
                // "1202198",
                // "12021200",
                // "2014-0307",
                // "12018663",
                // "12018662",
                // "1202424",
                // "1202425",
                // "10006502",
                // "10003342",
                // "10003338",
                // "10003341",
                // "10008207",
                // "10003347",
                // "10003349",
                // "10006699",
                // "10007335",
                // "2056984",
                // "10000258",
                // "1202263",
                // "10004243",
                // "10004243F",
                // "2013120041",
                // "2013120040",
                // "2013120061",
                // "2013120042",
                // "20230001214",
                // "10003595",
                // "10000074H",
                // "10006558",
                // "1202478",
                // "1008746",
                // "1201825",
                // "10001498",
                // "201403-11",
                // "1007724",
                // "10000365",
                // "10006925",
                // "10007038",
                // "10000164",
                // "10006829",
                // "102018112",
                // "10003173",
                // "G201311-26",
                // "10003176",
                // "10000505F",
                // "G201401-17",
                // "10008644",
                // "10000507",
                // "10000509F",
                // "10000706",
                // "1007439",
                // "10006983",
                // "G012014",
                // "10007194",
                // "102018330",
                // "10001463",
                // "10001715",
                // "10001715F",
                // "10001719",
                // "10002656",
                // "10002653",
                // "10002655",
                // "10002664",
                // "10002665",
                // "10002667",
                // "10002659",
                // "10003289",
                // "10003290",
                // "12018356",
                // "10003291",
                // "12018202",
                // "10005265",
                // "10004240",
                // "10004238",
                // "2014046",
                // "10008639",
                // "1003679",
                // "100201829",
                // "1003678",
                // "10005062",
                // "10000767",
                // "10008260",
                // "10000863",
                // "10000864",
                // "10000865",
                // "1007330",
                // "201305-47",
                // "201305-48",
                // "100008752",
                // "10000876",
                // "201305-45",
                // "1005665",
                // "10001529",
                // "1005667",
                // "10001563",
                // "10001550",
                // "10005201",
                // "10005975",
                // "10001189",
                // "10001199",
                // "10001200",
                // "10001202",
                // "10000891",
                // "10004249",
                // "10001180",
                // "10004939",
                // "10001184",
                // "10001182",
                // "10006992",
                // "10008509",
                // "2014010080",
                // "10007523",
                // "2014399",
                // "1202066",
                // "10004867",
                // "1202164",
                // "10007105",
                // "10003389",
                // "10007106",
                // "10005356",
                // "10008473",
                // "10000229",
                // "10008595",
                // "10000231",
                // "10007017",
                // "10006044",
                // "10007251",
                // "2013110040",
                // "2013110032",
                // "1008198",
                // "10007293",
                // "10001787",
                // "10006833",
                // "1003812",
                // "10006831",
                // "1005538",
                // "68944228",
                // "20131112",
                // "20131156",
                // "20131158",
                // "1003207",
                // "86049155",
                // "4195031",
                // "2014030004",
                // "105028",
                // "10000251",
                // "1007437",
                // "10007108",
                // "10004819",
                // "10004889",
                // "10006335",
                // "10004906",
                // "1202382",
                // "1202383",
                // "1202392",
                // "1004906",
                // "10008613",
                // "113034",
                // "10001133",
                // "10001153",
                // "10001150",
                // "100011541",
                // "100011544",
                // "100011542",
                // "67462921",
                // "1202218",
                // "10008310",
                // "10002524",
                // "10002541",
                // "10002550",
                // "10002551",
                // "10007400",
                // "10007954",
                // "10004062",
                // "10004136S",
                // "1011273",
                // "10006652",
                // "1012037",
                // "ALU00003769",
                // "10008126",
                // "10007922",
                // "10004655",
                // "10004605",
                // "SURFPWDR",
                // "10000539",
                // "10000540",
                // "10000541",
                // "10002335",
                // "10002338",
                // "10002337",
                // "10002343",
                // "1202391",
                // "1202388",
                // "1002745",
                // "10001440",
                // "10001788",
                // "10008150",
                // "10007405",
                // "12018388",
                // "12018723",
                // "10000475",
                // "2013110066",
                // "1006282",
                // "1006281",
                // "10004036",
                // "10004189",
                // "10008131",
                // "10008132",
                // "10004580",
                // "10004569",
                // "10007702",
                // "10006232",
                // "10007408",
                // "100201785",
                // "10008847",
                // "10000965"

                //Batch 2
                // "12018709",
                // "1002457",
                // "1002461",
                // "1002492",
                // "12018161",
                // "1002501",
                // "R0181",
                // "1002503",
                // "1002512",
                // "1002515",
                // "1002516",
                // "20135-152083",
                // "156130",
                // "156132",
                // "1002652",
                // "1002746",
                // "1002750",
                // "R0150",
                // "1002854",
                // "1003350",
                // "1201987",
                // "1003351",
                // "180595",
                // "180596",
                // "1003354",
                // "1003356",
                // "1003641",
                // "1003654",
                // "1003666",
                // "1003729",
                // "1003727",
                // "1003728",
                // "1003776",
                // "1003777",
                // "156328",
                // "201302-154964",
                // "201302-154965",
                // "1003881",
                // "1003882",
                // "1003883",
                // "1003886",
                // "1003887",
                // "1004631",
                // "1004894",
                // "1004892",
                // "1004900",
                // "20140802-1005023",
                // "170606",
                // "1005147",
                // "1005148",
                // "1005182",
                // "1005184",
                // "1005277",
                // "179395",
                // "183198",
                // "1005516",
                // "201210-154845D3",
                // "1005971",
                // "1010688",
                // "201402-155862",
                // "172998",
                // "1006732",
                // "1006789",
                // "1006889",
                // "1007011",
                // "1007015",
                // "1007031",
                // "156857",
                // "1007195",
                // "156259",
                // "1007847",
                // "1007851",
                // "12018288",
                // "201311-55",
                // "176816",
                // "1008044",
                // "201407-155936",
                // "1008913",
                // "174897",
                // "1009434",
                // "1009587",
                // "12018489",
                // "12018490",
                // "1009627",
                // "171244",
                // "171244P",
                // "1009874",
                // "201210-152557",
                // "201210-2",
                // "1011072",
                // "169812",
                // "1011967",
                // "12023113",
                // "201210-154878",
                // "1012201",
                // "1012202",
                // "1012204",
                // "1012216",
                // "R018",
                // "1012253",
                // "R020",
                // "102018149",
                // "701394",
                // "1012298",
                // "1012305",
                // "1012311",
                // "1012313",
                // "1012317",
                // "2012-11-28",
                // "20129-4551",
                // "12018136",
                // "1012346",
                // "1012348",
                // "1012360",
                // "1012362",
                // "1012951",
                // "12018163",
                // "167603",
                // "1013103",
                // "1013099",
                // "1013105",
                // "1013573",
                // "1013814",
                // "1013815",
                // "1013817",
                // "1014074",
                // "201401-155615",
                // "1202426",
                // "1202474",
                // "102018100",
                // "1014117",
                // "1002315",
                // "1014215",
                // "168296",
                // "1014977",
                // "1002375",
                // "1002534",
                // "1002717",
                // "1002808",
                // "102018249",
                // "1003262",
                // "1003523",
                // "1003524",
                // "1003681",
                // "1004086",
                // "1004088",
                // "1004704",
                // "1005009",
                // "21252021",
                // "21207284",
                // "1005328",
                // "1005890",
                // "1005894",
                // "1005896",
                // "1006277",
                // "1007747",
                // "1007832",
                // "1008164",
                // "1009727",
                // "1009729",
                // "20128-1017",
                // "1009951",
                // "21232515",
                // "1010078",
                // "1010398",
                // "1010399",
                // "23287406",
                // "1011575",
                // "1011577",
                // "1011579",
                // "1011896",
                // "1012577",
                // "1012593",
                // "1012715",
                // "1005300",
                // "1012921",
                // "1013636",
                // "1013639",
                // "1013641",
                // "1013633",
                // "1013644",
                // "1003534",
                // "1015095",
                // "1015094",
                // "201409-IS4561",
                // "1002383",
                // "201409-IS3456",
                // "201409-IS123",
                // "2014-02DW5013001",
                // "1003253",
                // "1003254",
                // "1003609",
                // "1003610",
                // "20128-10005",
                // "11016910",
                // "11016901",
                // "10007120",
                // "1004476",
                // "1004472",
                // "1005323",
                // "13043891",
                // "1005688",
                // "1005689",
                // "1005813",
                // "1006334",
                // "1006338",
                // "13037441",
                // "R077",
                // "1008481",
                // "1009087",
                // "1009189",
                // "1009192",
                // "1009194",
                // "1009810",
                // "12018586",
                // "1009845",
                // "82250",
                // "1010715",
                // "1011409",
                // "82190",
                // "11000032",
                // "1012902",
                // "1013261",
                // "82253",
                // "100201733",
                // "82231",
                // "1015181",
                // "1002806",
                // "1002442",
                // "12020115",
                // "1003296",
                // "1202051",
                // "1002434",
                // "1003349",
                // "201609",
                // "1003598",
                // "12018338",
                // "1202210",
                // "1202310",
                // "1202351",
                // "1004935",
                // "1008469",
                // "12018278",
                // "1006033",
                // "1006056",
                // "1202261",
                // "1006597",
                // "12018564",
                // "1202096",
                // "12018469",
                // "1007859",
                // "1007861",
                // "1007864",
                // "1202143",
                // "1201903",
                // "12021218",
                // "10002730",
                // "10002602",
                // "1008465",
                // "1003409",
                // "100634",
                // "12018563",
                // "1202175",
                // "1006707",
                // "10008589",
                // "12023111",
                // "12023154",
                // "R0215",
                // "1009551",
                // "102018327",
                // "1009674",
                // "1202046",
                // "1202047",
                // "12018499",
                // "1202411",
                // "1202337",
                // "1011191",
                // "1011192",
                // "1011193",
                // "1011194",
                // "1011190",
                // "23287414",
                // "1011350",
                // "23287418",
                // "1202229",
                // "1011368",
                // "1011636",
                // "1011870",
                // "1012758",
                // "1012762",
                // "20128-10000000015",
                // "12018566",
                // "1202130",
                // "1014054",
                // "12018290",
                // "1014053",
                // "1009675",
                // "1202413",
                // "1014417",
                // "1014418",
                // "10004775",
                // "171937",
                // "20128-100015",
                // "1002559",
                // "1002561",
                // "1002591",
                // "20128-1025",
                // "1002614",
                // "201305-12",
                // "12023164",
                // "1003712",
                // "1003717",
                // "1004042",
                // "201212-47",
                // "20128-100008",
                // "1004181",
                // "1004365",
                // "1004690",
                // "1005028",
                // "1005035",
                // "1005244",
                // "1010688",
                // "1002817",
                // "20130527",
                // "12023162"
//            ];



            for (int i = 0; i < months.Count; i++)
            {
                //change 01-28 to 29-31

                string yearMonth = $"2024{months[i]}01"; // Fixed year (2024) and month
                string endDate = "2024" + months[i] + "28q"; // Append 28th for the end date

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

                    content += $"http://strika5.alliancewebpos.net/appserv/app/batch/fix/rebuild_inv_daily_summary.php?fcompanyid=RIKA5-12020182&fsale_date={invLedger.finv_date}&fend_date={invLedger.fend_date}&fpassword=5678efgh&fsiteid=JRBORJA&fproductid={invLedger.fproductid},";
                    content1 += $"http://strika5.alliancewebpos.net/appserv/app/batch/fix/rebuild_inv_daily_summary.php?fcompanyid=RIKA5-12020182&fsale_date={invLedger.finv_date}&fend_date={invLedger.fend_date}&fpassword=5678efgh&fsiteid=JRBORJA&fproductid={invLedger.fproductid},\n";
                }
            }

            System.IO.File.WriteAllText(filePath, content);
            System.IO.File.WriteAllText(filePath1, content1);
            return newledgerDistributions;
        }
    }
}

namespace RikaDatas.Models
{
    public class RikaInventory
    {
        public string? fcompanyid { get; set; }
        public string? fproductid { get; set; }
        public string? fsiteid { get; set; }

        public string? flotno { get; set; }
        public string? ftrxdate { get; set; }
        public double? fqty { get; set; }
        public string? fcreated_by { get; set; }
        public string? fcreated_date { get; set; }
        public string? fupdated_by { get; set; }
        public string? fupdated_date { get; set; }
        public double fadjusted_qty { get; set; }
        public double fadjusted_amount { get; set; }
        public double fsold_qty { get; set; }
        public double fsold_amount { get; set; }
        public double fsold_cost { get; set; }
    }
}

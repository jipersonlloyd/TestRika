namespace RikaDatas.Models
{
    public class InvDailySummary
    {
        public string? fcompanyid { get; set; }
        public string? fproductid { get; set; }
        public string? fsiteid { get; set; }
        public string? flotno { get; set; }
        public string? ftrxdate { get; set; }
        public decimal fqty { get; set; }
        public decimal fadjusted_qty { get; set; }
        public decimal fadjusted_amount { get; set; }
        public decimal fsold_qty { get; set; }
        public decimal fsold_amount { get; set; }
        public decimal fsold_cost { get; set; }

    }
}

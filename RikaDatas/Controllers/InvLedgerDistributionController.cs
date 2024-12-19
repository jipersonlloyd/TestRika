using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RikaDatas.Models;

namespace RikaDatas.Controllers
{
    public class InvLedgerDistributionController : Controller
    {
        private static List<InvLedgerDistribution> ledgerDistributions = new List<InvLedgerDistribution>();
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var pagedData = PagedList.CreateLedgerPagination(page, pageSize);
            return View(pagedData);
        }


        
    }
}

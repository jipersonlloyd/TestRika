using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RikaDatas.Models;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RikaDatas.Controllers
{
    public class RikaSalesController : Controller
    {
        
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var pagedData = PagedList.Create(page, pageSize);
            return View(pagedData);
        }
    }
}

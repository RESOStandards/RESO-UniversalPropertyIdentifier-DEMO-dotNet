using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Upi.web.Controllers
{
    public class UsUpi
    {
        public string Country { get; set; } = "US";
        public string FIPSCounty { get; set; } = "04013";
        public string FIPSSubCounty { get; set; } = "N";
        public string PropertyType { get; set; } = "R";
        public string ParcelNumber { get; set; } = "30088622";
        public string SubPropertyId { get; set; } = "";

        public override string ToString()
        {
            return $"{Country}-{FIPSCounty}-{FIPSSubCounty}-{PropertyType}-{ParcelNumber}{(PropertyType !="R" && SubPropertyId!=null ? SubPropertyId:"")}";
        }
    }

    public class UpiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetUpi(UsUpi upi)
        {
            return View(upi);
        }
        public IActionResult Builder()
        {
            return View();
        }

    }
}
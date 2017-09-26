using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upi.web.Fips;

namespace Upi.web.Controllers
{
    public class UsUpi
    {
        const string UNKNOWN = "Invalid";

        public UsUpi(string upi)
        {
            var segments = upi.Split('-');
            FipsState state = null;
            FipsCounty county = null;
            FipsSubCounty subCounty = null;

            if (segments.Length == 5 || segments.Length == 6) {

                Country = (segments[0].ToUpper() == "US") ? "US" : UNKNOWN;

                if (Country != UNKNOWN )
                {
                    if (segments[1].Length == 5)
                    {
                        state = Fips.FipsCache.FIPS.States.FirstOrDefault(f => f.Code == segments[1].Substring(0, 2));
                        FIPSState = state != null ? state.Name : UNKNOWN;

                        county = state != null ? state.Counties.FirstOrDefault(f => f.Code == segments[1].Substring(2, 3)) : null;
                        FIPSCounty = county != null ? county.Name : UNKNOWN;
                    }
                }
                else
                {
                    FIPSCounty = UNKNOWN;
                }

                if (FIPSCounty != UNKNOWN)
                {
                    subCounty = county != null ? county.SubCounties.FirstOrDefault(f => f.Code == segments[2]) : null;
                    FIPSSubCounty = subCounty != null ? subCounty.Name : "N/A";
                }

                if (segments[3].Trim().Length == 1)
                {
                    switch (segments[3].Trim())
                    {
                        case "R":
                            PropertyType = "Real Property";
                            break;
                        case "S":
                            PropertyType = "Stock Coop, Apartment, business office, etc";
                            break;
                        case "T":
                            PropertyType = "Temporary Desgination";
                            break;
                        default:
                            PropertyType = UNKNOWN;
                            break;
                    }
                }
                else
                {
                    PropertyType = UNKNOWN;
                }

                if (PropertyType != UNKNOWN )
                {
                    ParcelNumber = segments[4];

                    if (PropertyType != "R")
                    {
                        if (segments.Length == 6)
                        {
                            SubPropertyId = segments[5];
                        }
                        else
                        {
                            ErrorText += "Invalid Missing sup-property Id";
                        }

                    }
                    else if (segments.Length == 6)
                    {
                        ErrorText += "Invalid Unexpected sub-property Id";
                    }

                }
                else
                {
                    ParcelNumber = UNKNOWN;
                }

                ErrorText = Country.Contains(UNKNOWN) ? "Invald Country code; " : "";
                ErrorText = FIPSState.Contains(UNKNOWN) ? "Invald State code; " : "";
                ErrorText = FIPSCounty.Contains(UNKNOWN) ? "Invald FIPS County code; " : "";
                ErrorText = FIPSSubCounty.Contains(UNKNOWN) ? "Invald FIPS Sub-county code; " : "";
                ErrorText = PropertyType.Contains(UNKNOWN) ? "Invald Property Type; " : "";
                ErrorText = ParcelNumber.Contains(UNKNOWN) ? "Invald Country; " : "";
                ErrorText = (segments.Length >= 5) && (ParcelNumber.Length == 0) ? "Missing parcel number; " : "";
                ErrorText = (segments.Length == 6) && (SubPropertyId.Length == 0) ? "Missing Sub-property Identifier; " : "";

            }
            else
            {
                ErrorText = "Invalid UPI";
            }
        }

        public UsUpi() { }

        public string Country { get; set; }
        public string FIPSState { get; set; }
        public string FIPSCounty { get; set; }
        public string FIPSSubCounty { get; set; }
        public string PropertyType { get; set; }
        public string ParcelNumber { get; set; }
        public string SubPropertyId { get; set; }
        public string ErrorText { get; set; }

        public override string ToString()
        {
            return $"{Country}-{FIPSCounty}-{FIPSSubCounty}-{PropertyType}-{ParcelNumber}{(PropertyType != "R" && SubPropertyId != null ? SubPropertyId : "")}";
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

        public IActionResult Viewer()
        {
            return View();
        }
        [HttpGet("{upi}", Name = "Viewer")]
        public IActionResult Viewer(string upi)
        {
            return View();
        }

        public UsUpi ViewUpi(string upi)
        {
            if(upi== null)
            {
                return new UsUpi();
            }
            // return a view of the UPI
            try
            {
                var upiBreakdown = new UsUpi(upi);
                return upiBreakdown;
            }
            catch (Exception ex)
            {
                var result = new UsUpi();
                result.ErrorText = ex.Message;
                return result;
            }
        }
    }
}
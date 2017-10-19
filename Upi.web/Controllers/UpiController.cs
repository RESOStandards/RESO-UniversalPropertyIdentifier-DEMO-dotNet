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
            upi = upi.ToUpper();

            var segments = upi.Split('-');
            FipsState state = null;
            FipsCounty county = null;
            FipsSubCounty subCounty = null;

            Country = UNKNOWN;
            FIPSState = UNKNOWN;
            FIPSCounty = UNKNOWN;
            FIPSSubCounty = UNKNOWN;
            PropertyType = UNKNOWN;
            ParcelNumber = UNKNOWN;
            SubPropertyId = UNKNOWN;


            ErrorText = "";

            var subPropertyError = "";
            var segmentCount = segments.Count(s => s.Length > 0);

            if ((segmentCount == 5 || segmentCount == 6) && (segments.Count(s=> s.Length==0)==0))
            {
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
                    FIPSState = UNKNOWN;
                    FIPSCounty = UNKNOWN;
                }

                if (FIPSCounty != UNKNOWN)
                {
                    if (segments[2] == "N")
                    {
                        FIPSSubCounty = "N/A";
                    }
                    else
                    {
                        subCounty = county.SubCounties.FirstOrDefault(f => f.Code == segments[2]);
                        FIPSSubCounty = subCounty != null ? subCounty.Name : UNKNOWN;
                    }
                }
                else
                {
                    FIPSSubCounty = UNKNOWN;
                }

                if (segments[3].Length == 1)
                {
                    switch (segments[3])
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

                    if (segments[3] != "R")
                    {
                        if (segments.Length == 6)
                        {
                            SubPropertyId = segments[5];
                        }
                        else
                        {
                            subPropertyError = "Invalid Missing sup-property Id";
                        }

                    }
                    else if (segments.Length == 6)
                    {
                        if (segments[5].ToUpper() != "N")
                            subPropertyError = "Invalid sub-property Id";
                        else
                            SubPropertyId = $"{segments[5].ToUpper()}";
                    }
                    else
                    {
                        subPropertyError = "Invalid sub-property Id";
                    }
                }
                else
                {
                    ParcelNumber = UNKNOWN;
                }

                ErrorText += Country.Contains(UNKNOWN) ? "Invald Country code; " : "";
                ErrorText += FIPSState.Contains(UNKNOWN) ? "Invald State code; " : "";
                ErrorText += FIPSCounty.Contains(UNKNOWN) ? "Invald FIPS County code; " : "";
                ErrorText += FIPSSubCounty.Contains(UNKNOWN) ? "Invald FIPS Sub-county code; " : "";
                ErrorText += PropertyType.Contains(UNKNOWN) ? "Invald Property Type; " : "";
                ErrorText += ParcelNumber.Contains(UNKNOWN) ? "Invald Country; " : "";
                ErrorText += (segments.Length >= 5) && (ParcelNumber.Length == 0) ? "Missing parcel number; " : "";
                ErrorText += (!string.IsNullOrEmpty(subPropertyError)) ? subPropertyError : "";

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
        public string UpiScore { get; set; }
        public string ErrorText { get; set; }

        public override string ToString()
        {
            return $"{Country}-{FIPSCounty}-{FIPSSubCounty}-{PropertyType}-{ParcelNumber}{(PropertyType != "R" && SubPropertyId != null ? SubPropertyId : "")}";
        }

    }

    [Produces("application/json")]
    [Route("api/upiCheck")]
    public class UpiCheck : Controller
    {
        [HttpGet("{upi}", Name = "Validate")]
        public UsUpi Get(string upi)
        {
            if (upi == null)
            {
                return new UsUpi();
            }
            // return a view of the UPI
            try
            {
                Random r = new Random();

                var upiBreakdown = new UsUpi(upi);
                if (upiBreakdown.ErrorText.Any())
                {
                    upiBreakdown.UpiScore = 0.ToString();
                }
                else
                {
                    var t = upiBreakdown.ParcelNumber.Substring(0, 2);
                    double result;
                    if (double.TryParse(t, out result))
                    {
                        upiBreakdown.UpiScore = ((result / 100 * 20) + 80).ToString("F2");
                    }
                    else
                    {
                        upiBreakdown.UpiScore = "77.98";
                    }
                }

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

        public IActionResult Viewer(string upi)
        {
            return View();
        }

    }
}
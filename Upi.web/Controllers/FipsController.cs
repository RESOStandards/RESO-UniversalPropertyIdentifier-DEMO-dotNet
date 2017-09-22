using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Upi.web.Fips;

namespace Upi.web.Controllers
{
    [Produces("application/json")]
    [Route("api/fips")]
    public class fipsController : Controller
    {
        [HttpGet]
        public IEnumerable<FipsState> Get()
        {
            return FipsCache.FIPS.States
                .OrderBy(c => c.Name)
                .Select(s=> new FipsState { Name= $"{s.Name} ({s.Code})", Code=s.Code })
                .ToList();
        }

        [HttpGet("{stateId}", Name = "GetCounties")]
        public IEnumerable<FipsCounty> Get(string stateId)
        {
            var counties = new List<FipsCounty>();
            var fipsState = FipsCache.FIPS.States.FirstOrDefault(s => s.Code == stateId || s.Name.Equals(stateId, StringComparison.CurrentCultureIgnoreCase));

            if (fipsState != null)
            {
                var fipsResults = fipsState
                                .Counties.Where(c => c.StatusCode != "H5")
                                .OrderBy(c => c.Name).ToList();

                if (fipsResults != null)
                {
                    counties.AddRange( 
                        fipsResults
                            .Select(c => new FipsCounty { Name = $"{c.Name} ({c.Code}) [{c.StatusCode}]", Code = c.Code, StatusCode = c.StatusCode })
                            .ToList());
                }
            }

            return counties;
        }

        [HttpGet("{stateId}/{countyId}", Name = "GetSubCounties")]
        public IEnumerable<FipsSubCounty> Get(string stateId, string countyId)
        {
            var subcounties = new List<FipsSubCounty>();
            var fipsState = FipsCache.FIPS.States.FirstOrDefault(s => s.Code == stateId || s.Name.Equals(stateId, StringComparison.CurrentCultureIgnoreCase));

            if (fipsState != null)
            {
                var fipsCounty = fipsState.Counties.FirstOrDefault(c => c.Code == countyId);

                if (fipsCounty != null)
                {
                    var naOption = new FipsSubCounty() { Code = "N", Name = "N/A", FunctionalStatus = "" };

                    subcounties.Add(naOption);
                    var invalidFunctionalStatuses = "FNS";

                    var definedSubcounties = fipsCounty
                                        .SubCounties.Where(s => !invalidFunctionalStatuses.Contains(s.FunctionalStatus))
                                        .OrderBy(c => c.Name)
                                        .Select(s => new FipsSubCounty { Name = $"{s.Name} ({s.Code}) [{s.FunctionalStatus}]", Code = s.Code, FunctionalStatus = s.FunctionalStatus })
                                        .ToList();

                    subcounties.AddRange(definedSubcounties);
                }

            }
            return subcounties;
        }

    }

}
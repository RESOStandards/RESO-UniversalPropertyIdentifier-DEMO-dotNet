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
            return FipsCache.FIPS.States.FirstOrDefault(s=> s.Code==stateId)
                                .Counties.Where(c=> c.StatusCode!="H5")
                                .OrderBy(c=> c.Name)
                                .Select(c=> new FipsCounty {Name= $"{c.Name} ({c.Code}) [{c.StatusCode}]", Code=c.Code, StatusCode=c.StatusCode })
                                .ToList();
        }

        [HttpGet("{stateId}/{countyId}", Name = "GetSubCounties")]
        public IEnumerable<FipsSubCounty> Get(string stateId, string countyId)
        {
            var subcounties = new List<FipsSubCounty>();
            subcounties.Add(new FipsSubCounty() { Code = "N", Name = "N/A", FunctionalStatus = "" });

            subcounties.AddRange(
                FipsCache.FIPS.States.FirstOrDefault(s => s.Code == stateId)
                                .Counties.FirstOrDefault(c=> c.Code == countyId)
                                .SubCounties.Where(s=> s.FunctionalStatus != "F" && s.FunctionalStatus != "N")
                                .OrderBy(c => c.Name)
                                .Select(s=> new FipsSubCounty { Name= $"{s.Name} ({s.Code}) [{s.FunctionalStatus}]", Code = s.Code, FunctionalStatus=s.FunctionalStatus })
                                .ToList()
                );

            return subcounties;
        }

    }

}
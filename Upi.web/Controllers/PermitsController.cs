using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Upi.web.BuildingPermits;

namespace Upi.web.Controllers
{
    [Produces("application/json")]
    [Route("api/Permits")]
    public class PermitsController : Controller
    {
        // GET: api/Permits/5
        [HttpGet("{upi}", Name = "GetPermits")]
        public IEnumerable<Permit> Get(string upi)
        {
            return PermitDb.GetPermits(upi);
        }
    }
}

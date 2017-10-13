using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Upi.web.ClaimData;

namespace Upi.web.Controllers
{
    [Produces("application/json")]
    [Route("api/Claims")]
    public class ClaimsController : Controller
    {
        // GET: api/Claims/5
        [HttpGet("{upi}", Name = "GetClaims")]
        public IEnumerable<Claim> Get(string upi)
        {
            return ClaimDb.GetClaims(upi);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Upi.web.ClaimData.ClaimDb;
using Upi.web.Common;

namespace Upi.web.ClaimData
{
    public static class ClaimDb
    {
        static List<Claim> AllClaims = new List<Claim>();
        static Random rnd = new Random();

        public static List<Claim> GetClaims(string upi)
        {
            if (!AllClaims.Where(p => p.UPI == upi).Any())
            {
                AddRandomClaims(upi);
            }

            return AllClaims.Where(p => p.UPI == upi).ToList();
        }

        static void AddRandomClaims(string upi)
        {
            int maxClaims = 3;
            int minClaims = 1;

            var permitCount = (int)(rnd.NextDouble() * maxClaims) + minClaims;

            for (int i = 0; i < permitCount; i++)
            {
                AllClaims.Add(Claim.Random(upi));
            }
        }
    }

    public class Claim
    {
        public string UPI { get; set; }
        public string ClaimType { get; set; }
        public string ClaimStatus { get; set; }
        public DateTime LossDate { get; set; }
        public string LossAmount { get; set; }
        public string ClaimNumber { get; set; }
        public string CausedBy { get; set; }

        public static Claim Random(string upi)
        {
            var lossDate = DateTime.Now.AddYears(-10).RandomDate(365 * 4);

            return new Claim()
            {
                UPI = upi,
                ClaimNumber = $"CL-{1000000.RandomNumber(9999999)}",
                LossDate = lossDate,
                LossAmount = 10000.RandomNumber(9999999),
                CausedBy = typeof(ClaimCause).Random(),
                ClaimStatus = typeof(ClaimStatus).Random(),
                ClaimType = typeof(ClaimType).Random()
            };
        }
    }

    public enum ClaimCause
    {
        Earthquake,
        Flood,
        Superhero,
        SuperVillian,
        Wyle_E_Coyote,
        AlienInvasion,
        DeathStarWeapon,
        Walkers,
        Apocalype,
        Dragon,
        Dinosaur,
        Vampire,
        WeatherControlSystem
    }

    public enum ClaimStatus
    {
        Ignored,
        InReview,
        UnderConsideration,
        LaughedAt,
        Ridiculed,
        RubberStamped
    }

    public enum ClaimType
    {
        Bogus,
        Fictional,
        Huge,
        Fraudulent,
        ShouldHaveKnownBetter
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upi.web.Common;
using static Upi.web.BuildingPermits.PermitDb;

namespace Upi.web.BuildingPermits
{
    public static class PermitDb
    {
        static List<Permit> AllPermits = new List<Permit>();
        static Random rnd = new Random();

        public static List<Permit> GetPermits(string upi)
        {
            if (!AllPermits.Where(p=> p.UPI==upi).Any())
            {
                AddRandomPermits(upi);
            }

            return AllPermits.Where(p => p.UPI == upi).ToList();
        }

        static void AddRandomPermits (string upi)
        {
            int maxPermits = 4;
            int minPermits = 1;

            var permitCount = (int)(rnd.NextDouble() * maxPermits) + minPermits;

            for(int i=0; i < permitCount; i++)
            {
                AllPermits.Add(Permit.Random(upi));
            }

        }

        public static List<string> Vendors = new List<string>
        {
            "Devon Mills",
            "Duff Builders",
            "Wayne Enterprises",
            "Stark Industries",
            "Death Star Construction",
            "Vendor X",
            ""

        };
        public static List<string> Projects = new List<string>
        {
            "Death Star",
            "DeadPool",
            "House",
            "MethLab",
            "Haunted House",
            "Laboratory",
            "Lavoratory",
            "Chamber of Secrets",
            "Basement Gun Range",
            "Inner Sanctum"
        };
    }
        
    public class Permit
    {
        public string UPI { get; set; }
        public string PermitType { get; set; }
        public DateTime ApprovedOn { get; set; }
        public string Status { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string ProjectName { get; set; }
        public string RecordId { get; set; }
        public string CaseNumber { get; set; }
        public string BuilderName { get; set; }
        public string ContractorNumber { get; set; }

        public static Permit Random(string upi)
        {
            var approvedOn = DateTime.Now.AddYears(-10).RandomDate(365 * 4);

            return new Permit()
            {
                UPI = upi,
                BuilderName = Vendors.Random(),
                ProjectName = Projects.Random(),
                CaseNumber = $"CS-{1000000.RandomNumber(9999999)}",
                ApprovedOn = approvedOn,
                ExpiresOn = approvedOn.RandomDate(365),
                Status = typeof(PermitStatus).Random(),
                PermitType = typeof(PermitType).Random(),
                ContractorNumber = $"ROC-{100000.RandomNumber(999999)}",
                RecordId = $"R-{10000000.RandomNumber(99999999)}"
            };
        }
    }

    public enum PermitStatus
    {
        Ignored,
        InReview,
        Granted,
        LaughedAt,
        Ridiculed,
        RubberStamped
    }

    public enum PermitType
    {
        EvilStructure,
        Containment,
        Secret,
        Nuclear,
        Fortress,
        BestType,
        HomeBasedWeapon,
        GovernmentSponsored,
        WorstType,
        Cemetary
    }



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upi.web.Common
{
    public static class Randomizer
    {
        static Random rnd { get; } = new Random();

        public static string Random(this List<string> list)
        {
            var index = (int)(rnd.NextDouble() * list.Count - 1);

            return list[index];
        }
        public static DateTime RandomDate(this DateTime earliest, int maxDays)
        {
            var index = (int)(rnd.NextDouble() * maxDays - 1);

            return earliest.AddDays(index);
        }
        public static string RandomNumber(this int min, int max)
        {
            var index = (int)(rnd.NextDouble() * max - 1) + min;

            return index.ToString();
        }
        public static string Random(this Type enumType)
        {
            var index = (int)(rnd.NextDouble() * Enum.GetNames(enumType).Count() - 1);

            return Enum.GetValues(enumType).GetValue(index).ToString();
        }
    }
}

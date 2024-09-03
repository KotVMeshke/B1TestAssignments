using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Generators
{
    /// <summary>
    /// This class provides generation of date
    /// </summary>
    internal class DateGenerator
    {
        private static readonly Random _random = new Random();
        /// <summary>
        /// Generates date between 5 years ago and now
        /// </summary>
        /// <returns>Returns generated date</returns>
        public static DateTime GenerateValue()
        {
            DateTime startDate = DateTime.Now.AddYears(-5);
            int range = (DateTime.Today - startDate).Days;
            DateTime randomDate = startDate.AddDays(_random.Next(range));

            return randomDate;
        }
    }
}

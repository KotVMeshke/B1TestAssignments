using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Generators
{
    internal class DateGenerator
    {
        private static readonly Random _random = new Random();
        public static DateTime GenerateValue()
        {
            DateTime startDate = DateTime.Now.AddYears(-5);
            int range = (DateTime.Today - startDate).Days;
            DateTime randomDate = startDate.AddDays(_random.Next(range));

            return randomDate;
        }
    }
}

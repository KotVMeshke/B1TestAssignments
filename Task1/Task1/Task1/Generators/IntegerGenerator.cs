using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Generators
{
    /// <summary>
    /// This class provides generation of integer value
    /// </summary>
    internal class IntegerGenerator
    {
        private static Random _random = new Random();

        /// <summary>
        /// Generates integer value
        /// </summary>
        /// <returns>Returns generated integer value</returns>
        public static int GenerateValue() => _random.Next(1, 100_000_000);
    }
}

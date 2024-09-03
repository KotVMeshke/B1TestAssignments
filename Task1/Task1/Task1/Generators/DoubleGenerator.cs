using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Generators
{
    /// <summary>
    /// This class provides generation of double values
    /// </summary>
    internal class DoubleGenerator
    {
        private static Random _random = new Random();

        /// <summary>
        /// Generates double value
        /// </summary>
        /// <returns>Returns generated value</returns>
        public static double GenerateValue() => double.Round(1 + _random.NextDouble()*19,8);
    }
}

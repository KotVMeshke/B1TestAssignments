using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Generators
{
    internal class DoubleGenerator
    {
        private static Random _random = new Random();

        public static double GenerateValue() => double.Round(1 + _random.NextDouble()*19,8);
    }
}

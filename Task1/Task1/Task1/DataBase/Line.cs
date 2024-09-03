using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.DataBase
{
    /// <summary>
    /// This class describes structure of line in file
    /// </summary>
    internal class Line
    {
        public int Id { get; set; }
        public DateOnly Date {  get; set; }
        public string LatinString { get; set; } = "";
        public string CyrillicString { get; set; } = "";
        public long IntegerValue { get; set; }
        public double DoubleValue { get; set; }
    }
}

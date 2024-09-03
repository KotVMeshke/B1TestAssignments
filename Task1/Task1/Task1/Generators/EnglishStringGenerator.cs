using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Generators
{
    /// <summary>
    /// This class provides generation of string which consist of cyrilic characters
    /// </summary>
    internal class EnglishStringGenerator
    {
        private const string _alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static Random _random = new Random();

        /// <summary>
        /// Generates a random string of 10 English characters.
        /// </summary>
        /// <returns>A string consisting of randomly selected Russian characters.</returns>
        public static string GenerateValue()
        {
            var resultString = new StringBuilder();
            int size = 10;
            for (int i = 0; i < size; i++)
            {
                resultString.Append(_alphabet[_random.Next(_alphabet.Length)]);
            }

            return resultString.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Generators
{
    internal class RussianStringGenerator
    {
        private const string _alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        private static Random _random = new Random();

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

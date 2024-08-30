using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.DataBase;

namespace Task1
{
    internal class FileImporter
    {
        public static void LoadFilesIntoDB(string directory)
        {
            if (!Path.Exists(directory)) throw new ArgumentException($"Directory {directory} doesn't exist");
            var files = Directory.GetFiles(directory);
            using (var dbContext = new ApplicationDBContext())
            {
                foreach (var file in files)
                {
                    ProcessFile(file, dbContext);
                }

                Console.WriteLine("All files were loaded");
            }
        }
        private static void ProcessFile(string file, ApplicationDBContext dbContext)
        {
            var lines = File.ReadAllLines(file);
            var numberOfLines = lines.Length;
            int loadedLines = 0;
            foreach (var line in lines)
            {
                var columns = line.Split("||").ToArray();
                var fileLine = new Line()
                {
                    Date = DateOnly.ParseExact(columns[0], "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    LatinString = columns[1],
                    CyrillicString = columns[2],
                    IntegerValue = long.Parse(columns[3]),
                    DoubleValue = double.Parse(columns[4], CultureInfo.InvariantCulture)
                };

                dbContext.Lines.Add(fileLine);

                loadedLines++;
                if (loadedLines % 1000 == 0)
                {
                    dbContext.SaveChanges();
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"File {Path.GetFileName(file)}: were loaded {loadedLines}/{numberOfLines} lines ");
                }
            }

            dbContext.SaveChanges();
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine($"File {Path.GetFileName(file)} was completely loaded {loadedLines}/{numberOfLines}");
        }

        public static void LoadFileIntoDB(string file)
        {
            if (!Path.Exists(file)) throw new ArgumentException($"File {file} doesn't exist");
            using (var dbContext = new ApplicationDBContext())
            {
                ProcessFile(file, dbContext);
            }
        }
    }
}

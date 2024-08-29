using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    internal class FilesCombiner
    {
        public static void CombineDirectoryFiles(string directory, string outputeFile)
        {

            if (Path.Exists(directory))
            {
                var filePath = Path.Combine(directory, outputeFile) + ".txt";
                var dir = Directory.GetFiles(directory);
                int counter = 0;
                foreach (var file in dir)
                {
                    var text = File.ReadAllText(file);
                    File.AppendAllText(filePath, text);
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"{++counter} files were procceded");
                }
            }
            else
            {
                throw new ArgumentException($"Directory {directory} is not exist");
            }

            Console.WriteLine("Files were combined succesfuly");
        }

        public static void CombineDirectoryFilesWithDeletion(string directory, string outputeFile, string combination)
        {
            int numberOfDeletedLines = 0;

            if (Path.Exists(directory))
            {
                var filePath = Path.Combine(directory, outputeFile);
                var dir = Directory.GetFiles(directory);
                File.Create(outputeFile);
                int counter = 0;
                foreach (var file in dir)
                {

                    var lines = File.ReadAllLines(file);
                    var numberOfLinesBefore = lines.Length;
                    lines = lines.Where(line => !line.Contains(combination)).ToArray();
                    var numberOfLinesAfter = lines.Length;
                    numberOfDeletedLines += (numberOfLinesBefore - numberOfLinesAfter);
                    File.AppendAllLines(filePath, lines);
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"{++counter} files were procceded");
                }

                if (numberOfDeletedLines > 0) Console.WriteLine($"Were deleted {numberOfDeletedLines}");

            }
            else
            {
                throw new ArgumentException($"Directory {directory} is not exist");
            }
        }
    }
}

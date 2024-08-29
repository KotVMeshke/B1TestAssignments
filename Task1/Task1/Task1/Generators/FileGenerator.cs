using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Generators
{
    internal class FileGenerator
    {
        private readonly string _workDirectory;
        private readonly string _newDirectoryName = "GeneratedFiles";
        private readonly string _fullPath;
        public FileGenerator(string workDirectory)
        {
            if (Path.Exists(workDirectory))
            {
                _workDirectory = workDirectory;

            }
            else
            {
                throw new ArgumentException($"Directory {workDirectory} is not exist");
            }
            _fullPath = Path.Combine(_workDirectory, _newDirectoryName);

        }

        public FileGenerator(string workDirectory, string newDirectoryName) : this(workDirectory)
        {
            var newPath = Path.Combine(workDirectory, newDirectoryName);
            if (!Path.Exists(newPath))
            {
                _newDirectoryName = newDirectoryName;
            }
            else
            {
                throw new ArgumentException($"Directory {newDirectoryName} already exists");
            }
        }

        public void GenerateFile(string fileName)
        {

            var sb = new StringBuilder();
            for (int i = 0; i < 100_000; i++)
            {
                sb.AppendLine($"" +
                    $"{DateGenerator.GenerateValue().ToShortDateString()}||" +
                    $"{EnglishStringGenerator.GenerateValue()}||" +
                    $"{RussianStringGenerator.GenerateValue()}||" +
                    $"{IntegerGenerator.GenerateValue()}||" +
                    $"{DoubleGenerator.GenerateValue()}||");
            }

            using var fileStream = new FileStream(Path.Combine(_fullPath, fileName), FileMode.Create, FileAccess.Write);

            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(sb.ToString());
            }
          
        }

        public void GenerateFiles()
        {
            Directory.CreateDirectory(_fullPath);
            var fileBasic = "File";
            for (int i = 0; i < 100; i++)
            {
                var fileName = fileBasic + (i + 1) + ".txt";
                GenerateFile(fileName);
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"{i+1} files were created");
            }
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write($"Were created 100 files succesfuly");
        }

    }
}

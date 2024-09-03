using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Generators
{
    /// <summary>
    /// The FileGenerator class is responsible for generating a set of text files with synthetic data.
    /// Each file contains 100,000 lines of generated data, including dates, English strings, Russian strings, integers, and doubles.
    /// </summary>
    internal class FileGenerator
    {
        private readonly string _workDirectory; // The directory where the generated files will be stored
        private readonly string _newDirectoryName = "GeneratedFiles"; // Default name for the subdirectory to hold the generated files
        private readonly string _fullPath; // The full path to the directory where files will be generated

        /// <summary>
        /// Constructor that initializes the FileGenerator with a working directory.
        /// </summary>
        /// <param name="workDirectory">The path of the directory where the generated files will be stored.</param>
        public FileGenerator(string workDirectory)
        {
            // Check if the provided directory exists
            if (Path.Exists(workDirectory))
            {
                _workDirectory = workDirectory;
            }
            else
            {
                // Throw an exception if the directory does not exist
                throw new ArgumentException($"Directory {workDirectory} does not exist");
            }

            // Set the full path for the directory where files will be generated
            _fullPath = Path.Combine(_workDirectory, _newDirectoryName);
        }

        /// <summary>
        /// Constructor that allows specifying a custom directory name for storing the generated files.
        /// </summary>
        /// <param name="workDirectory">The path of the base directory where the new directory will be created.</param>
        /// <param name="newDirectoryName">The name of the new directory where the generated files will be stored.</param>
        public FileGenerator(string workDirectory, string newDirectoryName) : this(workDirectory)
        {
            var newPath = Path.Combine(workDirectory, newDirectoryName);

            // Check if the new directory already exists
            if (!Path.Exists(newPath))
            {
                _newDirectoryName = newDirectoryName;
            }
            else
            {
                // Throw an exception if the directory already exists
                throw new ArgumentException($"Directory {newDirectoryName} already exists");
            }
        }

        /// <summary>
        /// Generates a single file with synthetic data.
        /// Each file contains 100,000 lines of data separated by "||".
        /// </summary>
        /// <param name="fileName">The name of the file to be generated.</param>
        public void GenerateFile(string fileName)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < 100_000; i++)
            {
                sb.AppendLine(
                    $"{DateGenerator.GenerateValue().ToShortDateString()}||" + 
                    $"{EnglishStringGenerator.GenerateValue()}||" + 
                    $"{RussianStringGenerator.GenerateValue()}||" +
                    $"{IntegerGenerator.GenerateValue()}||" + 
                    $"{DoubleGenerator.GenerateValue()}||" 
                );
            }

            // Create the file in the specified directory and write the generated data to it
            using var fileStream = new FileStream(Path.Combine(_fullPath, fileName), FileMode.Create, FileAccess.Write);
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(sb.ToString());
            }
        }

        /// <summary>
        /// Generates 100 files with synthetic data in the specified directory.
        /// </summary>
        public void GenerateFiles()
        {
            // Create the directory if it does not exist
            Directory.CreateDirectory(_fullPath);
            var fileBasic = "File"; // Basic name for each generated file

            // Generate 100 files
            for (int i = 0; i < 100; i++)
            {
                var fileName = fileBasic + (i + 1) + ".txt"; 
                GenerateFile(fileName); 

                // Update the console to show progress
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"{i + 1} files were created");
            }

            // Inform the user that all files were successfully created
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write("100 files were created successfully");
        }
    }
}

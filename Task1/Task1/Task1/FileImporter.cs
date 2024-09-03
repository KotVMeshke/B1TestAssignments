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
    /// <summary>
    /// The FileImporter class provides methods to load data from text files into a database.
    /// It supports loading data from multiple files in a directory or a single file.
    /// </summary>
    internal class FileImporter
    {
        /// <summary>
        /// Loads data from all files in the specified directory into the database.
        /// </summary>
        /// <param name="directory">The directory containing the files to be imported.</param>
        public static void LoadFilesIntoDB(string directory)
        {
            // Check if the specified directory exists
            if (!Path.Exists(directory))
                throw new ArgumentException($"Directory {directory} doesn't exist");

            // Get all file paths in the directory
            var files = Directory.GetFiles(directory);

            // Create a new instance of the database context
            using (var dbContext = new ApplicationDBContext())
            {
                // Process each file and load its data into the database
                foreach (var file in files)
                {
                    ProcessFile(file, dbContext);
                }

                // Inform the user that all files were successfully loaded
                Console.WriteLine("All files were loaded");
            }
        }

        /// <summary>
        /// Processes a single file, reading its content and saving it into the database.
        /// </summary>
        /// <param name="file">The path of the file to be processed.</param>
        /// <param name="dbContext">The database context used for saving data.</param>
        private static void ProcessFile(string file, ApplicationDBContext dbContext)
        {
            // Read all lines from the file
            var lines = File.ReadAllLines(file);
            var numberOfLines = lines.Length;
            int loadedLines = 0;

            // Iterate over each line in the file
            foreach (var line in lines)
            {
                // Split the line into columns based on the "||" separator
                var columns = line.Split("||").ToArray();

                // Create a new Line object and populate it with data from the columns
                var fileLine = new Line()
                {
                    Date = DateOnly.ParseExact(columns[0], "dd.MM.yyyy", CultureInfo.InvariantCulture), 
                    LatinString = columns[1], 
                    CyrillicString = columns[2], 
                    IntegerValue = long.Parse(columns[3]),
                    DoubleValue = double.Parse(columns[4], CultureInfo.InvariantCulture) 
                };

                // Add the Line object to the database context
                dbContext.Lines.Add(fileLine);

                loadedLines++;

                // Save changes to the database every 1000 lines to improve performance and reduce memory usage
                if (loadedLines % 1000 == 0)
                {
                    dbContext.SaveChanges();
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"File {Path.GetFileName(file)}: were loaded {loadedLines}/{numberOfLines} lines ");
                }
            }

            // Save any remaining changes to the database
            dbContext.SaveChanges();

            // Inform the user that the file was completely loaded
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine($"File {Path.GetFileName(file)} was completely loaded {loadedLines}/{numberOfLines}");
        }

        /// <summary>
        /// Loads data from a single file into the database.
        /// </summary>
        /// <param name="file">The path of the file to be imported.</param>
        public static void LoadFileIntoDB(string file)
        {
            // Check if the specified file exists
            if (!Path.Exists(file))
                throw new ArgumentException($"File {file} doesn't exist");

            using (var dbContext = new ApplicationDBContext())
            {
                // Process the file and load its data into the database
                ProcessFile(file, dbContext);
            }
        }
    }
}

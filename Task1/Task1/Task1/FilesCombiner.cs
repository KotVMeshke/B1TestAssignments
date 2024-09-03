namespace Task1
{
    /// <summary>
    /// The FilesCombiner class provides methods to combine text files from a specified directory into a single output file.
    /// It also includes a method to combine files while optionally deleting lines that contain a specific string.
    /// </summary>
    internal class FilesCombiner
    {
        /// <summary>
        /// Combines all files in the specified directory into a single output file.
        /// </summary>
        /// <param name="directory">The path of the directory containing the files to combine.</param>
        /// <param name="outputeFile">The name of the output file (without extension) to create in the directory.</param>
        public static void CombineDirectoryFiles(string directory, string outputeFile)
        {
            // Check if the specified directory exists
            if (Path.Exists(directory))
            {
                // Combine the directory path and output file name to get the full file path
                var filePath = Path.Combine(directory, outputeFile) + ".txt";
                // Get all file paths in the directory
                var dir = Directory.GetFiles(directory);
                int counter = 0;

                // Iterate over each file in the directory
                foreach (var file in dir)
                {
                    // Read the content of the current file
                    var text = File.ReadAllText(file);
                    // Append the content to the output file
                    File.AppendAllText(filePath, text);
                    // Update the console to show progress
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"{++counter} files were processed");
                }
            }
            else
            {
                // Throw an exception if the directory does not exist
                throw new ArgumentException($"Directory {directory} does not exist");
            }

            // Inform the user that the files were successfully combined
            Console.WriteLine("Files were combined successfully");
        }

        /// <summary>
        /// Combines all files in the specified directory into a single output file,
        /// while deleting lines that contain a specified combination of characters.
        /// </summary>
        /// <param name="directory">The path of the directory containing the files to combine.</param>
        /// <param name="outputeFile">The name of the output file (without extension) to create in the directory.</param>
        /// <param name="combination">The string combination to search for and delete from the files.</param>
        public static void CombineDirectoryFilesWithDeletion(string directory, string outputeFile, string combination)
        {
            int numberOfDeletedLines = 0;

            // Check if the specified directory exists
            if (Path.Exists(directory))
            {
                // Combine the directory path and output file name to get the full file path
                var filePath = Path.Combine(directory, outputeFile) + ".txt";
                // Get all file paths in the directory
                var dir = Directory.GetFiles(directory);
                // Create the output file
                File.Create(outputeFile);
                int counter = 0;

                // Iterate over each file in the directory
                foreach (var file in dir)
                {
                    // Read all lines from the current file
                    var lines = File.ReadAllLines(file);
                    var numberOfLinesBefore = lines.Length;

                    // Filter out lines that contain the specified combination
                    lines = lines.Where(line => !line.Contains(combination)).ToArray();
                    var numberOfLinesAfter = lines.Length;
                    numberOfDeletedLines += (numberOfLinesBefore - numberOfLinesAfter);

                    // Append the filtered lines to the output file
                    File.AppendAllLines(filePath, lines);
                    // Update the console to show progress
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"{++counter} files were processed");
                }

                // Inform the user of the number of lines deleted, if any
                if (numberOfDeletedLines > 0)
                    Console.WriteLine($"Deleted {numberOfDeletedLines} lines");

            }
            else
            {
                // Throw an exception if the directory does not exist
                throw new ArgumentException($"Directory {directory} does not exist");
            }
        }
    }
}

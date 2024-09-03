using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.DataBase;
using Task1.Generators;

namespace Task1
{
    /// <summary>
    /// The ProgramContext class encapsulates the main logic for interacting with the user via a console interface.
    /// It provides a menu for file operations and database interactions, handling user input and executing commands accordingly.
    /// </summary>
    internal class ProgramContext
    {
        /// <summary>
        /// Constructor that sets the input encoding to UTF-16 to support wider character sets in the console input.
        /// </summary>
        public ProgramContext()
        {
            Console.InputEncoding = Encoding.GetEncoding("utf-16");
        }

        /// <summary>
        /// Displays the menu of available options to the user.
        /// </summary>
        public void DisplayMenu()
        {
            Console.WriteLine("Select an option: ");
            Console.WriteLine("1) Create 100 files");
            Console.WriteLine("2) Combine files");
            Console.WriteLine("3) Export files into data base");
            Console.WriteLine("4) Calculate median and sum");
            Console.WriteLine();
        }

        /// <summary>
        /// Handles the user's menu selection, executing the corresponding operations.
        /// </summary>
        /// <returns>Returns true if the menu handling completes successfully, otherwise false.</returns>
        public bool HandleMenu()
        {
            if (!int.TryParse(Console.ReadLine(), out int option))
            {
                Console.WriteLine("Invalid input, try again");
                return false;
            }

            switch (option)
            {
                case 1:
                    // Handle file generation
                    Console.Write("Input path where directory will be created: ");
                    GetUserInput(out string path);
                    Console.Write("Input name of created directory (optional): ");
                    FileGenerator fileGenerator;
                    if (!GetUserInput(out string directoryName, true))
                        fileGenerator = new FileGenerator(path);
                    else
                        fileGenerator = new FileGenerator(path, directoryName);

                    fileGenerator.GenerateFiles();
                    Console.WriteLine();
                    break;

                case 2:
                    // Handle file combination
                    Console.Write("Input directory to proceed: ");
                    GetUserInput(out string directory);
                    Console.Write("Input output file: ");
                    GetUserInput(out string outputFile);
                    Console.Write("Enter combination for deletion (optional): ");

                    if (GetUserInput(out string combination, true))
                    {
                        FilesCombiner.CombineDirectoryFilesWithDeletion(directory, outputFile, combination);
                    }
                    else
                    {
                        FilesCombiner.CombineDirectoryFiles(directory, outputFile);
                    }
                    break;

                case 3:
                    // Handle exporting files to the database
                    Console.Write("Input directory of file: ");
                    GetUserInput(out path);
                    if (Directory.Exists(path))
                        FileImporter.LoadFilesIntoDB(path);
                    else if (File.Exists(path))
                        FileImporter.LoadFileIntoDB(path);
                    break;

                case 4:
                    // Handle calculation of sum and median from the database
                    using (var dbContext = new ApplicationDBContext())
                    {

                        // Setting sql's parameters for stored procedure
                        var sumParam = new SqlParameter
                        {
                            ParameterName = "@sum",
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Output
                        };

                        var medianParam = new SqlParameter
                        {
                            ParameterName = "@median",
                            SqlDbType = System.Data.SqlDbType.Decimal,
                            Precision = 9,
                            Scale = 8,
                            Direction = System.Data.ParameterDirection.Output
                        };

                        dbContext.Database.ExecuteSqlRaw("[Calculate_Sum_Median] @sum OUTPUT, @median OUTPUT", sumParam, medianParam);
                        Console.WriteLine($"Sum: {sumParam.Value}");
                        Console.WriteLine($"Median: {medianParam.Value}");
                    }
                    break;

                default:
                    // Handle invalid option input
                    Console.WriteLine("Incorrect number of options, try again");
                    break;
            }

            return true;
        }

        /// <summary>
        /// Handles user input from the console and ensures it meets the required conditions.
        /// </summary>
        /// <param name="output">The processed output from the user's input.</param>
        /// <param name="canBeEmpty">Specifies whether the input can be empty.</param>
        /// <returns>Returns true if valid input is obtained, otherwise false.</returns>
        private bool GetUserInput(out string output, bool canBeEmpty = false)
        {
            string? input = string.Empty;
            do
            {
                input = Console.ReadLine()!;
                if (canBeEmpty)
                {
                    output = input;

                    // Allow empty input if canBeEmpty is true
                    if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
                    {
                        return false;
                    }
                    return true;
                }
            } while (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input));

            output = input;
            return true;
        }
    }
}

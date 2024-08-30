using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.Generators;

namespace Task1
{
    internal class ProgramContext
    {
        public ProgramContext()
        {
            Console.InputEncoding = System.Text.Encoding.GetEncoding("utf-16");
        }
        public void DisplayMenu()
        {
            Console.WriteLine("Select an option: ");

            Console.WriteLine("1) Create 100 files");
            Console.WriteLine("2) Combine files");
            Console.WriteLine("3) Export files into data base");
            Console.WriteLine("4) Calculate median and sum");


            Console.WriteLine();
        }

        public bool HandleMenu(out int optionNumber)
        {
            if (!int.TryParse(Console.ReadLine(), out int option))
            {
                Console.WriteLine("Invalid input, try again");
                optionNumber = -1;
                return false;
            }
            optionNumber = option;
            switch (option)
            {
                case 1:
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
                    Console.Write("Input directory to procced: ");
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
                    Console.Write("Input directory of file: ");
                    GetUserInput(out path);
                    if (Directory.Exists(path))
                        FileImporter.LoadFilesIntoDB(path);
                    else if (File.Exists(path))
                        FileImporter.LoadFileIntoDB(path);
                    break;
                case 4:
                    break;
                default:
                    optionNumber = -1;
                    Console.WriteLine("Incorrent number of option, try again");
                    break;
            }


            return true;
        }

        private bool GetUserInput(out string output, bool canBeEmpty = false)
        {
            string? input = string.Empty;
            do
            {
                input = Console.ReadLine()!;
                if (canBeEmpty)
                {
                    output = input;

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

        public void DisplayCombineOptions()
        {
            Console.WriteLine("Select a combine option: ");
            Console.WriteLine("1) Combine with deletion");
            Console.WriteLine("2) Combine without deletion");
        }

    }
}

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.Util;
using Task2.Excel.ExcelData;

namespace Task2.Excel
{
    /// <summary>
    /// Provides methods to parse Excel files and convert them into <see cref="ParsedFile"/> objects.
    /// This class is able to parse only .XLS files
    /// </summary>
    public static class ExcelParser
    {
        /// <summary>
        /// Parses an Excel .XLS file from the given stream and returns a <see cref="ParsedFile"/> object containing the extracted data.
        /// </summary>
        /// <param name="file">The stream representing the Excel file to parse.</param>
        /// <returns>Returns a <see cref="ParsedFile"/> object containing the parsed data.</returns>
        public static ParsedFile ParseXLS(Stream file)
        {
            // Create Excel workbook and sheet
            var workBook = new HSSFWorkbook(file);
            var sheet = workBook.GetSheetAt(0);

            // Initialize the ParsedFile object with the bank name
            var parsedFile = new ParsedFile()
            {
                BankName = sheet.GetRow(0).GetCell(0).ToString()
            };

            // Find start point of table
            var startCell = FindCellByValue(sheet, "Б/сч");

            // Check if start point exists
            if (startCell is null) throw new NullReferenceException("File structure is not correct");

            // Loop to parse the bank classes and rows
            while (true)
            {
                // Get a next class in table
                var classCell = GetNext(sheet, startCell, "Класс ");

                //Check if it's the end of the table
                if (classCell!.ToString()!.Contains("Баланс", StringComparison.OrdinalIgnoreCase))
                {
                    // Process last row
                    parsedFile.BankClasses.Last().BankRows.Add(ParseBankRowXLS(classCell.Row, true, false, true));
                    break;

                }

                // Process each class
                parsedFile.BankClasses.Add(ParseBankClassXLS(classCell));
                startCell = classCell;
            }

            return parsedFile;

        }

        /// <summary>
        /// Parses a row in the Excel sheet and returns a <see cref="ParsedBankRow"/> object.
        /// </summary>
        /// <param name="row">The row to parse.</param>
        /// <param name="isSum">Indicates whether the row represents a sum.</param>
        /// <param name="isClassSum">Indicates whether the row represents a class sum.</param>
        /// <param name="isGlobalSum">Indicates whether the row represents a global sum.</param>
        private static ParsedBankRow ParseBankRowXLS(IRow row, bool isSum = false, bool isClassSum = false, bool isGlobalSum = false)
        {
            // Create bank row 
            var parsedBankRow = new ParsedBankRow()
            {
                AccountNumber = isClassSum ? "ПО КЛАССУ" : isGlobalSum ? "БАЛАНС" : row.GetCell(0).ToString()!,
                OpeningBalanceActive = (decimal)row.GetCell(1).NumericCellValue,
                OpeningBalancePassive = (decimal)row.GetCell(2).NumericCellValue,
                TurnoverDebit = (decimal)row.GetCell(3).NumericCellValue,
                TurnoverCredit = (decimal)row.GetCell(4).NumericCellValue,
                ClosingBalanceActive = (decimal)row.GetCell(5).NumericCellValue,
                ClosingBalancePassive = (decimal)row.GetCell(6).NumericCellValue,
                IsSum = isSum
            };

            return parsedBankRow;
        }

        /// <summary>
        /// Parses a bank class from the Excel sheet starting at the given cell and returns a <see cref="ParsedBankClass"/> object.
        /// </summary>
        /// <param name="startCell">The cell where the bank class starts.</param>
        /// <returns>A <see cref="ParsedBankClass"/> object containing the parsed data.</returns>
        private static ParsedBankClass ParseBankClassXLS(ICell startCell)
        {
            // Check if the cell exists
            if (startCell is null) throw new ArgumentNullException("Null argument");

            // Create a new instance of bank class
            var parsedBankClass = new ParsedBankClass()
            {
                ClassName = startCell.ToString()!
            };

            // Loop to parse bank class
            do
            {
                // Get each row in bank class
                startCell = startCell.Sheet.GetRow(startCell.RowIndex + 1).GetCell(startCell.ColumnIndex);
                if (startCell is null || startCell.ToString() is null) break;

                // Check if the row is a sum, end of class or regular row
                if (startCell.CellStyle.GetFont(startCell.Sheet.Workbook).IsBold)
                {
                    if (startCell.ToString()!.Contains("По классу", StringComparison.OrdinalIgnoreCase))
                    {
                        // Create class sum row
                        parsedBankClass.BankRows.Add(ParseBankRowXLS(startCell.Row, true, true));
                        break;
                    }
                    else
                    {
                        // Create sum of each group of rows
                        parsedBankClass.BankRows.Add(ParseBankRowXLS(startCell.Row, true));
                    }
                }
                else
                {
                    // Create regular row
                    parsedBankClass.BankRows.Add(ParseBankRowXLS(startCell.Row));
                }
              
            } while (startCell is not null);

            return parsedBankClass;
        }

        /// <summary>
        /// Finds a cell in the given sheet that contains the specified value.
        /// </summary>
        /// <param name="sheet">The sheet to search in.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>The cell that contains the value, or null if not found.</returns>
        private static ICell? FindCellByValue(ISheet sheet, string value)
        {
            // Loop through all rows in a sheet
            foreach (IRow row in sheet)
            {
                // Loop through all cell in the row
                foreach (var cell in row)
                {
                    // Check if the cell contains value
                    if (cell is not null && cell.ToString()!.Contains(value))
                    {
                        return cell;
                    }
                }
            }


            return null;
        }

        /// <summary>
        /// Gets the next cell in the sheet that contains the specified value, starting from the given cell.
        /// </summary>
        /// <param name="sheet">The sheet to search in.</param>
        /// <param name="cell">The cell to start from.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>The next cell that contains the value, or null if not found.</returns>
        private static ICell? GetNext(ISheet sheet, ICell cell, string value)
        {
            // Check if the cell exist
            if (cell is null) throw new NullReferenceException("Cell doesn't exist");

            // Copy the cell
            var nextCell = cell.Copy();

            // Loop to find next cell with value
            do
            {
                // Get next row in a column
                nextCell = sheet.GetRow(nextCell.RowIndex + 1).GetCell(nextCell.ColumnIndex);
            } while (nextCell is not null && !nextCell.ToString()!.Contains(value, StringComparison.OrdinalIgnoreCase) && !nextCell.ToString()!.Contains("Баланс", StringComparison.OrdinalIgnoreCase));
            return nextCell;
        }
    }
}

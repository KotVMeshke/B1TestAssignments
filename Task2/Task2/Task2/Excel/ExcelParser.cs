using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.Util;
using Task2.Excel.ExcelData;

namespace Task2.Excel
{
    public static class ExcelParser
    {
        public static ParsedFile ParseXLS(Stream file)
        {
            var workBook = new HSSFWorkbook(file);
            var sheet = workBook.GetSheetAt(0);

            var parsedFile = new ParsedFile()
            {
                BankName = sheet.GetRow(0).GetCell(0).ToString()
            };

            var startCell = FindCellByValue(sheet, "Б/сч");

            if (startCell is null) throw new NullReferenceException("File structure is not correct");

            while (true)
            {
                var classCell = GetNext(sheet, startCell, "Класс ");
                if (classCell!.ToString()!.Contains("Баланс", StringComparison.OrdinalIgnoreCase))
                {
                    parsedFile.BankClasses.Last().BankRows.Add(ParseBankRowXLS(classCell.Row, true, false, true));
                    break;

                }
                parsedFile.BankClasses.Add(ParseBankClassXLS(classCell));
                startCell = classCell;
            }

            return parsedFile;

        }

        private static ParsedBankRow ParseBankRowXLS(IRow row, bool isSum = false, bool isClassSum = false, bool isGlobalSum = false)
        {
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
        private static ParsedBankClass ParseBankClassXLS(ICell startCell)
        {
            if (startCell is null) throw new ArgumentNullException("Null argument");

            var parsedBankClass = new ParsedBankClass()
            {
                ClassName = startCell.ToString()!
            };
            do
            {
                startCell = startCell.Sheet.GetRow(startCell.RowIndex + 1).GetCell(startCell.ColumnIndex);
                if (startCell is null || startCell.ToString() is null) break;
                if (startCell.CellStyle.GetFont(startCell.Sheet.Workbook).IsBold)
                {
                    if (startCell.ToString()!.Contains("По классу", StringComparison.OrdinalIgnoreCase))
                    {
                        parsedBankClass.BankRows.Add(ParseBankRowXLS(startCell.Row, true, true));
                        break;
                    }
                    else
                    {
                        parsedBankClass.BankRows.Add(ParseBankRowXLS(startCell.Row, true));
                    }
                }
                else
                {
                    parsedBankClass.BankRows.Add(ParseBankRowXLS(startCell.Row));
                }
              
            } while (startCell is not null);

            return parsedBankClass;
        }

        private static ICell? FindCellByValue(ISheet sheet, string value)
        {
            foreach (IRow row in sheet)
            {
                foreach (var cell in row)
                {
                    if (cell is not null && cell.ToString()!.Contains(value))
                    {
                        return cell;
                    }
                }
            }


            return null;
        }

        private static ICell? GetNext(ISheet sheet, ICell cell, string value)
        {
            if (cell is null) throw new NullReferenceException("Cell doesn't exist");
            var nextCell = cell.Copy();
            do
            {
                nextCell = sheet.GetRow(nextCell.RowIndex + 1).GetCell(nextCell.ColumnIndex);
            } while (nextCell is not null && !nextCell.ToString()!.Contains(value, StringComparison.OrdinalIgnoreCase) && !nextCell.ToString()!.Contains("Баланс", StringComparison.OrdinalIgnoreCase));
            return nextCell;
        }

        private static ICell? FindCellByValueAndColumn(ISheet sheet, string value, int columnIndex)
        {
            foreach (IRow row in sheet)
            {
                foreach (var cell in row)
                {
                    if (cell is null) continue;
                    if (cell.ColumnIndex > columnIndex) continue;
                    if (cell.ColumnIndex == columnIndex && cell.ToString()!.Contains(value))
                    {
                        return cell;
                    }
                }
            }


            return null;
        }
    }
}

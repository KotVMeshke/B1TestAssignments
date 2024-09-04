namespace Task2.Excel.ExcelData
{
    /// <summary>
    /// Represents the data parsed from an Excel file.
    /// </summary>
    public class ParsedFile
    {
        public string? BankName { get; set; } = "";
        public List<ParsedBankClass> BankClasses { get; set; } = new List<ParsedBankClass>();
    }
}

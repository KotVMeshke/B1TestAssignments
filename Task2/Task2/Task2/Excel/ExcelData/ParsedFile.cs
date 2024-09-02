namespace Task2.Excel.ExcelData
{
    public class ParsedFile
    {
        public string? BankName { get; set; } = "";
        public List<ParsedBankClass> BankClasses { get; set; } = new List<ParsedBankClass>();
    }
}

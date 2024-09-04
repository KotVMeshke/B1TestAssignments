namespace Task2.Excel.ExcelData
{
    /// <summary>
    /// Represents the data parsed bank class
    /// </summary>
    public class ParsedBankClass
    {
        public string ClassName { get; set; } = "";
        public List<ParsedBankRow> BankRows {  get; set; } = new List<ParsedBankRow>();

    }
}

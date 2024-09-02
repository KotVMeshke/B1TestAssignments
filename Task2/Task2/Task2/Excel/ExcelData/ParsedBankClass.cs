namespace Task2.Excel.ExcelData
{
    public class ParsedBankClass
    {
        public string ClassName { get; set; } = "";
        public List<ParsedBankRow> BankRows {  get; set; } = new List<ParsedBankRow>();

    }
}

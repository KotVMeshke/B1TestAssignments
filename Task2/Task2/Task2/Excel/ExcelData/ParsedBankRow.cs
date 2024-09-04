namespace Task2.Excel.ExcelData
{
    /// <summary>
    /// Represents the data parsed row
    /// </summary>
    public class ParsedBankRow
    {
        public string AccountNumber { get; set; } = "";
        public decimal OpeningBalanceActive { get; set; }
        public decimal OpeningBalancePassive { get; set; }
        public decimal TurnoverDebit { get; set; }
        public decimal TurnoverCredit { get; set; }
        public decimal ClosingBalanceActive { get; set; }
        public decimal ClosingBalancePassive { get; set; }
        public bool IsSum { get; set; }
    }
}

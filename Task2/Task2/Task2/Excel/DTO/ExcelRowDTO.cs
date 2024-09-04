namespace Task2.Excel.DTO
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a row in bank class.
    /// </summary>
    public class ExcelRowDTO
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

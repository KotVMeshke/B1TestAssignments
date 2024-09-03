namespace Task2.Excel.DTO
{
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

        public void AddRow(ExcelRowDTO row)
        {
            OpeningBalanceActive += row.OpeningBalanceActive;
            OpeningBalancePassive += row.OpeningBalancePassive;
            TurnoverDebit += row.TurnoverDebit;
            TurnoverCredit += row.TurnoverCredit;
            ClosingBalanceActive += row.ClosingBalanceActive;
            ClosingBalancePassive += row.ClosingBalancePassive;
        }
    }
}

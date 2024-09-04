namespace Task2.DataBase.Entity
{
    /// <summary>
    /// Represents a bank row entity.
    /// </summary>
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = "";
        public decimal OpeningBalanceActive { get; set; }
        public decimal OpeningBalancePassive { get; set; }
        public decimal TurnoverDebit { get; set; }
        public decimal TurnoverCredit { get; set; }
        public decimal ClosingBalanceActive { get; set; }
        public decimal ClosingBalancePassive { get; set; }
        public int BankId { get; set; }
        public Bank? Bank { get; set; }
        public int FileId { get; set; }
        public File? File { get; set; }
        public int BankClassId { get; set; }
        public BankClass? BankClass { get; set; }
        public bool IsSum { get; set; }

    }
}

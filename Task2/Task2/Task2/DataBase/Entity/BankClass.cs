namespace Task2.DataBase.Entity
{
    /// <summary>
    /// Represents a bank class entity.
    /// </summary>
    public class BankClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

    }
}

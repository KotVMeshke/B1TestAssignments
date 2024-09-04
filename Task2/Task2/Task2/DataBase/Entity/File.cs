namespace Task2.DataBase.Entity
{
    /// <summary>
    /// Represents a file entity.
    /// </summary>
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

    }
}

namespace Task2.DataBase.Entity
{
    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    }
}

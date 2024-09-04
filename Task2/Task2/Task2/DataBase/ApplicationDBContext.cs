using Microsoft.EntityFrameworkCore;
using Task2.DataBase.Entity;

namespace Task2.DataBase
{
    /// <summary>
    /// Represents the application's database context, which includes the DbSet properties for accessing and managing entities in the database.
    /// </summary>
    public class ApplicationDBContext : DbContext
    {

        public DbSet<Bank> Banks { get; set; } = null!;
        public DbSet<BankAccount> BankAccounts { get; set; } = null!;
        public DbSet<BankClass> BankClasses { get; set; } = null!;
        public DbSet<Entity.File> Files { get; set; } = null!;
        public ApplicationDBContext() { Database.EnsureCreated(); }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { Database.EnsureCreated(); }

        /// <summary>
        /// Configures the model relationships and table mappings for the entities in the database.
        /// </summary>
        /// <param name="modelBuilder">The model builder to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>(b =>
            {
                b.ToTable("bank");

                b.HasKey(b => b.Id);

                b.Property(b => b.Id)
                .HasColumnName("b_id");

                b.Property(b => b.Name)
                .HasColumnName("b_name");

                b.HasMany(b => b.BankAccounts)
                .WithOne(ba => ba.Bank)
                .HasForeignKey(ba => ba.BankId);
            });

            modelBuilder.Entity<BankClass>(b =>
            {
                b.ToTable("bank_class");

                b.HasKey(b => b.Id);

                b.Property(b => b.Id)
                .HasColumnName("bc_id");

                b.Property(b => b.Name)
                .HasColumnName("bc_name");

                b.HasMany(b => b.BankAccounts)
                .WithOne(ba => ba.BankClass)
                .HasForeignKey(ba => ba.BankClassId);
            });

            modelBuilder.Entity<Entity.File>(f =>
            {
                f.ToTable("file");

                f.HasKey(b => b.Id);

                f.Property(b => b.Id)
                .HasColumnName("f_id");

                f.Property(b => b.Name)
                .HasColumnName("f_name");

                f.HasMany(b => b.BankAccounts)
                .WithOne(ba => ba.File)
                .HasForeignKey(ba => ba.FileId);

                f.HasIndex(f => f.Name)
                .IsUnique();
            });

            modelBuilder.Entity<BankAccount>(b =>
            {
                b.ToTable("back_account");
                b.HasKey(e => e.Id);

                b.Property(e => e.Id)
                .HasColumnName("bc_id");

                b.Property(e => e.AccountNumber)
                    .HasColumnName("bc_account_number");

                b.Property(e => e.OpeningBalanceActive)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("bc_opening_balance_active");

                b.Property(e => e.OpeningBalancePassive)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("bc_opening_balance_passive");

                b.Property(e => e.TurnoverDebit)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("bc_turnover_debit");


                b.Property(e => e.TurnoverCredit)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("bc_turnover_credit");

                b.Property(e => e.ClosingBalanceActive)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("bc_closing_balannce_active");

                b.Property(e => e.ClosingBalancePassive)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("bc_closing_balannce_passive");

                b.Property(e => e.IsSum)
                    .HasColumnName("bc_is_sum");

                b.HasOne(e => e.Bank)
                    .WithMany(b => b.BankAccounts) 
                    .HasForeignKey(e => e.BankId);

                b.HasOne(e => e.File)
                    .WithMany(f => f.BankAccounts) 
                    .HasForeignKey(e => e.FileId);

                b.HasOne(e => e.BankClass)
                    .WithMany(bc => bc.BankAccounts) 
                    .HasForeignKey(e => e.BankClassId);
            });
        }
    }
}

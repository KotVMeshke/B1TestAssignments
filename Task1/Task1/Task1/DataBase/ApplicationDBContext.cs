using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.DataBase
{
    using Microsoft.EntityFrameworkCore;

    internal class ApplicationDBContext : DbContext
    {
        /// <summary>
        /// DbSet representing the Lines table in the database.
        /// </summary>
        public DbSet<Line> Lines { get; set; } = null!;

        /// <summary>
        /// Constructor that ensures the database is created if it doesn't exist.
        /// </summary>
        public ApplicationDBContext()
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Configures the database connection string and other options.
        /// </summary>
        /// <param name="optionsBuilder">The options builder used to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Specifies the SQL Server database to connect to
            optionsBuilder.UseSqlServer("Server=localhost; Database=test_task1;Trusted_Connection=true;Encrypt=False");
        }

        /// <summary>
        /// Configures the entity model and maps the Line class to the corresponding database table.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Line>().ToTable("tbl_line");

            modelBuilder.Entity<Line>(l =>
            {
                l.HasKey(p => p.Id);

                l.Property(p => p.Id)
                 .HasColumnName("l_id");

                l.Property(p => p.Date)
                 .HasColumnType("Date")
                 .HasColumnName("l_date");

                l.Property(p => p.LatinString)
                 .HasMaxLength(10)
                 .HasColumnName("l_latin_string");

                l.Property(p => p.CyrillicString)
                 .HasMaxLength(10)
                 .HasColumnName("l_cyrillic_string");

                l.Property(p => p.IntegerValue)
                 .HasColumnName("l_integer_value");

                l.Property(p => p.DoubleValue)
                 .HasColumnName("l_double_value");
            });
        }
    }

}

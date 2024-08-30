using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.DataBase
{
    internal class ApplicationDBContext : DbContext
    {
        public DbSet<Line> Lines { get; set; } = null!;

        public ApplicationDBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Database=test_task1;Trusted_Connection=true;Encrypt=False");
        }

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

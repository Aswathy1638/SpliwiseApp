using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpliwiseApp.Models;

namespace SpliwiseApp.Data
{
    public class SplitContext : IdentityDbContext

    {
        public SplitContext(DbContextOptions<SplitContext> options) : base (options)
            { 
        }

        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Participants> Participants { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            // Group-Expense Relationship (One-to-Many)
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Group)
                .WithMany(g => g.Expenses)
                .HasForeignKey(e => e.GroupId);

            // Expense-Transaction Relationship (One-to-Many)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Expense)
                .WithMany(e => e.Transactions)
                .HasForeignKey(t => t.expenseId);

            // User-Balance Relationship (One-to-Many)
            modelBuilder.Entity<Balance>()
                .HasOne(b => b.user)
                .WithMany()
                .HasForeignKey(b => b.userId);
        }

    }
}

using Microsoft.EntityFrameworkCore;
using SpliwiseApp.Models;

namespace SpliwiseApp.Data
{
    public class SplitContext : DbContext

    {
        public SplitContext(DbContextOptions<SplitContext> options) : base (options)
            { 
        }
       
        public DbSet<User> Users { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Participants> Participants { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<User>()
                .HasMany(ug => ug.Groups)
                .WithMany(g => g.Users);

            modelBuilder.Entity<Expense>()
                    .HasOne(e => e.Group)
                    .WithMany(g => g.Expenses)
                    .HasForeignKey(e => e.GroupId);

            modelBuilder.Entity<Expense>()
                     .HasMany(e => e.Participants)
                      .WithMany(u => u.Expenses);

            modelBuilder.Entity<Transaction>()
                     .HasOne(t => t.Expense)
                    .WithMany(e => e.Transactions)
                    .HasForeignKey(t => t.expenseId);

            modelBuilder.Entity<Transaction>()
                    .HasMany(t => t.User)
                    .WithMany(u => u.Transactions);

            modelBuilder.Entity<Balance>()
                        .HasOne(b => b.user)
                        .WithMany(u => u.Balances)
                        .HasForeignKey(b => b.userId);



            base.OnModelCreating(modelBuilder);
        }

    }
}

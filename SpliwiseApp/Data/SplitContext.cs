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

        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Participants> Participants { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Group)
                .WithMany(g => g.Expenses)
                .HasForeignKey(e => e.GroupId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Expense)
                .WithMany(e => e.Transactions)
                .HasForeignKey(t => t.expenseId);

            
            modelBuilder.Entity<Balance>()
                .HasOne(b => b.user)
                .WithMany()
                .HasForeignKey(b => b.userId);

            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany()
                .HasForeignKey(ug => ug.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany()
                .HasForeignKey(ug => ug.GroupId);
        }
    }

    
}

using Microsoft.EntityFrameworkCore;

namespace Homework16;

public class TransactionsDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=transaction;Username=postgres;Password=11Serg11");
    }
}

using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using Homework16;
using Microsoft.EntityFrameworkCore;

static async Task Main()
{
    const string filePath = "transactions_10_thousand.csv";

    await using var dbContext = new TransactionsDbContext();
    await dbContext.Database.MigrateAsync(); // Применение миграций

    var transactions = await LoadTransactionsAsync(filePath);

    await SaveTransactionsToDatabaseAsync(dbContext, transactions);

    var userSummary = AnalyzeUserTransactions(transactions);
    var topCategories = GetTopCategories(transactions);
    var highestSpender = GetHighestSpender(transactions);

    await File.WriteAllTextAsync("report.json", JsonSerializer.Serialize(new
    {
        users_summary = userSummary,
        top_categories = topCategories,
        highest_spender = highestSpender
    }, new JsonSerializerOptions { WriteIndented = true }));

    Console.WriteLine("Report generated: report.json");
}

static UserExpense? GetHighestSpender(List<Transaction> transactions)
{
    return transactions.GroupBy(t => t.UserId)
        .Select(g => new UserExpense { UserId = g.Key, TotalExpense = g.Sum(t => Math.Min(t.Amount, 0)) })
        .OrderBy(x => x.TotalExpense)
        .FirstOrDefault();
}

static object GetTopCategories(List<Transaction> transactions)
{
    return transactions.GroupBy(t => t.Category)
        .OrderByDescending(g => g.Count())
        .Take(3)
        .Select(g => new CategorySummary { Category = g.Key, TransactionsCount = g.Count() })
        .ToList();
}

static object AnalyzeUserTransactions(List<Transaction> transactions)
{
    return transactions.GroupBy(t => t.UserId)
        .Select(g => new UserSummary
        {
            UserId = g.Key,
            TotalIncome = g.Sum(t => Math.Max(t.Amount, 0)),
            TotalExpense = g.Sum(t => Math.Min(t.Amount, 0))
        })
        .ToList();
}

static async Task SaveTransactionsToDatabaseAsync(TransactionsDbContext dbContext, List<Transaction> transactions)
{
    await dbContext.AddRangeAsync(transactions);
    await dbContext.SaveChangesAsync();
}

static async Task<List<Transaction>> LoadTransactionsAsync(string filePath)
{
    var transactions = new List<Transaction>();

    using var reader = new StreamReader(filePath);
    using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true });

    await foreach (var record in csv.GetRecordsAsync<Transaction>()) transactions.Add(record);

    return transactions;
}

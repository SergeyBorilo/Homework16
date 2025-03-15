namespace Homework16;

public record CategorySummary
{
    public required string Category { get; set; }
    public int TransactionsCount { get; set; }
}

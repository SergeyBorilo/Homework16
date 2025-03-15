namespace Homework16;

public record Transaction
{
    public Guid TransactionId { get; init; }

    public Guid UserId { get; init; }

    public DateTime Date { get; init; }

    public decimal Amount { get; init; }

    public required string Category { get; init; }

    public required string Description { get; init; }

    public required string Merchant { get; init; }
}

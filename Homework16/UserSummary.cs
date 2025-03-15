namespace Homework16;

public record UserSummary
{
    public Guid UserId { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
}

namespace Homework16;

public record UserExpense
{
    public Guid UserId { get; set; }
    public decimal TotalExpense { get; set; }
}

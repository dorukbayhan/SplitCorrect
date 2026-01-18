using SplitCorrect.Domain.Common;

namespace SplitCorrect.Domain.Entities;

public class Expense : BaseEntity
{
    private readonly List<ExpenseSplit> _splits = new();

    public string Description { get; private set; } = string.Empty;
    public Money Amount { get; private set; } = null!;
    public Member PaidBy { get; private set; } = null!;
    public Guid PaidById { get; private set; }
    public DateTime ExpenseDate { get; private set; }
    public Guid GroupId { get; private set; }

    public IReadOnlyCollection<ExpenseSplit> Splits => _splits.AsReadOnly();

    private Expense() { } // EF Core

    public Expense(
        string description,
        Money amount,
        Member paidBy,
        Guid groupId,
        DateTime? expenseDate = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        if (amount.Amount <= 0)
            throw new ArgumentException("Amount must be positive", nameof(amount));

        Description = description;
        Amount = amount;
        PaidBy = paidBy ?? throw new ArgumentNullException(nameof(paidBy));
        PaidById = paidBy.Id;
        GroupId = groupId;
        ExpenseDate = expenseDate ?? DateTime.UtcNow;
    }

    public void SplitEqually(List<Member> members)
    {
        if (members == null || members.Count == 0)
            throw new ArgumentException("Must have at least one member to split", nameof(members));

        _splits.Clear();
        var splitAmount = Amount / members.Count;

        foreach (var member in members)
        {
            _splits.Add(new ExpenseSplit(member, splitAmount));
        }

        UpdatedAt = DateTime.UtcNow;
    }
}

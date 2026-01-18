using SplitCorrect.Domain.Common;

namespace SplitCorrect.Domain.Entities;

public class Group : BaseEntity
{
    private readonly List<Member> _members = new();
    private readonly List<Expense> _expenses = new();

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string Currency { get; private set; }

    public IReadOnlyCollection<Member> Members => _members.AsReadOnly();
    public IReadOnlyCollection<Expense> Expenses => _expenses.AsReadOnly();

    private Group() { } // EF Core

    public Group(string name, string currency = "USD", string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Group name cannot be empty", nameof(name));

        Name = name;
        Currency = currency.ToUpperInvariant();
        Description = description;
    }

    public void AddMember(Member member)
    {
        if (_members.Any(m => m.Id == member.Id))
            throw new InvalidOperationException("Member already exists in group");

        _members.Add(member);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveMember(Guid memberId)
    {
        var member = _members.FirstOrDefault(m => m.Id == memberId);
        if (member == null)
            throw new InvalidOperationException("Member not found in group");

        if (_expenses.Any(e => e.PaidBy.Id == memberId || e.Splits.Any(s => s.Member.Id == memberId)))
            throw new InvalidOperationException("Cannot remove member with associated expenses");

        _members.Remove(member);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddExpense(Expense expense)
    {
        if (!_members.Any(m => m.Id == expense.PaidBy.Id))
            throw new InvalidOperationException("Payer must be a member of the group");

        if (expense.Splits.Any(s => !_members.Any(m => m.Id == s.Member.Id)))
            throw new InvalidOperationException("All split participants must be members of the group");

        _expenses.Add(expense);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Group name cannot be empty", nameof(name));

        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}

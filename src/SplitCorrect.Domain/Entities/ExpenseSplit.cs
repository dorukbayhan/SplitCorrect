using SplitCorrect.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCorrect.Domain.Entities;

public class ExpenseSplit : BaseEntity
{
    public Member Member { get; private set; } = null!;
    
    public decimal SplitAmount { get; private set; }
    public string SplitCurrency { get; private set; } = string.Empty;

    [NotMapped]
    public Money Amount => new Money(SplitAmount, SplitCurrency);
    
    public Guid ExpenseId { get; private set; }
    public Guid MemberId { get; private set; }

    private ExpenseSplit() { } // EF Core

    public ExpenseSplit(Member member, Money amount)
    {
        Member = member ?? throw new ArgumentNullException(nameof(member));
        MemberId = member.Id;
        
        if (amount == null)
            throw new ArgumentNullException(nameof(amount));
        
        if (amount.Amount <= 0)
            throw new ArgumentException("Split amount must be positive", nameof(amount));
            
        SplitAmount = amount.Amount;
        SplitCurrency = amount.Currency;
    }
}

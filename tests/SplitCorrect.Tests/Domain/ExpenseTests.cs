using SplitCorrect.Domain.Common;
using SplitCorrect.Domain.Entities;

namespace SplitCorrect.Tests.Domain;

public class ExpenseTests
{
    [Fact]
    public void Expense_Create_ValidData_Success()
    {
        var member = new Member("John", "john@test.com");
        var amount = new Money(100, "USD");
        var groupId = Guid.NewGuid();

        var expense = new Expense("Dinner", amount, member, groupId);

        Assert.Equal("Dinner", expense.Description);
        Assert.Equal(100, expense.Amount.Amount);
        Assert.Equal(member.Id, expense.PaidBy.Id);
    }

    [Fact]
    public void Expense_SplitEqually_ThreeMembers_Success()
    {
        var payer = new Member("John", "john@test.com");
        var member2 = new Member("Jane", "jane@test.com");
        var member3 = new Member("Bob", "bob@test.com");
        var amount = new Money(90, "USD");
        var groupId = Guid.NewGuid();

        var expense = new Expense("Dinner", amount, payer, groupId);
        expense.SplitEqually(new List<Member> { payer, member2, member3 });

        Assert.Equal(3, expense.Splits.Count);
        Assert.All(expense.Splits, split => Assert.Equal(30, split.Amount.Amount));
    }

    [Fact]
    public void Expense_SplitEqually_TwoMembers_HandlesRounding()
    {
        var payer = new Member("John", "john@test.com");
        var member2 = new Member("Jane", "jane@test.com");
        var amount = new Money(100, "USD");
        var groupId = Guid.NewGuid();

        var expense = new Expense("Dinner", amount, payer, groupId);
        expense.SplitEqually(new List<Member> { payer, member2 });

        Assert.Equal(2, expense.Splits.Count);
        Assert.All(expense.Splits, split => Assert.Equal(50, split.Amount.Amount));
    }

    [Fact]
    public void Expense_Create_EmptyDescription_ThrowsException()
    {
        var member = new Member("John", "john@test.com");
        var amount = new Money(100, "USD");
        var groupId = Guid.NewGuid();

        Assert.Throws<ArgumentException>(() => 
            new Expense("", amount, member, groupId));
    }

    [Fact]
    public void Expense_Create_ZeroAmount_ThrowsException()
    {
        var member = new Member("John", "john@test.com");
        var amount = new Money(0, "USD");
        var groupId = Guid.NewGuid();

        Assert.Throws<ArgumentException>(() => 
            new Expense("Dinner", amount, member, groupId));
    }
}

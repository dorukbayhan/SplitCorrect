using SplitCorrect.Domain.Common;
using SplitCorrect.Domain.Entities;
using SplitCorrect.Domain.Services;

namespace SplitCorrect.Tests.Domain;

public class BalanceCalculatorTests
{
    private readonly BalanceCalculator _calculator = new();

    [Fact]
    public void CalculateBalances_SingleExpense_EqualSplit_CorrectBalances()
    {
        // Arrange: John pays $90 for dinner, split equally among John, Jane, Bob
        var john = new Member("John", "john@test.com");
        var jane = new Member("Jane", "jane@test.com");
        var bob = new Member("Bob", "bob@test.com");
        var groupId = Guid.NewGuid();

        var expense = new Expense("Dinner", new Money(90, "USD"), john, groupId);
        expense.SplitEqually(new List<Member> { john, jane, bob });

        // Act
        var balances = _calculator.CalculateBalances(new List<Expense> { expense });

        // Assert
        Assert.Equal(3, balances.Count);
        
        var johnBalance = balances.First(b => b.Member.Id == john.Id);
        var janeBalance = balances.First(b => b.Member.Id == jane.Id);
        var bobBalance = balances.First(b => b.Member.Id == bob.Id);

        Assert.Equal(60, johnBalance.Amount.Amount); // Paid 90, owes 30
        Assert.Equal(-30, janeBalance.Amount.Amount); // Owes 30
        Assert.Equal(-30, bobBalance.Amount.Amount); // Owes 30
    }

    [Fact]
    public void CalculateBalances_MultipleExpenses_CorrectBalances()
    {
        // Arrange
        var john = new Member("John", "john@test.com");
        var jane = new Member("Jane", "jane@test.com");
        var groupId = Guid.NewGuid();

        // John pays $100, split equally
        var expense1 = new Expense("Dinner", new Money(100, "USD"), john, groupId);
        expense1.SplitEqually(new List<Member> { john, jane });

        // Jane pays $60, split equally
        var expense2 = new Expense("Lunch", new Money(60, "USD"), jane, groupId);
        expense2.SplitEqually(new List<Member> { john, jane });

        // Act
        var balances = _calculator.CalculateBalances(new List<Expense> { expense1, expense2 });

        // Assert
        var johnBalance = balances.First(b => b.Member.Id == john.Id);
        var janeBalance = balances.First(b => b.Member.Id == jane.Id);

        // John: paid 100, owes 50 (from expense1) + paid 0, owes 30 (from expense2) = +50 -30 = +20
        Assert.Equal(20, johnBalance.Amount.Amount);
        // Jane: paid 0, owes 50 (from expense1) + paid 60, owes 30 (from expense2) = -50 +30 = -20
        Assert.Equal(-20, janeBalance.Amount.Amount);
    }

    [Fact]
    public void SuggestSettlements_SimpleCase_OneSettlement()
    {
        // Arrange
        var john = new Member("John", "john@test.com");
        var jane = new Member("Jane", "jane@test.com");

        var balances = new List<Balance>
        {
            new(john, new Money(50, "USD")),
            new(jane, new Money(-50, "USD"))
        };

        // Act
        var settlements = _calculator.SuggestSettlements(balances);

        // Assert
        Assert.Single(settlements);
        Assert.Equal(jane.Id, settlements[0].From.Id);
        Assert.Equal(john.Id, settlements[0].To.Id);
        Assert.Equal(50, settlements[0].Amount.Amount);
    }

    [Fact]
    public void SuggestSettlements_ThreePeople_MinimalTransactions()
    {
        // Arrange
        var alice = new Member("Alice", "alice@test.com");
        var bob = new Member("Bob", "bob@test.com");
        var charlie = new Member("Charlie", "charlie@test.com");
        var groupId = Guid.NewGuid();

        // Alice pays $120 for all three
        var expense1 = new Expense("Dinner", new Money(120, "USD"), alice, groupId);
        expense1.SplitEqually(new List<Member> { alice, bob, charlie });

        // Bob pays $60 for all three
        var expense2 = new Expense("Lunch", new Money(60, "USD"), bob, groupId);
        expense2.SplitEqually(new List<Member> { alice, bob, charlie });

        // Calculate balances
        var balances = _calculator.CalculateBalances(new List<Expense> { expense1, expense2 });

        // Act
        var settlements = _calculator.SuggestSettlements(balances);

        // Assert
        // Alice: paid 120, owes 40 = +80
        // Bob: paid 60, owes 60 = 0
        // Charlie: paid 0, owes 60 = -60
        // Settlements: Charlie pays Alice $60
        Assert.Single(settlements);
        Assert.Equal(charlie.Id, settlements[0].From.Id);
        Assert.Equal(alice.Id, settlements[0].To.Id);
        Assert.Equal(60, settlements[0].Amount.Amount);
    }

    [Fact]
    public void SuggestSettlements_ComplexCase_MinimalTransactions()
    {
        // Arrange
        var alice = new Member("Alice", "alice@test.com");
        var bob = new Member("Bob", "bob@test.com");
        var charlie = new Member("Charlie", "charlie@test.com");
        var david = new Member("David", "david@test.com");

        var balances = new List<Balance>
        {
            new(alice, new Money(100, "USD")),   // Alice is owed $100
            new(bob, new Money(50, "USD")),      // Bob is owed $50
            new(charlie, new Money(-75, "USD")), // Charlie owes $75
            new(david, new Money(-75, "USD"))    // David owes $75
        };

        // Act
        var settlements = _calculator.SuggestSettlements(balances);

        // Assert: Should minimize transactions
        Assert.True(settlements.Count <= 3); // Maximum n-1 transactions for n people
        
        // Verify total settlements
        var totalSettled = settlements.Sum(s => s.Amount.Amount);
        Assert.Equal(150, totalSettled); // Total debt = $150
    }

    [Fact]
    public void CalculateBalances_EmptyList_ReturnsEmpty()
    {
        var balances = _calculator.CalculateBalances(new List<Expense>());
        Assert.Empty(balances);
    }

    [Fact]
    public void SuggestSettlements_BalancedGroup_NoSettlements()
    {
        var alice = new Member("Alice", "alice@test.com");
        
        var balances = new List<Balance>
        {
            new(alice, new Money(0, "USD"))
        };

        var settlements = _calculator.SuggestSettlements(balances);
        Assert.Empty(settlements);
    }
}

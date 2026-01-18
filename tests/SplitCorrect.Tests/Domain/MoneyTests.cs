using SplitCorrect.Domain.Common;

namespace SplitCorrect.Tests.Domain;

public class MoneyTests
{
    [Fact]
    public void Money_CreateWithValidAmount_Success()
    {
        var money = new Money(100.50m, "USD");

        Assert.Equal(100.50m, money.Amount);
        Assert.Equal("USD", money.Currency);
    }

    [Fact]
    public void Money_CreateWithNegativeAmount_Success()
    {
        // Negative amounts are allowed (for debts/balances)
        var money = new Money(-10, "USD");
        Assert.Equal(-10, money.Amount);
    }

    [Fact]
    public void Money_Add_SameCurrency_Success()
    {
        var money1 = new Money(100, "USD");
        var money2 = new Money(50, "USD");

        var result = money1 + money2;

        Assert.Equal(150, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void Money_Add_DifferentCurrencies_ThrowsException()
    {
        var money1 = new Money(100, "USD");
        var money2 = new Money(50, "EUR");

        Assert.Throws<InvalidOperationException>(() => money1 + money2);
    }

    [Fact]
    public void Money_Subtract_SameCurrency_Success()
    {
        var money1 = new Money(100, "USD");
        var money2 = new Money(30, "USD");

        var result = money1 - money2;

        Assert.Equal(70, result.Amount);
    }

    [Fact]
    public void Money_Divide_ValidDivisor_Success()
    {
        var money = new Money(100, "USD");

        var result = money / 4;

        Assert.Equal(25, result.Amount);
    }

    [Fact]
    public void Money_Divide_ByZero_ThrowsException()
    {
        var money = new Money(100, "USD");

        Assert.Throws<DivideByZeroException>(() => money / 0);
    }

    [Fact]
    public void Money_Multiply_Success()
    {
        var money = new Money(50, "USD");

        var result = money * 2.5m;

        Assert.Equal(125, result.Amount);
    }

    [Fact]
    public void Money_RoundsToTwoDecimals()
    {
        var money = new Money(10.12345m, "USD");

        Assert.Equal(10.12m, money.Amount);
    }
}

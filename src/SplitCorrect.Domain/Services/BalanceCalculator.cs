using SplitCorrect.Domain.Common;
using SplitCorrect.Domain.Entities;

namespace SplitCorrect.Domain.Services;

public record Balance(Member Member, Money Amount);

public record Settlement(Member From, Member To, Money Amount);

public interface IBalanceCalculator
{
    List<Balance> CalculateBalances(List<Expense> expenses);
    List<Settlement> SuggestSettlements(List<Balance> balances);
}

public class BalanceCalculator : IBalanceCalculator
{
    public List<Balance> CalculateBalances(List<Expense> expenses)
    {
        if (expenses == null || expenses.Count == 0)
            return new List<Balance>();

        var currency = expenses.First().Amount.Currency;
        var memberBalances = new Dictionary<Guid, (Member Member, decimal Amount)>();

        foreach (var expense in expenses)
        {
            // Credit the payer
            AddOrUpdateBalance(memberBalances, expense.PaidBy, expense.Amount.Amount);

            // Debit each participant
            foreach (var split in expense.Splits)
            {
                AddOrUpdateBalance(memberBalances, split.Member, -split.Amount.Amount);
            }
        }

        return memberBalances.Values
            .Select(kvp => new Balance(kvp.Member, new Money(kvp.Amount, currency)))
            .OrderByDescending(b => b.Amount.Amount)
            .ToList();
    }

    public List<Settlement> SuggestSettlements(List<Balance> balances)
    {
        if (balances == null || balances.Count == 0)
            return new List<Settlement>();

        var settlements = new List<Settlement>();
        var currency = balances.First().Amount.Currency;

        // Separate debtors and creditors
        var debtors = balances
            .Where(b => b.Amount.Amount < 0)
            .OrderBy(b => b.Amount.Amount)
            .ToList();

        var creditors = balances
            .Where(b => b.Amount.Amount > 0)
            .OrderByDescending(b => b.Amount.Amount)
            .ToList();

        int debtorIndex = 0;
        int creditorIndex = 0;

        while (debtorIndex < debtors.Count && creditorIndex < creditors.Count)
        {
            var debtor = debtors[debtorIndex];
            var creditor = creditors[creditorIndex];

            var debtAmount = Math.Abs(debtor.Amount.Amount);
            var creditAmount = creditor.Amount.Amount;

            var settlementAmount = Math.Min(debtAmount, creditAmount);

            if (settlementAmount > 0.01m) // Ignore negligible amounts
            {
                settlements.Add(new Settlement(
                    debtor.Member,
                    creditor.Member,
                    new Money(settlementAmount, currency)));
            }

            // Update balances
            if (Math.Abs(debtAmount - creditAmount) < 0.01m)
            {
                // Both settled
                debtorIndex++;
                creditorIndex++;
            }
            else if (debtAmount > creditAmount)
            {
                // Creditor settled, debtor still owes
                debtors[debtorIndex] = debtor with
                {
                    Amount = new Money(debtor.Amount.Amount + settlementAmount, currency)
                };
                creditorIndex++;
            }
            else
            {
                // Debtor settled, creditor still owed
                creditors[creditorIndex] = creditor with
                {
                    Amount = new Money(creditor.Amount.Amount - settlementAmount, currency)
                };
                debtorIndex++;
            }
        }

        return settlements;
    }

    private void AddOrUpdateBalance(
        Dictionary<Guid, (Member Member, decimal Amount)> balances,
        Member member,
        decimal amount)
    {
        if (balances.ContainsKey(member.Id))
        {
            var current = balances[member.Id];
            balances[member.Id] = (member, current.Amount + amount);
        }
        else
        {
            balances[member.Id] = (member, amount);
        }
    }
}

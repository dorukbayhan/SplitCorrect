using SplitCorrect.Application.DTOs;
using SplitCorrect.Application.Interfaces;
using SplitCorrect.Domain.Common;
using SplitCorrect.Domain.Entities;
using SplitCorrect.Domain.Services;

namespace SplitCorrect.Application.Services;

public class ExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IBalanceCalculator _balanceCalculator;

    public ExpenseService(
        IExpenseRepository expenseRepository,
        IMemberRepository memberRepository,
        IGroupRepository groupRepository,
        IBalanceCalculator balanceCalculator)
    {
        _expenseRepository = expenseRepository;
        _memberRepository = memberRepository;
        _groupRepository = groupRepository;
        _balanceCalculator = balanceCalculator;
    }

    public async Task<ExpenseDto?> CreateExpenseAsync(
        CreateExpenseDto dto,
        CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.GetByIdAsync(dto.GroupId, cancellationToken);
        if (group == null) return null;

        var payer = await _memberRepository.GetByIdAsync(dto.PaidByMemberId, cancellationToken);
        if (payer == null) return null;

        // If no specific members specified, split among all group members
        var splitMemberIds = dto.SplitBetweenMemberIds ?? group.Members.Select(m => m.Id).ToList();
        
        var splitMembers = new List<Member>();
        foreach (var memberId in splitMemberIds)
        {
            var member = await _memberRepository.GetByIdAsync(memberId, cancellationToken);
            if (member == null) return null;
            splitMembers.Add(member);
        }

        var amount = new Money(dto.TotalAmount, dto.Currency);
        var expense = new Expense(dto.Description, amount, payer, dto.GroupId, dto.ExpenseDate);
        expense.SplitEqually(splitMembers);

        // Just add the expense to repository, no need to update group
        var created = await _expenseRepository.AddAsync(expense, cancellationToken);
        return MapToExpenseDto(created);
    }

    public async Task<ExpenseDto?> GetExpenseByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var expense = await _expenseRepository.GetByIdAsync(id, cancellationToken);
        return expense == null ? null : MapToExpenseDto(expense);
    }

    public async Task<List<ExpenseDto>> GetExpensesByGroupIdAsync(
        Guid groupId,
        CancellationToken cancellationToken = default)
    {
        var expenses = await _expenseRepository.GetByGroupIdAsync(groupId, cancellationToken);
        return expenses.Select(MapToExpenseDto).ToList();
    }

    public async Task<bool> DeleteExpenseAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var expense = await _expenseRepository.GetByIdAsync(id, cancellationToken);
        if (expense == null) return false;

        await _expenseRepository.DeleteAsync(id, cancellationToken);
        return true;
    }

    public async Task<GroupDetailDto?> GetGroupDetailsWithBalancesAsync(
        Guid groupId,
        CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.GetByIdAsync(groupId, cancellationToken);
        if (group == null) return null;

        var expenses = await _expenseRepository.GetByGroupIdAsync(groupId, cancellationToken);
        var balances = _balanceCalculator.CalculateBalances(expenses);
        var settlements = _balanceCalculator.SuggestSettlements(balances);

        return new GroupDetailDto(
            group.Id,
            group.Name,
            group.Currency,
            group.Description,
            group.CreatedAt,
            group.Members.Select(m => new MemberDto(m.Id, m.Name, m.Email, m.CreatedAt)).ToList(),
            expenses.Select(MapToExpenseDto).ToList(),
            balances.Select(b => new BalanceDto(
                b.Member.Id,
                b.Member.Name,
                b.Amount.Amount,
                b.Amount.Currency)).ToList(),
            settlements.Select(s => new SettlementDto(
                s.From.Id,
                s.From.Name,
                s.To.Id,
                s.To.Name,
                s.Amount.Amount,
                s.Amount.Currency)).ToList());
    }

    private ExpenseDto MapToExpenseDto(Expense expense)
    {
        return new ExpenseDto(
            expense.Id,
            expense.Description,
            expense.Amount.Amount,
            expense.Amount.Currency,
            new MemberDto(expense.PaidBy.Id, expense.PaidBy.Name, expense.PaidBy.Email, expense.PaidBy.CreatedAt),
            expense.ExpenseDate,
            expense.Splits.Select(s => new ExpenseSplitDto(
                s.Member.Id,
                s.Member.Name,
                s.Amount.Amount,
                s.Amount.Currency)).ToList());
    }
}

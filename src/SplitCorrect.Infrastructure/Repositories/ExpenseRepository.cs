using Microsoft.EntityFrameworkCore;
using SplitCorrect.Application.Interfaces;
using SplitCorrect.Domain.Entities;
using SplitCorrect.Infrastructure.Persistence;

namespace SplitCorrect.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly SplitCorrectDbContext _context;

    public ExpenseRepository(SplitCorrectDbContext context)
    {
        _context = context;
    }

    public async Task<Expense?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Expenses
            .Include(e => e.PaidBy)
            .Include(e => e.Splits)
                .ThenInclude(s => s.Member)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<List<Expense>> GetByGroupIdAsync(
        Guid groupId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Expenses
            .Include(e => e.PaidBy)
            .Include(e => e.Splits)
                .ThenInclude(s => s.Member)
            .Where(e => e.GroupId == groupId)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Expense> AddAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        await _context.Expenses.AddAsync(expense, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return expense;
    }

    public async Task UpdateAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        _context.Expenses.Update(expense);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var expense = await _context.Expenses.FindAsync(new object[] { id }, cancellationToken);
        if (expense != null)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

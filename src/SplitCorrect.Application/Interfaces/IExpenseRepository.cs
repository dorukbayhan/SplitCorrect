using SplitCorrect.Domain.Entities;

namespace SplitCorrect.Application.Interfaces;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Expense>> GetByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default);
    Task<Expense> AddAsync(Expense expense, CancellationToken cancellationToken = default);
    Task UpdateAsync(Expense expense, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

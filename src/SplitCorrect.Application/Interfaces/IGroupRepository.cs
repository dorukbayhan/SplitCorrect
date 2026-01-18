using SplitCorrect.Domain.Entities;

namespace SplitCorrect.Application.Interfaces;

public interface IGroupRepository
{
    Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Group>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Group> AddAsync(Group group, CancellationToken cancellationToken = default);
    Task UpdateAsync(Group group, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

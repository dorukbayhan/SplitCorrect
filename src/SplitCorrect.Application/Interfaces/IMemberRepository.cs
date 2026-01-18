using SplitCorrect.Domain.Entities;

namespace SplitCorrect.Application.Interfaces;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Member>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Member> AddAsync(Member member, CancellationToken cancellationToken = default);
    Task UpdateAsync(Member member, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

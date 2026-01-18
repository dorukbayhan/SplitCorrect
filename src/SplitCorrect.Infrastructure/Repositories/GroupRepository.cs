using Microsoft.EntityFrameworkCore;
using SplitCorrect.Application.Interfaces;
using SplitCorrect.Domain.Entities;
using SplitCorrect.Infrastructure.Persistence;

namespace SplitCorrect.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly SplitCorrectDbContext _context;

    public GroupRepository(SplitCorrectDbContext context)
    {
        _context = context;
    }

    public async Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Include(g => g.Members)
            .Include(g => g.Expenses)
                .ThenInclude(e => e.PaidBy)
            .Include(g => g.Expenses)
                .ThenInclude(e => e.Splits)
                    .ThenInclude(s => s.Member)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<List<Group>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Include(g => g.Members)
            .Include(g => g.Expenses)
            .ToListAsync(cancellationToken);
    }

    public async Task<Group> AddAsync(Group group, CancellationToken cancellationToken = default)
    {
        await _context.Groups.AddAsync(group, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return group;
    }

    public async Task UpdateAsync(Group group, CancellationToken cancellationToken = default)
    {
        _context.Groups.Update(group);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var group = await _context.Groups.FindAsync(new object[] { id }, cancellationToken);
        if (group != null)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

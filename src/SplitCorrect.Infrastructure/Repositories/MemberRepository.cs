using Microsoft.EntityFrameworkCore;
using SplitCorrect.Application.Interfaces;
using SplitCorrect.Domain.Entities;
using SplitCorrect.Infrastructure.Persistence;

namespace SplitCorrect.Infrastructure.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly SplitCorrectDbContext _context;

    public MemberRepository(SplitCorrectDbContext context)
    {
        _context = context;
    }

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Members.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<List<Member>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Members.ToListAsync(cancellationToken);
    }

    public async Task<Member> AddAsync(Member member, CancellationToken cancellationToken = default)
    {
        await _context.Members.AddAsync(member, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return member;
    }

    public async Task UpdateAsync(Member member, CancellationToken cancellationToken = default)
    {
        _context.Members.Update(member);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var member = await _context.Members.FindAsync(new object[] { id }, cancellationToken);
        if (member != null)
        {
            _context.Members.Remove(member);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

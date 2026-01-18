using SplitCorrect.Application.DTOs;
using SplitCorrect.Application.Interfaces;
using SplitCorrect.Domain.Entities;

namespace SplitCorrect.Application.Services;

public class MemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IGroupRepository _groupRepository;

    public MemberService(IMemberRepository memberRepository, IGroupRepository groupRepository)
    {
        _memberRepository = memberRepository;
        _groupRepository = groupRepository;
    }

    public async Task<MemberDto?> CreateMemberAsync(
        CreateMemberDto dto,
        CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.GetByIdAsync(dto.GroupId, cancellationToken);
        if (group == null) return null;

        var member = new Member(dto.Name, dto.Email);
        var created = await _memberRepository.AddAsync(member, cancellationToken);
        
        // Add member to group
        group.AddMember(created);
        await _groupRepository.UpdateAsync(group, cancellationToken);
        
        return MapToMemberDto(created);
    }

    public async Task<MemberDto?> GetMemberByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(id, cancellationToken);
        return member == null ? null : MapToMemberDto(member);
    }

    public async Task<List<MemberDto>> GetAllMembersAsync(
        CancellationToken cancellationToken = default)
    {
        var members = await _memberRepository.GetAllAsync(cancellationToken);
        return members.Select(MapToMemberDto).ToList();
    }

    public async Task<MemberDto?> UpdateMemberAsync(
        Guid id,
        UpdateMemberDto dto,
        CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(id, cancellationToken);
        if (member == null) return null;

        member.UpdateDetails(dto.Name, dto.Email);
        await _memberRepository.UpdateAsync(member, cancellationToken);
        return MapToMemberDto(member);
    }

    public async Task<bool> DeleteMemberAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(id, cancellationToken);
        if (member == null) return false;

        await _memberRepository.DeleteAsync(id, cancellationToken);
        return true;
    }

    private MemberDto MapToMemberDto(Member member)
    {
        return new MemberDto(member.Id, member.Name, member.Email, member.CreatedAt);
    }
}

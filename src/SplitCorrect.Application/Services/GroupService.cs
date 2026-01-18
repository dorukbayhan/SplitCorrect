using SplitCorrect.Application.DTOs;
using SplitCorrect.Application.Interfaces;
using SplitCorrect.Domain.Entities;

namespace SplitCorrect.Application.Services;

public class GroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMemberRepository _memberRepository;

    public GroupService(IGroupRepository groupRepository, IMemberRepository memberRepository)
    {
        _groupRepository = groupRepository;
        _memberRepository = memberRepository;
    }

    public async Task<GroupDto> CreateGroupAsync(
        CreateGroupDto dto,
        CancellationToken cancellationToken = default)
    {
        var group = new Group(dto.Name, dto.Currency, dto.Description);
        var created = await _groupRepository.AddAsync(group, cancellationToken);
        return MapToGroupDto(created);
    }

    public async Task<GroupDto?> GetGroupByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
        return group == null ? null : MapToGroupDto(group);
    }

    public async Task<List<GroupDto>> GetAllGroupsAsync(
        CancellationToken cancellationToken = default)
    {
        var groups = await _groupRepository.GetAllAsync(cancellationToken);
        return groups.Select(MapToGroupDto).ToList();
    }

    public async Task<GroupDto?> UpdateGroupAsync(
        Guid id,
        UpdateGroupDto dto,
        CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
        if (group == null) return null;

        group.UpdateDetails(dto.Name, dto.Description);
        await _groupRepository.UpdateAsync(group, cancellationToken);
        return MapToGroupDto(group);
    }

    public async Task<bool> DeleteGroupAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
        if (group == null) return false;

        await _groupRepository.DeleteAsync(id, cancellationToken);
        return true;
    }

    public async Task<GroupDto?> AddMemberToGroupAsync(
        Guid groupId,
        Guid memberId,
        CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.GetByIdAsync(groupId, cancellationToken);
        if (group == null) return null;

        var member = await _memberRepository.GetByIdAsync(memberId, cancellationToken);
        if (member == null) return null;

        group.AddMember(member);
        await _groupRepository.UpdateAsync(group, cancellationToken);
        return MapToGroupDto(group);
    }

    public async Task<GroupDto?> RemoveMemberFromGroupAsync(
        Guid groupId,
        Guid memberId,
        CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.GetByIdAsync(groupId, cancellationToken);
        if (group == null) return null;

        group.RemoveMember(memberId);
        await _groupRepository.UpdateAsync(group, cancellationToken);
        return MapToGroupDto(group);
    }

    public async Task<List<MemberDto>> GetMembersByGroupIdAsync(
        Guid groupId,
        CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.GetByIdAsync(groupId, cancellationToken);
        if (group == null) return new List<MemberDto>();

        return group.Members.Select(m => new MemberDto(m.Id, m.Name, m.Email, m.CreatedAt)).ToList();
    }

    private GroupDto MapToGroupDto(Group group)
    {
        return new GroupDto(
            group.Id,
            group.Name,
            group.Currency,
            group.Description,
            group.CreatedAt,
            group.Members.Select(m => new MemberDto(m.Id, m.Name, m.Email, m.CreatedAt)).ToList(),
            group.Expenses.Count);
    }
}

using Microsoft.AspNetCore.Mvc;
using SplitCorrect.Application.DTOs;
using SplitCorrect.Application.Services;

namespace SplitCorrect.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly GroupService _groupService;

    public GroupsController(GroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GroupDto>>> GetAll(CancellationToken cancellationToken)
    {
        var groups = await _groupService.GetAllGroupsAsync(cancellationToken);
        return Ok(groups);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);
        if (group == null)
            return NotFound();

        return Ok(group);
    }

    [HttpPost]
    public async Task<ActionResult<GroupDto>> Create(
        CreateGroupDto dto,
        CancellationToken cancellationToken)
    {
        var group = await _groupService.CreateGroupAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = group.Id }, group);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GroupDto>> Update(
        Guid id,
        UpdateGroupDto dto,
        CancellationToken cancellationToken)
    {
        var group = await _groupService.UpdateGroupAsync(id, dto, cancellationToken);
        if (group == null)
            return NotFound();

        return Ok(group);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _groupService.DeleteGroupAsync(id, cancellationToken);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{groupId}/members/{memberId}")]
    public async Task<ActionResult<GroupDto>> AddMember(
        Guid groupId,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        var group = await _groupService.AddMemberToGroupAsync(groupId, memberId, cancellationToken);
        if (group == null)
            return NotFound();

        return Ok(group);
    }

    [HttpDelete("{groupId}/members/{memberId}")]
    public async Task<ActionResult<GroupDto>> RemoveMember(
        Guid groupId,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        var group = await _groupService.RemoveMemberFromGroupAsync(groupId, memberId, cancellationToken);
        if (group == null)
            return NotFound();

        return Ok(group);
    }
}

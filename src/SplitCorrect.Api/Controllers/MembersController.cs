using Microsoft.AspNetCore.Mvc;
using SplitCorrect.Application.DTOs;
using SplitCorrect.Application.Services;

namespace SplitCorrect.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly MemberService _memberService;
    private readonly GroupService _groupService;

    public MembersController(MemberService memberService, GroupService groupService)
    {
        _memberService = memberService;
        _groupService = groupService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MemberDto>>> GetAll(CancellationToken cancellationToken)
    {
        var members = await _memberService.GetAllMembersAsync(cancellationToken);
        return Ok(members);
    }

    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<List<MemberDto>>> GetByGroupId(Guid groupId, CancellationToken cancellationToken)
    {
        var members = await _groupService.GetMembersByGroupIdAsync(groupId, cancellationToken);
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var member = await _memberService.GetMemberByIdAsync(id, cancellationToken);
        if (member == null)
            return NotFound();

        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<MemberDto>> Create(
        CreateMemberDto dto,
        CancellationToken cancellationToken)
    {
        var member = await _memberService.CreateMemberAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MemberDto>> Update(
        Guid id,
        UpdateMemberDto dto,
        CancellationToken cancellationToken)
    {
        var member = await _memberService.UpdateMemberAsync(id, dto, cancellationToken);
        if (member == null)
            return NotFound();

        return Ok(member);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _memberService.DeleteMemberAsync(id, cancellationToken);
        if (!result)
            return NotFound();

        return NoContent();
    }
}

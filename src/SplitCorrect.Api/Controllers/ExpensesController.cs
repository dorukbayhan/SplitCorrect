using Microsoft.AspNetCore.Mvc;
using SplitCorrect.Application.DTOs;
using SplitCorrect.Application.Services;

namespace SplitCorrect.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly ExpenseService _expenseService;

    public ExpensesController(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var expense = await _expenseService.GetExpenseByIdAsync(id, cancellationToken);
        if (expense == null)
            return NotFound();

        return Ok(expense);
    }

    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<List<ExpenseDto>>> GetByGroupId(
        Guid groupId,
        CancellationToken cancellationToken)
    {
        var expenses = await _expenseService.GetExpensesByGroupIdAsync(groupId, cancellationToken);
        return Ok(expenses);
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> Create(
        CreateExpenseDto dto,
        CancellationToken cancellationToken)
    {
        var expense = await _expenseService.CreateExpenseAsync(dto, cancellationToken);
        if (expense == null)
            return BadRequest("Invalid expense data");

        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _expenseService.DeleteExpenseAsync(id, cancellationToken);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpGet("group/{groupId}/details")]
    public async Task<ActionResult<GroupDetailDto>> GetGroupDetails(
        Guid groupId,
        CancellationToken cancellationToken)
    {
        var details = await _expenseService.GetGroupDetailsWithBalancesAsync(groupId, cancellationToken);
        if (details == null)
            return NotFound();

        return Ok(details);
    }
}

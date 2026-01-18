namespace SplitCorrect.Application.DTOs;

public record CreateGroupDto(string Name, string Currency = "USD", string? Description = null);

public record UpdateGroupDto(string Name, string? Description);

public record GroupDto(
    Guid Id,
    string Name,
    string Currency,
    string? Description,
    DateTime CreatedAt,
    List<MemberDto> Members,
    int ExpenseCount);

public record GroupDetailDto(
    Guid Id,
    string Name,
    string Currency,
    string? Description,
    DateTime CreatedAt,
    List<MemberDto> Members,
    List<ExpenseDto> Expenses,
    List<BalanceDto> Balances,
    List<SettlementDto> Settlements);

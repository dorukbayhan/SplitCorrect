namespace SplitCorrect.Application.DTOs;

public record CreateExpenseDto(
    string Description,
    decimal TotalAmount,
    string Currency,
    Guid PaidByMemberId,
    Guid GroupId,
    List<Guid>? SplitBetweenMemberIds = null,
    DateTime? ExpenseDate = null);

public record ExpenseDto(
    Guid Id,
    string Description,
    decimal Amount,
    string Currency,
    MemberDto PaidBy,
    DateTime ExpenseDate,
    List<ExpenseSplitDto> Splits);

public record ExpenseSplitDto(
    Guid MemberId,
    string MemberName,
    decimal Amount,
    string Currency);

public record BalanceDto(
    Guid MemberId,
    string MemberName,
    decimal Amount,
    string Currency);

public record SettlementDto(
    Guid FromMemberId,
    string FromMemberName,
    Guid ToMemberId,
    string ToMemberName,
    decimal Amount,
    string Currency);

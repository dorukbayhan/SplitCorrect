namespace SplitCorrect.Application.DTOs;

public record CreateMemberDto(Guid GroupId, string Name, string Email);

public record UpdateMemberDto(string Name, string Email);

public record MemberDto(Guid Id, string Name, string Email, DateTime CreatedAt);

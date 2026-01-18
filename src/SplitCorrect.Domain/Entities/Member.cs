using SplitCorrect.Domain.Common;

namespace SplitCorrect.Domain.Entities;

public class Member : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }

    private Member() { } // EF Core

    public Member(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Member name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        Name = name;
        Email = email;
    }

    public void UpdateDetails(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Member name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        Name = name;
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }
}

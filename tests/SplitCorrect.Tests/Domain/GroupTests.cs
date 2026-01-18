using SplitCorrect.Domain.Entities;

namespace SplitCorrect.Tests.Domain;

public class GroupTests
{
    [Fact]
    public void Group_Create_ValidData_Success()
    {
        var group = new Group("Trip to Paris", "EUR", "Summer vacation");

        Assert.Equal("Trip to Paris", group.Name);
        Assert.Equal("EUR", group.Currency);
        Assert.Equal("Summer vacation", group.Description);
    }

    [Fact]
    public void Group_AddMember_Success()
    {
        var group = new Group("Trip", "USD");
        var member = new Member("John", "john@test.com");

        group.AddMember(member);

        Assert.Single(group.Members);
        Assert.Contains(member, group.Members);
    }

    [Fact]
    public void Group_AddDuplicateMember_ThrowsException()
    {
        var group = new Group("Trip", "USD");
        var member = new Member("John", "john@test.com");

        group.AddMember(member);

        Assert.Throws<InvalidOperationException>(() => group.AddMember(member));
    }

    [Fact]
    public void Group_RemoveMember_NoExpenses_Success()
    {
        var group = new Group("Trip", "USD");
        var member = new Member("John", "john@test.com");
        
        group.AddMember(member);
        group.RemoveMember(member.Id);

        Assert.Empty(group.Members);
    }

    [Fact]
    public void Group_UpdateDetails_Success()
    {
        var group = new Group("Old Name", "USD");

        group.UpdateDetails("New Name", "Updated description");

        Assert.Equal("New Name", group.Name);
        Assert.Equal("Updated description", group.Description);
    }
}

using GodSharp.FluentMember;

using System;

using Xunit;
using Xunit.Abstractions;

namespace GodSharpFluentMemberTest;

public class TypeSetterUnitTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TypeSetterUnitTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void SetMethodTest()
    {
        var user = new User();
        TypeSetter.Set(user, x => x.Id, 1);
        TypeSetter.Set(user, x => x.Age, 18);
        TypeSetter.Set(user, x => x.Name, "abc");

        Assert.Equal(1, user.Id);
        Assert.Equal(18, user.Age);
        Assert.Equal("abc", user.Name);

        var guid = Guid.NewGuid();
        var role = new Role();
        TypeSetter.Set(role, x => x.Id, 1);
        TypeSetter.Set(role, x => x.Name, "Admin");
        TypeSetter.Set(role, x => x.Code, "admin");
        TypeSetter.Set(role, x => x.InternalId, guid);

        Assert.Equal(1, role.Id);
        Assert.Equal("Admin", role.Name);
        Assert.Equal("admin", role.Code);
        Assert.Equal(guid, role.InternalId);

        _testOutputHelper.WriteLine($"User=>{user.Id}/{user.Age}/{user.Name}");
        _testOutputHelper.WriteLine($"Role=>{role.Id}/{role.Name}/{role.Code}/{role.InternalId}");
    }
}
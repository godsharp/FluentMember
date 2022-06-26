using Xunit;
using Xunit.Abstractions;

namespace GodSharpFluentMemberTest;

public class FluentMemberUnitTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public FluentMemberUnitTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    //[Fact]
    // public void WithMethodTest()
    // {
    //     var user = new User()
    //         .WithId(1)
    //         .WithAge(18)
    //         .WithName("abc");
    //     // Not support
    //     //user.Set(x => x.Role.Code, "admin");
    //
    //     Assert.Equal(1, user.Id);
    //     Assert.Equal(18, user.Age);
    //     Assert.Equal("abc", user.Name);
    //     //Assert.Equal("admin", user.Role.Code);
    //
    //     var address = new User.Address()
    //         .WithCountry("China")
    //         .WithCity("WuHan")
    //         .WithPostcode("430000");
    //     Assert.Equal("China", address.Country);
    //     Assert.Equal("WuHan", address.City);
    //     Assert.Equal("430000", address.Postcode);
    //
    //     var ic = new IdCard("123").WithAge(12);
    //
    //     Assert.Equal("123", ic.Id);
    //     Assert.Equal(12, ic.Age);
    //
    //     var guid = Guid.NewGuid();
    //     var role = new Role()
    //         .WithId(1)
    //         .WithName("Admin")
    //         .WithCode("admin")
    //         .WithInternalId(guid);
    //
    //     Assert.Equal(1, role.Id);
    //     Assert.Equal("Admin", role.Name);
    //     Assert.Equal("admin", role.Code);
    //     Assert.Equal(guid, role.InternalId);
    //
    //     _testOutputHelper.WriteLine($"User=>{user.Id}/{user.Age}/{user.Name}");
    //     _testOutputHelper.WriteLine($"Address=>{address.Country}/{address.City}/{address.Postcode}");
    //     _testOutputHelper.WriteLine($"IdCard=>{ic.Id}/{ic.Age}");
    //     _testOutputHelper.WriteLine($"Role=>{role.Id}/{role.Name}/{role.Code}/{role.InternalId}");
    // }
    //
    // [Fact]
    // public void SetMethodTest()
    // {
    //     var user = new User()
    //         .Set(x => x.Id, 1)
    //         .Set(x => x.Age, 18)
    //         .Set(x => x.Name, "abc");
    //     //.Set(x => x.Role.Code, "admin");
    //     // Not support
    //
    //     Assert.Equal(1, user.Id);
    //     Assert.Equal(18, user.Age);
    //     Assert.Equal("abc", user.Name);
    //     //Assert.Equal("admin", user.Role.Code);
    //
    //     var guid = Guid.NewGuid();
    //     var role = new Role()
    //         .Set(x => x.Id, 1)
    //         .Set(x => x.Name, "Admin")
    //         .Set(x => x.Code, "admin")
    //         .Set(x => x.InternalId, guid);
    //
    //     Assert.Equal(1, role.Id);
    //     Assert.Equal("Admin", role.Name);
    //     Assert.Equal("admin", role.Code);
    //     Assert.Equal(guid, role.InternalId);
    //
    //     _testOutputHelper.WriteLine($"User=>{user.Id}/{user.Age}/{user.Name}");
    //     _testOutputHelper.WriteLine($"Role=>{role.Id}/{role.Name}/{role.Code}/{role.InternalId}");
    // }
}
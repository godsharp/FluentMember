using GodSharp.FluentMember;

namespace GodSharpFluentMemberTest;

[FluentMemberGenerator(Partial = true)]
public partial record IdCard(string Id)
{
    public int Age { get; set; }
};
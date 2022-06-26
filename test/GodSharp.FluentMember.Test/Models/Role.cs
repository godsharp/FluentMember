using GodSharp.FluentMember;

using System;

namespace GodSharpFluentMemberTest;

[FluentMemberGenerator(Partial = true)]
public partial record class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    internal Guid InternalId { get; set; }
}
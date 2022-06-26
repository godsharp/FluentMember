using System.Collections.Generic;

namespace GodSharp.FluentMember;

public record FluentMemberArguments
{
    public string Accessibility { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public string TypeName { get; set; }
    public string Namespace { get; set; }
    public bool CanSetter { get; set; }
    public List<string> Usings { get; private set; }
    public List<FluentMemberItemArguments> Items { get; private set; }

    public FluentMemberArguments()
    {
        Usings = new();
        Items = new();
    }
}

public record FluentMemberItemArguments
{
    public string Accessibility { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
}
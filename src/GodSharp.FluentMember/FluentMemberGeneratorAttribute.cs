using System;
using System.Diagnostics;

namespace GodSharp.FluentMember;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
[Conditional("CodeGeneration")]
public sealed class FluentMemberGeneratorAttribute : Attribute
{
    public bool Extension { get; set; } = true;
    public bool Partial { get; set; } = false;

    public FluentMemberGeneratorAttribute()
    {
        Extension = true;
        Partial = false;
    }
}
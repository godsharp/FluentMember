﻿namespace GodSharp.FluentMember.Generator;

internal class Configuration
{
    public static string AttributeName { get; private set; } = "FluentMemberGenerator";
    public static string AttributeFullName { get; private set; } = "GodSharp.FluentMember.FluentMemberGeneratorAttribute";

    public static readonly string Suffix = "FluentMember.g.cs";
    public static readonly string Header =
@"//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by SourceGenerator generator at {1}.
//     Source: {0}
// </auto-generated>
//------------------------------------------------------------------------------";

    internal static bool HasAttributeName( string dst)
    {
        return !string.IsNullOrWhiteSpace(dst) && AttributeName.Equals(dst);
    }
}
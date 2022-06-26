using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

namespace GodSharp.FluentMember.Generator;

public class FluentMemberSyntaxReceiver : ISyntaxReceiver
{
    internal readonly List<SyntaxNode> SyntaxNodes = new();

    void ISyntaxReceiver.OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (
            (syntaxNode is RecordDeclarationSyntax rcd && HasAttribute(rcd.AttributeLists)) ||
            (syntaxNode is ClassDeclarationSyntax cls && HasAttribute(cls.AttributeLists)) ||
            (syntaxNode is StructDeclarationSyntax str && HasAttribute(str.AttributeLists)) ||
            (syntaxNode is InterfaceDeclarationSyntax inf && HasAttribute(inf.AttributeLists))
            )
        {
            SyntaxNodes.Add(syntaxNode);
        }
    }

    private static bool HasAttribute(SyntaxList<AttributeListSyntax> attributes)
    {
        if (attributes.Count == 0) return false;
        return attributes
            .Any(a => a?.Attributes
                .Any(x => Configuration.HasAttributeName(x?.Name?.ToString())) == true
            );
    }
}
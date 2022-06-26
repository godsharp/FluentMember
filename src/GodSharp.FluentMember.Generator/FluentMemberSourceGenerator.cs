using GodSharp.FluentMember;
using GodSharp.FluentMember.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

using System;
using System.Linq;
using System.Text;

//namespace GodSharp.FluentMember.Generator;

[Generator]
public class FluentMemberSourceGenerator : ISourceGenerator
{
    private PartialCodeGenerator _patCodeGenerator;
    private ExtensionCodeGenerator _extCodeGenerator;

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not FluentMemberSyntaxReceiver receiver) return;

        var attr = context.Compilation.GetTypeByMetadataName(Configuration.AttributeFullName);
        if (attr == null) return;

        var symbols = receiver.SyntaxNodes
            .Select(x => context.Compilation.GetSemanticModel(x.SyntaxTree).GetDeclaredSymbol(x) as INamedTypeSymbol).ToArray();

        if (symbols.Length == 0) return;
        _patCodeGenerator = new PartialCodeGenerator();
        _extCodeGenerator = new ExtensionCodeGenerator();
        foreach (var symbol in symbols)
        {
            if (symbol == null) continue;

            var code = Generator(symbol, attr);

            if (code != null) context.AddSource($"{symbol.Name}.{Configuration.Suffix}", SourceText.From(code, Encoding.UTF8));
        }
    }

    private string Generator(INamedTypeSymbol type, INamedTypeSymbol attr)
    {
        var attributeData = type.GetAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attr));
        if (attr == null) return null;

        //bool partial = false;
        //bool extension = false;
        FluentMemberGeneratorAttribute attribute = new();
        foreach (var item in attributeData.NamedArguments)
        {
            switch (item.Key)
            {
                case nameof(FluentMemberGeneratorAttribute.Partial):
                    attribute.Partial = bool.Parse(item.Value.ToCSharpString());
                    break;

                case nameof(FluentMemberGeneratorAttribute.Extension):
                    attribute.Extension = bool.Parse(item.Value.ToCSharpString());
                    break;
            }
        }

        if (!attribute.Partial && !attribute.Extension) return null;

        var members = type.GetMembers()
            .Where(x => x.Kind is SymbolKind.Field or SymbolKind.Property)
            .Where(x => x.DeclaredAccessibility is Accessibility.Public or Accessibility.Internal)
            .Where(x => !x.IsAbstract && !x.IsStatic)
            .Where(x => (x is IPropertySymbol p && !p.SetMethod.IsInitOnly) || x is not IPropertySymbol)
            .ToArray();

        if (members.Length == 0) return null;

        var namespaced = !string.IsNullOrWhiteSpace(type.ContainingNamespace.ToDisplayString());

        var typeFullName = type.ContainingType == null ? type.Name : $"{type.ContainingType.Name}.{type.Name}";

        FluentMemberArguments arguments = new()
        {
            Name = type.Name,
            FullName = typeFullName,
            Namespace = type.ContainingNamespace.ToDisplayString(),
            Accessibility = type.DeclaredAccessibility.ToString().ToLower(),
            TypeName = type.TypeKind == TypeKind.Struct ? "struct" : type.IsRecord ? "record" : "class",
            CanSetter = type.TypeKind is not TypeKind.Interface && type.TypeKind is not TypeKind.Struct
        };

        foreach (var member in members)
        {
            var (typeSymbol, access) = member switch
            {
                IPropertySymbol
                {
                    IsIndexer: false
                    , IsReadOnly: false
                    , SetMethod.DeclaredAccessibility: Accessibility.Public or Accessibility.Internal or Accessibility.Friend
                } p => p.SetMethod.IsInitOnly ? (null, null) : (p.Type, p.SetMethod.DeclaredAccessibility.ToString().ToLower()),
                IFieldSymbol
                {
                    IsReadOnly: false
                    , DeclaredAccessibility: Accessibility.Public or Accessibility.Internal or Accessibility.Friend
                } f => (f.Type, f.DeclaredAccessibility.ToString().ToLower()),
                _ => (null, null)
            };

            if (typeSymbol == null) continue;
            string ns = typeSymbol.ContainingNamespace?.ToDisplayString();
            if (typeSymbol.ContainingNamespace?.IsNamespace == true &&
                !string.IsNullOrWhiteSpace(ns) &&
                !arguments.Usings.Contains(ns) &&
                arguments.Namespace != ns)
            {
                arguments.Usings.Add(ns);
            }

            arguments.Items.Add(new FluentMemberItemArguments
            {
                Name = member.Name,
                Accessibility = access,
                Type = typeSymbol.ContainingType == null ? typeSymbol.Name : $"{typeSymbol.ContainingType.Name}.{typeSymbol.Name}"
            });
        }

        if (type.ContainingType != null && !attribute.Extension) return null;

        var sb = new StringBuilder();

        sb.AppendLine(string.Format(Configuration.Header, type.Name, DateTime.Now.ToLocalTime()));

        if (arguments.CanSetter)
        {
            if (!arguments.Usings.Contains("System")) arguments.Usings.Add("System");
            if (!arguments.Usings.Contains("System.Linq.Expressions")) arguments.Usings.Add("System.Linq.Expressions");
            if (!arguments.Usings.Contains("GodSharp.FluentMember")) arguments.Usings.Add("GodSharp.FluentMember");
        }

        if (arguments.Usings.Count > 0)
        {
            sb.AppendLine();
            foreach (var item in arguments.Usings)
            {
                sb.AppendLine($"using {item};");
            }
        }

        if (namespaced)
        {
            sb.AppendLine();
            sb.AppendLine($"namespace {arguments.Namespace};");
        }

        sb.AppendLine();
        sb.AppendLine("#pragma warning disable CS1591");

        if (attribute.Partial && type.ContainingType == null) _patCodeGenerator.Gen(sb, arguments);
        if (attribute.Extension) _extCodeGenerator.Gen(sb, arguments);

        return sb.ToString();
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        //System.Diagnostics.Debugger.Launch();
        context.RegisterForSyntaxNotifications(() => new FluentMemberSyntaxReceiver());
    }
}
using System.Text;

namespace GodSharp.FluentMember;

public class ExtensionCodeGenerator : ICodeGenerator
{
    private static string _member = @"    {0} static {1} With{2}(this {1} {3} ,{4} {5})
    {{
        {3}.{2} = {5};
        return {3};
    }}";

    public StringBuilder Gen(StringBuilder sb, FluentMemberArguments arguments)
    {
        if (arguments == null || arguments.Items == null || arguments.Items.Count == 0) return sb;

        sb.AppendLine();
        sb.AppendLine($"{arguments.Accessibility} static class {arguments.Name}Extensions");
        sb.AppendLine("{");
        foreach (var item in arguments.Items)
        {
            var line = string.Format(_member, item.Accessibility, arguments.FullName, item.Name, Utils.ToCamelName(arguments.Name), item.Type, Utils.ToCamelName(item.Name));
            sb.AppendLine(line);
            sb.AppendLine();
        }

        if (arguments.CanSetter)
        {
            sb.AppendLine($"    {arguments.Accessibility} static {arguments.FullName} Set<TMember>(this {arguments.FullName} {Utils.ToCamelName(arguments.Name)}, Expression<Func<{arguments.FullName},TMember>> predicate, TMember value)");
            sb.AppendLine($"        => TypeSetter.Set({Utils.ToCamelName(arguments.Name)}, predicate, value);");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        return sb;
    }
}
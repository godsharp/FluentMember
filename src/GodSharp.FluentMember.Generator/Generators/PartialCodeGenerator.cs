using System.Text;

namespace GodSharp.FluentMember;

public class PartialCodeGenerator : ICodeGenerator
{
    private static string _member = @"    {0} {1} With{2}({3} {4})
    {{
        this.{2} = {4};
        return this;
    }}";

    public StringBuilder Gen(StringBuilder sb, FluentMemberArguments arguments)
    {
        if (arguments == null || arguments.Items == null || arguments.Items.Count == 0) return sb;
        sb.AppendLine();
        sb.AppendLine($"{arguments.Accessibility} partial {arguments.TypeName} {arguments.FullName}");
        sb.AppendLine("{");
        foreach (var item in arguments.Items)
        {
            var line = string.Format(_member, item.Accessibility, arguments.FullName, item.Name, item.Type, Utils.ToCamelName(item.Name));
            sb.AppendLine(line);
            sb.AppendLine();
        }

        if (arguments.CanSetter)
        {
            sb.AppendLine($"    {arguments.Accessibility} {arguments.FullName} Set<TMember>(Expression<Func<{arguments.FullName},TMember>> predicate, TMember value)");
            sb.AppendLine($"        => TypeSetter.Set(this,predicate,value);");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        return sb;
    }
}
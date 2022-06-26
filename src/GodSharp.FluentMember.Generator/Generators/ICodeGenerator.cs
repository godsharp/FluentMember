using System.Text;

namespace GodSharp.FluentMember;

public interface ICodeGenerator
{
    StringBuilder Gen(StringBuilder sb, FluentMemberArguments arguments);
}
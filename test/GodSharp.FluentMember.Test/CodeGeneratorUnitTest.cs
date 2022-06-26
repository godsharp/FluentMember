using Xunit.Abstractions;

namespace GodSharpFluentMemberTest;

public class CodeGeneratorUnitTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    //private readonly FluentMemberArguments _arguments;

    //public CodeGeneratorUnitTest(ITestOutputHelper testOutputHelper)
    //{
    //    _testOutputHelper = testOutputHelper;
    //    _arguments = new FluentMemberArguments()
    //    {
    //        Accessibility = "public",
    //        Name = "Person",
    //        FullName = "Person",
    //        TypeName = "class",
    //        Namespace = "Demo"
    //    };
    //    _arguments.Usings.Add("System");
    //    _arguments.Usings.Add("GodSharp.FluentMember");
    //    _arguments.Items.Add(new() { Accessibility = "internal", Name = "Name", Type = "string" });
    //    _arguments.Items.Add(new() { Accessibility = "public", Name = "Age", Type = "int" });
    //}

    //[Fact]
    //public void Test()
    //{
    //    StringBuilder sb = new();
    //    new PartialCodeGenerator().Gen(sb, _arguments);
    //    new ExtensionCodeGenerator().Gen(sb, _arguments);

    //    _testOutputHelper.WriteLine(sb.ToString());
    //}
}
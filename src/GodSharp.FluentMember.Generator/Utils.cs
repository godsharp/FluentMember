namespace GodSharp.FluentMember;

internal class Utils
{
    public static string ToCamelName(string name)
    {
        if (!char.IsUpper(name[0])) return name;
        var array = name.ToCharArray();
        array[0] = char.ToLower(array[0]);
        return new string(array);
    }
}
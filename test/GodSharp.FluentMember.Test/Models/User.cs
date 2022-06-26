using GodSharp.FluentMember;

namespace GodSharpFluentMemberTest;

[FluentMemberGenerator(Partial = true)]
public partial class User
{
    public int Id { get; internal set; }
    public int Age { get; set; }
    internal string Name { get; set; }
    public string Secret { get; private set; }
    public Role Role { get; set; } = new();

    [FluentMemberGenerator(Partial = true)]
    public partial struct Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        internal string Postcode { get; set; }
        public string Secret { get; private set; }
    }
}
namespace GoogleLibrary.Custom.Events
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class ContactAttribute : Attribute
    {
        public ContactAttribute(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
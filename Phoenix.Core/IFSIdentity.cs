namespace Phoenix.Core
{
    public interface IFSIdentity
    {
        bool Deleted { get; }
        bool NotPresent { get; }
        string Username { get; }
        string FirstName { get; }
        string Surname { get; }
        string EmailAddress { get; }
        string Department { get; }
        string Telephone { get; }
        string DisplayName();
        string Path();
    }
}
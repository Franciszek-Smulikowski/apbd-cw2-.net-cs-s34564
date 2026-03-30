namespace Cw2App.Models;

public abstract class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserType UserType { get; set; }

    protected User(int id, string firstName, string lastName, UserType userType)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        UserType = userType;
    }

    public override string ToString()
    {
        return $"[{Id}] {FirstName} {LastName} (Typ: {UserType})";
    }
}

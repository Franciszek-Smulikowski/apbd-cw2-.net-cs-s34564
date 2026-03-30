namespace Cw2App.Models;

public class Employee : User
{
    public Employee(int id, string firstName, string lastName) 
        : base(id, firstName, lastName, UserType.Employee)
    {
    }
}

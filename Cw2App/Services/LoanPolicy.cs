using System;
using Cw2App.Models;

namespace Cw2App.Services;

public class LoanPolicy
{
    private const int StudentLimit = 2;
    private const int EmployeeLimit = 5;

    public int GetMaxActiveLoans(User user)
    {
        return user.UserType switch
        {
            UserType.Student => StudentLimit,
            UserType.Employee => EmployeeLimit,
            _ => throw new ArgumentException($"Unknown user type: {user.UserType}")
        };
    }
}

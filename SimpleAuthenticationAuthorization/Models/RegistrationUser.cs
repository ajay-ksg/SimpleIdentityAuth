using SimpleAuthenticationAuthorization.Enums;

namespace SimpleAuthenticationAuthorization.Models;

public class RegistrationUser()
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    public string Password { get; set; }
    public Roles Role { get; set; }
}
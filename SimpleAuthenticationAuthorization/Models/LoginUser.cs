namespace SimpleAuthenticationAuthorization.Models;

public record LoginUser
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    
}
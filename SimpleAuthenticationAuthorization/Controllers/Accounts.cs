using System.ComponentModel;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using SimpleAuthenticationAuthorization.Database;
using SimpleAuthenticationAuthorization.Enums;
using SimpleAuthenticationAuthorization.Models;
using SimpleAuthenticationAuthorization.Services;

namespace SimpleAuthenticationAuthorization.Controllers;

[Route("api/[controller]")]
public class Accounts(
    AuthDbContext context,
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    SignInManager<IdentityUser> signInManager,
    IdentityService identityService) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegistrationUser user)
    {
        // Create Identity user 
        IdentityUser identityUser = new()
        {
            UserName = user.Email,
            Email = user.Email,
            PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(user.Password)))
        };

        await userManager.CreateAsync(identityUser);
        // identityUser.PasswordHash = user.Password;
        // await userManager.UpdateAsync(identityUser);

        // Add Claims 

        var claims = new List<Claim>()
        {
            new("FirstName", user.FirstName),
            new("LastName", user.LastName),
        };

        await userManager.AddClaimsAsync(identityUser, claims);

        // Add Role
        if (user.Role == Roles.Admin)
        {
            await AssignRoleToUser(identityUser, claims, Roles.Admin);
        }
        else
        {
            await AssignRoleToUser(identityUser, claims, Roles.User);
        }
        
        // Create claims Identity with email and sub and all other claims
        var claimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Email, user.Email ?? throw new InvalidOperationException()),
                new(JwtRegisteredClaimNames.Sub, user.Email ?? throw new InvalidOperationException()),
                
            }
        );
        
        claimsIdentity.AddClaims(claims);
        
        // Create Jwt token with these claims and return 
        var jwtToken = identityService.GenerateJwtToken(claimsIdentity);

        return Ok(identityService.GenerateJwtTokenString(jwtToken));

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUser loginUser)
    {
        // verify if user Exist 
        var user  = await userManager.FindByEmailAsync(loginUser.Email);
        if (user is null)
            return BadRequest("User does not exists");
        
        // verify password 

        var result = await signInManager.CheckPasswordSignInAsync(user, loginUser.Password, false);
        if (!result.Succeeded)
            return BadRequest("Incorrect Password");
        
        // Get Roles and Claims

        var claims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        
        // create claimsIdentity for token

        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new(JwtRegisteredClaimNames.Email, loginUser.Email),
            new(JwtRegisteredClaimNames.Sub, loginUser.Email),
        });
        
        // Add roles and claims to claims Identity 
        claimsIdentity.AddClaims(claims);
        foreach (var role in roles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        }
        
        // create token 

        var token = identityService.GenerateJwtToken(claimsIdentity);
        return Ok(identityService.GenerateJwtTokenString(token));
        
    }

    private async Task AssignRoleToUser(IdentityUser identityUser, List<Claim> claims, Roles role)
    {
        if (!Enum.IsDefined(typeof(Roles), role))
            throw new InvalidEnumArgumentException(nameof(role), (int)role, typeof(Roles));
        
        var roleName = Enum.GetName(typeof(Roles), role)!;
        await GetOrCreateRole(roleName);
        
        await userManager.AddToRoleAsync(identityUser, roleName);
        claims.Add(new Claim(ClaimTypes.Role, roleName));
    }


    private async Task GetOrCreateRole(string role)
    {
        var userRole = await roleManager.FindByNameAsync(role);
        if (userRole == null)
        {
            // create new role 
            var newRole = new IdentityRole(role);
            await roleManager.CreateAsync(newRole);
        }
    }
    
    
}
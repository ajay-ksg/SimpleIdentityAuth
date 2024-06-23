using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleAuthenticationAuthorization.Models.Entities;

namespace SimpleAuthenticationAuthorization.Database;

public class AuthDbContext(DbContextOptions options): IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Product> Products { get; set; }
}

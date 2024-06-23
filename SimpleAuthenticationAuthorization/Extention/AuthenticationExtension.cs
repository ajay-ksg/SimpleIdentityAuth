using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SimpleAuthenticationAuthorization.Database;
using SimpleAuthenticationAuthorization.Options;

namespace SimpleAuthenticationAuthorization.Extention;

public static class AuthenticationExtension
{
     public static WebApplicationBuilder RegisterAuthentication(this WebApplicationBuilder builder)
    {
        // builder.Services.AddOptions<JwtSettings>().BindConfiguration(nameof(JwtSettings));
        var jwtSettings = new JwtSettings();
        builder.Configuration.Bind(nameof(JwtSettings),jwtSettings);
        var jwtSection = builder.Configuration.GetSection(nameof(jwtSettings));
        builder.Services.Configure<JwtSettings>(jwtSection);

        builder.Services.AddAuthentication(a =>
                {
                    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    a.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(jwtSettings.SigningKey ?? throw new InvalidOperationException())),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudiences = jwtSettings.Audience,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
                jwt.Audience = jwtSettings.Audience[0];
                jwt.ClaimsIssuer = jwtSettings.Issuer;
            });

        builder.Services.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<IdentityRole>()
            .AddSignInManager()
            .AddEntityFrameworkStores<AuthDbContext>();
        
        return builder;
    }

}
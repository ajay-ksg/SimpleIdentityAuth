using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SimpleAuthenticationAuthorization.Database;
using SimpleAuthenticationAuthorization.Database.Repository;
using SimpleAuthenticationAuthorization.Services;

namespace SimpleAuthenticationAuthorization.Extention;

public static class ConfigurationExtension
{
    public static IServiceCollection Configure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMvcCore().AddApiExplorer();
        services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(
            configuration.GetConnectionString("Default"), npgSqlOption =>
        {
            npgSqlOption.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        }));
        services.AddScoped<IdentityService>();
        services.AddScoped<ProductRepository>();
        return services;
    }
    
    public static IServiceCollection ConfigureDb(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<AuthDbContext>();
        dbContext.Database.Migrate();
        return services;
    }
}
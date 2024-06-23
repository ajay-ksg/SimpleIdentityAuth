using Microsoft.OpenApi.Models;

namespace SimpleAuthenticationAuthorization.Extention;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1",
                new OpenApiInfo()
                {
                    Title = "Auth API", Version = "v1",
                    Contact = new OpenApiContact() { Name = "Ajay Kumar", Email = "ajaygepal@gmail.com" }
                });
            
            // Define a security definition having key as Bearer
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            // Configure Swagger to add above define security 
            option.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                
            });
        });
        
    return services;
    }

    public static WebApplication UseMySwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI((c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
        }));
        return app;
    }
}
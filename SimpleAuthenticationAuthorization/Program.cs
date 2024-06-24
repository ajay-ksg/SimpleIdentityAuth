using SimpleAuthenticationAuthorization.Extention;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterAuthentication();

builder.Services
    .Configure(builder.Configuration)
    .AddSwagger();

builder.Services.ConfigureDb();

var app = builder.Build();

app.UseMySwagger()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

//app.MapGet("/", () => "Hello World!");

app.Run();
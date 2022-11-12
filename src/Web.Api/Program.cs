using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web.Api;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddDbContextFactory<MyContext>(options =>
{
    options.UseInMemoryDatabase("mycontext");
});

services.AddScoped(p =>
{
    var context = p.GetRequiredService<IDbContextFactory<MyContext>>().CreateDbContext();
    context.Database.EnsureCreated();
    return context;
});

services.AddIdentity<User, Role>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<MyContext>()
    .AddDefaultTokenProviders();

services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = Constants.Domain,
            ValidateIssuer = true,
            ValidIssuer = Constants.Domain,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Constants.JwtKey)),
        };
    });
services.AddCors(options =>
{
    options.AddPolicy(name: "Cors Allow Local Blazor",
        builder =>
        {
            builder.WithOrigins("https://localhost:7112")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
services.AddAuthorization(options =>
{
    
    options.AddPolicy("SuperDuper", policy =>
        policy.RequireRole("Super", "Duper"));
});
services.AddControllers();

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    
    await roleManager.CreateAsync(new Role("Super"));
    await roleManager.CreateAsync(new Role("Duper"));
}

app.UseCors("Cors Allow Local Blazor");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
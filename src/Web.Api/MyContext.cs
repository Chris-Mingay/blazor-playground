using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Web.Api;

public class MyContext : IdentityDbContext<User, Role, Guid>
{
    public MyContext(DbContextOptions options) : base(options)
    {
    }
}
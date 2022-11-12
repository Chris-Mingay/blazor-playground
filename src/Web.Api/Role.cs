using Microsoft.AspNetCore.Identity;

namespace Web.Api;

public class Role : IdentityRole<Guid>
{
    public Role() : base() {}

    public Role(string roleName) : base(roleName) {}
}
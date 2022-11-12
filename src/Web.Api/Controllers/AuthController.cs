using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazor.Shared.Commands.Auth.ChangePassword;
using Blazor.Shared.Commands.Auth.Login;
using Blazor.Shared.Commands.Auth.Refresh;
using Blazor.Shared.Commands.Auth.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<string> Hello()
    {
        return await Task.FromResult("hello");
    }
    
    [HttpPost("register")]
    public async Task<RegisterResponse> Register(RegisterRequest request)
    {
        var user = new User()
        {
            Email = request.EmailAddress,
            UserName = request.DisplayName,
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        
        await _userManager.AddToRoleAsync(user, "Super");
        await _userManager.AddToRoleAsync(user, "Duper");

        
        return new RegisterResponse(result.Succeeded);
    }
    
    [HttpPost("login")]
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.EmailAddress);
        if (user != null)
        {
            var signIn = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (signIn.Succeeded)
            {
                string jwt = await CreateJWT(user);
                AppendRefreshTokenCookie(user, HttpContext.Response.Cookies);

                return new LoginResponse(true, jwt);
            }
            else
            {
                return LoginResponse.Failed;
            }
        }
        else
        {
            return LoginResponse.Failed;
        }
    }
    
    [HttpGet("refresh")]
    public async Task<RefreshResponse> Refresh()
    {
        var cookie = HttpContext.Request.Cookies[Constants.RefreshTokenCookieKey];
        if (cookie != null)
        {
            var user = _userManager.Users.FirstOrDefault(user => user.SecurityStamp == cookie);
            if (user != null)
            {
                var jwtToken = await CreateJWT(user);
                return new RefreshResponse(true, jwtToken);
            }
        }
        return new RefreshResponse(false, "");
    }
    
    [HttpPost("changepassword")]
    public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.EmailAddress);
        if (user != null)
        {
            var signIn = await _signInManager.CheckPasswordSignInAsync(user, request.CurrentPassword, false);
            if (signIn.Succeeded)
            {
                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                if (result.Succeeded)
                {
                    return new ChangePasswordResponse(true);
                }
            }
        }
        
        return ChangePasswordResponse.Failed;
    }
    
    private static void AppendRefreshTokenCookie(User user, IResponseCookies cookies)
    {
        var options = new CookieOptions();
        options.HttpOnly = true;
        options.Secure = true;
        options.SameSite = SameSiteMode.Strict;
        options.Expires = DateTime.Now.AddMinutes(60);
        cookies.Append(Constants.RefreshTokenCookieKey, user.SecurityStamp, options);
    }
    
    private async Task<string> CreateJWT(User user)
    {
        var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Constants.JwtKey));
        var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName), // NOTE: this will be the "User.Identity.Name" value
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
        };
        
        claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

        var token = new JwtSecurityToken(
            issuer: Constants.Domain,
            audience: Constants.Domain,
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);
    
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}
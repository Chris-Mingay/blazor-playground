using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class DataController : Controller
{
    
    [HttpGet]
    public async Task<List<string>> Index()
    {
        var list = new List<string>() { "Index" };
        return await Task.FromResult(list);
    }
    
    [HttpGet("super")]
    [Authorize(Roles = "Super")]
    public async Task<List<string>> Super()
    {
        var list = new List<string>() { "Super" };
        return await Task.FromResult(list);
    }
    
    [HttpGet("duper")]
    [Authorize(Roles = "Duper")]
    public async Task<List<string>> Duper()
    {
        var list = new List<string>() { "Duper" };
        return await Task.FromResult(list);
    }
    
    [HttpGet("superduper")]
    [Authorize(Policy = "SuperDuper")]
    public async Task<List<string>> SuperDuper()
    {
        var list = new List<string>() { "Super", "Duper" };
        return await Task.FromResult(list);
    }
    
    [HttpGet("awesome")]
    [Authorize(Roles = "Awesome")]
    public async Task<List<string>> Awesome()
    {
        var list = new List<string>() { "Awesome" };
        return await Task.FromResult(list);
    }
}
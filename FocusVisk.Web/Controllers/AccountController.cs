using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FocusVisk.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FocusVisk.Web.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IHttpClientFactory httpClientFactory,
        IConfiguration config)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: true, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Email ou senha inválidos.");
            return View();
        }

        var token = await LoginNaApiAsync(email, password);
        if (token is null)
        {
            ModelState.AddModelError(string.Empty, "Login no Web funcionou, mas falhou ao autenticar na API.");
            await _signInManager.SignOutAsync();
            return View();
        }

        Response.Cookies.Append("focusvisk_api_token", token, new CookieOptions
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        return RedirectToAction("Index", "Tasks");
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password)
    {
        var user = new ApplicationUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View();
        }

        await _signInManager.SignInAsync(user, isPersistent: true);

        var token = await LoginNaApiAsync(email, password);
        if (token is not null)
        {
            Response.Cookies.Append("focusvisk_api_token", token, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }

        return RedirectToAction("Index", "Tasks");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        Response.Cookies.Delete("focusvisk_api_token");
        return RedirectToAction("Login");
    }

    private async Task<string?> LoginNaApiAsync(string email, string password)
    {
        var client = _httpClientFactory.CreateClient();
        var apiBaseUrl = _config["ApiBaseUrl"]; 

        var payload = JsonSerializer.Serialize(new { email, password });
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"{apiBaseUrl}/api/auth/login", content);
        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.GetProperty("token").GetString();
    }
}
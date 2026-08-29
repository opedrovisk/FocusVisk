using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusVisk.Web.Controllers;

public class HomeController : Controller
{
    [Authorize]
    public IActionResult Index() => RedirectToAction("Index", "Tasks");

    public IActionResult Error() => View();
}
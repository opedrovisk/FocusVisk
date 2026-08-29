using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusVisk.Web.Controllers;

[Authorize]
public class TasksController : Controller
{
    public IActionResult Index() => View();
}
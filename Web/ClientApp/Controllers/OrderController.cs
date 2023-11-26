using Microsoft.AspNetCore.Mvc;

namespace ClientApp.Controllers;

public class OrderController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Details()
    {
        return View();
    }
}

using Microsoft.AspNetCore.Mvc;

namespace ClientApp.Controllers;

public class CartController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClientApp.ViewModels.CommonViewModels;

namespace ClientApp.Controllers;

public sealed class ErrorController : Controller
{
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
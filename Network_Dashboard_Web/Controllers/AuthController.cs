using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Network_Dashboard_Web.Models;

namespace Network_Dashboard_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
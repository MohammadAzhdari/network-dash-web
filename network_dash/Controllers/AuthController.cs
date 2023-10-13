using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using network_dash.Models;

namespace network_dash.Controllers
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
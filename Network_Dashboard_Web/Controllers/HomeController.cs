using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Network_Dashboard_Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace Network_Dashboard_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult ServerStatus()
        {
            return View();
        }

        [Authorize]
        public IActionResult Bandwidth()
        {
            return View();
        }

        [Authorize]
        public IActionResult Health()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
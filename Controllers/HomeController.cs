using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using LibraryManagement.Models; 

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Customer> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<Customer> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var user = await _userManager.GetUserAsync(User);
                ViewBag.Username = user?.Name ?? user?.UserName;
            }

            return View();
        }

        public IActionResult Privacy() => View();
        
        //for live chat
        public IActionResult Chat()
        {
            ViewBag.Username = User.Identity?.Name ?? "anonymous visitor";
            return View();
        }
       
        //this is for testing the error controller
        public IActionResult ThrowException()
        {
            throw new NullReferenceException("Simulated exception for testing.");
        }

    }
}
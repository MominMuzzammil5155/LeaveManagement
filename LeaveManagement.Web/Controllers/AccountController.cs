using LeaveManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService = new();

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            var registeredUser = await _apiService.RegisterAsync(user);
            if (registeredUser == null)
            {
                ViewBag.Error = "Registration failed.";
                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _apiService.LoginAsync(email, password);
            if (user == null)
            {
                ViewBag.Error = "Invalid credentials.";
                return View();
            }

            // Store session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("UserName", user.Name);

            if (user.Role == "Manager")
                return RedirectToAction("ViewPendingRequests", "Manager");
            else
                return RedirectToAction("ApplyLeave", "Employee");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

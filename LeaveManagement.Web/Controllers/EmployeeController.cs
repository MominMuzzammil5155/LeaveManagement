using LeaveManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApiService _apiService = new();

        public IActionResult ApplyLeave()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyLeave(LeaveRequest leave)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            leave.UserId = userId.Value;
            leave.Status = "Pending";

            var result = await _apiService.ApplyLeaveAsync(leave);
            if (!result)
            {
                ViewBag.Error = "Failed to apply leave.";
                return View(leave);
            }

            return RedirectToAction("ViewStatus");
        }

        public async Task<IActionResult> ViewStatus()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var leaves = await _apiService.GetLeavesByUserAsync(userId.Value);
            return View(leaves);
        }
    }
}

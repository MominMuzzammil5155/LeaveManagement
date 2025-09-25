using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Web.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ApiService _apiService = new();

        public async Task<IActionResult> ViewPendingRequests()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Manager") return Unauthorized();

            var pendingLeaves = await _apiService.GetPendingLeavesAsync();
            return View(pendingLeaves);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int leaveId, string status)
        {
            var result = await _apiService.UpdateLeaveStatusAsync(leaveId, status);
            if (!result)
            {
                TempData["Error"] = "Failed to update status.";
            }

            return RedirectToAction("ViewPendingRequests");
        }
    }
}

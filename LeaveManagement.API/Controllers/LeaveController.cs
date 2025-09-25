using LeaveManagement.DAL;
using LeaveManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly LeaveDbContext _context;

        public LeaveController(LeaveDbContext context)
        {
            _context = context;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyLeave(LeaveRequest request)
        {
            _context.LeaveRequests.Add(request);
            await _context.SaveChangesAsync();
            return Ok(request);
        }

        [HttpGet("status/{userId}")]
        public async Task<IActionResult> GetLeaveStatus(int userId)
        {
            var leaves = await _context.LeaveRequests
                .Where(l => l.UserId == userId)
                .ToListAsync();
            return Ok(leaves);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingLeaves()
        {
            var pending = await _context.LeaveRequests
                .Where(l => l.Status == "Pending")
                .Include(l => l.User)
                .ToListAsync();
            return Ok(pending);
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateLeaveStatus([FromBody] UpdateLeaveStatusRequest req)
        {
            var leave = await _context.LeaveRequests.FindAsync(req.LeaveId);
            if (leave == null) return NotFound();

            leave.Status = req.Status;
            await _context.SaveChangesAsync();
            return Ok(leave);
        }
    }

    public class UpdateLeaveStatusRequest
    {
        public int LeaveId { get; set; }
        public string Status { get; set; } // "Approved" or "Rejected"
    }
}

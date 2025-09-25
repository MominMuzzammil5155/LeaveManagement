using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EmailAddress(ErrorMessage ="Email is not in format")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public virtual ICollection<LeaveRequest>? LeaveRequests { get; set; }
    }
}

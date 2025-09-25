namespace LeaveManagement.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } = "Pending"; // Pending / Approved / Rejected

        public virtual User User { get; set; }
    }
}

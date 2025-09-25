using LeaveManagement.Models;

namespace LeaveManagement.Web
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7161/api/")
            };
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var response = await _client.PostAsJsonAsync("auth/login", new { Email = email, Password = password });
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<User?> RegisterAsync(User user)
        {
            var response = await _client.PostAsJsonAsync("auth/register", user);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<bool> ApplyLeaveAsync(LeaveRequest leave)
        {
            var response = await _client.PostAsJsonAsync("leave/apply", leave);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<LeaveRequest>> GetLeavesByUserAsync(int userId)
        {
            var response = await _client.GetAsync($"leave/status/{userId}");
            if (!response.IsSuccessStatusCode) return new();

            return await response.Content.ReadFromJsonAsync<List<LeaveRequest>>();
        }

        public async Task<List<LeaveRequest>> GetPendingLeavesAsync()
        {
            var response = await _client.GetAsync("leave/pending");
            if (!response.IsSuccessStatusCode) return new();

            return await response.Content.ReadFromJsonAsync<List<LeaveRequest>>();
        }

        public async Task<bool> UpdateLeaveStatusAsync(int leaveId, string status)
        {
            var response = await _client.PostAsJsonAsync("leave/update-status", new
            {
                LeaveId = leaveId,
                Status = status
            });

            return response.IsSuccessStatusCode;
        }
    }
}

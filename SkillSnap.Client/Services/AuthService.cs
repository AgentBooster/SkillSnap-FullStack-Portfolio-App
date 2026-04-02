using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SkillSnap.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResult>();
                return result.Token;
            }
            return null;
        }
        
        public async Task<bool> RegisterAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", new { Email = email, Password = password });
            return response.IsSuccessStatusCode;
        }

        private class LoginResult { public string Token { get; set; } }
    }
}

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SkillSnap.Api.Models;

namespace SkillSnap.Client.Services
{
    public class SkillService
    {
        private readonly HttpClient _httpClient;

        public SkillService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Skill>> GetSkillsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Skill>>("api/skills");
        }

        public async Task AddSkillAsync(Skill newSkill)
        {
            await _httpClient.PostAsJsonAsync("api/skills", newSkill);
        }
    }
}

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SkillSnap.Api.Models; // Assumes shared or copies

namespace SkillSnap.Client.Services
{
    public class ProjectService
    {
        private readonly HttpClient _httpClient;

        public ProjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Project>>("api/projects");
        }

        public async Task AddProjectAsync(Project newProject)
        {
            await _httpClient.PostAsJsonAsync("api/projects", newProject);
        }
    }
}

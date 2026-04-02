using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SkillSnap.Client.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SkillSnap.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });

            builder.Services.AddScoped<ProjectService>();
            builder.Services.AddScoped<SkillService>();
            builder.Services.AddScoped<AuthService>();

            await builder.Build().RunAsync();
        }
    }
}

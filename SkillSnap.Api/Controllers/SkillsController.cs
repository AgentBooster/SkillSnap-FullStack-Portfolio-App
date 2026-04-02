using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SkillSnap.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillSnap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly SkillSnapContext _context;
        private readonly IMemoryCache _cache;

        public SkillsController(SkillSnapContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetSkills()
        {
            if (!_cache.TryGetValue("skills", out List<Skill> skills))
            {
                skills = await _context.Skills.AsNoTracking().ToListAsync();
                _cache.Set("skills", skills, TimeSpan.FromMinutes(5));
            }
            return Ok(skills);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddSkill(Skill skill)
        {
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
            _cache.Remove("skills");
            return Ok(skill);
        }
    }
}

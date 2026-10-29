using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimalQuests.Data;
using AnimalQuests.Models;

namespace AnimalQuests.Controllers
{
    public class MissionController : Controller
    {
        private readonly AppDbContext _context;

        public MissionController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var missions = await _context.Missions
                .Include(m => m.AssignedBiome)
                .ToListAsync();

            return View(missions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var mission = await _context.Missions
                .Include(m => m.AssignedBiome)
                .Include(m => m.AnimalMissions)
                    .ThenInclude(am => am.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mission == null)
            {
                return NotFound();
            }

            return View(mission);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadBiomesAsync();

            return View(new Mission());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mission mission)
        {
            bool biomeExists = await _context.Biomes
                .AnyAsync(b => b.Id == mission.BiomeId);

            if (!biomeExists)
            {
                ModelState.AddModelError(
                    nameof(mission.BiomeId),
                    "Wybierz istniejący biom."
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadBiomesAsync();
                return View(mission);
            }

            _context.Missions.Add(mission);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var mission = await _context.Missions.FindAsync(id);

            if (mission == null)
            {
                return NotFound();
            }

            return View(mission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Mission mission)
        {
            if (id != mission.Id)
            {
                return BadRequest();
            }

            var existingMission = await _context.Missions.FindAsync(id);

            if (existingMission == null)
            {
                return NotFound();
            }

            if (mission.Status < 1 || mission.Status > 3)
            {
                ModelState.AddModelError(
                    nameof(mission.Status),
                    "Wybierz prawidłowy status misji."
                );

                return View(existingMission);
            }

            existingMission.Status = mission.Status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private async Task LoadBiomesAsync()
        {
            ViewBag.Biomes = new SelectList(
                await _context.Biomes.ToListAsync(),
                "Id",
                "Name"
            );
        }
    }
}

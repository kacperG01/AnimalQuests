using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using testing.Data;
using testing.Models;

namespace AnimalQuests.Controllers
{
    public class AnimalController : Controller
    {
        private readonly AppDbContext _context;
        public AnimalController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var animals = await _context.Animals
                .Include(a => a.Biome)
                .ToListAsync();
            return View(animals);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadBiomesAsync();
            return View(new Animal());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Animal animal)
        {
            await LoadBiomesAsync();
            if (!ModelState.IsValid)
            {
                return View(animal);
            }

            await _context.Animals.AddAsync(animal);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            await LoadBiomesAsync();
            var animal = await _context.Animals.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Animal animal)
        {
            await LoadBiomesAsync();
            if (id != animal.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(animal);
            }

            _context.Animals.Update(animal);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _context.Animals.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _context.Animals.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> AssignToMission(int id)
        {
            var animal = await _context.Animals
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            if (!await _context.Missions.AnyAsync())
            {
                TempData["Error"] = "Najpierw utwórz przynajmniej jedną misję.";

                return RedirectToAction(nameof(Index));
            }

            await LoadMissionsAsync();

            ViewBag.AnimalName = animal.Name;

            var model = new AnimalMission
            {
                AnimalId = animal.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToMission(AnimalMission animalMission)
        {
            bool animalExists = await _context.Animals
                .AnyAsync(a => a.Id == animalMission.AnimalId);

            bool missionExists = await _context.Missions
                .AnyAsync(m => m.Id == animalMission.MissionId);

            if (!animalExists || !missionExists)
            {
                return NotFound();
            }

            bool alreadyAssigned = await _context.AnimalMissions
                .AnyAsync(am =>
                    am.AnimalId == animalMission.AnimalId &&
                    am.MissionId == animalMission.MissionId);

            if (alreadyAssigned)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "To zwierzę jest już przypisane do tej misji."
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadMissionsAsync();

                var animal = await _context.Animals
                    .FirstAsync(a => a.Id == animalMission.AnimalId);

                ViewBag.AnimalName = animal.Name;

                return View(animalMission);
            }

            _context.AnimalMissions.Add(animalMission);

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

        private async Task LoadMissionsAsync()
        {
            var missions = await _context.Missions
                .Include(m => m.AssignedBiome)
                .ToListAsync();

            ViewBag.Missions = new SelectList(
                missions.Select(m => new
                {
                    m.Id,
                    DisplayName = $"{m.Objective} — {m.AssignedBiome.Name}"
                }),
                "Id",
                "DisplayName"
            );
        }
    }
}

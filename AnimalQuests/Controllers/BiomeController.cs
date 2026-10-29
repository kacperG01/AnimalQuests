using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnimalQuests.Data;
using AnimalQuests.Models;

namespace AnimalQuests.Controllers
{
    public class BiomeController : Controller
    {
        private readonly AppDbContext _context;
        public BiomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var biomes = await _context.Biomes.ToListAsync();
            return View(biomes);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new Biome());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Biome biome)
        {
            if (!ModelState.IsValid)
            {
                return View(biome);
            }

            await _context.Biomes.AddAsync(biome);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var biome = await _context.Biomes.FindAsync(id);

            if (biome == null)
            {
                return NotFound();
            }

            return View(biome);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Biome biome)
        {
            if (id != biome.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(biome);
            }

            _context.Biomes.Update(biome);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var biome = await _context.Biomes.FindAsync(id);

            if (biome == null)
            {
                return NotFound();
            }

            return View(biome);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var biome = await _context.Biomes.FindAsync(id);

            if (biome == null)
            {
                return NotFound();
            }

            _context.Biomes.Remove(biome);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

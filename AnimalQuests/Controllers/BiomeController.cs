using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testing.Data;
using testing.Models;

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
            var employees = await _context.Biomes.ToListAsync();
            return View(employees);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new Biome());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Biome employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            await _context.Biomes.AddAsync(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Biomes.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Biome employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            _context.Biomes.Update(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Biomes.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Biomes.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Biomes.Remove(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

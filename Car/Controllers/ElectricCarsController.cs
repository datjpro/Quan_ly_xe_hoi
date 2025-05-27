using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Car.Data;
using Car.Models;

namespace Car.Controllers
{
    public class ElectricCarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ElectricCarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ElectricCars
        public async Task<IActionResult> Index()
        {
            var electricCars = await _context.ElectricCars
                .Include(e => e.Branch)
                .ToListAsync();
            return View(electricCars);
        }

        // GET: ElectricCars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricCar = await _context.ElectricCars.FindAsync(id);
            if (electricCar == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", electricCar.BranchId);
            return View(electricCar);
        }

        // POST: ElectricCars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Battery,RangeKm,Model,BranchId,Price,ImageUrl")] ElectricCar electricCar)
        {
            if (id != electricCar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(electricCar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectricCarExists(electricCar.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", electricCar.BranchId);
            return View(electricCar);
        }

        // GET: ElectricCars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricCar = await _context.ElectricCars
                .Include(e => e.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (electricCar == null)
            {
                return NotFound();
            }

            return View(electricCar);
        }

        // POST: ElectricCars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var electricCar = await _context.ElectricCars.FindAsync(id);
            if (electricCar != null)
            {
                _context.ElectricCars.Remove(electricCar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectricCarExists(int id)
        {
            return _context.ElectricCars.Any(e => e.Id == id);
        }
    }
}

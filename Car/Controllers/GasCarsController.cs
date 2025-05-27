using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Car.Data;
using Car.Models;

namespace Car.Controllers
{
    public class GasCarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GasCarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GasCars
        public async Task<IActionResult> Index()
        {
            var gasCars = await _context.GasCars
                .Include(g => g.Branch)
                .ToListAsync();
            return View(gasCars);
        }

        // GET: GasCars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasCar = await _context.GasCars.FindAsync(id);
            if (gasCar == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", gasCar.BranchId);
            return View(gasCar);
        }

        // POST: GasCars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FuelEff,Model,BranchId,Price,ImageUrl")] GasCar gasCar)
        {
            if (id != gasCar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gasCar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GasCarExists(gasCar.Id))
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", gasCar.BranchId);
            return View(gasCar);
        }

        // GET: GasCars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasCar = await _context.GasCars
                .Include(g => g.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gasCar == null)
            {
                return NotFound();
            }

            return View(gasCar);
        }

        // POST: GasCars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gasCar = await _context.GasCars.FindAsync(id);
            if (gasCar != null)
            {
                _context.GasCars.Remove(gasCar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GasCarExists(int id)
        {
            return _context.GasCars.Any(e => e.Id == id);
        }
    }
}

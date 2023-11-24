using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Actual.Models;

namespace Actual.Controllers
{
    public class CategoriaAsesoramientoesController : Controller
    {
        private readonly CrowdFundingContext _context;

        public CategoriaAsesoramientoesController(CrowdFundingContext context)
        {
            _context = context;
        }

        // GET: CategoriaAsesoramientoes
        public async Task<IActionResult> Index()
        {
              return _context.CategoriaAsesoramientos != null ? 
                          View(await _context.CategoriaAsesoramientos.ToListAsync()) :
                          Problem("Entity set 'CrowdFundingContext.CategoriaAsesoramientos'  is null.");
        }

        // GET: CategoriaAsesoramientoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoriaAsesoramientos == null)
            {
                return NotFound();
            }

            var categoriaAsesoramiento = await _context.CategoriaAsesoramientos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoriaAsesoramiento == null)
            {
                return NotFound();
            }

            return View(categoriaAsesoramiento);
        }

        // GET: CategoriaAsesoramientoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriaAsesoramientoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreAsesoramiento,Estado,FechaRegistro,FechaActualizacion")] CategoriaAsesoramiento categoriaAsesoramiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoriaAsesoramiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaAsesoramiento);
        }

        // GET: CategoriaAsesoramientoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoriaAsesoramientos == null)
            {
                return NotFound();
            }

            var categoriaAsesoramiento = await _context.CategoriaAsesoramientos.FindAsync(id);
            if (categoriaAsesoramiento == null)
            {
                return NotFound();
            }
            return View(categoriaAsesoramiento);
        }

        // POST: CategoriaAsesoramientoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreAsesoramiento,Estado,FechaRegistro,FechaActualizacion")] CategoriaAsesoramiento categoriaAsesoramiento)
        {
            if (id != categoriaAsesoramiento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoriaAsesoramiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaAsesoramientoExists(categoriaAsesoramiento.Id))
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
            return View(categoriaAsesoramiento);
        }

        // GET: CategoriaAsesoramientoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoriaAsesoramientos == null)
            {
                return NotFound();
            }

            var categoriaAsesoramiento = await _context.CategoriaAsesoramientos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoriaAsesoramiento == null)
            {
                return NotFound();
            }

            return View(categoriaAsesoramiento);
        }

        // POST: CategoriaAsesoramientoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoriaAsesoramientos == null)
            {
                return Problem("Entity set 'CrowdFundingContext.CategoriaAsesoramientos'  is null.");
            }
            var categoriaAsesoramiento = await _context.CategoriaAsesoramientos.FindAsync(id);
            if (categoriaAsesoramiento != null)
            {
                _context.CategoriaAsesoramientos.Remove(categoriaAsesoramiento);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaAsesoramientoExists(int id)
        {
          return (_context.CategoriaAsesoramientos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Actual.Models;

namespace Actual.Controllers
{
    public class ImagensController : Controller
    {
        private readonly CrowdFundingContext _context;

        public ImagensController(CrowdFundingContext context)
        {
            _context = context;
        }

        // GET: Imagens
        public async Task<IActionResult> Index()
        {
            var crowdFundingContext = _context.Imagens.Include(i => i.IdCampañaNavigation);
            return View(await crowdFundingContext.ToListAsync());
        }

        // GET: Imagens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Imagens == null)
            {
                return NotFound();
            }

            var imagen = await _context.Imagens
                .Include(i => i.IdCampañaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imagen == null)
            {
                return NotFound();
            }

            return View(imagen);
        }

        // GET: Imagens/Create
        public IActionResult Create()
        {
            ViewData["IdCampaña"] = new SelectList(_context.Campañas, "Id", "Id");
            return View();
        }

        // POST: Imagens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ruta,IdCampaña,Estado,FechaRegistro,FechaActualizacion")] Imagen imagen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imagen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCampaña"] = new SelectList(_context.Campañas, "Id", "NombreCampaña", imagen.IdCampaña);
            return View(imagen);
        }

        // GET: Imagens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Imagens == null)
            {
                return NotFound();
            }

            var imagen = await _context.Imagens.FindAsync(id);
            if (imagen == null)
            {
                return NotFound();
            }
            ViewData["IdCampaña"] = new SelectList(_context.Campañas, "Id", "NombreCampaña", imagen.IdCampaña);
            return View(imagen);
        }

        // POST: Imagens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ruta,IdCampaña,Estado,FechaRegistro,FechaActualizacion")] Imagen imagen)
        {
            if (id != imagen.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imagen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagenExists(imagen.Id))
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
            ViewData["IdCampaña"] = new SelectList(_context.Campañas, "Id", "NombreCampaña", imagen.IdCampaña);
            return View(imagen);
        }

        // GET: Imagens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Imagens == null)
            {
                return NotFound();
            }

            var imagen = await _context.Imagens
                .Include(i => i.IdCampañaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imagen == null)
            {
                return NotFound();
            }

            return View(imagen);
        }

        // POST: Imagens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int idCampaña)
        {
            if (_context.Imagens == null)
            {
                return Problem("Entity set 'CrowdFundingContext.Imagens'  is null.");
            }
            var imagen = await _context.Imagens.FindAsync(id);
            if (imagen != null)
            {
                _context.Imagens.Remove(imagen);
            }
            
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Edit", "Campaña", new { id = idCampaña });
        }

        private bool ImagenExists(int id)
        {
          return (_context.Imagens?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

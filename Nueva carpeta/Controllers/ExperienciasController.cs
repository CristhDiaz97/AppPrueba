using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEmpleo.Models;

namespace WebEmpleo.Controllers
{
    public class ExperienciasController : Controller
    {
        private readonly DbPruebaContext _context;

        public ExperienciasController(DbPruebaContext context)
        {
            _context = context;
        }

        // GET: Experiencias
        public async Task<IActionResult> Index()
        {
            var dbPruebaContext = _context.Experiencias.Include(e => e.IdPersonaNavigation);
            return View(await dbPruebaContext.ToListAsync());
        }

        // GET: Experiencias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Experiencias == null)
            {
                return NotFound();
            }

            var experiencia = await _context.Experiencias
                .Include(e => e.IdPersonaNavigation)
                .FirstOrDefaultAsync(m => m.IdExperienciaLaboral == id);
            if (experiencia == null)
            {
                return NotFound();
            }

            return View(experiencia);
        }

        // GET: Experiencias/Create
        public IActionResult Create()
        {
            ViewData["IdPersona"] = new SelectList(_context.People, "IdPersona", "IdPersona");
            return View();
        }

        // POST: Experiencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdExperienciaLaboral,IdPersona,FchInicio,FchFin,Descripcion,Estado,FchCreate,FchUpdate")] Experiencia experiencia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(experiencia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPersona"] = new SelectList(_context.People, "IdPersona", "IdPersona", experiencia.IdPersona);
            return View(experiencia);
        }

        // GET: Experiencias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Experiencias == null)
            {
                return NotFound();
            }

            var experiencia = await _context.Experiencias.FindAsync(id);
            if (experiencia == null)
            {
                return NotFound();
            }
            ViewData["IdPersona"] = new SelectList(_context.People, "IdPersona", "IdPersona", experiencia.IdPersona);
            return View(experiencia);
        }

        // POST: Experiencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdExperienciaLaboral,IdPersona,FchInicio,FchFin,Descripcion,Estado,FchCreate,FchUpdate")] Experiencia experiencia)
        {
            if (id != experiencia.IdExperienciaLaboral)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(experiencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExperienciaExists(experiencia.IdExperienciaLaboral))
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
            ViewData["IdPersona"] = new SelectList(_context.People, "IdPersona", "IdPersona", experiencia.IdPersona);
            return View(experiencia);
        }

        // GET: Experiencias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Experiencias == null)
            {
                return NotFound();
            }

            var experiencia = await _context.Experiencias
                .Include(e => e.IdPersonaNavigation)
                .FirstOrDefaultAsync(m => m.IdExperienciaLaboral == id);
            if (experiencia == null)
            {
                return NotFound();
            }

            return View(experiencia);
        }

        // POST: Experiencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Experiencias == null)
            {
                return Problem("Entity set 'DbPruebaContext.Experiencias'  is null.");
            }
            var experiencia = await _context.Experiencias.FindAsync(id);
            if (experiencia != null)
            {
                _context.Experiencias.Remove(experiencia);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExperienciaExists(int id)
        {
          return (_context.Experiencias?.Any(e => e.IdExperienciaLaboral == id)).GetValueOrDefault();
        }
    }
}

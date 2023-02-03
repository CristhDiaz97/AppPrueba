using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEmpleo.Models;

namespace WebEmpleo.Controllers
{
    public class VacanteController : Controller
    {
        private readonly DbPruebaContext _context;

        public VacanteController(DbPruebaContext context)
        {
            _context = context;
        }

        // GET: Vacante
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Error");
            }
            if (!User.IsInRole("Empresa"))
            {
                return Redirect("/Home/Error");
            }
            return _context.Vacantes != null ? 
                          View(await _context.Vacantes.ToListAsync()) :
                          Problem("Entity set 'DbPruebaContext.Vacantes'  is null.");
        }

        // GET: Vacante/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Error");
            }
            if (!User.IsInRole("Empresa"))
            {
                return Redirect("/Home/Error");
            }
            if (id == null || _context.Vacantes == null)
            {
                return NotFound();
            }

            var vacante = await _context.Vacantes
                .FirstOrDefaultAsync(m => m.IdVacante == id);
            if (vacante == null)
            {
                return NotFound();
            }

            return View(vacante);
        }

        // GET: Vacante/Create
        public IActionResult Create()
        {
            
            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "IdEmpresa");
            
            return View();
        }

        // POST: Vacante/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVacante,IdEmpresa,Descripcion,Localizacion,Estudios,Experiencia,TipoContrato,CantidadVacantes,FchVencimiento,Estado,FchCreate,FchUpdate")] Vacante vacante)
        {
            var user = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value;
            var emp = _context.Empresas.Where(x => x.IdUsuarios == user).FirstOrDefaultAsync().Result;
            vacante.IdEmpresa = emp.IdEmpresa; 
            vacante.Estado = true;
            vacante.FchCreate = DateTime.Now;
            vacante.FchUpdate = DateTime.Now;
            ModelState.Remove("IdEmpresa");
            ModelState.Remove("Estado");
            ModelState.Remove("FchCreate");
            ModelState.Remove("FchUpdate");
            if (ModelState.IsValid)
            {
                _context.Add(vacante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEmpresa"] = new SelectList(_context.AspNetUsers, "Id", "Id", vacante.IdEmpresa);
            return View(vacante);
        }

        // GET: Vacante/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.Vacantes == null)
            {
                return NotFound();
            }

            var vacante = await _context.Vacantes.FindAsync(id);
            if (vacante == null)
            {
                return NotFound();
            }
            return View(vacante);
        }

        // POST: Vacante/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVacante,IdEmpresa,Descripcion,Localizacion,Estudios,Experiencia,TipoContrato,CantidadVacantes,FchVencimiento,Estado,FchCreate,FchUpdate")] Vacante vacante)
        {
            if (id != vacante.IdVacante)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacanteExists(vacante.IdVacante))
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
            return View(vacante);
        }

        // GET: Vacante/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Error");
            }
            if (!User.IsInRole("Empresa"))
            {
                return Redirect("/Home/Error");
            }
            if (id == null || _context.Vacantes == null)
            {
                return NotFound();
            }

            var vacante = await _context.Vacantes
                .FirstOrDefaultAsync(m => m.IdVacante == id);
            if (vacante == null)
            {
                return NotFound();
            }

            return View(vacante);
        }

        // POST: Vacante/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vacantes == null)
            {
                return Problem("Entity set 'DbPruebaContext.Vacantes'  is null.");
            }
            var vacante = await _context.Vacantes.FindAsync(id);
            if (vacante != null)
            {
                _context.Vacantes.Remove(vacante);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacanteExists(int id)
        {
          return (_context.Vacantes?.Any(e => e.IdVacante == id)).GetValueOrDefault();
        }
    }
}

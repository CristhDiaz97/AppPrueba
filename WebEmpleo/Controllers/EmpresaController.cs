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
    public class EmpresaController : Controller
    {
        private readonly DbPruebaContext _context;

        public EmpresaController(DbPruebaContext context)
        {
            _context = context;
        }

        // GET: Empresa
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
            var dbPruebaContext = _context.Empresas.Include(e => e.IdUsuariosNavigation).Include(e => e.IndustriaNavigation);
            return View(await dbPruebaContext.ToListAsync());
        }

        // GET: Empresa/Details/5
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
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .Include(e => e.IdUsuariosNavigation)
                .Include(e => e.IndustriaNavigation)
                .FirstOrDefaultAsync(m => m.IdEmpresa == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Empresa/Create
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Error");
            }
            if (!User.IsInRole("Empresa"))
            {
                return Redirect("/Home/Error");
            }
            ViewData["IdUsuarios"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["Industria"] = new SelectList(_context.Industria, "IdIndustria", "Descripcion");
            
            //var empresa = _context.Empresas.Where(x => x.IdEmpresa == user).FirstOrDefaultAsync().Result;
            //if (empresa != null)
            //{
            //    return Redirect("Edit/" + empresa.IdPersona);
            //}

            return View();
        }

        // POST: Empresa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa,IdUsuarios,Ciudad,Nombre,Industria,Telefono,Direccion,CantidadEmpleados,CantidadVacantes,Estado,FchCreate,FchUpdate")] Empresa empresa)
        {
            var user = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value;
            empresa.IdUsuarios = user;
            empresa.Estado = true;
            empresa.FchCreate = DateTime.Now;
            empresa.FchUpdate = DateTime.Now;
            ModelState.Remove("IdUsuarios");
            ModelState.Remove("Estado");
            ModelState.Remove("FchCreate");
            ModelState.Remove("FchUpdate");
            if (ModelState.IsValid)
            {
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUsuarios"] = new SelectList(_context.AspNetUsers, "Id", "Id", empresa.IdUsuarios);
            ViewData["Industria"] = new SelectList(_context.Industria, "IdIndustria", "IdIndustria", empresa.Industria);
            return View(empresa);
        }

        // GET: Empresa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Error");
            }
            if (!User.IsInRole("Empresa"))
            {
                return Redirect("/Home/Error");
            }
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            ViewData["IdUsuarios"] = new SelectList(_context.AspNetUsers, "Id", "Id", empresa.IdUsuarios);
            ViewData["Industria"] = new SelectList(_context.Industria, "IdIndustria", "IdIndustria", empresa.Industria);
            return View(empresa);
        }

        // POST: Empresa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmpresa,IdUsuarios,Ciudad,Nombre,Industria,Telefono,Direccion,CantidadEmpleados,CantidadVacantes,Estado,FchCreate,FchUpdate")] Empresa empresa)
        {
            if (id != empresa.IdEmpresa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.IdEmpresa))
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
            ViewData["IdUsuarios"] = new SelectList(_context.AspNetUsers, "Id", "Id", empresa.IdUsuarios);
            ViewData["Industria"] = new SelectList(_context.Industria, "IdIndustria", "IdIndustria", empresa.Industria);
            return View(empresa);
        }

        // GET: Empresa/Delete/5
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
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .Include(e => e.IdUsuariosNavigation)
                .Include(e => e.IndustriaNavigation)
                .FirstOrDefaultAsync(m => m.IdEmpresa == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empresas == null)
            {
                return Problem("Entity set 'DbPruebaContext.Empresas'  is null.");
            }
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa != null)
            {
                _context.Empresas.Remove(empresa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
          return (_context.Empresas?.Any(e => e.IdEmpresa == id)).GetValueOrDefault();
        }
    }
}

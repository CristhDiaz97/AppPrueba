using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEmpleo.Models;

namespace WebEmpleo.Controllers
{
    public class PeopleController : Controller
    {
        private readonly DbPruebaContext _context;

        public PeopleController(DbPruebaContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {                
                return Redirect("/Home/Error");
            }
            if (!User.IsInRole("Persona"))
            {
                return Redirect("/Home/Error");
            }
            var dbPruebaContext = _context.People.Include(p => p.IdNivelEducativoNavigation);
            return View(await dbPruebaContext.ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Error");
            }
            if (!User.IsInRole("Persona"))
            {
                return Redirect("/Home/Error");
            }
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .Include(p => p.IdNivelEducativoNavigation)
                .FirstOrDefaultAsync(m => m.IdPersona == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Error");
            }
            if (!User.IsInRole("Persona"))
            {
                return Redirect("/Home/Error");
            }
            var user = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value;
            var person = _context.People.Where(x => x.IdUsuarios == user).FirstOrDefaultAsync().Result;
            if (person != null)
            {
                return Redirect("Edit/" + person.IdPersona);
            }
            ViewData["IdNivelEducativo"] = new SelectList(_context.NivelesEducativos, "IdNivelEducativo", "Descripcion");
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPersona,IdUsuarios,Nombres,Apellidos,Edad,Telefono,IdNivelEducativo,Notas,Estado,FchCreate,FchUpdate")] Person person)
        {
            if (person.IdNivelEducativo != 0)
            {
                person.IdNivelEducativoNavigation = _context.NivelesEducativos.Where(x => x.IdNivelEducativo == person.IdNivelEducativo).First();
                var user = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value;
                person.IdUsuarios = user;
                person.Estado = true;
                person.FchCreate = DateTime.Now;
                person.FchUpdate = DateTime.Now;
                ModelState.Remove("IdNivelEducativoNavigation");
                ModelState.Remove("IdUsuarios");
                ModelState.Remove("Estado");
                ModelState.Remove("FchCreate");
                ModelState.Remove("FchUpdate");
            }

            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdNivelEducativo"] = new SelectList(_context.NivelesEducativos, "IdNivelEducativo", "Descripcion", person.IdNivelEducativo);
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Error");
            }
            if (!User.IsInRole("Persona"))
            {
                return Redirect("/Home/Error");
            }
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            ViewData["IdNivelEducativo"] = new SelectList(_context.NivelesEducativos, "IdNivelEducativo", "Descripcion", person.IdNivelEducativo);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPersona,IdUsuarios,Nombres,Apellidos,Edad,Telefono,IdNivelEducativo,Notas,Estado,FchCreate,FchUpdate")] Person person)
        {
            if (id != person.IdPersona)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.IdPersona))
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
            ViewData["IdNivelEducativo"] = new SelectList(_context.NivelesEducativos, "IdNivelEducativo", "IdNivelEducativo", person.IdNivelEducativo);
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Error");
            }
            if (!User.IsInRole("Persona"))
            {
                return Redirect("/Home/Error");
            }
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .Include(p => p.IdNivelEducativoNavigation)
                .FirstOrDefaultAsync(m => m.IdPersona == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.People == null)
            {
                return Problem("Entity set 'DbPruebaContext.People'  is null.");
            }
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.People.Remove(person);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return (_context.People?.Any(e => e.IdPersona == id)).GetValueOrDefault();
        }
    }
}

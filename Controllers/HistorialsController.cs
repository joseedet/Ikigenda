using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ikigenda.Data;
using Ikigenda.Models;

namespace Ikigenda.Controllers
{
    public class HistorialsController : Controller
    {
        private readonly AgendaDbContext _context;

        public HistorialsController(AgendaDbContext context)
        {
            _context = context;
        }

        // GET: Historials
        public async Task<IActionResult> Index()
        {
            var agendaDbContext = _context.Historials.Include(h => h.Cliente).Include(h => h.Planin).Include(h => h.TipoServicio);
            return View(await agendaDbContext.ToListAsync());
        }

        // GET: Historials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historial = await _context.Historials
                .Include(h => h.Cliente)
                .Include(h => h.Planin)
                .Include(h => h.TipoServicio)
                .FirstOrDefaultAsync(m => m.HistorialId == id);
            if (historial == null)
            {
                return NotFound();
            }

            return View(historial);
        }

        // GET: Historials/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Documento");
            ViewData["PlaninId"] = new SelectList(_context.Planins, "PlaninId", "PlaninId");
            ViewData["TipoServicioId"] = new SelectList(_context.TipoServicios, "TipoServicioId", "TipoServicioId");
            return View();
        }

        // POST: Historials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HistorialId,PlaninId,ClienteId,TipoServicioId,Descripcion,Observaciones,DateLocal")] Historial historial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Documento", historial.ClienteId);
            ViewData["PlaninId"] = new SelectList(_context.Planins, "PlaninId", "PlaninId", historial.PlaninId);
            ViewData["TipoServicioId"] = new SelectList(_context.TipoServicios, "TipoServicioId", "TipoServicioId", historial.TipoServicioId);
            return View(historial);
        }

        // GET: Historials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historial = await _context.Historials.FindAsync(id);
            if (historial == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Documento", historial.ClienteId);
            ViewData["PlaninId"] = new SelectList(_context.Planins, "PlaninId", "PlaninId", historial.PlaninId);
            ViewData["TipoServicioId"] = new SelectList(_context.TipoServicios, "TipoServicioId", "TipoServicioId", historial.TipoServicioId);
            return View(historial);
        }

        // POST: Historials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HistorialId,PlaninId,ClienteId,TipoServicioId,Descripcion,Observaciones,DateLocal")] Historial historial)
        {
            if (id != historial.HistorialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistorialExists(historial.HistorialId))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Documento", historial.ClienteId);
            ViewData["PlaninId"] = new SelectList(_context.Planins, "PlaninId", "PlaninId", historial.PlaninId);
            ViewData["TipoServicioId"] = new SelectList(_context.TipoServicios, "TipoServicioId", "TipoServicioId", historial.TipoServicioId);
            return View(historial);
        }

        // GET: Historials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historial = await _context.Historials
                .Include(h => h.Cliente)
                .Include(h => h.Planin)
                .Include(h => h.TipoServicio)
                .FirstOrDefaultAsync(m => m.HistorialId == id);
            if (historial == null)
            {
                return NotFound();
            }

            return View(historial);
        }

        // POST: Historials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historial = await _context.Historials.FindAsync(id);
            if (historial != null)
            {
                _context.Historials.Remove(historial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistorialExists(int id)
        {
            return _context.Historials.Any(e => e.HistorialId == id);
        }
    }
}

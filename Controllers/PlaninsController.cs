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
    public class PlaninsController : Controller
    {
        private readonly AgendaDbContext _context;

        public PlaninsController(AgendaDbContext context)
        {
            _context = context;
        }

        // GET: Planins
        public async Task<IActionResult> Index()
        {
            return View(await _context.Planins.ToListAsync());
        }

        // GET: Planins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planin = await _context.Planins
                .FirstOrDefaultAsync(m => m.PlaninId == id);
            if (planin == null)
            {
                return NotFound();
            }

            return View(planin);
        }

        // GET: Planins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Planins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaninId,Notas,Disponible,FechaLocal")] Planin planin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(planin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(planin);
        }

        // GET: Planins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planin = await _context.Planins.FindAsync(id);
            if (planin == null)
            {
                return NotFound();
            }
            return View(planin);
        }

        // POST: Planins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaninId,Notas,Disponible,FechaLocal")] Planin planin)
        {
            if (id != planin.PlaninId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaninExists(planin.PlaninId))
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
            return View(planin);
        }

        // GET: Planins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planin = await _context.Planins
                .FirstOrDefaultAsync(m => m.PlaninId == id);
            if (planin == null)
            {
                return NotFound();
            }

            return View(planin);
        }

        // POST: Planins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planin = await _context.Planins.FindAsync(id);
            if (planin != null)
            {
                _context.Planins.Remove(planin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaninExists(int id)
        {
            return _context.Planins.Any(e => e.PlaninId == id);
        }
    }
}

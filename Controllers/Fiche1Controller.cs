using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BANA.Data;
using BANA.Models;
using Microsoft.AspNetCore.Authorization;

namespace BANA.Controllers
{
    [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
    public class Fiche1Controller : Controller
    {
        private readonly Fiche1Context _context;

        public Fiche1Controller(Fiche1Context context)
        {
            _context = context;
        }

        // GET: Fiche1
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fiche1.ToListAsync());
        }

        // GET: Fiche1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche1 = await _context.Fiche1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fiche1 == null)
            {
                return NotFound();
            }

            return View(fiche1);
        }

        // GET: Fiche1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fiche1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroDossier,Patient,Pc,CreateDate,Trumps,infirmere,Medecin,Etat")] Fiche1 fiche1)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fiche1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fiche1);
        }

        // GET: Fiche1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche1 = await _context.Fiche1.FindAsync(id);
            if (fiche1 == null)
            {
                return NotFound();
            }
            return View(fiche1);
        }

        // POST: Fiche1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroDossier,Patient,Pc,CreateDate,Trumps,infirmere,Medecin,Etat")] Fiche1 fiche1)
        {
            if (id != fiche1.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fiche1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Fiche1Exists(fiche1.Id))
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
            return View(fiche1);
        }

        // GET: Fiche1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche1 = await _context.Fiche1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fiche1 == null)
            {
                return NotFound();
            }

            return View(fiche1);
        }

        // POST: Fiche1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fiche1 = await _context.Fiche1.FindAsync(id);
            if (fiche1 != null)
            {
                _context.Fiche1.Remove(fiche1);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Fiche1Exists(int id)
        {
            return _context.Fiche1.Any(e => e.Id == id);
        }
    }
}

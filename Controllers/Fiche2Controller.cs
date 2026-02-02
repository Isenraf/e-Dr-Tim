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
    public class Fiche2Controller : Controller
    {
        private readonly Fiche2Context _context;

        public Fiche2Controller(Fiche2Context context)
        {
            _context = context;
        }

        // GET: Fiche2
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fiche2.ToListAsync());
        }

        // GET: Fiche2/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche2 = await _context.Fiche2
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fiche2 == null)
            {
                return NotFound();
            }

            return View(fiche2);
        }

        // GET: Fiche2/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fiche2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroDossier,Patient,Pc,CreateDate,Trumps,infirmere,Medecin,Etat")] Fiche2 fiche2)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fiche2);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fiche2);
        }

        // GET: Fiche2/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche2 = await _context.Fiche2.FindAsync(id);
            if (fiche2 == null)
            {
                return NotFound();
            }
            return View(fiche2);
        }

        // POST: Fiche2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroDossier,Patient,Pc,CreateDate,Trumps,infirmere,Medecin,Etat")] Fiche2 fiche2)
        {
            if (id != fiche2.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fiche2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Fiche2Exists(fiche2.Id))
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
            return View(fiche2);
        }

        // GET: Fiche2/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche2 = await _context.Fiche2
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fiche2 == null)
            {
                return NotFound();
            }

            return View(fiche2);
        }

        // POST: Fiche2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fiche2 = await _context.Fiche2.FindAsync(id);
            if (fiche2 != null)
            {
                _context.Fiche2.Remove(fiche2);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Fiche2Exists(int id)
        {
            return _context.Fiche2.Any(e => e.Id == id);
        }
    }
}

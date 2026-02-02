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
    public class Fiche4Controller : Controller
    {
        private readonly Fiche4Context _context;

        public Fiche4Controller(Fiche4Context context)
        {
            _context = context;
        }

        // GET: Fiche4
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fiche4.ToListAsync());
        }

        // GET: Fiche4/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche4 = await _context.Fiche4
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fiche4 == null)
            {
                return NotFound();
            }

            return View(fiche4);
        }

        // GET: Fiche4/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fiche4/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroDossier,Patient,Pc,CreateDate,Trumps,infirmere,Medecin,Etat")] Fiche4 fiche4)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fiche4);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fiche4);
        }

        // GET: Fiche4/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche4 = await _context.Fiche4.FindAsync(id);
            if (fiche4 == null)
            {
                return NotFound();
            }
            return View(fiche4);
        }

        // POST: Fiche4/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroDossier,Patient,Pc,CreateDate,Trumps,infirmere,Medecin,Etat")] Fiche4 fiche4)
        {
            if (id != fiche4.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fiche4);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Fiche4Exists(fiche4.Id))
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
            return View(fiche4);
        }

        // GET: Fiche4/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche4 = await _context.Fiche4
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fiche4 == null)
            {
                return NotFound();
            }

            return View(fiche4);
        }

        // POST: Fiche4/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fiche4 = await _context.Fiche4.FindAsync(id);
            if (fiche4 != null)
            {
                _context.Fiche4.Remove(fiche4);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Fiche4Exists(int id)
        {
            return _context.Fiche4.Any(e => e.Id == id);
        }
    }
}

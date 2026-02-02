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
    public class Fiche5Controller : Controller
    {
        private readonly Fiche5Context _context;
        private readonly UserContext _context_utilisateur;

        public Fiche5Controller(Fiche5Context context, UserContext context_utilisateur)
        {
            _context = context;
            _context_utilisateur = context_utilisateur;
        }

        // GET: Fiche5
        public async Task<IActionResult> Index(string nd)
        {
            var fiche = await _context.Fiche5.Where(x => x.NumeroDossier == nd).ToListAsync();
            if (fiche.Count() == 0)
            {
                return RedirectToAction("Create","Fiche5", new { nd = nd });
            }
             ViewBag.user = Userconnected();
            return View(fiche);
        }

        // GET: Fiche5/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche5 = await _context.Fiche5
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fiche5 == null)
            {
                return NotFound();
            }
             ViewBag.user = Userconnected();
            return View(fiche5);
        }

        // GET: Fiche5/Create
        public IActionResult Create()
        {
             ViewBag.user = Userconnected();
            return View();
        }

        // POST: Fiche5/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroDossier,Patient,Pc,CreateDate,Trumps,Medecin,Etat")] Fiche5 fiche5)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fiche5);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
             ViewBag.user = Userconnected();
            return View(fiche5);
        }

        // GET: Fiche5/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche5 = await _context.Fiche5.FindAsync(id);
            if (fiche5 == null)
            {
                return NotFound();
            }
             ViewBag.user = Userconnected();
            return View(fiche5);
        }

        // POST: Fiche5/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroDossier,Patient,Pc,CreateDate,Trumps,Medecin,Etat")] Fiche5 fiche5)
        {
            if (id != fiche5.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fiche5);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Fiche5Exists(fiche5.Id))
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
             ViewBag.user = Userconnected();
            return View(fiche5);
        }

        // GET: Fiche5/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche5 = await _context.Fiche5
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fiche5 == null)
            {
                return NotFound();
            }
             ViewBag.user = Userconnected();
            return View(fiche5);
        }

        // POST: Fiche5/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fiche5 = await _context.Fiche5.FindAsync(id);
            if (fiche5 != null)
            {
                _context.Fiche5.Remove(fiche5);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public User Userconnected()
        {
            ViewData["15"] = "active";
            var user = from u in _context_utilisateur.User
                       select u;

            user = user.Where(x => x.nom == HttpContext.User.Identity.Name);
            var connectuser = user.First();
            connectuser.Initial = GetInitials(connectuser.nom);

            return connectuser;
        }

        public static string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return string.Empty;

            var words = fullName
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var initials = words
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Select(word => char.ToUpper(word[0]));

            return string.Concat(initials);
        }

        private bool Fiche5Exists(int id)
        {
            return _context.Fiche5.Any(e => e.Id == id);
        }
    }
}

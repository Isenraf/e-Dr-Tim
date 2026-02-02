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
    public class Fiche3Controller : Controller
    {
        private readonly Fiche3Context _context;
        private readonly UserContext _context_utilisateur;

        public Fiche3Controller(Fiche3Context context, UserContext context_utilisateur)
        {
            _context = context;
            _context_utilisateur = context_utilisateur;
        }

        // GET: Fiche3
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fiche3.ToListAsync());
        }

        // GET: Fiche3/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche3 = await _context.Fiche3
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fiche3 == null)
            {
                return NotFound();
            }

            return View(fiche3);
        }

        // GET: Fiche3/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fiche3/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroDossier,Patient,Pc,CreateDate,Trumps,infirmere,Medecin,Etat")] Fiche3 fiche3)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fiche3);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fiche3);
        }

        // GET: Fiche3/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche3 = await _context.Fiche3.FindAsync(id);
            if (fiche3 == null)
            {
                return NotFound();
            }
            return View(fiche3);
        }

        // POST: Fiche3/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroDossier,Patient,Pc,CreateDate,Trumps,infirmere,Medecin,Etat")] Fiche3 fiche3)
        {
            if (id != fiche3.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fiche3);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Fiche3Exists(fiche3.Id))
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
            return View(fiche3);
        }

        // GET: Fiche3/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche3 = await _context.Fiche3
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fiche3 == null)
            {
                return NotFound();
            }

            return View(fiche3);
        }

        // POST: Fiche3/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fiche3 = await _context.Fiche3.FindAsync(id);
            if (fiche3 != null)
            {
                _context.Fiche3.Remove(fiche3);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<object> Add([FromBody] Fiche3 fiche3)
        {
            fiche3.CreateDate = DateTime.Now;
            fiche3.infirmere = Userconnected().nom;
            _context.Add(fiche3);
            await _context.SaveChangesAsync();
            Console.WriteLine(fiche3.Id);
            return Json(fiche3.Id);

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

        private bool Fiche3Exists(int id)
        {
            return _context.Fiche3.Any(e => e.Id == id);
        }
    }
}

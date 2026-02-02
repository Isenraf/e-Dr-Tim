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
    public class LNMEController : Controller
    {
        private readonly LNMEContext _context;
        private readonly UserContext _context_user;

        public LNMEController(LNMEContext context, UserContext context_user)
        {
            _context = context;
            _context_user = context_user;
        }

        // GET: LNME
        public async Task<IActionResult> Index(string cat,string code)
        {
            ViewData["table"] = "ok";
            ViewBag.user = Userconnected();
            if (cat != null)
            {
                return View(await _context.LNME.Where(x => x.Catégorie.ToLower() == cat.ToLower()).ToListAsync());

            }

            if (code != null)
            {
                return View(await _context.LNME.Where(x => x.Nom.ToLower().Contains(code.ToLower())).ToListAsync());

            }
            return View(await _context.LNME.Take(200).ToListAsync());
        }


        public async Task<IActionResult> Index00()
        {
            ViewData["table"] = "ok";
            ViewBag.user = Userconnected();
            ViewBag.cat = _context.LNME.Select(x => x.Catégorie).Distinct().ToList();
            return View(await _context.LNME.ToListAsync());
        }

        // GET: LNME/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lNME = await _context.LNME
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lNME == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(lNME);
        }

        // GET: LNME/Create
        public IActionResult Create()
        {
            ViewBag.user = Userconnected();
            return View();
        }

        // POST: LNME/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,DCI,Catégorie,Dosage,Forme,actif,Create_date")] LNME lNME)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lNME);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.user = Userconnected();
            return View(lNME);
        }

        // GET: LNME/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lNME = await _context.LNME.FindAsync(id);
            if (lNME == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(lNME);
        }

        // POST: LNME/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,DCI,Catégorie,Dosage,Forme,actif,Create_date")] LNME lNME)
        {
            if (id != lNME.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lNME);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LNMEExists(lNME.Id))
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
            return View(lNME);
        }

        // GET: LNME/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lNME = await _context.LNME
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lNME == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(lNME);
        }

        // POST: LNME/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lNME = await _context.LNME.FindAsync(id);
            if (lNME != null)
            {
                _context.LNME.Remove(lNME);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public User Userconnected()
        {
            ViewData["17"] = "active";
            var user = from u in _context_user.User
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

        private bool LNMEExists(int id)
        {
            return _context.LNME.Any(e => e.Id == id);
        }
    }
}

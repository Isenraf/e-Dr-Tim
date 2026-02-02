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
    public class CimController : Controller
    {
        private readonly CimContext _context;
        private readonly UserContext _context_user;

        public CimController(CimContext context, UserContext context_user)
        {
            _context = context;
            _context_user = context_user;
        }

        // GET: Cim
        public async Task<IActionResult> Index(string cat, string code)
        {
            ViewData["table"] = "ok";
            ViewBag.user = Userconnected();
            if (code != null)
            {
                return View(await _context.Cim.Where(x => x.Code.ToLower() == code.ToLower()).ToListAsync());
            }
            else
            {
                return View(await _context.Cim.Where(x => x.Trump1 == cat).ToListAsync());
            }

        }
        public async Task<IActionResult> Index0()
        {
            ViewData["table"] = "ok";
            ViewBag.user = Userconnected();
            ViewBag.cat = _context.Cim.Select(x => x.Trump1).Distinct().ToList();
            return View(await _context.Cim.ToListAsync());
        }
        public async Task<IActionResult> Index00()
        {
            ViewData["table"] = "ok";
            ViewBag.user = Userconnected();
            ViewBag.cat = _context.Cim.Select(x => x.Trump2).Distinct().ToList();
            return View(await _context.Cim.ToListAsync());
        }

        // GET: Cim/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cim = await _context.Cim
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cim == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(cim);
        }

        // GET: Cim/Create
        public IActionResult Create()
        {
            ViewBag.user = Userconnected();
            return View();
        }

        // POST: Cim/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Code,Maladie_fr,Maladie_en,Trump1,Trump2,Trump3,Version")] Cim cim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.user = Userconnected();
            return View(cim);
        }

        // GET: Cim/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cim = await _context.Cim.FindAsync(id);
            if (cim == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(cim);
        }

        // POST: Cim/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Code,Maladie_fr,Maladie_en,Trump1,Trump2,Trump3,Version")] Cim cim)
        {
            if (id != cim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CimExists(cim.Id))
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
            return View(cim);
        }

        // GET: Cim/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cim = await _context.Cim
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cim == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(cim);
        }

        // POST: Cim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cim = await _context.Cim.FindAsync(id);
            if (cim != null)
            {
                _context.Cim.Remove(cim);
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

        private bool CimExists(int id)
        {
            return _context.Cim.Any(e => e.Id == id);
        }
    }
}

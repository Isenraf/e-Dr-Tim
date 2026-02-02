#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BANA.Models;
using Microsoft.AspNetCore.Authorization;

namespace BANA.Controllers
{
    [Authorize]
    public class TacheController : Controller
    {
        private readonly TacheContext _context;
        private readonly UserContext _context_user;

        public TacheController(TacheContext context,UserContext context_user)
        {
            _context = context;
            _context_user = context_user;
            
        }

        // GET: Tache
        public async Task<IActionResult> Index()
        {
            ViewBag.user=Userconnected();
            return View(await _context.Tache.ToListAsync());
        }

        // GET: Tache/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var tache = await _context.Tache
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tache == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user=Userconnected();
            return View(tache);
        }

        // GET: Tache/Create
        public IActionResult Create()
        {
            ViewBag.user=Userconnected();
            return View();
        }

        // GET: Tache/Create
        public IActionResult Center()
        {
            ViewData["map"]="ok";
            ViewData["19"]="active";
            ViewBag.user=Userconnected();
            return View();
        }

        // POST: Tache/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Utilisateur,titre,Create_date,DateDebut,DateFin,duree,Description,Intervenant")] Tache tache)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tache);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.user=Userconnected();
            return View(tache);
        }

        // GET: Tache/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var tache = await _context.Tache.FindAsync(id);
            if (tache == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user=Userconnected();
            return View(tache);
        }

        // POST: Tache/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Utilisateur,titre,Create_date,DateDebut,DateFin,duree,Description,Intervenant")] Tache tache)
        {
            if (id != tache.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tache);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TacheExists(tache.Id))
                    {
                        return RedirectToAction("Page404", "Stock");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.user=Userconnected();
            return View(tache);
        }

        // GET: Tache/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var tache = await _context.Tache
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tache == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user=Userconnected();

            return View(tache);
        }

        // POST: Tache/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tache = await _context.Tache.FindAsync(id);
            _context.Tache.Remove(tache);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TacheExists(int id)
        {
            return _context.Tache.Any(e => e.Id == id);
        }
        

        public User Userconnected()
        {
            ViewData["5"] = "active";
            var user = from u in _context_user.User
                         select u;

            user = user.Where(x => x.nom == HttpContext.User.Identity.Name);
            var connectuser=user.First();
                connectuser.Initial=GetInitials(connectuser.nom);
            
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
    }
}

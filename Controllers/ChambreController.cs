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
    public class ChambreController : Controller
    {
        private readonly ChambreContext _context;
        private readonly UserContext _context_user;

        public ChambreController(ChambreContext context,UserContext context_user)
        {
            _context = context;
             _context_user = context_user;
        }

        // GET: Chambre
        public async Task<IActionResult> Index()
        {
            ViewData["c0"]="active";
            ViewData["c1"]="active";
            ViewBag.user=Userconnected();
            
            return View(await _context.Chambre.ToListAsync());
        }

        // GET: Chambre/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var chambre = await _context.Chambre
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chambre == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user=Userconnected();

            return View(chambre);
        }

        // GET: Chambre/Create
        public IActionResult Create()
        {
            ViewData["c0"]="active";
            ViewData["c2"]="active";
            ViewBag.user=Userconnected();
            return View();
        }

        // POST: Chambre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero_chambre,Prix,TypeChambre,Patient,Create_date,DateDebut,DateFin,Busy")] Chambre chambre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chambre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["c0"]="active";
            ViewData["c2"]="active";
            ViewBag.user=Userconnected();
            return View(chambre);
        }

        // GET: Chambre/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var chambre = await _context.Chambre.FindAsync(id);
            if (chambre == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user=Userconnected();
            return View(chambre);
        }

        // POST: Chambre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero_chambre,Prix,TypeChambre,Patient,Create_date,DateDebut,DateFin,Busy")] Chambre chambre)
        {
            if (id != chambre.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chambre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChambreExists(chambre.Id))
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
            return View(chambre);
        }

        // GET: Chambre/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var chambre = await _context.Chambre
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chambre == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user=Userconnected();

            return View(chambre);
        }

        // POST: Chambre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chambre = await _context.Chambre.FindAsync(id);
            _context.Chambre.Remove(chambre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChambreExists(int id)
        {
            return _context.Chambre.Any(e => e.Id == id);
        }

        

        public User Userconnected()
        {
            ViewData["10"] = "active";
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

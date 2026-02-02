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
    public class EmplacementController : Controller
    {
        private readonly EmplacementContext _context;
        private readonly UserContext _context_utilisateur;

        public EmplacementController(EmplacementContext context,UserContext context_utilisateur)
        {
            _context = context;
            _context_utilisateur=context_utilisateur;
            
        }

        // GET: Emplacement
        public async Task<IActionResult> Index()
        {
           
            ViewBag.magasin=_context.Emplacement.ToList();
            ViewBag.user=Userconnected();
            ViewData["eee1"]="active";
            ViewData["sss0"]="active";
            return View(await _context.Emplacement.ToListAsync());
        }

        // GET: Emplacement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.magasin=_context.Emplacement.ToList();
            ViewBag.user=Userconnected();
            if (id == null)
            {
               return RedirectToAction("Page404", "Stock");
            }

            var emplacement = await _context.Emplacement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emplacement == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["sss0"]="active";
            ViewData["eee1"]="active";
            return View(emplacement);
        }

        // GET: Emplacement/Create
        public IActionResult Create()
        {
            ViewBag.magasin=_context.Emplacement.ToList();
            ViewBag.user=Userconnected();
            ViewData["sss0"]="active";
            ViewData["eee1"]="active";
            return View();
        }

        // POST: Emplacement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Description,Create_date,LastModification")] Emplacement emplacement)
        {
            if (ModelState.IsValid)
            {
                emplacement.Create_date=DateTime.Now;
                emplacement.LastModification=DateTime.Now;
                _context.Add(emplacement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.magasin=_context.Emplacement.ToList();
            ViewBag.user=Userconnected();
            ViewData["sss0"]="active";
            ViewData["eee1"]="active";
            return View(emplacement);
        }

        // GET: Emplacement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.magasin=_context.Emplacement.ToList();
            ViewBag.user=Userconnected();
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var emplacement = await _context.Emplacement.FindAsync(id);
            if (emplacement == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["sss0"]="active";
            ViewData["eee1"]="active";
            return View(emplacement);
        }

        // POST: Emplacement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Description,Create_date,LastModification")] Emplacement emplacement)
        {
            ViewBag.magasin=_context.Emplacement.ToList();
            ViewBag.user=Userconnected();
            if (id != emplacement.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    emplacement.LastModification=DateTime.Now;
                    _context.Update(emplacement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmplacementExists(emplacement.Id))
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
            ViewData["sss0"]="active";
            ViewData["eee1"]="active";
            return View(emplacement);
        }

        // GET: Emplacement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.magasin=_context.Emplacement.ToList();
            ViewBag.user=Userconnected();
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var emplacement = await _context.Emplacement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emplacement == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["sss0"]="active";
            ViewData["eee1"]="active";
            return View(emplacement);
        }

        // POST: Emplacement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emplacement = await _context.Emplacement.FindAsync(id);
            _context.Emplacement.Remove(emplacement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Page404()
        {
            ViewBag.magasin=_context.Emplacement.ToList();
            ViewBag.user=Userconnected();
            return View();
        }

        private bool EmplacementExists(int id)
        {
            return _context.Emplacement.Any(e => e.Id == id);
        }

        

        public User Userconnected()
        {
            ViewData["21"]="active";
            var user = from u in _context_utilisateur.User
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

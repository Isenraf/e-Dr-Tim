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
    public class AssuranceController : Controller
    {
        private readonly AssuranceContext _context;
        private readonly UserContext _context_user;

        public AssuranceController(AssuranceContext context, UserContext context_user)
        {
            _context = context;
            _context_user = context_user;
            
        }

        // GET: Assurance
        public async Task<IActionResult> Index()
        {
            
            ViewBag.user=Userconnected();
            ViewData["Actifs"]=_context.Assurance.Where(x=>x.Actif==true).Count();
            ViewData["Inactifs"]=_context.Assurance.Where(x=>x.Actif==false).Count();
            return View(await _context.Assurance.ToListAsync());
        }

        // GET: Assurance/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
            
        //     if (id == null)
        //     {
        //         return RedirectToAction("Page404", "Stock");
        //     }

        //     var assurance = await _context.Assurance
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (assurance == null)
        //     {
        //         return RedirectToAction("Page404", "Stock");
        //     }
        //     ViewBag.user=Userconnected();

        //     return View(assurance);
        // }

        // GET: Assurance/Create
        public IActionResult Create()
        {
            ViewBag.user=Userconnected();
            return View();
        }

        // POST: Assurance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Adresse,Bp,Niu,Telephone,RC,Create_date,Actif")] Assurance assurance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assurance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.user=Userconnected();
            return View(assurance);
        }

        // GET: Assurance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var assurance = await _context.Assurance.FindAsync(id);
            if (assurance == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user=Userconnected();
            
            return View(assurance);
        }

        // POST: Assurance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Adresse,Bp,Telephone,Niu,RC,Create_date,Actif")] Assurance assurance)
        {
            
            if (id != assurance.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assurance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssuranceExists(assurance.Id))
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
            return View(assurance);
        }

        // GET: Assurance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var assurance = await _context.Assurance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assurance == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user=Userconnected();

            return View(assurance);
        }

        // POST: Assurance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assurance = await _context.Assurance.FindAsync(id);
            _context.Assurance.Remove(assurance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssuranceExists(int id)
        {
            return _context.Assurance.Any(e => e.Id == id);
        }
        
        public User Userconnected()
        {
            ViewData["4"] = "active";
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

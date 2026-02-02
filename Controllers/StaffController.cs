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
    public class StaffController : Controller
    {
        private readonly StaffContext _context;
       private readonly UserContext _context_user;

        public StaffController(StaffContext context,UserContext context_user)
        {
            _context = context;
            _context_user = context_user;
        }

        // GET: Staff
        public async Task<IActionResult> Index()
        {
            ViewBag.user=Userconnected();
            ViewData["Actifs"]=_context.Staff.Where(x=>x.Actif==true).Count();
            ViewData["Inactifs"]=_context.Staff.Where(x=>x.Actif==false).Count();
            ViewData["ppp1"]="active";
            return View(await _context.Staff.ToListAsync());
        }

        // GET: Staff/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["ppp1"]="active";
            ViewBag.user=Userconnected();
            return View(staff);
        }

        // GET: Staff/Create
        public IActionResult Create()
        {
            ViewBag.cat=_context.Staff.Select(x=>x.Désignation).Distinct().ToList();
            ViewBag.user=Userconnected();
            ViewData["ppp1"]="active";
            return View();
        }

        // POST: Staff/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Email,Désignation,Phone,Adresse,Catégorie,Create_date,Description,Actif")] Staff staff)
        {
            staff.Create_date=DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.cat=_context.Staff.Select(x=>x.Nom).ToList();
            ViewBag.user=Userconnected();
            ViewData["ppp1"]="active";
            return View(staff);
        }

        // GET: Staff/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.cat=_context.Staff.Select(x=>x.Nom).ToList();
            ViewBag.user=Userconnected();
            ViewData["ppp1"]="active";
            return View(staff);
        }

        // POST: Staff/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Email,Désignation,Phone,Adresse,Catégorie,Create_date,Description,Actif")] Staff staff)
        {
            if (id != staff.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.Id))
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
            ViewBag.cat=_context.Staff.Select(x=>x.Nom).ToList();
            ViewBag.user=Userconnected();
            ViewData["ppp1"]="active";
            return View(staff);
        }

        // GET: Staff/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.cat=_context.Staff.Select(x=>x.Nom).ToList();
            ViewBag.user=Userconnected();
            ViewData["ppp1"]="active";
            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.Id == id);
        }
        

        public User Userconnected()
        {
            ViewData["16"]="active";
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

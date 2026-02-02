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
    public class ContactController : Controller
    {
        private readonly ContactContext _context;
        private readonly UserContext _context_user;

        public ContactController(ContactContext context,UserContext context_user)
        {
            _context = context;
            _context_user=context_user;
        }

        // GET: Contact
        public async Task<IActionResult> Index()
        {
            ViewData["total"]=_context.Contact.ToList().Count();
            ViewData["actifs"]=_context.Contact.Where(x=>x.Actif==true).ToList().Count();
            ViewData["inactifs"]=_context.Contact.Where(x=>x.Actif==false).ToList().Count();
            ViewBag.user=Userconnected();
            ViewData["contact"]="active";
            return View(await _context.Contact.ToListAsync());
        }

        // GET: Contact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["total"]=_context.Contact.ToList().Count();
            ViewData["actifs"]=_context.Contact.Where(x=>x.Actif==true).ToList().Count();
            ViewData["inactifs"]=_context.Contact.Where(x=>x.Actif==false).ToList().Count();
            ViewBag.user=Userconnected();
            ViewData["contact"]="active";

            return View(contact);
        }

        // GET: Contact/Create
        public IActionResult Create()
        {
            ViewData["total"]=_context.Contact.ToList().Count();
            ViewData["actifs"]=_context.Contact.Where(x=>x.Actif==true).ToList().Count();
            ViewData["inactifs"]=_context.Contact.Where(x=>x.Actif==false).ToList().Count();
            ViewBag.cat=_context.Contact.Select(x =>x.Categorie).ToList().Distinct();
            ViewBag.user=Userconnected();
            ViewData["contact"]="active";
            return View();
        }

        // POST: Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Email,Categorie,Phone,Create_date,Description,Actif")] Contact contact)
        {
            contact.Create_date=DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["total"]=_context.Contact.ToList().Count();
            ViewData["actifs"]=_context.Contact.Where(x=>x.Actif==true).ToList().Count();
            ViewData["inactifs"]=_context.Contact.Where(x=>x.Actif==false).ToList().Count();
            ViewBag.cat=_context.Contact.Select(x =>x.Categorie).ToList().Distinct();
            ViewBag.user=Userconnected();
            ViewData["contact"]="active";
            return View(contact);
        }

        // GET: Contact/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["total"]=_context.Contact.ToList().Count();
            ViewData["actifs"]=_context.Contact.Where(x=>x.Actif==true).ToList().Count();
            ViewData["inactifs"]=_context.Contact.Where(x=>x.Actif==false).ToList().Count();
            ViewBag.cat=_context.Contact.Select(x =>x.Categorie).ToList().Distinct();
            ViewBag.user=Userconnected();
            ViewData["contact"]="active";
            return View(contact);
        }

        // POST: Contact/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Email,Categorie,Phone,Create_date,Description,Actif")] Contact contact)
        {
            if (id != contact.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
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
            ViewData["total"]=_context.Contact.ToList().Count();
            ViewData["actifs"]=_context.Contact.Where(x=>x.Actif==true).ToList().Count();
            ViewData["inactifs"]=_context.Contact.Where(x=>x.Actif==false).ToList().Count();
            ViewBag.cat=_context.Contact.Select(x =>x.Categorie).ToList().Distinct();
            ViewBag.user=Userconnected();
            ViewData["contact"]="active";
            return View(contact);
        }

        // GET: Contact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["total"]=_context.Contact.ToList().Count();
            ViewData["actifs"]=_context.Contact.Where(x=>x.Actif==true).ToList().Count();
            ViewData["inactifs"]=_context.Contact.Where(x=>x.Actif==false).ToList().Count();
            ViewBag.cat=_context.Contact.Select(x =>x.Categorie).ToList().Distinct();
            ViewBag.user=Userconnected();
            ViewData["contact"]="active";
            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contact.FindAsync(id);
            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Disable(int id)
        {
            var contact = await _context.Contact.FindAsync(id);
            if(contact.Actif==true){contact.Actif=false;}else{contact.Actif=true;}
            _context.Update(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contact.Any(e => e.Id == id);
        }
        

        public User Userconnected()
        {
            ViewData["17"]="active";
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

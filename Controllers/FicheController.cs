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
using BANA.PDF;
using System.Text.RegularExpressions;

namespace BANA.Controllers
{
    
    public class FicheController : Controller
    {
        private readonly FicheContext _context;
        private readonly UserContext _context_utilisateur;
        private readonly DoctorContext _context_doctor;
        private readonly PatientContext _context_patient;

        public FicheController(FicheContext context, UserContext context_utilisateur, DoctorContext context_doctor, PatientContext context_patient)
        {
            _context = context;
            _context_utilisateur = context_utilisateur;
            _context_doctor = context_doctor;
            _context_patient = context_patient;
        }

        // GET: Fiche
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.user = Userconnected();
            return View(await _context.Fiche.ToListAsync());
        }

        // GET: Fiche/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();

            var fiche = await _context.Fiche
                .FirstOrDefaultAsync(m => m.Id == id);
            fiche.Contenu = ReplaceEmptyParagraphs(fiche.Contenu);
            if (fiche.Contenu == null)
            {
                fiche.Contenu = "";
            }
            if (fiche == null)
            {
                return NotFound();
            }

            return View(fiche);
        }

        // GET: Fiche/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.user = Userconnected();
            return View();
        }

        [Authorize]
        public IActionResult Fiche0()
        {
            ViewBag.user = Userconnected();
            return View();
        }

        // POST: Fiche/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Nom,Contenu,Categorie,Create_date,Dernieremodification,Trump1,Trump2,Trump3")] Fiche fiche)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fiche);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.user = Userconnected();
            return View(fiche);
        }

        // GET: Fiche/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche = await _context.Fiche.FindAsync(id);
            if (fiche == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(fiche);
        }

        // GET: Fiche/Edit/5
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Edit0(int? id)
        {
            var fiche = await _context.Fiche.FindAsync(id);
            ViewBag.user = Userconnected();
            return View(fiche);
        }

        // GET: Fiche/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit00(int? id)
        {
            var fiche = await _context.Fiche.FindAsync(id);
            ViewBag.user = Userconnected();
            if (fiche!=null && fiche.Contenu!=null)
            {
                fiche.Contenu = fiche.Contenu.Replace("<br>", "<p></p>");
            }
            return View(fiche);
        }

        // POST: Fiche/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Contenu,Categorie,Create_date,Dernieremodification,Trump1,Trump2,Trump3")] Fiche fiche)
        {
            if (id != fiche.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fiche);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FicheExists(fiche.Id))
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
            return View(fiche);
        }

        // GET: Fiche/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiche = await _context.Fiche
                .FirstOrDefaultAsync(m => m.Id == id);

            if (fiche == null)
            {
                return NotFound();
            }
            if (fiche.Contenu == null)
            {
                fiche.Contenu = "";
            }
            ViewBag.user = Userconnected();
            return View(fiche);
        }

        // POST: Fiche/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fiche = await _context.Fiche.FindAsync(id);
            if (fiche != null)
            {
                _context.Fiche.Remove(fiche);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<object> Add([FromBody] Fiche MyData)
        {
            if (MyData.Id == 0)
            {
                
                MyData.Contenu = MyData.Contenu;
                MyData.Create_date = DateTime.Now;
                MyData.Dernieremodification = DateTime.Now;
                _context.Add(MyData);
                await _context.SaveChangesAsync();
                return Json("ok");
            }
            else
            {
                var fiche = await _context.Fiche
                    .FirstOrDefaultAsync(m => m.Id == MyData.Id);
                fiche.Nom = MyData.Nom;
                fiche.Categorie = MyData.Categorie;
                fiche.Contenu = MyData.Contenu;
                fiche.Create_date = DateTime.Now;
                fiche.Dernieremodification = DateTime.Now;
                _context.Update(fiche);
                await _context.SaveChangesAsync();
                return Json("ok");
            }
            

        }

        public async Task<IActionResult> Pdf(int id)
        {
            PdfGeneration8 x = new(_context);
            return new FileStreamResult(await x.DetailsFiche(id), "application/pdf");
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

         static string ReplaceEmptyParagraphs(string input)
        {
            string resultat = Regex.Replace(input,
            @"(<p[^>]*>(\s|<span[^>]*>(\s|&nbsp;)*<\/span>)*<\/p>)+",
            match =>
            {
                int count = Regex.Matches(match.Value, @"<p[^>]*>(\s|<span[^>]*>(\s|&nbsp;)*<\/span>)*<\/p>").Count;
                return string.Concat(new string[count + 1].Select(_ => "<br/>"));
            },
            RegexOptions.IgnoreCase);

            return resultat;
        }


        private bool FicheExists(int id)
        {
            return _context.Fiche.Any(e => e.Id == id);
        }
    }
}

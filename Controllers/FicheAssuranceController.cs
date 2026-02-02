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
    public class FicheAssuranceController : Controller
    {
        private readonly FicheAssuranceContext _context;
        private readonly UserContext _context_utilisateur;
        private readonly AssuranceContext _context_assurance;

        public FicheAssuranceController(FicheAssuranceContext context, UserContext context_utilisateur, AssuranceContext context_assurance)
        {
            _context = context;
            _context_utilisateur = context_utilisateur;
            _context_assurance = context_assurance;
        }

        // GET: FicheAssurance
        public async Task<IActionResult> Index(string target, string cat)
        {
            ViewBag.user = Userconnected();
            if (cat != null)
            {
                return View(await _context.FicheAssurance.Where(x => x.Assurance == cat).ToListAsync());
            }
            return View(await _context.FicheAssurance.Where(x => x.Assurance.Contains(target) || x.Nom.Contains(target)).ToListAsync());
        }

        public async Task<IActionResult> Index0()
        {
            ViewData["table"] = "ok";
            ViewBag.user = Userconnected();
            ViewBag.cat = _context.FicheAssurance.Select(x => x.Assurance).Distinct().ToList();
            return View(await _context.FicheAssurance.ToListAsync());
        }

        // GET: FicheAssurance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ficheAssurance = await _context.FicheAssurance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ficheAssurance == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(ficheAssurance);
        }

        public async Task<IActionResult> Pdf(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ficheAssurance = await _context.FicheAssurance
                .FirstOrDefaultAsync(m => m.Id == id);
            // Chemin vers le dossier contenant les PDF
            var cheminDossier = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Fiche_assurance");

            var cheminPdf = Path.Combine(cheminDossier, ficheAssurance.Id + ".pdf");

            if (!System.IO.File.Exists(cheminPdf))
            {
                return NotFound("Le fichier PDF n'existe pas.");
            }
            var stream = new FileStream(cheminPdf, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(stream, "application/pdf");
        }

        // GET: FicheAssurance/Create
        public IActionResult Create()
        {
            ViewBag.cat = _context.FicheAssurance.Select(x => x.Categorie).ToList();
            ViewBag.assurance = _context_assurance.Assurance.ToList();
            ViewBag.user = Userconnected();
            return View();
        }

        // POST: FicheAssurance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Assurance,Categorie,Create_date,Dernieremodification,Trump1,Trump2,Trump3")] FicheAssurance ficheAssurance)
        {
            if (ModelState.IsValid)
            {
                ficheAssurance.Create_date = DateTime.Now;
                ficheAssurance.Dernieremodification = DateTime.Now;
                _context.Add(ficheAssurance);
                await _context.SaveChangesAsync();

                var dernier_element = _context.FicheAssurance.OrderBy(x => x.Id).LastOrDefault();

                //uploads file
                var files = HttpContext.Request.Form.Files;
                int nb = files.Count();
                string folder = "wwwroot//Fiche_assurance";
                foreach (var Image in files)
                {

                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        //There is an error here
                        var uploads = Path.Combine(Directory.GetCurrentDirectory(), folder);
                        var fileName = dernier_element.Id.ToString();
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName + ".pdf"), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);

                        }

                    }
                }
                return RedirectToAction(nameof(Index0));
            }
            ViewBag.user = Userconnected();
            return View(ficheAssurance);
        }

        // GET: FicheAssurance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ficheAssurance = await _context.FicheAssurance.FindAsync(id);
            if (ficheAssurance == null)
            {
                return NotFound();
            }
            ViewBag.cat = _context.FicheAssurance.Select(x => x.Categorie).ToList();
            ViewBag.assurance = _context_assurance.Assurance.ToList();
            ViewBag.user = Userconnected();
            return View(ficheAssurance);
        }

        // POST: FicheAssurance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Assurance,Categorie,Create_date,Dernieremodification,Trump1,Trump2,Trump3")] FicheAssurance ficheAssurance)
        {
            if (id != ficheAssurance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ficheAssurance.Dernieremodification = DateTime.Now;
                try
                {
                    _context.Update(ficheAssurance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FicheAssuranceExists(ficheAssurance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index0));
            }
            ViewBag.user = Userconnected();
            return View(ficheAssurance);
        }

        // GET: FicheAssurance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ficheAssurance = await _context.FicheAssurance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ficheAssurance == null)
            {
                return NotFound();
            }
            ViewBag.cat = _context.FicheAssurance.Select(x => x.Categorie).ToList();
            ViewBag.assurance = _context_assurance.Assurance.ToList();
            ViewBag.user = Userconnected();
            return View(ficheAssurance);
        }

        // POST: FicheAssurance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ficheAssurance = await _context.FicheAssurance.FindAsync(id);
            if (ficheAssurance != null)
            {
                _context.FicheAssurance.Remove(ficheAssurance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index0));
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

        private bool FicheAssuranceExists(int id)
        {
            return _context.FicheAssurance.Any(e => e.Id == id);
        }
    }
}

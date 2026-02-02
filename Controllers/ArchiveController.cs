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
    public class ArchiveController : Controller
    {
        private readonly ArchiveContext _context;
        private readonly UserContext _context_user;

        public ArchiveController(ArchiveContext context, UserContext context_user)
        {
            _context = context;
            _context_user = context_user;
        }

        // GET: Archive
        [Authorize(AuthenticationSchemes = "CookieAuthDmi")]
        public async Task<IActionResult> Index(string ndossier)
        {
            ViewBag.user = Userconnected();
            ViewData["doss"] = ndossier;

            string dossier = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/archives", ndossier);
            
            if (!Directory.Exists(dossier))
                Directory.CreateDirectory(dossier);

            var fichiers = Directory.GetFiles(dossier)
                .Select(f => new FichierModel
                {
                    Numero_dossier=ndossier,
                    Nom = Path.GetFileName(f),
                    DateCreation = System.IO.File.GetCreationTime(f),
                    DateModification = System.IO.File.GetLastWriteTime(f),
                    TailleKo = new FileInfo(f).Length / 1024.0
                })
                .ToList();

            return View(fichiers);
        }


        // GET: Archive/Details/5
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var archive = await _context.Archive
                .FirstOrDefaultAsync(m => m.Id == id);
            if (archive == null)
            {
                return NotFound();
            }

            return View(archive);
        }

        // GET: Archive/Create
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public IActionResult Create(string dossier)
        {
            ViewData["doss"] = dossier;
            ViewBag.user = Userconnected();
            return View();
        }

        // POST: Archive/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Patient,Numero_dossier,DatedeNaissance,Genre,Create_date,ajoute_par,Trump2,Trump3,Etat")] Archive archive)
        {
            if (ModelState.IsValid)
            {
                _context.Add(archive);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(archive);
        }

        // GET: Archive/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var archive = await _context.Archive.FindAsync(id);
            if (archive == null)
            {
                return NotFound();
            }
            return View(archive);
        }

        // POST: Archive/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Patient,Numero_dossier,DatedeNaissance,Genre,Create_date,ajoute_par,Trump2,Trump3,Etat")] Archive archive)
        {
            if (id != archive.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(archive);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArchiveExists(archive.Id))
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
            return View(archive);
        }

        // GET: Archive/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var archive = await _context.Archive
                .FirstOrDefaultAsync(m => m.Id == id);
            if (archive == null)
            {
                return NotFound();
            }

            return View(archive);
        }

        public async Task<IActionResult> Pdf(int? id,string filen)
        {
            if (id == null)
            {
                return NotFound();
            }

          
            // Chemin vers le dossier contenant les PDF
            var cheminDossier = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archives");

            var cheminPdf = Path.Combine(cheminDossier, id + "/"+filen);

            if (!System.IO.File.Exists(cheminPdf))
            {
                return NotFound("Le fichier PDF n'existe pas.");
            }
            var stream = new FileStream(cheminPdf, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(stream, "application/pdf");
        }

        // POST: Archive/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var archive = await _context.Archive.FindAsync(id);
            if (archive != null)
            {
                _context.Archive.Remove(archive);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string name, [FromForm] string dossier, [FromForm] IFormFileCollection extraFiles)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Aucun fichier reçu.");

            if (string.IsNullOrWhiteSpace(dossier))
                return BadRequest("Le numéro de dossier est obligatoire.");

            dossier = string.Concat(dossier.Split(Path.GetInvalidFileNameChars()));
            name = string.IsNullOrWhiteSpace(name) ? Path.GetFileNameWithoutExtension(file.FileName) : name;
            name = string.Concat(name.Split(Path.GetInvalidFileNameChars()));

            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archives");
            var dossierPath = Path.Combine(rootPath, dossier);
            if (!Directory.Exists(dossierPath))
                Directory.CreateDirectory(dossierPath);

            // 💾 Gestion PDF avec doublons
            string finalPdfName = name + ".pdf";
            string pdfPath = Path.Combine(dossierPath, finalPdfName);
            int counter = 1;
            while (System.IO.File.Exists(pdfPath))
            {
                finalPdfName = $"{name}({counter}).pdf";
                pdfPath = Path.Combine(dossierPath, finalPdfName);
                counter++;
            }
            using (var stream = new FileStream(pdfPath, FileMode.Create))
                await file.CopyToAsync(stream);

            

            return Ok(new
            {
                message = "Fichiers reçus et stockés avec succès.",
                doss = dossier,
                pdf = finalPdfName,
            });
        }



         public User Userconnected()
        {
            ViewData["9"] = "active";
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

        private bool ArchiveExists(int id)
        {
            return _context.Archive.Any(e => e.Id == id);
        }
    }
}

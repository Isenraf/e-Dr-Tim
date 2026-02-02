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
using iText.Kernel.Pdf;
using iText.Forms;

namespace BANA.Controllers
{
    public class FinalFicheController : Controller
    {
        private readonly FinalFicheContext _context;
        private readonly FicheContext _context_fiche;
        private readonly UserContext _context_utilisateur;
        private readonly DoctorContext _context_doctor;
        private readonly PatientContext _context_patient;


        public FinalFicheController(FinalFicheContext context, UserContext context_utilisateur, DoctorContext context_doctor, PatientContext context_patient, FicheContext context_fiche)
        {
            _context = context;
            _context_utilisateur = context_utilisateur;
            _context_doctor = context_doctor;
            _context_patient = context_patient;
            _context_fiche = context_fiche;
        }

        // GET: FinalFiche
        [Authorize]
        public async Task<IActionResult> Index(int? mois, int? annee)
        {
            ViewBag.user = Userconnected();
            var docs = await _context.FinalFiche.OrderByDescending(x => x.Id).ToListAsync();
            if (mois != null && annee != null)
            {
                docs = docs.Where(x => x.Create_date.Month == mois && x.Create_date.Year == annee).ToList();
            }
            return View(docs);
        }

       [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Index1(string nd,string target)
        {
            ViewBag.user = Userconnected();
            var docs = await _context.FinalFiche.Where(x=>x.Numero_dossier==nd && x.Categorie==target).OrderByDescending(x => x.Id).ToListAsync();
            return View(docs);
        }

         [Authorize]
        public async Task<IActionResult> Index0()
        {
            // Create a list of string.
            List<string> dates = new List<string>();
            List<string> dates1 = new List<string>();
            List<Stat> datesfinal = new List<Stat>();
            ViewData["xx2"] = "active current-page";


            foreach (var item in _context.FinalFiche.Select(x => x.Create_date))
            {
                dates1.Add(item.Month + "-" + item.Year);
                dates.Add(item.ToString("MMMM") + " " + item.Year);
            }
            var dates_dist1 = dates.Distinct();

            List<string> dates11 = new List<string>();
            dates11.AddRange(dates1.Distinct());
            //Console.WriteLine(dates1[0].IndexOf("-"));


            int i = 0;
            foreach (var item in dates_dist1)
            {
                Stat ob = new Stat();
                ob.mois = int.Parse(dates11[i].Substring(0, dates11[i].IndexOf("-")));
                ob.annee = int.Parse(dates11[i].Substring(dates11[i].IndexOf("-") + 1, 4));
                ob.date_en_lettre = item;
                datesfinal.Add(ob);
                i++;
            }
            ViewBag.dates = datesfinal;
            ViewBag.user = Userconnected();
            return View(await _context.FinalFiche.ToListAsync());
        }

        // GET: FinalFiche/Details/5
         [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finalFiche = await _context.FinalFiche
                .FirstOrDefaultAsync(m => m.Id == id);
            if (finalFiche == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            finalFiche.Contenu = ReplaceEmptyParagraphs(finalFiche.Contenu);
            return View(finalFiche);
        }
        
        [Authorize(AuthenticationSchemes = "CookieAuthDmi")]
        public async Task<IActionResult> Details2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finalFiche = await _context.FinalFiche
                .FirstOrDefaultAsync(m => m.Id == id);
            if (finalFiche == null)
            {
                return NotFound();
            }
            Console.WriteLine(finalFiche.Contenu);
            finalFiche.Contenu = ReplaceEmptyParagraphs(finalFiche.Contenu);
            ViewBag.user = Userconnected();
            Console.WriteLine(finalFiche.Contenu);
            return View(finalFiche);
        }
        

        

        // GET: FinalFiche/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.user = Userconnected();
            return View();
        }

        // POST: FinalFiche/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Nom,Patient,Numero_dossier,DatedeNaissance,Genre,Contenu,Categorie,Create_date,Dernieremodification,Trump1,Trump2,Trump3,Etat")] FinalFiche finalFiche)
        {
            if (ModelState.IsValid)
            {
                _context.Add(finalFiche);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.user = Userconnected();
            return View(finalFiche);
        }

        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Edit0(int? id,string nd)
        {
            var fiche = await _context_fiche.Fiche.FindAsync(id);
            var medecin = _context_doctor.Doctor.Where(x => x.User_Name == Userconnected().nom).FirstOrDefault();
            var patient = _context_patient.Patient.Where(x => x.Numero_dossier == nd).FirstOrDefault();
            ViewBag.user = Userconnected();
            fiche.Contenu = fiche.Contenu.Replace("xxxx1", DateTime.Now.ToLongDateString());
            fiche.Contenu = fiche.Contenu.Replace("xxxx2", medecin.Nom+" "+medecin.Prenom);
            fiche.Contenu = fiche.Contenu.Replace("xxxx3", patient.Nom+" "+patient.Prenom);
            //fiche.Contenu = fiche.Contenu.Replace("trump4",DateTime.Now.ToString());
            return View(fiche);
        }

        // GET: FinalFiche/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finalFiche = await _context.FinalFiche.FindAsync(id);
            if (finalFiche == null)
            {
                return NotFound();
            }
            return View(finalFiche);
        }

        // POST: FinalFiche/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Patient,Numero_dossier,DatedeNaissance,Genre,Contenu,Categorie,Create_date,Dernieremodification,Trump1,Trump2,Trump3,Etat")] FinalFiche finalFiche)
        {
            if (id != finalFiche.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(finalFiche);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinalFicheExists(finalFiche.Id))
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
            return View(finalFiche);
        }

        // GET: FinalFiche/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finalFiche = await _context.FinalFiche
                .FirstOrDefaultAsync(m => m.Id == id);
            if (finalFiche == null)
            {
                return NotFound();
            }

            return View(finalFiche);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<object> Add([FromBody] FinalFiche MyData)
        {
            var userNom = Userconnected().nom.ToLower().Trim().Replace(" ", "");
            var medecin = _context_doctor.Doctor.Where(x => x.User_Name.ToLower().Trim().Replace(" ", "") == userNom).FirstOrDefault();
            var patient=_context_patient.Patient.Where(x => x.Numero_dossier == MyData.Numero_dossier).FirstOrDefault();
            if (MyData.Id == 0)
            {

                MyData.Contenu = MyData.Contenu;
                MyData.Create_date = DateTime.Now;
                MyData.Dernieremodification = DateTime.Now;
                if (medecin != null)
                {
                    MyData.Trump1 = medecin.Nom + " " + medecin.Prenom;
                }
                if (patient != null)
                {
                    MyData.Patient = patient.Nom + " " + patient.Prenom;
                    MyData.Genre = patient.Genre;
                    MyData.DatedeNaissance = patient.DateNaissance;
                }
                MyData.Etat = "Cloturé";

                _context.Add(MyData);
                await _context.SaveChangesAsync();
                return Json("ok");
            }
            return Json("non");
        }

        [Authorize]
        public async Task<IActionResult> Pdf(int id)
        {
            PdfGeneration9 x = new(_context, _context_doctor);
            return new FileStreamResult(await x.DetailsFiche(id), "application/pdf");
        }

        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Pdf2(int id)
        {
            PdfGeneration9 x = new(_context, _context_doctor);

            var originalStream = await x.DetailsFiche(id);

            var buffer = new MemoryStream();
            await originalStream.CopyToAsync(buffer);
            buffer.Position = 0;

            var flattenedStream = PdfUtils.FlattenPdf(buffer);

            return new FileStreamResult(flattenedStream, "application/pdf");
            

           
        }

        // POST: FinalFiche/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var finalFiche = await _context.FinalFiche.FindAsync(id);
            if (finalFiche != null)
            {
                _context.FinalFiche.Remove(finalFiche);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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


        private bool FinalFicheExists(int id)
        {
            return _context.FinalFiche.Any(e => e.Id == id);
        }
    }
}

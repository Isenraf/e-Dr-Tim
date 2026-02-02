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
using BANA.IA;
using HtmlAgilityPack;

namespace BANA.Controllers
{
    [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
    public class DmiController : Controller
    {
        private readonly DmiContext _context;
        private readonly UserContext _context_user;
        private readonly PatientContext _context_patient;
        private readonly FactureContext _context_facture;
        private readonly DoctorContext _context_doctor;
        private readonly IAIService _aiService;

        public DmiController(DmiContext context, UserContext context_user, PatientContext context_patient, FactureContext context_facture, DoctorContext context_doctor, IAIService aiService)
        {
            _context = context;
            _context_user = context_user;
            _context_patient = context_patient;
            _context_facture = context_facture;
            _context_doctor = context_doctor;
            _aiService = aiService;
        }

        // GET: Dmi
        public async Task<IActionResult> Index(int mois, int annee)
        {
            ViewBag.user = Userconnected();
            return View(await _context.Dmi.Where(x => x.Medecin.ToLower().Trim().Replace(" ", "") == Userconnected().nom.ToLower().Trim().Replace(" ", "") && x.CreateDate.Month == mois && x.CreateDate.Year == annee).ToListAsync());
        }

        public async Task<IActionResult> Indexx(string nd)
        {
            ViewBag.user = Userconnected();
            return View(await _context.Dmi.Where(x => x.NumeroDossier==nd).OrderByDescending(x=>x.Id).ToListAsync());
        }

        public async Task<IActionResult> Index0()
        {
            // Create a list of string.
            List<string> dates = new List<string>();
            List<string> dates1 = new List<string>();
            List<Stat> datesfinal = new List<Stat>();

            string nom_medecin = "";
            var userNom = Userconnected().nom.ToLower().Trim().Replace(" ", "");
            var medecin = _context_doctor.Doctor.Where(x => x.User_Name.ToLower().Trim().Replace(" ", "") == userNom).FirstOrDefault();
            if(medecin!=null){nom_medecin = medecin.Nom.ToLower().Trim().Replace(" ", ""); }


            foreach (var item in _context.Dmi.OrderByDescending(x => x.Id).Where(x => x.Medecin.ToLower().Trim().Replace(" ", "") == nom_medecin).Select(x => x.CreateDate))
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
            ViewData["xxx2"] = "active";
            
            ViewBag.dates = datesfinal;
            ViewBag.user = Userconnected();
            return View(await _context.Dmi.OrderByDescending(x => x.Id).Where(x => x.Medecin.ToLower().Trim().Replace(" ", "") == nom_medecin).ToListAsync());
        }
        public async Task<IActionResult> Consultations(string nd)
        {
            ViewBag.user = Userconnected();
            ViewBag.patient = _context_patient.Patient.FirstOrDefault(x => x.Numero_dossier == nd);
            return View(await _context.Dmi.Where(x => x.NumeroDossier == nd).ToListAsync());
        }

        // GET: Dmi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dmi = await _context.Dmi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dmi == null)
            {
                return NotFound();
            }
            ViewBag.patient = _context_patient.Patient.FirstOrDefault(x => x.Numero_dossier == dmi.NumeroDossier);
            ViewBag.facture = _context_facture.Facture.FirstOrDefault(x => x.Numero_de_facture == dmi.NumeroDeFacture);
            ViewData["id"] = _context_patient.Patient.FirstOrDefault(x => x.Numero_dossier == dmi.NumeroDossier).Id;

            ViewBag.user = Userconnected();
            return View(dmi);
        }

        // GET: Dmi/Details/5
        public async Task<IActionResult> Details2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dmi = await _context.Dmi
                .FirstOrDefaultAsync(m => m.Id == id);


            if (dmi == null)
            {
                return NotFound();
            }

            string html = dmi.Trumps[7];
            string html2 = dmi.Trumps[9];

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlDocument doc2 = new HtmlDocument();
            doc2.LoadHtml(html2);

            var toRemove = doc.DocumentNode.SelectNodes("//div[contains(@class,'bg-info')]");
            var toRemove2 = doc2.DocumentNode.SelectNodes("//div[contains(@class,'bg-info')]");
            if (toRemove != null)
            {
                foreach (var div in toRemove)
                {
                    div.Remove();
                }
            }

            if (toRemove2 != null)
            {
                foreach (var div in toRemove2)
                {
                    div.Remove();
                }
            }

            // Supprimer la première colonne (toutes les <td> et <th>)
            foreach (var row in doc.DocumentNode.SelectNodes("//tr"))
            {
                var firstCell = row.SelectSingleNode("th|td");
                firstCell?.Remove();
            }

            string cleanedHtml = doc.DocumentNode.OuterHtml;
            dmi.Trumps[7] = cleanedHtml;
            ViewBag.patient = _context_patient.Patient.FirstOrDefault(x => x.Numero_dossier == dmi.NumeroDossier);
            ViewBag.facture = _context_facture.Facture.FirstOrDefault(x => x.Numero_de_facture == dmi.NumeroDeFacture);
            ViewData["id"] = _context_patient.Patient.FirstOrDefault(x => x.Numero_dossier == dmi.NumeroDossier).Id;

            ViewBag.user = Userconnected();
            return View(dmi);
        }

        // GET: Dmi/Create
        public IActionResult Create()
        {
            ViewBag.user = Userconnected();
            return View();
        }

        // POST: Dmi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroDossier,NumeroDeFacture,CreateDate,Trumps,Etat")] Dmi dmi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dmi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.user = Userconnected();
            return View(dmi);
        }

        // GET: Dmi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dmi = await _context.Dmi.FindAsync(id);
            if (dmi == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(dmi);
        }

        // POST: Dmi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroDossier,NumeroDeFacture,CreateDate,Trumps,Etat")] Dmi dmi)
        {
            if (id != dmi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dmi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DmiExists(dmi.Id))
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
            return View(dmi);
        }

        // GET: Dmi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dmi = await _context.Dmi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dmi == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(dmi);
        }

        // POST: Dmi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dmi = await _context.Dmi.FindAsync(id);
            if (dmi != null)
            {
                _context.Dmi.Remove(dmi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<object> Add([FromBody] Dmi dmi)
        {
            var patient=_context_patient.Patient.Where(x => x.Numero_dossier == dmi.NumeroDossier).FirstOrDefault();
            dmi.CreateDate = DateTime.Now;
            if (patient != null)
            {
                dmi.Patient = patient.Nom + " " + patient.Prenom;
            }
            

            _context.Add(dmi);
            await _context.SaveChangesAsync();
            Console.WriteLine(dmi.Id);
            return Json(dmi.Id);

        }



        [HttpPost]
        public async Task<IActionResult> SaveAudio(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                Console.WriteLine("kkkkkk" + file.Length);
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/voices/dmi");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploads, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                Console.WriteLine("Fichier enregistré ici : " + filePath); // vérifier le chemin
                return Ok("Fichier sauvegardé : " + uniqueFileName);
            }

            return BadRequest("Aucun fichier reçu");
        }
        
        [HttpPost]
        //[Authorize]
        public async Task<object> IAtest([FromBody] Prompt MyData)
        {
            Console.WriteLine(MyData.Contenu);

            var result = await _aiService.AskAsync(MyData.Contenu);
             return  Json(result); 
            
        }

        private bool DmiExists(int id)
        {
            return _context.Dmi.Any(e => e.Id == id);
        }

        public User Userconnected()
        {
            ViewData["3"] = "active";
            var user = from u in _context_user.User
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
    }
}

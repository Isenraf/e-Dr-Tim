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
    public class VaccinationController : Controller
    {
        private readonly VaccinationContext _context;
        private readonly UserContext _context_utilisateur;
        private readonly PatientContext _context_patient;


        public VaccinationController(VaccinationContext context, UserContext context_utilisateur, PatientContext context_patient)
        {
            _context = context;
            _context_utilisateur = context_utilisateur;
            _context_patient = context_patient;
        }

        // GET: Vaccination
        public async Task<IActionResult> Index()
        {
            ViewBag.user = Userconnected();
            return View(await _context.Vaccination.ToListAsync());
        }

        public async Task<IActionResult> Index0()
        {
            // Create a list of string.
            List<string> dates = new List<string>();
            List<string> dates1 = new List<string>();
            List<Stat> datesfinal = new List<Stat>();


            foreach (var item in _context.Vaccination.OrderByDescending(x => x.Id).Select(x => x.Create_date))
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
            return View(await _context.Vaccination.OrderByDescending(x => x.Id).ToListAsync());
        }

        // GET: Vaccination/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccination = await _context.Vaccination
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vaccination == null)
            {
                return NotFound();
            }
            ViewBag.patient = _context_patient.Patient.FirstOrDefault(x => x.Numero_dossier == vaccination.Numero_Dossier);
            ViewBag.user = Userconnected();
            return View(vaccination);
        }

        // GET: Vaccination/Create
        public IActionResult Create()
        {
            ViewBag.user = Userconnected();
            return View();
        }

        // POST: Vaccination/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero_Dossier,Patient,Create_date,contenu,Trump1,Trump2,Trump3")] Vaccination vaccination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vaccination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.user = Userconnected();
            return View(vaccination);
        }

        // GET: Vaccination/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccination = await _context.Vaccination.FindAsync(id);
            if (vaccination == null)
            {
                return NotFound();
            }
            return View(vaccination);
        }

        // POST: Vaccination/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero_Dossier,Patient,Create_date,contenu,Trump1,Trump2,Trump3")] Vaccination vaccination)
        {
            if (id != vaccination.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vaccination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaccinationExists(vaccination.Id))
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
            return View(vaccination);
        }

        // GET: Vaccination/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccination = await _context.Vaccination
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vaccination == null)
            {
                return NotFound();
            }

            return View(vaccination);
        }

        // POST: Vaccination/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vaccination = await _context.Vaccination.FindAsync(id);
            if (vaccination != null)
            {
                _context.Vaccination.Remove(vaccination);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<object> Add([FromBody] Vaccination vaccination)
        {
            var patient = _context_patient.Patient.FirstOrDefault(x => x.Numero_dossier == vaccination.Numero_Dossier);

            vaccination.Create_date = DateTime.Now;
            vaccination.Patient = patient.Nom + " " + patient.Prenom;



            if (VaccinationExists(vaccination.Numero_Dossier))
            {

                _context.Update(vaccination);
                await _context.SaveChangesAsync();
                return Json(vaccination.Id);
            }
            else
            {
                _context.Add(vaccination);
                await _context.SaveChangesAsync();
                return Json(vaccination.Id);
            }


        }

        public User Userconnected()
        {

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

        private bool VaccinationExists(int id)
        {
            return _context.Vaccination.Any(e => e.Id == id);
        }
        private bool VaccinationExists(string dossier)
        {
            return _context.Vaccination.Any(e => e.Numero_Dossier == dossier);
        }
    }
}

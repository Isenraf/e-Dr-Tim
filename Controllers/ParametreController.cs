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
    public class ParametreController : Controller
    {
        private readonly ParametreContext _context;
        private readonly UserContext _context_utilisateur;
        private readonly DepartementContext _context1;
        private readonly PatientContext _context_patient;
        private readonly FactureContext _context_facture;
        private readonly DoctorContext _context_doctor;


        public ParametreController(ParametreContext context, UserContext context_utilisateur, PatientContext context_patient, DepartementContext context1, FactureContext context_facture, DoctorContext context_doctor)
        {
            _context = context;
            _context_utilisateur = context_utilisateur;
            _context_patient = context_patient;
            _context1 = context1;
            _context_facture = context_facture;
            _context_doctor = context_doctor;
        }

        // GET: Parametre
        public async Task<IActionResult> Index1()
        {
            ViewData["xx2"] = "active current-page";
            ViewBag.user = Userconnected();
            ViewBag.medecins = _context_doctor.Doctor.Where(x => x.Interne == true).ToList();
            return View(await _context.Parametre.Where(x => x.Create_date.Year == DateTime.Now.Year && x.Create_date.Month == DateTime.Now.Month && x.Create_date.Day == DateTime.Now.Day).OrderByDescending(x => x.Id).Take(200).ToListAsync());
        }

        public async Task<IActionResult> Patient(string ndossier)
        {
            ViewData["xx2"] = "active current-page";
            ViewBag.user = Userconnected();
            ViewData["dossier"] = ndossier;
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == ndossier).FirstOrDefault();
            ViewBag.medecins = _context_doctor.Doctor.Where(x => x.Interne == true).ToList();
            return View(await _context.Parametre.Where(x => x.Numero_Dossier == ndossier).OrderByDescending(x => x.Id).Take(200).ToListAsync());
        }


        

        public async Task<IActionResult> Index(int mois, int annee)
        {
            ViewData["xx2"] = "active current-page";
            ViewBag.user = Userconnected();
            return View(await _context.Parametre.Where(x => x.Create_date.Month == mois && x.Create_date.Year == annee).OrderByDescending(x => x.Id).ToListAsync());
        }

        public async Task<IActionResult> Index0()
        {
            // Create a list of string.
            List<string> dates = new List<string>();
            List<string> dates1 = new List<string>();
            List<Stat> datesfinal = new List<Stat>();
            ViewData["xx2"] = "active current-page";


            foreach (var item in _context.Parametre.Select(x => x.Create_date))
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
            return View(await _context.Parametre.ToListAsync());
        }

        // GET: Parametre/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parametre = await _context.Parametre
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parametre == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(parametre);
        }

        // GET: Parametre/Create
        public IActionResult Create(string nd,string dossier)
        {
            ViewData["xx2"] = "active current-page";
            ViewData["d0"] = "active";
            ViewData["d2"] = "active";
            ViewBag.departements = _context1.Departement.ToList();
            if (nd!=null && dossier==null)
            {
                ViewBag.facture = _context_facture.Facture.Where(x => x.Numero_de_facture == "00" + Int32.Parse(nd)).FirstOrDefault();
                string dossier1 = _context_facture.Facture.Where(x => x.Numero_de_facture == "00" + Int32.Parse(nd)).FirstOrDefault().Numero_dossier;
                ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == dossier1).FirstOrDefault();
                
            }else if(nd==null && dossier != null)
            {
                ViewBag.facture = null;
                ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == dossier).FirstOrDefault();
            }
            

            ViewBag.user = Userconnected();
            return View();
        }

        public IActionResult Create0()
        {
            ViewBag.user = Userconnected();
            return View();
        }

        // POST: Parametre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero_Dossier,Pc,Pt,Pb,Numero_Facture,Patient,Temperature,Poids,Taille,Saturation,Rpm,Tsysbg,Tsysbd,Tdiasbg,Tdiasbd,Pbg,Pbd,Glycemie_capillaire,Create_date,Observation")] Parametre parametre)
        {
            if (ModelState.IsValid)
            {
                Facture facture = _context_facture.Facture.Where(x => x.Numero_de_facture == parametre.Numero_Facture).FirstOrDefault();
                Patient patient = _context_patient.Patient.Where(x => x.Numero_dossier == parametre.Numero_Dossier).FirstOrDefault();
                parametre.Create_date = DateTime.Now;
                parametre.Patient = patient.Nom + " " + patient.Prenom;
                parametre.Infirmier = Userconnected().nom;
                _context.Add(parametre);
                await _context.SaveChangesAsync();
                // if (_context_doctor.Doctor.Where(m => m.User_Name == Userconnected().nom).ToList().Count()>0)
                // {
                //     return RedirectToAction("Details2","Patient", new{fact=parametre.Numero_Facture});
                // }
                return RedirectToAction(nameof(Index1));
            }
            ViewData["xx2"] = "active current-page";
            ViewBag.user = Userconnected();
            return View(parametre);
        }

        // GET: Parametre/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parametre = await _context.Parametre.FindAsync(id);
            if (parametre == null)
            {
                return NotFound();
            }
            ViewData["xx2"] = "active current-page";
            ViewBag.user = Userconnected();
            ViewBag.facture = _context_facture.Facture.Where(x => x.Numero_de_facture == parametre.Numero_Facture).FirstOrDefault();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == parametre.Numero_Dossier).FirstOrDefault();
            return View(parametre);
        }

        // POST: Parametre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero_Dossier,Pc,Pt,Pb,Numero_Facture,Patient,Temperature,Poids,Taille,Saturation,Rpm,Tsysbg,Tsysbd,Tdiasbg,Tdiasbd,Pbg,Pbd,Glycemie_capillaire,Create_date,Observation")] Parametre parametre)
        {
            if (id != parametre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Patient patient = _context_patient.Patient.Where(x => x.Numero_dossier == parametre.Numero_Dossier).FirstOrDefault();
                    parametre.Create_date = DateTime.Now;
                    parametre.Patient = patient.Nom + " " + patient.Prenom;
                    parametre.Infirmier = Userconnected().nom;

                    _context.Update(parametre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParametreExists(parametre.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index1));
            }
            ViewBag.facture = _context_facture.Facture.Where(x => x.Numero_de_facture == parametre.Numero_Facture).FirstOrDefault();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == parametre.Numero_Dossier).FirstOrDefault();
            ViewData["xx2"] = "active current-page";
            ViewBag.user = Userconnected();

            return View(parametre);
        }

        // GET: Parametre/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            var parametre = await _context.Parametre
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parametre == null)
            {
                return NotFound();
            }
            ViewData["xx2"] = "active current-page";
            ViewBag.user = Userconnected();
            ViewBag.facture = _context_facture.Facture.Where(x => x.Numero_de_facture == parametre.Numero_Facture).FirstOrDefault();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == parametre.Numero_Dossier).FirstOrDefault();
            return View(parametre);
        }

        // POST: Parametre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parametre = await _context.Parametre.FindAsync(id);
            if (parametre != null)
            {
                _context.Parametre.Remove(parametre);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index1));
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


        private bool ParametreExists(int id)
        {
            return _context.Parametre.Any(e => e.Id == id);
        }
    }
}

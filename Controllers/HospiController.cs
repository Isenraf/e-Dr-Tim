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
    public class HospiController : Controller
    {
        private readonly HospiContext _context;
        private readonly UserContext _context_utilisateur;
        private readonly PatientContext _context_patient;
        private readonly DepartementContext _context_departement;
        private readonly DoctorContext _context_docteur;
        private readonly ParametreContext _context_parametre;
        private readonly Fiche3Context _context_fiche3;
        private readonly RondeContext _context_ronde;
        private readonly ExamenContext _context_examen;
        private readonly LNMEContext _context_lnme;
        private readonly ServiceContext _context_actes;



        public HospiController(HospiContext context, UserContext context_utilisateur, PatientContext context_patient, DepartementContext context_departement, DoctorContext context_docteur, ParametreContext context_parametre, Fiche3Context context_fiche3, RondeContext context_ronde, ExamenContext context_examen, LNMEContext context_lnme, ServiceContext context_actes)
        {
            _context = context;
            _context_utilisateur = context_utilisateur;
            _context_patient = context_patient;
            _context_departement = context_departement;
            _context_docteur = context_docteur;
            _context_parametre = context_parametre;
            _context_fiche3 = context_fiche3;
            _context_ronde = context_ronde;
            _context_examen = context_examen;
            _context_lnme = context_lnme;
            _context_actes = context_actes;
        }

        // GET: Hospi
        public async Task<IActionResult> Index()
        {
            ViewBag.user = Userconnected();
            return View(await _context.Hospi.ToListAsync());
        }



        public async Task<IActionResult> Index1()
        {
            ViewBag.user = Userconnected();
            return View(await _context.Hospi.ToListAsync());
        }

        public async Task<IActionResult> Index00(string target)
        {
            ViewBag.user = Userconnected();
            var hospis = await _context.Hospi.ToListAsync();
            if (target != null)
            {
                hospis = hospis.Where(x => x.Numero_dossier == target).ToList();
            }else
            {
                hospis = hospis.Where(x => x.EtatSortie != "Cloturé").ToList();
            }
            ViewBag.patient = _context_patient.Patient.FirstOrDefault(m => m.Numero_dossier == target);
            return View(hospis);
        }

        public async Task<IActionResult> Index0()
        {
            // Create a list of string.
            List<string> dates = new List<string>();
            List<string> dates1 = new List<string>();
            List<Stat> datesfinal = new List<Stat>();


            foreach (var item in _context.Hospi.Select(x => x.DateAdmission))
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
            return View(await _context.Hospi.ToListAsync());
        }




        // GET: Hospi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospi = await _context.Hospi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospi == null)
            {
                return NotFound();
            }
            ViewBag.fiche3 = _context_fiche3.Fiche3.Where(x => x.NumeroDossier == hospi.Numero_dossier).ToList();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == hospi.Numero_dossier).FirstOrDefault();
            ViewBag.parametres = _context_parametre.Parametre.Where(x => x.Numero_Dossier == hospi.Numero_dossier && x.Create_date>hospi.DateAdmission.AddDays(-1)).ToList();
            ViewBag.user = Userconnected();
            return View(hospi);
        }

        public async Task<IActionResult> Cloture(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospi = await _context.Hospi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospi == null)
            {
                return NotFound();
            }

            hospi.EtatSortie = "Cloturé";
            _context.Update(hospi);
                await _context.SaveChangesAsync();

            ViewBag.fiche3 = _context_fiche3.Fiche3.Where(x => x.NumeroDossier == hospi.Numero_dossier).ToList();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == hospi.Numero_dossier).FirstOrDefault();
            ViewBag.parametres = _context_parametre.Parametre.Where(x => x.Numero_Dossier == hospi.Numero_dossier).ToList();
            ViewBag.user = Userconnected();
            return View(hospi);
        }

        

        public async Task<IActionResult> Fiche1(int? id)
        {
            ViewBag.user = Userconnected();
            var hospi = await _context.Hospi
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == hospi.Numero_dossier).FirstOrDefault();
            ViewBag.parametres = _context_parametre.Parametre.Where(x => x.Numero_Facture == hospi.Numero_facture).FirstOrDefault();
            return View(hospi);
        }
        public async Task<IActionResult> Fiche2(int? id)
        {
            ViewBag.user = Userconnected();
            var hospi = await _context.Hospi
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == hospi.Numero_dossier).FirstOrDefault();
            return View(hospi);
        }
        public async Task<IActionResult> Fiche3(int? id)
        {
            ViewBag.user = Userconnected();
            var hospi = await _context.Hospi
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == hospi.Numero_dossier).FirstOrDefault();
            ViewBag.fiche3 = _context_fiche3.Fiche3.Where(x => x.NumeroDossier == hospi.Numero_dossier).ToList();
            return View(hospi);
        }
        public async Task<IActionResult> Fiche4(int? id)
        {
            ViewBag.user = Userconnected();
            var hospi = await _context.Hospi
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == hospi.Numero_dossier).FirstOrDefault();
            return View(hospi);
        }

        public IActionResult Fiche5()
        {
            ViewBag.user = Userconnected();

            return View();
        }
        public async Task<IActionResult>  Fiche6(int? id)
        {
            var hospi = await _context.Hospi
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (hospi == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == hospi.Numero_dossier).FirstOrDefault();
            ViewBag.parametre = _context_parametre.Parametre.Where(x => x.Numero_Dossier == hospi.Numero_dossier).OrderBy(x=>x.Id).LastOrDefault();
            ViewBag.rondes = _context_ronde.Ronde.Where(x => x.NumeroDossier == id.ToString()).ToList();
            ViewBag.examens = _context_examen.Examen.ToList();
            ViewBag.medicaments = _context_lnme.LNME.ToList();
            ViewBag.actes = _context_actes.Service.ToList();
            return View(hospi);
        }
        public IActionResult Fiche7()
        {
            ViewBag.user = Userconnected();

            return View();
        }
        public IActionResult Fiche8()
        {
            ViewBag.user = Userconnected();

            return View();
        }

        // GET: Hospi/Create
        public IActionResult Create(string nd)
        {
            ViewBag.user = Userconnected();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == nd).FirstOrDefault();
            ViewBag.departement = _context_departement.Departement.Select(x => x.Nom).ToList();
            ViewBag.doctors = _context_docteur.Doctor.ToList();

            return View();
        }

        // POST: Hospi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero_dossier,Nom,Sexe,DateNaissance,Adresse,Telephone,PersonneContactUrgence,TelephoneContactUrgence,GroupeSanguin,NumeroHospitalisation,DateAdmission,MotifAdmission,TypeHospitalisation,Service,Chambre,Lit,MedecinResponsable,DiagnosticAdmission,Temperature,TensionArterielle,Pouls,FrequenceRespiratoire,SaturationOxygene,Allergies,Antecedents,TraitementsEnCours,Prescriptions,ActesMedicaux,ExamensEtResultats,NotesInfirmieres,EvolutionClinique,DateSortie,DiagnosticSortie,EtatSortie,Destination,ResumeMedical,OrdonnanceSortie,Assurance,CoutSejour,PaiementEffectue,SoldeRestant")] Hospi hospi)
        {
            if (ModelState.IsValid)
            {
                var patient = await _context_patient.Patient.Where(x => x.Numero_dossier == hospi.Numero_dossier).FirstOrDefaultAsync();
                if (patient != null)
                {
                    hospi.Nom = patient.Nom + patient.Prenom;
                    hospi.DateNaissance = patient.DateNaissance;
                    hospi.Sexe = patient.Genre;
                    hospi.GroupeSanguin = patient.GroupeSanguin;
                }

                _context.Add(hospi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index00));
            }
            ViewBag.user = Userconnected();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == hospi.Numero_dossier).FirstOrDefault();
            ViewBag.departement = _context_departement.Departement.Select(x => x.Nom).ToList();
            ViewBag.doctors = _context_docteur.Doctor.ToList();
            return View(hospi);
        }

        // GET: Hospi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospi = await _context.Hospi.FindAsync(id);
            if (hospi == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(hospi);
        }

        // POST: Hospi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Numero_dossier,Nom,Sexe,DateNaissance,Adresse,Telephone,PersonneContactUrgence,TelephoneContactUrgence,GroupeSanguin,NumeroHospitalisation,DateAdmission,MotifAdmission,TypeHospitalisation,Service,Chambre,Lit,MedecinResponsable,DiagnosticAdmission,Temperature,TensionArterielle,Pouls,FrequenceRespiratoire,SaturationOxygene,Allergies,Antecedents,TraitementsEnCours,Prescriptions,ActesMedicaux,ExamensEtResultats,NotesInfirmieres,EvolutionClinique,DateSortie,DiagnosticSortie,EtatSortie,Destination,ResumeMedical,OrdonnanceSortie,Assurance,CoutSejour,PaiementEffectue,SoldeRestant")] Hospi hospi)
        {
            if (id != hospi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var patient = await _context_patient.Patient.Where(x => x.Numero_dossier == hospi.Numero_dossier).FirstOrDefaultAsync();
                    if (patient != null)
                    {
                        hospi.Nom = patient.Nom + patient.Prenom;
                        hospi.DateNaissance = patient.DateNaissance;
                        hospi.Sexe = patient.Genre;
                        hospi.GroupeSanguin = patient.GroupeSanguin;
                    }
                    _context.Update(hospi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospiExists(hospi.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index00));
            }
            ViewBag.user = Userconnected();
            return View(hospi);
        }

        // GET: Hospi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospi = await _context.Hospi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospi == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(hospi);
        }

        // POST: Hospi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var hospi = await _context.Hospi.FindAsync(id);
            if (hospi != null)
            {
                _context.Hospi.Remove(hospi);
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

        private bool HospiExists(int? id)
        {
            return _context.Hospi.Any(e => e.Id == id);
        }
    }
}

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
using ExcelEngine;
using Aspose.Cells;
using IronXL;
using BANA.Data;

namespace BANA.Controllers
{
    
    public class PatientController : Controller
    {
        private readonly PatientContext _context;
        private readonly FactureContext _context2;
        private readonly UserContext _context_user;
        private readonly ResultatContext _context_resultat;
        private readonly FicheContext _context_fiche;
        private readonly ParametreContext _context_parametre;
        private readonly CimContext _context_cim;
        private readonly ExamenContext _context_Examen;
        private readonly LNMEContext _context_LNME;
        private readonly FactureContext _context_facture;
        private readonly VaccinationContext _context_vaccination;
        private readonly DmiContext _context_dmi;
        private readonly ArchiveContext _context_archive;


        public PatientController(PatientContext context, FactureContext context2, UserContext context_user, ResultatContext context_resultat, FicheContext context_fiche, ParametreContext context_parametre, CimContext context_cim, ExamenContext context_Examen, LNMEContext context_LNME, FactureContext context_facture, VaccinationContext context_vaccination,DmiContext context_dmi,ArchiveContext context_archive)
        {
            _context = context;
            _context2 = context2;
            _context_user = context_user;
            _context_resultat = context_resultat;
            _context_fiche = context_fiche;
            _context_parametre = context_parametre;
            _context_cim = context_cim;
            _context_Examen = context_Examen;
            _context_LNME = context_LNME;
            _context_facture = context_facture;
            _context_vaccination = context_vaccination;
            _context_dmi = context_dmi;
            _context_archive = context_archive;
        }

        // GET: Patient
        [Authorize]
        public async Task<IActionResult> Index(string SearchString2)
        {

            ViewData["fasttable"] = "oui";
            ViewData["p0"] = "active";
            ViewData["p1"] = "active";
            ViewData["total"] = _context.Patient.Count();
            ViewData["Actifs"] = _context.Patient.Where(x => x.Acrif == true).Count();
            ViewData["Inactifs"] = _context.Patient.Where(x => x.Acrif == false).Count();
            ViewBag.user = Userconnected();
            if (SearchString2 != null)
            {
                var lespatients = await _context.Patient.Where(x => x.Numero_dossier.ToLower() == SearchString2.ToLower()).OrderByDescending(x => x.Derniere_visite).ToListAsync();
                return View(lespatients.Take(100));
            }
            else
            {
                return View(await _context.Patient.OrderByDescending(x => x.Derniere_visite).Take(100).ToListAsync());
            }
        }
         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Indexx(string SearchString2, string SearchString)
        {
           
            ViewData["fasttable"]="oui";
            ViewData["p0"]="active";
            ViewData["p1"]="active";
            ViewData["total"]=_context.Patient.Count();
            ViewData["Actifs"]=_context.Patient.Where(x => x.Acrif == true).Count();
            ViewData["Inactifs"]=_context.Patient.Where(x => x.Acrif == false).Count();
            ViewBag.user=Userconnected();
            var patients = _context.Patient.OrderByDescending(x => x.Derniere_visite).ToListAsync();
            if (SearchString!=null)
            {
                var lespatients = await _context.Patient.Where(x => x.Numero_dossier.ToLower().Contains(SearchString.ToLower()) || x.Nom.Trim().ToLower().Contains(SearchString.Trim().ToLower()) || x.Phone.ToLower().Contains(SearchString.ToLower()) || x.DateNaissance.ToString().ToLower().Contains(SearchString.ToLower())).OrderByDescending(x => x.Derniere_visite).ToListAsync();
            return View(lespatients.Take(10000));
            }
            if (SearchString2 != null)
            {
                var lespatients = await _context.Patient.Where(x => x.Numero_dossier.ToLower() == SearchString2.ToLower()).OrderByDescending(x => x.Derniere_visite).ToListAsync();
                return View(lespatients.Take(100));
            }
            else
            {
                return View(await _context.Patient.OrderByDescending(x => x.Derniere_visite).Take(100).ToListAsync());
            }
            
        }
         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
         public async Task<IActionResult> Index0()
        {
            // Create a list of string.
            List<string> dates = new List<string>();
            List<string> dates1 = new List<string>();
            List<Stat> datesfinal = new List<Stat>();


            foreach (var item in _context2.Facture.OrderByDescending(x => x.Id).Where(x => x.Medecin.ToLower().Trim().Replace(" ", "") == Userconnected().nom.ToLower().Trim().Replace(" ", "") && x.Type.Contains("consultation".ToLower()) && x.Etat_patient == "Cloturé").Select(x => x.Create_date))
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
            return View(await _context2.Facture.Where(x => x.Medecin.ToLower().Trim().Replace(" ", "") == Userconnected().nom.ToLower().Trim().Replace(" ", "") && x.Type.Contains("consultation".ToLower()) && x.Etat_patient == "Cloturé").ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> SearchWord(string SearchString, bool notUsed)
        {
            if (String.IsNullOrEmpty(SearchString))
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"] = "active";
            ViewData["p1"] = "active";
            ViewData["total"] = _context.Patient.Count();
            ViewData["Actifs"] = _context.Patient.Where(x => x.Acrif == true).Count();
            ViewData["Inactifs"] = _context.Patient.Where(x => x.Acrif == false).Count();
            ViewBag.user = Userconnected();

            var lespatients = await _context.Patient.Where(x => x.Numero_dossier.ToLower().Contains(SearchString.ToLower()) || x.Nom.Trim().ToLower().Contains(SearchString.Trim().ToLower()) || x.Phone.ToLower().Contains(SearchString.ToLower()) || x.DateNaissance.ToString().ToLower().Contains(SearchString.ToLower())).OrderByDescending(x => x.Derniere_visite).ToListAsync();
            return View(lespatients.Take(10000));

        }
         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> SearchWord2(string SearchString2,bool notUsed)
        {
            if (String.IsNullOrEmpty(SearchString2))
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"]="active";
            ViewData["p1"]="active";
            ViewData["total"]=_context.Patient.Count();
            ViewData["Actifs"]=_context.Patient.Where(x => x.Acrif == true).Count();
            ViewData["Inactifs"]=_context.Patient.Where(x => x.Acrif == false).Count();
            ViewBag.user=Userconnected();
   
              var  lespatients=await _context.Patient.Where(x=>x.Numero_dossier.ToLower()==SearchString2.ToLower()).OrderByDescending(x=>x.Derniere_visite).ToListAsync();
            return View(lespatients.Take(200));

        }


        // GET: Patient/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            // ⚡ Un seul passage en base pour total, actifs, inactifs
            var stats = await _context.Patient
                .GroupBy(p => 1)
                .Select(g => new
                {
                    Total = g.Count(),
                    Actifs = g.Count(x => x.Acrif == true),
                    Inactifs = g.Count(x => x.Acrif == false)
                })
                .FirstOrDefaultAsync();

            ViewData["total"] = stats?.Total ?? 0;
            ViewData["Actifs"] = stats?.Actifs ?? 0;
            ViewData["Inactifs"] = stats?.Inactifs ?? 0;

            // ⚡ Chargement direct du patient
            var patient = await _context.Patient
                .AsNoTracking() // lecture seule = plus rapide
                .FirstOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (patient.Taille == null)
            {
                return RedirectToAction("Noti", new { id });
            }

            ViewData["p0"] = "active";
            ViewData["p3"] = "active";

            // ⚡ Factures filtrées et ordonnées directement en SQL
            var factures = await _context2.Facture
                .Where(x => x.Numero_dossier == patient.Numero_dossier && !x.Type.Contains("proforma"))
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            ViewBag.factures = factures;

            // ⚡ Résultats idem
            var resultats = await _context_resultat.Resultat
                .Where(x => x.NumeroDossier == patient.Numero_dossier)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            ViewBag.Resultat = resultats;

            // ⚡ Calcul du solde directement en SQL (SUM en DB au lieu de ramener tout)
            var cautionQuery = _context2.Facture
                .Where(x => x.Numero_dossier == patient.Numero_dossier && x.Type == "caution");

            var solde = await cautionQuery.SumAsync(x => (decimal?)x.Montant_recu_patient) ?? 0
                    - await cautionQuery.SumAsync(x => (decimal?)x.Nombre_impression) ?? 0;

            ViewData["solde"] = solde;

            ViewBag.user = Userconnected();

            return View(patient);
        }


        // GET: Patient/Details/5
        [Authorize(AuthenticationSchemes = "CookieAuthDmi")]
        public async Task<IActionResult> Archives(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            // ⚡ Chargement direct du patient
            var patient = await _context.Patient
                .AsNoTracking() // lecture seule = plus rapide
                .FirstOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.archives = _context_archive.Archive.ToList();

            ViewBag.user = Userconnected();
            return View(patient);
        }
        
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> CreateArchives(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            // ⚡ Chargement direct du patient
            var patient = await _context.Patient
                .AsNoTracking() // lecture seule = plus rapide
                .FirstOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.archives = null;

            ViewBag.user = Userconnected();
            return View(patient);
        }

        



         // GET: Patient/Details/5
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Ordonnances(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user = Userconnected();

            // ⚡ Chargement direct du patient
            var patient = await _context_dmi.Dmi
                .AsNoTracking() // lecture seule = plus rapide
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient != null)
            {

                ViewBag.dmi = _context_dmi.Dmi.Where(x => x.NumeroDossier == patient.NumeroDossier).ToList();
            }
            else
            {
                ViewBag.dmi = null;
            }
            //ViewData["nombre"] = _context_dmi.Dmi.Where(x => x.NumeroDossier == patient.NumeroDossier).ToList().Count();
            
            return View(patient);
        }

        

        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Noti([Bind("Id,Nom,Numero_dossier,Prenom,Phone,Email,Img,Taille,Description,Notesimportant,Create_date,DateNaissance,Genre,Profession,Quartier,GroupeSanguin,AutresContact,apparition,Intervenant,Etat,Acrif")] Patient patient)
        {
            

            var patient1 = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == patient.Id);
            // if (patient == null)
            // {
            //     return RedirectToAction("Page404", "Stock");
            // }
            patient1.Taille = patient.Taille;
            ViewData["p0"] = "active";
            ViewData["p3"] = "active";
             _context.Update(patient1);
            await _context.SaveChangesAsync();
            
            ViewBag.user = Userconnected();
            return RedirectToAction("Details",new{id=patient1.Id});
        }

        [Authorize]
        public async Task<IActionResult> Noti(int? id)
        {
            

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"] = "active";
            ViewData["p3"] = "active";
            
            ViewBag.user = Userconnected();
            return View(patient);
        }


         [Authorize(AuthenticationSchemes = "CookieAuthDmi")]
        public async Task<IActionResult> Detailss(int? id)
        {
           
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.fiches = _context_fiche.Fiche.Where(x => x.Categorie != "Certificat Médical").ToList();
            ViewBag.cartificats = _context_fiche.Fiche.Where(x => x.Categorie == "Certificat Médical").ToList();
            ViewData["p0"]="active";
            ViewData["p3"]="active";
           
            ViewBag.user=Userconnected();
            return View(patient);
        }
         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Vaccination(int? id)
        {
           
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.fiches = _context_fiche.Fiche.Where(x => x.Categorie != "Certificat Médical").ToList();
            ViewBag.cartificats = _context_fiche.Fiche.Where(x => x.Categorie == "Certificat Médical").ToList();
            ViewData["p0"]="active";
            ViewData["p3"]="active";
            ViewBag.vaccination = _context_vaccination.Vaccination.FirstOrDefault(x => x.Numero_Dossier == patient.Numero_dossier);
           
            ViewBag.user=Userconnected();
            return View(patient);
        }
         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Fichev1(int? id)
        {
           
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"]="active";
            ViewData["p3"]="active";
           
            ViewBag.user=Userconnected();
            return View(patient);
        }
         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Fichev2(int? id)
        {
           
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"]="active";
            ViewData["p3"]="active";
           
            ViewBag.user=Userconnected();
            return View(patient);
        }
         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Fichev3(int? id)
        {
           
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"]="active";
            ViewData["p3"]="active";
           
            ViewBag.user=Userconnected();
            return View(patient);
        }
         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Fichev4(int? id)
        {
           
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"]="active";
            ViewData["p3"]="active";
           
            ViewBag.user=Userconnected();
            return View(patient);
        }
         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Fichev5(int? id)
        {
           
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"]="active";
            ViewData["p3"]="active";


            var parametres = _context_parametre.Parametre
                .Where(x => x.Numero_Dossier == patient.Numero_dossier
                            && x.Create_date <= patient.DateNaissance.AddDays(1095)   // SQL filtre min
                           ) // SQL filtre max
                      .ToList();

            ViewBag.parametres = parametres;

            ViewBag.user=Userconnected();
            return View(patient);
        }
         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Fichev55(int? id)
        {
           
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"]="active";
            ViewData["p3"]="active";

            var parametres = _context_parametre.Parametre
                .Where(x => x.Numero_Dossier == patient.Numero_dossier
                            && x.Create_date <= patient.DateNaissance.AddDays(1095)   // SQL filtre min
                           ) // SQL filtre max
                      .ToList();

            ViewBag.parametres = parametres;

            ViewBag.user=Userconnected();
            return View(patient);
        }
        
         [Authorize(AuthenticationSchemes = "CookieAuthDmi")]
        public async Task<IActionResult> Fichev6(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"] = "active";
            ViewData["p3"] = "active";

            var parametres = _context_parametre.Parametre
                .Where(x => x.Numero_Dossier == patient.Numero_dossier
                            && x.Create_date >= patient.DateNaissance.AddDays(1095)   // SQL filtre min
                            && x.Create_date <= patient.DateNaissance.AddYears(20)) // SQL filtre max
                .ToList();

            ViewBag.parametres = parametres;
            ViewBag.user = Userconnected();
            return View(patient);
        }

         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Fichev66(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"] = "active";
            ViewData["p3"] = "active";

            var parametres = _context_parametre.Parametre
                 .Where(x => x.Numero_Dossier == patient.Numero_dossier
                             && x.Create_date >= patient.DateNaissance.AddDays(1095)   // SQL filtre min
                             && x.Create_date <= patient.DateNaissance.AddYears(20)) // SQL filtre max
                 .ToList();

            ViewBag.parametres = parametres;
            ViewBag.user = Userconnected();
            return View(patient);
        }
        
         [Authorize(AuthenticationSchemes = "CookieAuthDmi")]
        public async Task<IActionResult> Fichev7(int? id)
        {
           
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"]="active";
            ViewData["p3"]="active";


            var parametres = _context_parametre.Parametre
                .Where(x => x.Numero_Dossier == patient.Numero_dossier
                            && x.Create_date <= patient.DateNaissance.AddYears(2)   // SQL filtre min
                           ) // SQL filtre max
                      .ToList();

            ViewBag.parametres = parametres;

            ViewBag.user=Userconnected();
            return View(patient);
        }

         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Fichev77(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"] = "active";
            ViewData["p3"] = "active";


            var parametres = _context_parametre.Parametre
                .Where(x => x.Numero_Dossier == patient.Numero_dossier
                            && x.Create_date <= patient.DateNaissance.AddYears(2)   // SQL filtre min
                           ) // SQL filtre max
                      .ToList();

            ViewBag.parametres = parametres;

            ViewBag.user = Userconnected();
            return View(patient);
        }

         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> FicheCroissance(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"] = "active";
            ViewData["p3"] = "active";

            ViewBag.user = Userconnected();
            return View(patient);
        }


        [Authorize(AuthenticationSchemes = "CookieAuthDmi")]
        public async Task<IActionResult> Details0(int? id, string fact)
        {
            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.cim = _context_cim.Cim.ToList();
            ViewBag.LNME = _context_LNME.LNME.Where(x => x.actif == true).ToList();
            ViewBag.Labo = _context_Examen.Examen.Where(x => x.Categorie == "Analyse Médicale").ToList();
            ViewBag.Imagerie = _context_Examen.Examen.Where(x => x.Categorie == "Imagerie Médicale" || x.Categorie == "Ophtalmologie").ToList();
            ViewBag.Anapath = _context_Examen.Examen.Where(x => x.Categorie == "Anapath").ToList();
            ViewBag.Exploration = _context_Examen.Examen.Where(x => x.Categorie == "Exploration Fonctionnelle").ToList();
            ViewBag.parametre = await _context_parametre.Parametre.Where(x => x.Numero_Dossier == patient.Numero_dossier).OrderBy(x => x.Id).LastOrDefaultAsync();
            ViewBag.facture = _context_facture.Facture.FirstOrDefault(m => m.Numero_de_facture == fact);
            return View(patient);
        }
        
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Details00(int? id,string fact)
        {
            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.cim = _context_cim.Cim.ToList();
            ViewBag.LNME = _context_LNME.LNME.Where(x=>x.actif==true).ToList();
            ViewBag.Labo = _context_Examen.Examen.Where(x=>x.Categorie=="Analyse Médicale").ToList();
            ViewBag.Imagerie = _context_Examen.Examen.Where(x=>x.Categorie=="Imagerie Médicale" || x.Categorie=="Ophtalmologie").ToList();
            ViewBag.Anapath = _context_Examen.Examen.Where(x=>x.Categorie=="Anapath").ToList();
            ViewBag.Exploration = _context_Examen.Examen.Where(x=>x.Categorie=="Exploration Fonctionnelle").ToList();
            ViewBag.parametre = await _context_parametre.Parametre.Where(x => x.Numero_Dossier == patient.Numero_dossier).OrderBy(x=>x.Id).LastOrDefaultAsync();
            ViewBag.facture = _context_facture.Facture.FirstOrDefault(m => m.Numero_de_facture == fact);
            return View(patient);
        }

        [Authorize(AuthenticationSchemes = "CookieAuthDmi")]
        // GET: Patient/Details/5
        public async Task<IActionResult> Details2(string fact)
        {
            var facture = await _context_facture.Facture
                .FirstOrDefaultAsync(m => m.Numero_de_facture == fact);
            ViewData["fact"] = fact;

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Numero_dossier == facture.Numero_dossier);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"] = "active";
            ViewData["p3"] = "active";
            ViewData["dmip"] = "ok";
            //ViewBag.factures =  _context2.Facture.Where(x=>x.Numero_dossier==patient.Numero_dossier && !x.Type.Contains("proforma")).OrderByDescending(x=>x.Id).ToList();
            //ViewBag.Resultat =  _context_resultat.Resultat.Where(x=>x.NumeroDossier==patient.Numero_dossier).OrderByDescending(x=>x.Id).ToList();
            //ViewData["ca"]=_context2.Facture.Where(x=>x.Numero_dossier==patient.Numero_dossier && !x.Type.Contains("proforma")).Select(x=>x.Total_ht).Sum();
            ViewBag.user = Userconnected();
            ViewBag.fiches = _context_fiche.Fiche.Where(x => x.Categorie == "Certificat Médical").ToList();
            //ViewData["solde"]=_context2.Facture.Where(x=>x.Numero_dossier==patient.Numero_dossier && x.Type=="caution").Select(x=>x.Montant_recu_patient).ToList().Sum()-_context2.Facture.Where(x=>x.Numero_dossier==patient.Numero_dossier && x.Type=="caution").Select(x=>x.Nombre_impression).ToList().Sum();
            ViewBag.parametre = await _context_parametre.Parametre.Where(x => x.Numero_Facture == fact).OrderBy(x => x.Id).LastOrDefaultAsync();
            if (ViewBag.parametre == null)
            {
                return RedirectToAction("Create", "Parametre", new { nd = fact });
            }
            return View(patient);
        }
        
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        // GET: Patient/Details/5
        public async Task<IActionResult> Details22(string fact)
        {
            var facture = await _context_facture.Facture
                .FirstOrDefaultAsync(m => m.Numero_de_facture == fact);
            ViewData["fact"] = fact;

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Numero_dossier == facture.Numero_dossier);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["p0"] = "active";
            ViewData["p3"] = "active";
            ViewData["dmip"] = "ok";
            //ViewBag.factures =  _context2.Facture.Where(x=>x.Numero_dossier==patient.Numero_dossier && !x.Type.Contains("proforma")).OrderByDescending(x=>x.Id).ToList();
            //ViewBag.Resultat =  _context_resultat.Resultat.Where(x=>x.NumeroDossier==patient.Numero_dossier).OrderByDescending(x=>x.Id).ToList();
            //ViewData["ca"]=_context2.Facture.Where(x=>x.Numero_dossier==patient.Numero_dossier && !x.Type.Contains("proforma")).Select(x=>x.Total_ht).Sum();
            ViewBag.user = Userconnected();
            ViewBag.fiches = _context_fiche.Fiche.Where(x => x.Categorie == "Certificat Médical").ToList();
            //ViewData["solde"]=_context2.Facture.Where(x=>x.Numero_dossier==patient.Numero_dossier && x.Type=="caution").Select(x=>x.Montant_recu_patient).ToList().Sum()-_context2.Facture.Where(x=>x.Numero_dossier==patient.Numero_dossier && x.Type=="caution").Select(x=>x.Nombre_impression).ToList().Sum();
            ViewBag.parametre = await _context_parametre.Parametre.Where(x => x.Numero_Facture == fact).OrderBy(x => x.Id).LastOrDefaultAsync();
            if (ViewBag.parametre == null)
            {
                return RedirectToAction("Create", "Parametre", new { nd = fact });
            }
            return View(patient);
        }
        
         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public IActionResult Fiche1()
        {
            ViewBag.user = Userconnected();
            ViewData["fiche1"] = "ok";
            return View();
        }

         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public IActionResult Fiche2()
        {
            ViewBag.user = Userconnected();
            ViewData["fiche1"] = "ok";
            return View();
        }

         [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public IActionResult Fiche3()
        {
            ViewBag.user = Userconnected();
            ViewData["fiche1"] = "ok";
            return View();
        }
        [Authorize]
        // GET: Patient/Details/5
        public async Task<IActionResult> Fusion(int? id, string ne)
        {
            ViewData["total"] = _context.Patient.Count();
            ViewData["Actifs"] = _context.Patient.Where(x => x.Acrif == true).Count();
            ViewData["Inactifs"] = _context.Patient.Where(x => x.Acrif == false).Count();
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["error"] = ne;
            ViewData["p0"] = "active";
            ViewData["p3"] = "active";
            var mesfactures=_context2.Facture.Where(x => x.Numero_dossier == patient.Numero_dossier && !x.Type.Contains("proforma") && x.Etat_patient == "Cloturé").OrderByDescending(x => x.Id).ToList();
            ViewBag.factures = mesfactures;
            if (mesfactures.Count()==0){ViewData["nombrefcature"] = "0";
            }else if(mesfactures.Count()==1){ViewData["nombrefcature"] = "1";}else{ViewData["nombrefcature"] = "2";}
            
            ViewData["ca"] = _context2.Facture.Where(x => x.Numero_dossier == patient.Numero_dossier && !x.Type.Contains("proforma")).Select(x => x.Total_ht).Sum();
            ViewBag.user = Userconnected();
            return View(patient);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fusion1([Bind("Id,Numero_dossier,Create_date,Genre,Derniere_visite")] Patient fusion)
        {

            var factures= await _context2.Facture.Where(x=>x.Numero_dossier==fusion.Numero_dossier &&!x.Type.Contains("proforma") &&!x.Type.Contains("caution") &&  x.Create_date>=fusion.Create_date && x.Create_date<=fusion.Derniere_visite.AddDays(1).AddSeconds(-1) && x.Etat_patient=="Cloturé").ToListAsync();
            if (fusion.Genre=="assurance")
            {
              factures=factures.Where(x=>x.Type.Contains("assurance")).ToList();
            }else{
              factures=factures.Where(x=>!x.Type.Contains("assurance")).ToList();
            }
            var MaFacturePrincipale=factures.FirstOrDefault();
            // Console.WriteLine(MaFacturePrincipale.Assureur);
            // Console.WriteLine(MaFacturePrincipale.assure_prin);
            // Console.WriteLine(MaFacturePrincipale.Assur_Adresse);
            // Console.WriteLine(MaFacturePrincipale.Assur_BP);
            // Console.WriteLine(MaFacturePrincipale.Assur_NIU);
            // Console.WriteLine(MaFacturePrincipale.Assur_Tel);
            if (MaFacturePrincipale==null)
            {
                return RedirectToAction("Fusion",new{Id=fusion.Id, ne="2"});
            }
                
            int j=0;
            string source="";
            string l1="λ";
            string l2="λ";
            string l3="λ";
            string l4="λ";
            foreach (var item in factures)
            {
                //Console.WriteLine(item.Numero_de_facture  +item.Type);
                
                    
                   
                    MaFacturePrincipale.Date_entree=fusion.Create_date;
                    MaFacturePrincipale.Date_sortie=fusion.Derniere_visite;
                    l1+=item.Ligne_facturation1.ToString().Remove(0,1);
                    l2+=item.Ligne_facturation2.ToString().Remove(0,1);
                    l3+=item.Ligne_facturation3.ToString().Remove(0,1);
                    l4+=item.Ligne_facturation4.ToString().Remove(0,1);
                 if (j!=0)
                    {   
                    MaFacturePrincipale.Net_a_payer_patient+=item.Net_a_payer_patient;
                    MaFacturePrincipale.Net_a_payer_assurance+=item.Net_a_payer_assurance;
                    MaFacturePrincipale.Create_date=fusion.Derniere_visite;
                    MaFacturePrincipale.Montant_recu_patient+=item.Montant_recu_patient;
                    MaFacturePrincipale.Remise+=item.Remise;
                    MaFacturePrincipale.Taxe1+=item.Taxe1;
                    MaFacturePrincipale.Total_ht+=item.Total_ht;
                    MaFacturePrincipale.Total_ttc+=item.Total_ttc;
                    MaFacturePrincipale.ticketmoderateur+=item.ticketmoderateur;
                     }
                j++;
                source=source+item.Numero_de_facture.ToString()+";";
                    
            }
            MaFacturePrincipale.Ligne_facturation1=l1;
            MaFacturePrincipale.Ligne_facturation2=l2;
            MaFacturePrincipale.Ligne_facturation3=l3;
            MaFacturePrincipale.Ligne_facturation4=l4;
            MaFacturePrincipale.Type="generique_assurance";
            
            

             List<string> liste_de_facturation1 = new List<string>();
            
            int i=0;
            string result="";
            foreach(char c in MaFacturePrincipale.Ligne_facturation1) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                      liste_de_facturation1.Add(result);
                      result="";
                  }
                  
              }else{
                  result+=c.ToString();
              }
              i++;
              //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes1=liste_de_facturation1;

            List<string> liste_de_facturation2 = new List<string>();
            
             i=0;
             result="";
            foreach(char c in MaFacturePrincipale.Ligne_facturation2) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                      liste_de_facturation2.Add(result);
                      result="";
                  }
                  
              }else{
                  result+=c.ToString();
              }
              i++;
              //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes2=liste_de_facturation2;

            List<string> liste_de_facturation3 = new List<string>();
            
            i=0;
            result="";
            foreach(char c in MaFacturePrincipale.Ligne_facturation3) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                      liste_de_facturation3.Add(result);
                      result="";
                  }
                  
              }else{
                  result+=c.ToString();
              }
              i++;
              //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes3=liste_de_facturation3;

            List<string> liste_de_facturation4 = new List<string>();
            
            i=0;
            result="";
            foreach(char c in MaFacturePrincipale.Ligne_facturation4) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                      liste_de_facturation4.Add(result);
                      result="";
                  }
                  
              }else{
                  result+=c.ToString();
              }
              i++;
              //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes4=liste_de_facturation4;

            
            
            ViewData["f0"]="active";
            ViewData["f3"]="active";
            ViewData["details"]="ok";
            ViewData["qrcode"]="ok";
            ViewData["source"]=source;
            ViewData["type"]=fusion.Genre;
            ViewBag.user=Userconnected();
            return View(MaFacturePrincipale);
        }

        // GET: Patient/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["p0"] = "active";
            ViewData["p2"] = "active";
            ViewData["inputimage"] = "ok";
            ViewData["total"] = _context.Patient.Count();
            ViewData["Actifs"] = _context.Patient.Where(x => x.Acrif == true).Count();
            ViewData["Inactifs"] = _context.Patient.Where(x => x.Acrif == false).Count();
            ViewBag.user = Userconnected();
            return View();
        }

        // POST: Patient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Numero_dossier,Prenom,Phone,Email,Img,Taille,Description,Notesimportant,Create_date,DateNaissance,Genre,Profession,Quartier,GroupeSanguin,AutresContact,apparition,Intervenant,Etat,Acrif")] Patient patient)
        {
            ViewBag.doublons = null;
            if (ModelState.IsValid)
            {
                string premierMot = patient.Nom.Split(' ')[0];
                var patients = _context.Patient.Where(x => x.DateNaissance == patient.DateNaissance && x.Genre == patient.Genre && x.Nom.ToLower().Contains(premierMot.ToLower())).ToList();
                ViewBag.doublons = patients;
                //Console.WriteLine(premierMot);
                if (patients.Count() > 0)
                {
                    Console.WriteLine(patient.apparition);
                }
                else if (patients.Count() == 0)
                {
                    patient.Create_date = DateTime.Now;
                    patient.Etat = "Approuvé";
                    patient.Derniere_visite = DateTime.Now;
                    patient.Acrif = true;
                    patient.Numero_dossier = (Convert.ToInt32(GetlastPatient()) + 1).ToString();
                    _context.Add(patient);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details",new{id=patient.Id});
                }
                 if (patient.apparition == 33)
                {
                    patient.Create_date = DateTime.Now;
                    patient.Etat = "Approuvé";
                    patient.Derniere_visite = DateTime.Now;
                    patient.Acrif = true;
                    patient.Numero_dossier = (Convert.ToInt32(GetlastPatient()) + 1).ToString();
                    _context.Add(patient);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details",new{id=patient.Id});
                }
                
                patient.apparition = 33;
                
            }
            ViewData["total"]=_context.Patient.Count();
            ViewData["Actifs"]=_context.Patient.Where(x => x.Acrif == true).Count();
            ViewData["Inactifs"]=_context.Patient.Where(x => x.Acrif == false).Count();
            ViewData["p0"]="active";
            ViewData["p2"]="active";
            ViewBag.user=Userconnected();
            return View(patient);
        }

        // GET: Patient/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["inputimage"] = "ok";
            ViewData["p0"] = "active";
            ViewData["p4"] = "active";
            ViewBag.user = Userconnected();
            return View(patient);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Numero_dossier,Prenom,Phone,Email,Img,Taille,Description,Notesimportant,Create_date,DateNaissance,Derniere_visite,Genre,Profession,Quartier,GroupeSanguin,AutresContact,apparition,Intervenant,Etat,Acrif")] Patient patient)
        {
            if (id != patient.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
                    {
                        return RedirectToAction("Page404", "Stock");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details",new{id=patient.Id});
            }
            ViewData["p0"]="active";
            ViewData["p4"]="active";
            ViewBag.user=Userconnected();
            return View(patient);
        }

        // GET: Patient/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["p0"] = "active";
            ViewData["p4"] = "active";
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user = Userconnected();
            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patient.FindAsync(id);
            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.Id == id);
        }

        private string GetlastPatient()
        {
            if (_context.Patient.Count()==0)
            {
                return "0";
            }else
            {
                //return 0;
                return _context.Patient.OrderBy(x=>x.Id).Select(x=>x.Numero_dossier).LastOrDefault();
            }
            
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
    }
}

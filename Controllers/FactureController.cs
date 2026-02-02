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
using NuGet.Packaging;
using iTextSharp.text;
using BANA.PDF;
using System.Globalization;

namespace BANA.Controllers
{
    
    public class FactureController : Controller
    {
        private readonly FactureContext _context;
        private readonly ServiceContext _context_service;
        private readonly PatientContext _context_patient;
        private readonly DoctorContext _context_doctor;
        private readonly AssuranceContext _context_assurance;
        private readonly UserContext _context_utilisateur;
        private readonly StockContext _context_stock;
        private readonly ExamenContext _context_examen;
        private readonly StaffContext _context_staff;
        private readonly PaidContext _context_paid;



        public FactureController(FactureContext context, ServiceContext context_service, PatientContext context_patient, DoctorContext context_doctor, UserContext context_utilisateur, AssuranceContext context_assurance, StockContext context_stock, ExamenContext context_examen, StaffContext context_staff, PaidContext context_paid)
        {
            _context = context;
            _context_service = context_service;
            _context_patient = context_patient;
            _context_doctor = context_doctor;
            _context_assurance = context_assurance;
            _context_utilisateur = context_utilisateur;
            _context_stock = context_stock;
            _context_examen = context_examen;
            _context_staff = context_staff;
            _context_paid = context_paid;

        }
        [Authorize]
        public async Task<IActionResult> Tableau()
        {
            var trump = UserAccess("A1");
            if (Userconnected().Type_de_compte != "administrateur" && Userconnected().Type_de_compte != "super-Admin")
            {
                return RedirectToAction("Page405", "Stock");
            }
            ViewData["chart"] = "ok";
            ViewData["1"] = "active";

            ViewBag.user = Userconnected();
            ViewData["patients"] = _context_patient.Patient.Count();
            ViewData["docteurs"] = _context_doctor.Doctor.Count();
            ViewData["assurances"] = _context_assurance.Assurance.Count();
            ViewData["staff"] = _context_staff.Staff.Count();

            var factures = await _context.Facture.Where(x => !x.Type.Contains("proforma")).ToListAsync();
            // ViewData["caassur"] = factures.Select(x => (long)x.Net_a_payer_assurance).Sum();
            // ViewData["creanceassr"] = factures.Select(x => (long)x.Net_a_payer_assurance).Sum() - factures.Select(x => (long)x.Montant_recu_assurance).Sum();
            // ViewData["creanceesp"] = factures.Where(x => x.Intervenant != "" && x.Intervenant != null).Select(x => (long)x.Net_a_payer_patient).Sum() - factures.Where(x => x.Intervenant != "" && x.Intervenant != null).Select(x => (long)x.Montant_recu_patient).Sum();
            // ViewData["caesp"] = factures.Select(x => (long)x.Net_a_payer_patient).Sum();
            // ViewData["remise"] = factures.Select(x => x.Remise).Sum();
            // ViewData["taxe"] = factures.Select(x => x.Taxe1).Sum();
            // ViewData["camoisactuel"] = factures.Where(x => x.Create_date.Year == DateTime.Now.Year && x.Create_date.Month == DateTime.Now.Month).Select(x => x.Net_a_payer_assurance).Sum() + factures.Where(x => x.Create_date.Year == DateTime.Now.Year && x.Create_date.Month == DateTime.Now.Month).Select(x => x.Net_a_payer_patient).Sum();
            // ViewData["caannuel"] = factures.Where(x => x.Create_date.Year == DateTime.Now.Year && x.Create_date.Month == DateTime.Now.Month).Select(x => (long)x.Net_a_payer_assurance).Sum() + factures.Where(x => x.Create_date.Year == DateTime.Now.Year).Select(x => (long)x.Net_a_payer_patient).Sum();
            // ViewData["catotal"] = factures.Where(x => x.Create_date.Year == DateTime.Now.Year && x.Create_date.Month == DateTime.Now.Month).Select(x => (long)x.Net_a_payer_assurance).Sum() + factures.Select(x => (long)x.Net_a_payer_patient).Sum();
            // ViewData["x1"] = (100 * (factures.Select(x => x.Net_a_payer_assurance).Sum() - factures.Select(x => x.Montant_recu_assurance).Sum()) / factures.Select(x => x.Net_a_payer_assurance).Sum()) + " %";
            // ViewData["x2"] = (100 * (factures.Select(x => (long)x.Net_a_payer_patient).Sum() - factures.Select(x => x.Montant_recu_patient).Sum()) / factures.Select(x => (long)x.Net_a_payer_patient).Sum()) + " %";

            // var Lignespaid = _context_paid.Paid.Select(x => x.Create_date.ToShortDateString());
            // List<string> mesdates = new List<string>();
            // IQueryable<string> DateQuery = from m in _context_paid.Paid
            //                                orderby m.Id
            //                                select m.Create_date.ToShortDateString();
            // mesdates.AddRange(DateQuery.Distinct());
            // if (mesdates.Count() != 0 && DateTime.Now >= DateTime.Parse(DateTime.Now.ToShortDateString()).AddHours(13))
            // {
            //     mesdates.Add(DateTime.Parse(mesdates.LastOrDefault()).AddDays(1).ToShortDateString());
            // }

            // ViewBag.dates = mesdates.Distinct().Reverse().Take(10);
            return View(_context_paid.Paid);
        }
        
        [Authorize]
        public async Task<IActionResult> Index(string Etat, string date, string SearchString)
        {

            ViewData["14"] = "active";
            ViewData["date"] = date;
            var factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution")).OrderByDescending(x => x.Id).ToListAsync();
            if (Etat != null)
            {
                factures = factures.Where(x => x.Etat_patient == Etat).ToList();
            }
            if (date != null)
            {
                factures = factures.Where(x => x.Create_date >= DateTime.Parse(date) && x.Create_date <= DateTime.Parse(date).AddDays(1).AddSeconds(-1)).ToList();
            }

            if (SearchString != null)
            {
                factures = _context.Facture.Where(x => x.Numero_de_facture.ToLower().Contains(SearchString.ToLower())).OrderByDescending(x => x.Create_date).ToList();
            }
            else
            {
                factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution")).OrderByDescending(x => x.Id).Take(300).ToListAsync();
            }
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();
            ViewBag.user = Userconnected();
            //ajustement
            // foreach (var item in _context.Facture.Where(x=>(x.Type=="medicament_0" && x.Etat_patient=="Cloturé" && x.Create_date>DateTime.Parse("16/12/2023")) ||(x.Type=="medicament_assurance" && x.Intervenant!="" && x.Intervenant!=null && x.Create_date>DateTime.Parse("16/12/2023"))).ToList())
            // {
            //     List<string> liste_de_facturation4 = new List<string>();
            //     List<string> quantites = new List<string>();

            //     int i=0;
            //     int compteur=0;
            //     int compteur2=0;
            //     string chaine="";
            //     foreach(char c in item.Ligne_facturation4) {

            //     if(c.Equals('λ')){
            //         if(i!=0){
            //             if (compteur%7==0 && chaine!="")
            //             {
            //                 liste_de_facturation4.Add(chaine);
            //                 compteur2=compteur;
            //             }
            //             if (compteur==compteur2+5)
            //             {
            //                 quantites.Add(chaine);
            //             }
            //             compteur+=1;
            //             chaine="";

            //         }

            //     }else{
            //         chaine+=c.ToString();
            //     }
            //     i++;

            //     }

            //     int j=0;
            // foreach (var item2 in liste_de_facturation4)
            // {


            //     var monstck= _context_stock.Stock.Where(x=>x.Nom_article==item2 && x.Lieu_stockage=="PHARMACIE").FirstOrDefault();
            //     if (monstck!=null)
            //     {
            //         if (monstck.Quantitee==null)
            //         {
            //             monstck.Quantitee=0;
            //         }
            //             monstck.Quantitee-=Decimal.Parse(quantites[j]);

            //         _context_stock.Update(monstck);
            //         await _context_stock.SaveChangesAsync();
            //     }

            //     j++;
            // }
            // }
            return View(factures.Take(1000));
        }
        
        [Authorize]
        public async Task<IActionResult> SearchWord(string SearchString, bool notUsed)
        {
            ViewData["14"] = "active";
            if (String.IsNullOrEmpty(SearchString))
            {
                return RedirectToAction("Page404", "Stock");
            }

            ViewBag.user = Userconnected();
            var factures = await _context.Facture.Where(x => x.Numero_de_facture.ToLower().Contains(SearchString.ToLower())).OrderByDescending(x => x.Create_date).ToListAsync();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();
            return View(factures.Take(10000));

        }

        [Authorize]
        public async Task<IActionResult> Index0()
        {
            List<string> datas = new List<string>();
            ViewData["14"] = "active";
            var factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution")).OrderByDescending(x => x.Id).ToListAsync();

            datas.AddRange(factures.Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct().Reverse();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();

            ViewBag.user = Userconnected();
            return View(factures);
        }
        
        [Authorize]
        public async Task<IActionResult> Indexdentiste0()
        {
            List<string> datas = new List<string>();
            ViewData["7"] = "active";
            var factures = await _context.Facture.Where(x => x.Type.Contains("dentiste_0")).OrderByDescending(x => x.Id).ToListAsync();
            datas.AddRange(_context.Facture.Where(x => x.Type.Contains("dentiste_0")).Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct().Reverse();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();

            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
         public async Task<IActionResult> Indexdentiste02()
        {
            List<string> datas = new List<string>();
            ViewData["7"] = "active";
            var factures = await _context.Facture.Where(x => x.Type == "dentiste_assurance").OrderByDescending(x => x.Id).ToListAsync();
            datas.AddRange(_context.Facture.Where(x => x.Type == "dentiste_assurance").Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct().Reverse();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();

            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> Indexdentiste03()
        {
            List<string> datas = new List<string>();
            ViewData["7"] = "active";
            var factures = await _context.Facture.Where(x => x.Type.Contains("proforma_dentiste")).OrderByDescending(x => x.Id).ToListAsync();
            datas.AddRange(_context.Facture.Where(x => x.Type.Contains("proforma_dentiste")).Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct().Reverse();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();

            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> comptafact01()
        {
            ViewData["22"] = "active";
            var factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution")).OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();

            var moisAnneesTries = factures
            .Select(d => new { d.Create_date.Month, d.Create_date.Year })
            .Distinct()
            .OrderByDescending(ma => ma.Year)
            .ThenBy(ma => ma.Month)
            .ToList();

            List<Stat> datas = new();
            foreach (var ma in moisAnneesTries)
            {
                string nomMois = CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat.GetMonthName(ma.Month);
                Console.WriteLine($"{nomMois} {ma.Year}");
                Stat nouveau = new();
                nouveau.mois = ma.Month;
                nouveau.annee = ma.Year;
                nouveau.date_en_lettre = nomMois + " " + ma.Year;
                datas.Add(nouveau);

            }

            ViewBag.dates = datas;

            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> RecapFacture(DateTime datedebut, DateTime datefin, List<string> assurances, string tipe)
        {
            ViewData["14"] = "active";
            ViewData["multi"] = "ok";
            ViewBag.user = Userconnected();

            if (datedebut.Year == 0001 || datefin.Year == 0001)
            {
                datedebut = DateTime.Now;
                datefin = DateTime.Now.AddDays(1);
            }

            int mois = datedebut.Month;
            int jour = datedebut.Day;
            int heure = datedebut.Hour;
            int minute = datedebut.Minute;
            int Seconde = datedebut.Second;


            string mois_str = "";
            string jour_str = "";
            string heure_str = "";
            string minute_str = "";
            string seconde_str = "";
            if (mois < 10) { mois_str = "0" + mois; } else { mois_str += mois; }
            if (jour < 10) { jour_str = "0" + jour; } else { jour_str += jour; }
            if (heure < 10) { heure_str = "0" + heure; } else { heure_str += heure; }
            if (minute < 10) { minute_str = "0" + minute; } else { minute_str += minute; }
            if (Seconde < 10) { seconde_str = "0" + Seconde; } else { seconde_str += Seconde; }

            ViewData["debut"] = datedebut.Year + "-" + mois_str + "-" + jour_str + "T" + heure_str + ":" + minute_str + ":" + seconde_str;

            mois = datefin.Month;
            jour = datefin.Day;
            heure = datefin.Hour;
            minute = datefin.Minute;
            Seconde = datefin.Second;

            mois_str = "";
            jour_str = "";
            heure_str = "";
            minute_str = "";
            seconde_str = "";
            if (mois < 10) { mois_str = "0" + mois; } else { mois_str += mois; }
            if (jour < 10) { jour_str = "0" + jour; } else { jour_str += jour; }
            if (heure < 10) { heure_str = "0" + heure; } else { heure_str += heure; }
            if (minute < 10) { minute_str = "0" + minute; } else { minute_str += minute; }
            if (Seconde < 10) { seconde_str = "0" + Seconde; } else { seconde_str += Seconde; }

            ViewData["fin"] = datefin.Year + "-" + mois_str + "-" + jour_str + "T" + heure_str + ":" + minute_str + ":" + seconde_str;

            assurances.Sort();
            ViewBag.select = assurances;


            var mesfactures = await _context.Facture.Where(x => (x.Type == "hospi_assurance" || x.Type == "consultation_assurance" || x.Type == "generique_assurance" || x.Type == "actes_assurance" || x.Type == "examens_assurance") && x.Encaisse_par != null && x.Etat_patient == "Cloturé" && x.Derniere_modification >= datedebut && x.Derniere_modification <= datefin).OrderBy(x => x.Assureur).ThenBy(x => x.Derniere_modification).ToListAsync();
            if (tipe != null)
            {
                mesfactures = mesfactures.Where(x => x.Type.Contains(tipe)).ToList();
            }

            List<Facture> facturesfinales = new();
            if (assurances == null)
            {
                facturesfinales.AddRange(mesfactures);
            }
            else
            {
                foreach (var item in assurances)
                {
                    facturesfinales.AddRange(mesfactures.Where(x => x.Assureur.Replace(" ", "") == item.Replace(" ", "")));
                }
            }

            List<string> assur = new List<string>();
            assur.AddRange(mesfactures.OrderBy(x => x.Assureur).Select(x => x.Assureur));
            ViewBag.assurance = assur.Distinct();

            ViewBag.date1 = datedebut;
            ViewBag.date2 = datefin;
            foreach (var item in facturesfinales)
            {
                //item.Nom= Truncate(item.Nom,12);
                //item.Assureur=Truncate(item.Assureur,12);
                //item.assure_prin=Truncate(item.assure_prin,12);
                //item.Societe=Truncate(item.Societe,12);
            }

            return View(facturesfinales);
        }


        [Authorize]
        public async Task<IActionResult> RecapFacture2(DateTime datedebut, DateTime datefin, List<string> assurances, string tipe)
        {
            ViewData["14"] = "active";
            ViewData["multi"] = "ok";
            ViewBag.user = Userconnected();

            if (datedebut.Year == 0001 || datefin.Year == 0001)
            {
                datedebut = DateTime.Now;
                datefin = DateTime.Now.AddDays(1);
            }

            int mois = datedebut.Month;
            int jour = datedebut.Day;
            int heure = datedebut.Hour;
            int minute = datedebut.Minute;
            int Seconde = datedebut.Second;


            string mois_str = "";
            string jour_str = "";
            string heure_str = "";
            string minute_str = "";
            string seconde_str = "";
            if (mois < 10) { mois_str = "0" + mois; } else { mois_str += mois; }
            if (jour < 10) { jour_str = "0" + jour; } else { jour_str += jour; }
            if (heure < 10) { heure_str = "0" + heure; } else { heure_str += heure; }
            if (minute < 10) { minute_str = "0" + minute; } else { minute_str += minute; }
            if (Seconde < 10) { seconde_str = "0" + Seconde; } else { seconde_str += Seconde; }

            ViewData["debut"] = datedebut.Year + "-" + mois_str + "-" + jour_str + "T" + heure_str + ":" + minute_str + ":" + seconde_str;

            mois = datefin.Month;
            jour = datefin.Day;
            heure = datefin.Hour;
            minute = datefin.Minute;
            Seconde = datefin.Second;

            mois_str = "";
            jour_str = "";
            heure_str = "";
            minute_str = "";
            seconde_str = "";
            if (mois < 10) { mois_str = "0" + mois; } else { mois_str += mois; }
            if (jour < 10) { jour_str = "0" + jour; } else { jour_str += jour; }
            if (heure < 10) { heure_str = "0" + heure; } else { heure_str += heure; }
            if (minute < 10) { minute_str = "0" + minute; } else { minute_str += minute; }
            if (Seconde < 10) { seconde_str = "0" + Seconde; } else { seconde_str += Seconde; }

            ViewData["fin"] = datefin.Year + "-" + mois_str + "-" + jour_str + "T" + heure_str + ":" + minute_str + ":" + seconde_str;

            assurances.Sort();
            ViewBag.select = assurances;


            var mesfactures = await _context.Facture.Where(x => (x.Type == "hospi_assurance" || x.Type == "consultation_assurance" || x.Type == "generique_assurance" || x.Type == "actes_assurance" || x.Type == "examens_assurance") && x.Encaisse_par != null && x.Etat_patient == "Cloturé" && x.Derniere_modification >= datedebut && x.Derniere_modification <= datefin).OrderBy(x => x.Assureur).ThenBy(x => x.Derniere_modification).ToListAsync();
            if (tipe != null)
            {
                mesfactures = mesfactures.Where(x => x.Type.Contains(tipe)).ToList();
            }

            List<Facture> facturesfinales = new();
            if (assurances == null)
            {
                facturesfinales.AddRange(mesfactures);
            }
            else
            {
                foreach (var item in assurances)
                {
                    facturesfinales.AddRange(mesfactures.Where(x => x.Assureur.Replace(" ", "") == item.Replace(" ", "")));
                }
            }

            List<string> assur = new List<string>();
            assur.AddRange(mesfactures.OrderBy(x => x.Assureur).Select(x => x.Assureur));
            ViewBag.assurance = assur.Distinct();

            ViewBag.date1 = datedebut;
            ViewBag.date2 = datefin;


            return View(facturesfinales);
        }



        [Authorize]
        public async Task<IActionResult> CumulFacture(DateTime datedebut, DateTime datefin, List<string> assurances, string tipe)
        {
            ViewData["14"] = "active";
            ViewData["multi"] = "ok";
            ViewBag.user = Userconnected();

            if (datedebut.Year == 0001 || datefin.Year == 0001)
            {
                datedebut = DateTime.Now;
                datefin = DateTime.Now.AddDays(1);
            }

            int mois = datedebut.Month;
            int jour = datedebut.Day;
            int heure = datedebut.Hour;
            int minute = datedebut.Minute;
            int Seconde = datedebut.Second;


            string mois_str = "";
            string jour_str = "";
            string heure_str = "";
            string minute_str = "";
            string seconde_str = "";
            if (mois < 10) { mois_str = "0" + mois; } else { mois_str += mois; }
            if (jour < 10) { jour_str = "0" + jour; } else { jour_str += jour; }
            if (heure < 10) { heure_str = "0" + heure; } else { heure_str += heure; }
            if (minute < 10) { minute_str = "0" + minute; } else { minute_str += minute; }
            if (Seconde < 10) { seconde_str = "0" + Seconde; } else { seconde_str += Seconde; }

            ViewData["debut"] = datedebut.Year + "-" + mois_str + "-" + jour_str + "T" + heure_str + ":" + minute_str + ":" + seconde_str;

            mois = datefin.Month;
            jour = datefin.Day;
            heure = datefin.Hour;
            minute = datefin.Minute;
            Seconde = datefin.Second;

            mois_str = "";
            jour_str = "";
            heure_str = "";
            minute_str = "";
            seconde_str = "";
            if (mois < 10) { mois_str = "0" + mois; } else { mois_str += mois; }
            if (jour < 10) { jour_str = "0" + jour; } else { jour_str += jour; }
            if (heure < 10) { heure_str = "0" + heure; } else { heure_str += heure; }
            if (minute < 10) { minute_str = "0" + minute; } else { minute_str += minute; }
            if (Seconde < 10) { seconde_str = "0" + Seconde; } else { seconde_str += Seconde; }

            ViewData["fin"] = datefin.Year + "-" + mois_str + "-" + jour_str + "T" + heure_str + ":" + minute_str + ":" + seconde_str;

            assurances.Sort();
            ViewBag.select = assurances;


            var mesfactures = await _context.Facture.Where(x => (x.Type == "hospi_assurance" || x.Type == "consultation_assurance" || x.Type == "generique_assurance" || x.Type == "actes_assurance" || x.Type == "examens_assurance") && x.Encaisse_par != null && x.Etat_patient == "Cloturé" && x.Derniere_modification >= datedebut && x.Derniere_modification <= datefin).OrderBy(x => x.Assureur).ThenBy(x => x.Derniere_modification).ToListAsync();
            if (tipe != null)
            {
                mesfactures = mesfactures.Where(x => x.Type.Contains(tipe)).ToList();
            }

            List<Facture> facturesfinales = new();
            if (assurances == null)
            {
                facturesfinales.AddRange(mesfactures);
            }
            else
            {
                foreach (var item in assurances)
                {
                    facturesfinales.AddRange(mesfactures.Where(x => x.Assureur.Replace(" ", "") == item.Replace(" ", "")));
                }
            }

            List<string> assur = new List<string>();
            assur.AddRange(mesfactures.OrderBy(x => x.Assureur).Select(x => x.Assureur));
            ViewBag.assurance = assur.Distinct();

            ViewBag.date1 = datedebut;
            ViewBag.date2 = datefin;
            foreach (var item in facturesfinales)
            {
                item.Nom = Truncate(item.Nom, 12);
                item.Assureur = Truncate(item.Assureur, 12);
                item.assure_prin = Truncate(item.assure_prin, 12);
                item.Societe = Truncate(item.Societe, 12);
            }
            ViewData["js_cumul"] = "ok";

            ViewBag.fact = facturesfinales.FirstOrDefault();
            return View(facturesfinales);
        }
        
        [Authorize]
        public async Task<IActionResult> comptafact02()
        {
            ViewData["22"] = "active";
            var factures = await _context.Facture.Where(x => x.Type.Contains("assurance") && !x.Type.Contains("proforma") && !x.Type.Contains("caution") && x.Etat_patient == "").OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();

            // Create a list of string.
            List<string> dates = new List<string>();
            List<string> dates1 = new List<string>();
            List<Stat> datesfinal = new List<Stat>();


            foreach (var item in _context.Facture.Where(x => x.Type.Contains("assurance") && !x.Type.Contains("proforma") && !x.Type.Contains("caution")).Select(x => x.Create_date))
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
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> comptadetails01()
        {
            List<string> datas = new List<string>();
            ViewData["22"] = "active";
            var factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution") && x.Etat_patient == "Cloturé").OrderByDescending(x => x.Id).ToListAsync();
            datas.AddRange(_context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution")).Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct().Reverse();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();



            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> comptacaution0()
        {
            ViewData["22"] = "active";
            var factures = await _context.Facture.Where(x => x.Type.Contains("caution")).OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();

            // Create a list of string.
            List<string> dates = new List<string>();
            List<string> dates1 = new List<string>();
            List<Stat> datesfinal = new List<Stat>();


            foreach (var item in _context.Facture.Where(x => x.Type.Contains("caution")).Select(x => x.Create_date))
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
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> comptacaution(int mois, int annee)
        {
            ViewData["22"] = "active";
            ViewData["table"] = "ok";
            var factures = await _context.Facture.Where(x => x.Type.Contains("caution") && x.Create_date.Month == mois && x.Create_date.Year == annee).ToListAsync();
            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> comptafact1(int mois, int annee)
        {
            ViewData["22"] = "active";
            ViewData["table"] = "ok";
            var factures = await _context.Facture.Where(x => !x.Type.Contains("caution") && !x.Type.Contains("proforma") && x.Create_date.Month == mois && x.Create_date.Year == annee).ToListAsync();
            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> comptadetails1(string date)
        {
            ViewData["22"] = "active";
            ViewData["table"] = "ok";
            var factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution")).OrderByDescending(x => x.Id).ToListAsync();

            if (date != null)
            {
                factures = factures.Where(x => x.Create_date >= DateTime.Parse(date) && x.Create_date <= DateTime.Parse(date).AddDays(1).AddSeconds(-1)).ToList();
            }
            else
            {
                factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution")).OrderByDescending(x => x.Id).Take(300).ToListAsync();
            }
            List<ListObject1> obj = new();
            ViewBag.tableauCroise = obj;
            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> comptadetails2(string date)
        {
            ViewData["22"] = "active";
            ViewData["table"] = "ok";
            var factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution")).OrderByDescending(x => x.Id).ToListAsync();

            if (date != null)
            {
                factures = factures.Where(x => x.Create_date >= DateTime.Parse(date) && x.Create_date <= DateTime.Parse(date).AddDays(1).AddSeconds(-1)).ToList();
            }
            else
            {
                factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution")).OrderByDescending(x => x.Id).Take(300).ToListAsync();
            }
            List<ListObject1> obj = new();
            ViewBag.tableauCroise = obj;
            ViewBag.user = Userconnected();
            return View(factures);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> comptadetailsp([Bind("Create_date,Derniere_modification")] Facture facts)
        {
            ViewData["22"] = "active";
            ViewData["table"] = "ok";
            var factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution") && x.Etat_patient == "Cloturé" && (x.Net_a_payer_patient - x.Montant_recu_patient) == 0 && x.Encaisse_par != null).OrderByDescending(x => x.Id).ToListAsync();

            if (facts != null)
            {
                //Console.WriteLine("youpi1");
                factures = factures.Where(x => x.Create_date >= facts.Create_date && x.Create_date <= facts.Derniere_modification).ToList();
            }
            else
            {
                //Console.WriteLine("youpi2");
                //factures=factures.Take(300).ToList();
            }
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();
            ViewBag.user = Userconnected();
            ViewBag.factures = factures;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> comptadetailsp1([Bind("Create_date,Derniere_modification")] Facture facts)
        {
            ViewData["22"] = "active";
            ViewData["table"] = "ok";
            var factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution") && x.Etat_patient == "Cloturé" && (x.Net_a_payer_patient - x.Montant_recu_patient) == 0 && x.Encaisse_par != null).OrderByDescending(x => x.Id).ToListAsync();

            if (facts != null)
            {
                factures = factures.Where(x => x.Create_date >= facts.Create_date && x.Create_date <= facts.Derniere_modification).ToList();
            }

            List<string> liste_de_facturation1 = new();
            List<ListObject1> liste2 = new();

            foreach (var item in factures)
            {

                int i = 0;
                string result = "";
                foreach (char c in item.Ligne_facturation1)
                {

                    if (c.Equals('λ'))
                    {
                        if (i != 0)
                        {
                            liste_de_facturation1.Add(result);
                            result = "";
                        }

                    }
                    else
                    {
                        result += c.ToString();
                    }
                    i++;

                }

                i = 0;
                result = "";
                foreach (char c in item.Ligne_facturation2)
                {

                    if (c.Equals('λ'))
                    {
                        if (i != 0)
                        {
                            liste_de_facturation1.Add(result);
                            result = "";
                        }

                    }
                    else
                    {
                        result += c.ToString();
                    }
                    i++;

                }

                i = 0;
                result = "";
                foreach (char c in item.Ligne_facturation3)
                {

                    if (c.Equals('λ'))
                    {
                        if (i != 0)
                        {
                            liste_de_facturation1.Add(result);
                            result = "";
                        }

                    }
                    else
                    {
                        result += c.ToString();
                    }
                    i++;

                }

                i = 0;
                result = "";
                foreach (char c in item.Ligne_facturation4)
                {

                    if (c.Equals('λ'))
                    {
                        if (i != 0)
                        {
                            liste_de_facturation1.Add(result);
                            result = "";
                        }

                    }
                    else
                    {
                        result += c.ToString();
                    }
                    i++;
                }
            }


            for (int t = 0; t < liste_de_facturation1.Count - 1; t += 7)
            {
                int x1 = 0;
                int x2 = 0;

                if (liste_de_facturation1[t + 4].Replace(" ", "") != "") { int.TryParse(liste_de_facturation1[t + 4].Replace(" ", ""), out x1); }
                if (liste_de_facturation1[t + 5].Replace(" ", "") != "") { int.TryParse(liste_de_facturation1[t + 5].Replace(" ", ""), out x2); }


                ListObject1 element = new();
                element.designation = liste_de_facturation1[t];
                element.montant = x1;
                element.quantite = x2;
                liste2.Add(element);
            }

            var tableauCroise = liste2
                .GroupBy(v => new { v.designation })
                .Select(g => new
                {
                    designation = g.Key.designation,
                    Quantite = g.Sum(v => v.quantite),
                    Total = g.Sum(v => v.montant)
                })
                .OrderByDescending(x => x.Total);

            foreach (var ligne in tableauCroise)
            {
                Console.WriteLine($"designation: {ligne.designation},Quantité: {ligne.Quantite}, Total: {ligne.Total}");
            }
            ViewBag.tableauCroise = tableauCroise;

            ViewBag.user = Userconnected();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> comptadetailsp2([Bind("Create_date,Derniere_modification")] Facture facts)
        {
            ViewData["22"] = "active";
            ViewData["table"] = "ok";
            var factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution") && x.Etat_patient == "Cloturé" && (x.Net_a_payer_patient - x.Montant_recu_patient) == 0 && x.Encaisse_par != null).OrderByDescending(x => x.Id).ToListAsync();

            if (facts != null)
            {
                factures = factures.Where(x => x.Create_date >= facts.Create_date && x.Create_date <= facts.Derniere_modification).ToList();
            }

            List<string> liste_de_facturation1 = new();
            List<string> liste_de_facturation2 = new();
            List<string> liste_de_facturation3 = new();
            List<string> liste_de_facturation4 = new();
            List<ListObject2> liste2 = new();

            foreach (var item in factures)
            {

                liste_de_facturation1 = new();
                liste_de_facturation2 = new();
                liste_de_facturation3 = new();
                liste_de_facturation4 = new();

                int i = 0;
                string result = "";
                foreach (char c in item.Ligne_facturation1)
                {

                    if (c.Equals('λ'))
                    {
                        if (i != 0)
                        {
                            liste_de_facturation1.Add(result);
                            result = "";
                        }

                    }
                    else
                    {
                        result += c.ToString();
                    }
                    i++;

                }



                for (int t = 0; t < liste_de_facturation1.Count - 1; t += 7)
                {
                    int x1 = 0;
                    int x2 = 0;

                    if (liste_de_facturation1[t + 4].Replace(" ", "") != "") { int.TryParse(liste_de_facturation1[t + 4].Replace(" ", ""), out x1); }
                    if (liste_de_facturation1[t + 5].Replace(" ", "") != "") { int.TryParse(liste_de_facturation1[t + 5].Replace(" ", ""), out x2); }


                    ListObject2 element = new();
                    element.Departement = "CONSULTATION";
                    element.designation = liste_de_facturation1[t];
                    element.montant = x1;
                    element.quantite = 1;
                    liste2.Add(element);

                }

                i = 0;
                result = "";
                foreach (char c in item.Ligne_facturation2)
                {

                    if (c.Equals('λ'))
                    {
                        if (i != 0)
                        {
                            liste_de_facturation2.Add(result);
                            result = "";
                        }

                    }
                    else
                    {
                        result += c.ToString();
                    }
                    i++;

                }

                for (int t = 0; t < liste_de_facturation2.Count - 1; t += 7)
                {
                    int x1 = 0;
                    int x2 = 0;

                    if (liste_de_facturation2[t + 4].Replace(" ", "") != "") { int.TryParse(liste_de_facturation2[t + 4].Replace(" ", ""), out x1); }
                    if (liste_de_facturation2[t + 5].Replace(" ", "") != "") { int.TryParse(liste_de_facturation2[t + 5].Replace(" ", ""), out x2); }


                    ListObject2 element = new();
                    element.Departement = "NON CLASSÉS";

                    element.designation = liste_de_facturation2[t];
                    element.montant = x1;
                    element.quantite = 1;
                    if (_context_examen.Examen.Where(x => x.Nom == liste_de_facturation2[t]).FirstOrDefault() != null) { element.Departement = _context_examen.Examen.Where(x => x.Nom == liste_de_facturation2[t]).FirstOrDefault().ExamType; }
                    liste2.Add(element);

                }

                i = 0;
                result = "";
                foreach (char c in item.Ligne_facturation3)
                {

                    if (c.Equals('λ'))
                    {
                        if (i != 0)
                        {
                            liste_de_facturation3.Add(result);
                            result = "";
                        }

                    }
                    else
                    {
                        result += c.ToString();
                    }
                    i++;

                }

                for (int t = 0; t < liste_de_facturation3.Count - 1; t += 7)
                {
                    int x1 = 0;
                    int x2 = 0;

                    if (liste_de_facturation3[t + 4].Replace(" ", "") != "") { int.TryParse(liste_de_facturation3[t + 4].Replace(" ", ""), out x1); }
                    if (liste_de_facturation3[t + 5].Replace(" ", "") != "") { int.TryParse(liste_de_facturation3[t + 5].Replace(" ", ""), out x2); }


                    ListObject2 element = new();
                    element.Departement = "NON CLASSÉS";
                    element.designation = liste_de_facturation3[t];
                    element.montant = x1;
                    element.quantite = x2;
                    if (_context_service.Service.Where(x => x.Nom == liste_de_facturation3[t]).FirstOrDefault() != null) { element.Departement = _context_service.Service.Where(x => x.Nom == liste_de_facturation3[t]).FirstOrDefault().Departement; }
                    liste2.Add(element);

                }

                i = 0;
                result = "";
                foreach (char c in item.Ligne_facturation4)
                {

                    if (c.Equals('λ'))
                    {
                        if (i != 0)
                        {
                            liste_de_facturation4.Add(result);
                            result = "";
                        }

                    }
                    else
                    {
                        result += c.ToString();
                    }
                    i++;
                }

                for (int t = 0; t < liste_de_facturation4.Count - 1; t += 7)
                {
                    int x1 = 0;
                    int x2 = 0;

                    if (liste_de_facturation4[t + 4].Replace(" ", "") != "") { int.TryParse(liste_de_facturation4[t + 4].Replace(" ", ""), out x1); }
                    if (liste_de_facturation4[t + 5].Replace(" ", "") != "") { int.TryParse(liste_de_facturation4[t + 5].Replace(" ", ""), out x2); }


                    ListObject2 element = new();
                    element.Departement = "MÉDICAMENT";
                    element.designation = liste_de_facturation4[t];
                    element.montant = x1;
                    element.quantite = x2;
                    liste2.Add(element);

                }

            }




            var tableauCroise = liste2
                .GroupBy(v => new { v.Departement })
                .Select(g => new
                {
                    departement = g.Key.Departement,
                    Quantite = g.Sum(v => v.quantite),
                    Total = g.Sum(v => v.montant)
                })
                .OrderByDescending(x => x.Total);

            foreach (var ligne in tableauCroise)
            {

                Console.WriteLine($"designation: {ligne.departement},Quantité: {ligne.Quantite}, Total: {ligne.Total}");
            }
            ViewBag.tableauCroise = tableauCroise;

            ViewBag.user = Userconnected();
            return View();
        }

        [Authorize]
        public async Task<IActionResult> comptadetailsp()
        {
            ViewData["22"] = "active";
            ViewData["table"] = "ok";
            var factures = await _context.Facture.Take(0).ToListAsync();

            //factures=factures.Where(x=>x.Create_date>=DateTime.Parse(DateTime.Now.ToShortTimeString()) && x.Create_date<=DateTime.Now).ToList();

            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();
            ViewBag.user = Userconnected();
            ViewBag.factures = factures;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> comptadetailsp1()
        {
            ViewData["22"] = "active";
            ViewData["table"] = "ok";
            var factures = await _context.Facture.Take(0).ToListAsync();
            ViewBag.user = Userconnected();
            ViewBag.factures = factures;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> comptadetailsp2()
        {
            ViewData["22"] = "active";
            ViewData["table"] = "ok";
            var factures = await _context.Facture.Take(0).ToListAsync();
            ViewBag.user = Userconnected();
            ViewBag.factures = factures;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> comptafact2(int mois, int annee)
        {
            ViewData["22"] = "active";
            ViewData["table"] = "ok";
            var factures = await _context.Facture.Where(x => !x.Type.Contains("proforma") && x.Type.Contains("assurance") && x.Create_date.Month == mois && x.Create_date.Year == annee).ToListAsync();
            ViewBag.user = Userconnected();
            return View(factures);
        }




        [Authorize]
        public async Task<IActionResult> Index02(string Etat)
        {
            List<string> datas = new List<string>();
            ViewData["14"] = "active";
            var factures = await _context.Facture.Where(x => x.Type.Contains("assurance") && !x.Type.Contains("proforma")).OrderByDescending(x => x.Id).ToListAsync();
            datas.AddRange(_context.Facture.Where(x => x.Type.Contains("assurance") && !x.Type.Contains("proforma") && !x.Type.Contains("caution")).Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct().Reverse();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => x.Net_a_payer_assurance).Sum();
            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> Caution0(string Etat)
        {
            List<string> datas = new List<string>();
            ViewData["14"] = "active";
            var factures = await _context.Facture.Where(x => x.Type.Contains("caution")).OrderByDescending(x => x.Id).ToListAsync();
            datas.AddRange(_context.Facture.Where(x => x.Type.Contains("caution")).Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct().Reverse();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => x.Net_a_payer_assurance).Sum();
            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> Caisseprincipale()
        {


            ViewData["2"] = "active";
            var factures = await _context.Facture.Where(x => x.Type != "medicament_0" && x.Type != "medicament_assurance" && !x.Type.Contains("proforma") && x.Etat_patient != "Cloturé").OrderByDescending(x => x.Id).ToListAsync();

            ViewBag.encours = factures.Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();
            ViewBag.user = Userconnected();
            return View(factures.Take(200));
        }
        
        [Authorize]
        public async Task<IActionResult> Caissepharmacie()
        {

            ViewData["2"] = "active";
            ViewData["tb2"] = "active";
            var factures = await _context.Facture.OrderByDescending(x => x.Id).Where(x => (x.Type == "medicament_0" || x.Type == "medicament_assurance") && x.Etat_patient != "Cloturé").ToListAsync();

            ViewBag.encours = factures.Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();
            ViewBag.user = Userconnected();
            return View(factures.Take(300));
        }

        [Authorize]
        public async Task<IActionResult> Index2(string Etat, string date)
        {

            // foreach (var item in _context.Facture.Where(x=>x.Etat_patient=="En attente Paiement" && x.Numero_dossier!="113930" && x.Numero_dossier!="94944" && x.Numero_dossier!="93893" && x.Numero_dossier!="100378" && x.Numero_dossier!="113194" && x.Numero_dossier!="113860" && x.Numero_dossier!="113915" && x.Numero_dossier!="113911" && x.Numero_dossier!="113743" && x.Numero_dossier!="100011"&& x.Numero_dossier!="70451" && !x.Type.Contains("proforma")).OrderByDescending(x=>x.Id).ToList())
            // {
            //     item.Etat_patient="Cloturé";
            //     _context.Update(item);
            //     await _context.SaveChangesAsync();
            // }
            ViewData["14"] = "active";
            var factures = await _context.Facture.Where(x => x.Type.Contains("assurance") && !x.Type.Contains("proforma")).OrderByDescending(x => x.Id).ToListAsync();
            if (Etat != null)
            {
                factures = factures.Where(x => x.Etat_patient == Etat).ToList();
            }
            if (date != null)
            {
                factures = factures.Where(x => x.Create_date >= DateTime.Parse(date) && x.Create_date <= DateTime.Parse(date).AddDays(1).AddSeconds(-1)).ToList();
            }
            else
            {
                factures = await _context.Facture.Where(x => x.Type.Contains("assurance") && !x.Type.Contains("proforma")).OrderByDescending(x => x.Id).Take(300).ToListAsync();
            }
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => x.Net_a_payer_assurance).Sum();
            ViewBag.user = Userconnected();
            return View(factures);
        }
        
        [Authorize]
         public async Task<IActionResult> Indexdentiste(string Etat, string date)
        {

            ViewData["14"] = "active";
            var factures = await _context.Facture.Where(x => x.Type.Contains("dentiste_0")).OrderByDescending(x => x.Id).ToListAsync();
            if (Etat != null)
            {
                factures = factures.Where(x => x.Etat_patient == Etat).ToList();
            }
            if (date != null)
            {
                factures = factures.Where(x => x.Create_date >= DateTime.Parse(date) && x.Create_date <= DateTime.Parse(date).AddDays(1).AddSeconds(-1)).ToList();
            }
            else
            {
                factures = await _context.Facture.Where(x => x.Type.Contains("assurance") && !x.Type.Contains("proforma")).OrderByDescending(x => x.Id).Take(300).ToListAsync();
            }
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => x.Net_a_payer_assurance).Sum();
            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
         public async Task<IActionResult> Indexdentiste2(string Etat, string date)
        {

            ViewData["14"] = "active";
            var factures = await _context.Facture.Where(x => x.Type.Contains("dentiste_assurance")).OrderByDescending(x => x.Id).ToListAsync();
            if (Etat != null)
            {
                factures = factures.Where(x => x.Etat_patient == Etat).ToList();
            }
            if (date != null)
            {
                factures = factures.Where(x => x.Create_date >= DateTime.Parse(date) && x.Create_date <= DateTime.Parse(date).AddDays(1).AddSeconds(-1)).ToList();
            }
            else
            {
                factures = await _context.Facture.Where(x => x.Type.Contains("assurance") && !x.Type.Contains("proforma")).OrderByDescending(x => x.Id).Take(300).ToListAsync();
            }
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => x.Net_a_payer_assurance).Sum();
            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> Indexdentiste3(string Etat, string date)
        {

            ViewData["14"] = "active";
            var factures = await _context.Facture.Where(x => x.Type.Contains("proforma_dentiste")).OrderByDescending(x => x.Id).ToListAsync();
            if (Etat != null)
            {
                factures = factures.Where(x => x.Etat_patient == Etat).ToList();
            }
            if (date != null)
            {
                factures = factures.Where(x => x.Create_date >= DateTime.Parse(date) && x.Create_date <= DateTime.Parse(date).AddDays(1).AddSeconds(-1)).ToList();
            }
            else
            {
                factures = await _context.Facture.Where(x => x.Type.Contains("assurance") && !x.Type.Contains("proforma")).OrderByDescending(x => x.Id).Take(300).ToListAsync();
            }
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => x.Net_a_payer_assurance).Sum();
            ViewBag.user = Userconnected();
            return View(factures);
        }


        [Authorize]
        public async Task<IActionResult> Index3(string Etat)
        {

            ViewData["ph0"] = "active";
            ViewData["ph1"] = "active";
            var factures = await _context.Facture.OrderByDescending(x => x.Id).Where(x => x.Type == "medicament_0" || x.Type == "medicament_assurance").ToListAsync();
            if (Etat != null)
            {
                factures = factures.Where(x => x.Etat_patient == Etat).ToList();
            }
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();
            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> Index4(string Etat)
        {

            ViewData["ph0"] = "active";
            ViewData["ph2"] = "active";
            var factures = await _context.Facture.OrderByDescending(x => x.Id).Where(x => x.Type == "medicament_assurance").ToListAsync();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => x.Net_a_payer_assurance).Sum();
            ViewBag.user = Userconnected();
            return View(factures);
        }

        [Authorize]
        public async Task<IActionResult> Index05(string Etat)
        {
            List<string> datas = new List<string>();
            ViewData["14"] = "active";
            var factures = await _context.Facture.OrderByDescending(x => x.Id).Where(x => x.Type.Contains("proforma")).ToListAsync();
            datas.AddRange(_context.Facture.Where(x => x.Type.Contains("proforma")).Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct().Reverse();
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => x.Net_a_payer_assurance).Sum();
            ViewBag.user = Userconnected();
            return View(factures);

        }

        [Authorize]
        public async Task<IActionResult> Index5(string Etat, string date, string target)
        {

            ViewData["14"] = "active";

            var factures = await _context.Facture.OrderByDescending(x => x.Id).Where(x => x.Type.Contains("proforma")).ToListAsync();
            // var factregul=await _context.Facture.OrderByDescending(x=>x.Id).Where(x=>!x.Type.Contains("proforma") && x.Etat_patient=="En attente Paiement").ToListAsync();
            // foreach (var item in factregul)
            // {
            //     if (_context.Facture.Where(x=>!x.Type.Contains("proforma") && x.Etat_patient=="En attente Paiement" &&(x.Type=="hospi_assurance" || x.Type=="generique_assurance")  && x.Id>=item.Id && x.Numero_dossier==item.Numero_dossier ).Count()>0)
            //     {
            //         item.Etat_patient="Cloturé";
            //         _context.Update(item);
            //         await _context.SaveChangesAsync();
            //     }
            // }
            if (Etat != null)
            {
                factures = factures.Where(x => x.Etat_patient == Etat).ToList();
            }
            if (date != null)
            {
                factures = factures.Where(x => x.Create_date >= DateTime.Parse(date) && x.Create_date <= DateTime.Parse(date).AddDays(1).AddSeconds(-1)).ToList();
            }
            if (target!=null)
            {
                factures = factures.Where(x => x.Numero_dossier==target).ToList();
            }

            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => x.Net_a_payer_assurance).Sum();
            ViewBag.user = Userconnected();
            return View(factures);
        }
        [Authorize]
        public async Task<IActionResult> Caution(string date, string target)
        {

            ViewData["14"] = "active";
            var factures = await _context.Facture.OrderByDescending(x => x.Id).Where(x => x.Type.Contains("caution")).ToListAsync();
            if (target!=null)
            {
                factures = factures.Where(x => x.Numero_dossier==target).ToList();
            }
            if (date != null)
            {
                factures = factures.Where(x => x.Create_date >= DateTime.Parse(date) && x.Create_date <= DateTime.Parse(date).AddDays(1).AddSeconds(-1)).ToList();
            }
            else
            {
                factures = factures.Take(300).ToList();
            }
            ViewBag.fini = factures.Where(x => x.Etat_patient == "Cloturé").Count();
            ViewBag.encours = factures.Where(x => x.Etat_patient == "En attente Paiement").Count();
            ViewBag.montanttotal = factures.Select(x => (long)x.Net_a_payer_patient).Sum();
            ViewBag.user = Userconnected();
            return View(factures);
        }




        // GET: Facture/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            var facture = await _context.Facture
                .FirstOrDefaultAsync(m => m.Id == id);

            if (facture == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in facture.Ligne_facturation1)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation1.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes1 = liste_de_facturation1;

            List<string> liste_de_facturation2 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation2)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation2.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes2 = liste_de_facturation2;

            List<string> liste_de_facturation3 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation3)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation3.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes3 = liste_de_facturation3;

            List<string> liste_de_facturation4 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation4)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation4.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes4 = liste_de_facturation4;



            ViewData["14"] = "active";
            ViewData["details"] = "ok";
            ViewData["qrcode"] = "ok";

            ViewBag.user = Userconnected();

            return View(facture);
        }

        [Authorize]
        public async Task<IActionResult> Detailsproforma1(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            var facture = await _context.Facture
                .FirstOrDefaultAsync(m => m.Id == id);

            if (facture == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in facture.Ligne_facturation1)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation1.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes1 = liste_de_facturation1;

            List<string> liste_de_facturation2 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation2)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation2.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes2 = liste_de_facturation2;

            List<string> liste_de_facturation3 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation3)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation3.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes3 = liste_de_facturation3;

            List<string> liste_de_facturation4 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation4)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation4.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes4 = liste_de_facturation4;



            ViewData["14"] = "active";
            ViewData["details"] = "ok";
            ViewData["qrcode"] = "ok";

            ViewBag.user = Userconnected();

            return View(facture);
        }

        // GET: Facture/Details/5
        [Authorize]
        public async Task<IActionResult> Matricielle(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            var facture = await _context.Facture
                .FirstOrDefaultAsync(m => m.Id == id);

            if (facture == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in facture.Ligne_facturation1)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation1.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes1 = liste_de_facturation1;

            List<string> liste_de_facturation2 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation2)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation2.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes2 = liste_de_facturation2;

            List<string> liste_de_facturation3 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation3)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation3.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes3 = liste_de_facturation3;

            List<string> liste_de_facturation4 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation4)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation4.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes4 = liste_de_facturation4;
            ViewBag.caution = _context.Facture.Where(x => x.Numero_dossier == facture.Numero_dossier && x.Type == "caution" && x.Etat_patient == "Cloturé" && (x.Net_a_payer_patient - x.Nombre_impression > 0)).ToList();


            ViewData["14"] = "active";
            ViewData["details"] = "ok";
            ViewData["qrcode"] = "ok";

            ViewBag.user = Userconnected();

            return View(facture);
        }

        [Authorize]
        public async Task<IActionResult> Details4(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            var facture = await _context.Facture
                .FirstOrDefaultAsync(m => m.Id == id);

            if (facture == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in facture.Ligne_facturation1)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation1.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes1 = liste_de_facturation1;

            List<string> liste_de_facturation2 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation2)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation2.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes2 = liste_de_facturation2;

            List<string> liste_de_facturation3 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation3)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation3.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes3 = liste_de_facturation3;

            List<string> liste_de_facturation4 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation4)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation4.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes4 = liste_de_facturation4;



            ViewData["14"] = "active";
            ViewData["details"] = "ok";
            ViewData["qrcode"] = "ok";

            ViewBag.user = Userconnected();

            return View(facture);
        }


        [Authorize]
        public async Task<IActionResult> Detailsproforma2(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            var facture = await _context.Facture
                .FirstOrDefaultAsync(m => m.Id == id);

            if (facture == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in facture.Ligne_facturation1)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation1.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes1 = liste_de_facturation1;

            List<string> liste_de_facturation2 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation2)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation2.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes2 = liste_de_facturation2;

            List<string> liste_de_facturation3 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation3)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation3.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes3 = liste_de_facturation3;

            List<string> liste_de_facturation4 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation4)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation4.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes4 = liste_de_facturation4;



            ViewData["14"] = "active";
            ViewData["details"] = "ok";
            ViewData["qrcode"] = "ok";

            ViewBag.user = Userconnected();

            return View(facture);
        }



        [Authorize]
        public async Task<IActionResult> Details3(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            var facture = await _context.Facture
                .FirstOrDefaultAsync(m => m.Id == id);

            if (facture == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in facture.Ligne_facturation1)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation1.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes1 = liste_de_facturation1;

            List<string> liste_de_facturation2 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation2)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation2.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes2 = liste_de_facturation2;

            List<string> liste_de_facturation3 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation3)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation3.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes3 = liste_de_facturation3;

            List<string> liste_de_facturation4 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation4)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation4.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes4 = liste_de_facturation4;
            ViewBag.caution = _context.Facture.Where(x => x.Numero_dossier == facture.Numero_dossier && x.Type == "caution" && x.Etat_patient == "Cloturé" && (x.Net_a_payer_patient - x.Nombre_impression > 0)).ToList();


            ViewData["14"] = "active";
            ViewData["details"] = "ok";
            ViewData["qrcode"] = "ok";

            ViewBag.user = Userconnected();

            return View(facture);
        }

        // GET: Facture/Create
        [Authorize]
        public IActionResult Create(string nd, string target)
        {
            ViewData["js_fact"] = "ok";
            ViewBag.examen = _context_examen.Examen.ToList();
            ViewBag.actes = _context_service.Service.Where(x => x.Categorie == "Actes").ToList();
            ViewBag.consultation = _context_service.Service.Where(x => x.Categorie == "Consultation").ToList();
            ViewBag.Pharmacie = _context_stock.Stock.Where(x => x.Lieu_stockage == "PHARMACIE").ToList();
            ViewBag.doctors = _context_doctor.Doctor.ToList();
            ViewBag.assurances = _context_assurance.Assurance.Where(x => x.Actif == true).ToList();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == nd).FirstOrDefault();

            ViewData["14"] = "active";
            ViewData["target"] = target;
            ViewBag.user = Userconnected();
            return View();
        }

        // GET: Facture/Create
        [Authorize]
        public IActionResult Create3(string nd, string target)
        {
            ViewData["js_fact2"] = "ok";
            ViewBag.examen = _context_examen.Examen.ToList();
            ViewBag.actes = _context_service.Service.Where(x => x.Categorie == "Actes").ToList();
            ViewBag.consultation = _context_service.Service.Where(x => x.Categorie == "Consultation").ToList();
            ViewBag.Pharmacie = _context_stock.Stock.Where(x => x.Lieu_stockage == "PHARMACIE").ToList();
            ViewBag.doctors = _context_doctor.Doctor.ToList();
            ViewBag.assurances = _context_assurance.Assurance.Where(x => x.Actif == true).ToList();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == nd).FirstOrDefault();

            ViewData["14"] = "active";
            ViewData["target"] = target;
            ViewBag.user = Userconnected();
            return View();
        }

        [Authorize]
         public async Task<IActionResult> Dmi0()
        {
            // Create a list of string.
            List<string> dates = new List<string>();
            List<string> dates1 = new List<string>();
            List<Stat> datesfinal = new List<Stat>();


            foreach (var item in _context.Facture.Select(x => x.Create_date))
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
            return View(await _context.Facture.Take(100).ToListAsync());
        }

        // GET: Facture/Create
        [Authorize]
        public IActionResult Create2(string nd, string target)
        {
            ViewData["js_fact2"] = "ok";
            ViewBag.examen = _context_examen.Examen.ToList();
            ViewBag.actes = _context_service.Service.Where(x => x.Categorie == "Actes").ToList();
            ViewBag.consultation = _context_service.Service.Where(x => x.Categorie == "Consultation").ToList();
            ViewBag.Pharmacie = _context_stock.Stock.Where(x => x.Lieu_stockage == "PHARMACIE").ToList();
            ViewBag.doctors = _context_doctor.Doctor.ToList();
            ViewBag.assurances = _context_assurance.Assurance.Where(x => x.Actif == true).ToList();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == nd).FirstOrDefault();

            ViewData["14"] = "active";
            ViewData["target"] = target;
            ViewBag.user = Userconnected();
            ViewBag.dernierefacture = _context.Facture.Where(x => x.Numero_dossier == nd).OrderBy(x => x.Id).LastOrDefault();
            if (target == "hospi_assurance")
            {
                var factures = _context.Facture.Where(x => x.Numero_dossier == nd && x.Type == "medicament_assurance" && x.Etat_patient != "Cloturé" && x.Intervenant != "" && x.Intervenant != null).ToList();
                var examens = _context.Facture.Where(x => x.Numero_dossier == nd && x.Type == "examens_assurance" && x.Etat_patient != "Cloturé" && x.Intervenant != "" && x.Intervenant != null).ToList();
                if (factures != null)
                {
                    string l4 = "λ";
                    foreach (var item in factures)
                    {

                        l4 += item.Ligne_facturation4.ToString().Remove(0, 1);
                        List<string> liste_de_facturation4 = new List<string>();

                        int i = 0;
                        string result = "";
                        foreach (char c in l4)
                        {

                            if (c.Equals('λ'))
                            {
                                if (i != 0)
                                {
                                    liste_de_facturation4.Add(result);
                                    result = "";
                                }

                            }
                            else
                            {
                                result += c.ToString();
                            }
                            i++;
                            //Console.WriteLine(i+"-"+result);
                        }
                        ViewBag.Lignes4 = liste_de_facturation4;

                    }

                }

                if (examens != null)
                {
                    string l2 = "λ";
                    foreach (var item in examens)
                    {

                        l2 += item.Ligne_facturation2.ToString().Remove(0, 1);
                        List<string> liste_de_facturation2 = new List<string>();

                        int i = 0;
                        string result = "";
                        foreach (char c in l2)
                        {

                            if (c.Equals('λ'))
                            {
                                if (i != 0)
                                {
                                    liste_de_facturation2.Add(result);
                                    result = "";
                                }

                            }
                            else
                            {
                                result += c.ToString();
                            }
                            i++;
                            //Console.WriteLine(i+"-"+result);
                        }
                        ViewBag.Lignes2 = liste_de_facturation2;

                    }

                }


            }
            return View();
        }





        // GET: Facture/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var facture = await _context.Facture.FindAsync(id);
            if (facture == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            if (facture.Etat_patient == "Cloturé")
            {
                return RedirectToAction("Page404", "Stock");
            }

            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in facture.Ligne_facturation1)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation1.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes1 = liste_de_facturation1;

            List<string> liste_de_facturation2 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation2)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation2.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes2 = liste_de_facturation2;

            List<string> liste_de_facturation3 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation3)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation3.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes3 = liste_de_facturation3;

            List<string> liste_de_facturation4 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation4)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation4.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes4 = liste_de_facturation4;

            ViewData["js_fact"] = "ok";
            ViewBag.examen = _context_examen.Examen.ToList();
            ViewBag.actes = _context_service.Service.Where(x => x.Categorie == "Actes").ToList();
            ViewBag.consultation = _context_service.Service.Where(x => x.Categorie == "Consultation").ToList();
            ViewBag.Pharmacie = _context_stock.Stock.Where(x => x.Lieu_stockage == "PHARMACIE").ToList();
            ViewBag.doctors = _context_doctor.Doctor.ToList();
            ViewBag.assurances = _context_assurance.Assurance.Where(x => x.Actif == true).ToList();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == facture.Numero_dossier).FirstOrDefault();

            ViewData["14"] = "active";

            ViewBag.user = Userconnected();
            return View(facture);
        }

        // GET: Facture/Edit/5
        [Authorize]
        public async Task<IActionResult> Transform1(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var facture = await _context.Facture.FindAsync(id);
            if (facture == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in facture.Ligne_facturation1)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation1.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes1 = liste_de_facturation1;

            List<string> liste_de_facturation2 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation2)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation2.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes2 = liste_de_facturation2;

            List<string> liste_de_facturation3 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation3)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation3.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes3 = liste_de_facturation3;

            List<string> liste_de_facturation4 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation4)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation4.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes4 = liste_de_facturation4;

            ViewData["js_fact"] = "ok";
            ViewBag.examen = _context_examen.Examen.ToList();
            ViewBag.actes = _context_service.Service.Where(x => x.Categorie == "Actes").ToList();
            ViewBag.consultation = _context_service.Service.Where(x => x.Categorie == "Consultation").ToList();
            ViewBag.Pharmacie = _context_stock.Stock.Where(x => x.Lieu_stockage == "PHARMACIE").ToList();
            ViewBag.doctors = _context_doctor.Doctor.ToList();
            ViewBag.assurances = _context_assurance.Assurance.Where(x => x.Actif == true).ToList();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == facture.Numero_dossier).FirstOrDefault();

            ViewData["14"] = "active";

            ViewBag.user = Userconnected();
            return View(facture);
        }

        // GET: Facture/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit2(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            Console.WriteLine("1");

            var facture = await _context.Facture.FindAsync(id);
            if (facture == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            Console.WriteLine("2");
            if (facture.Etat_patient == "Cloturé")
            {
                return RedirectToAction("Page404", "Stock");
            }
            Console.WriteLine("3");

            if (facture.Type == "hospi_assurance")
            {
                var factures = _context.Facture.Where(x => x.Numero_dossier == facture.Numero_dossier && x.Type == "medicament_assurance" && x.Etat_patient != "Cloturé" && x.Intervenant != "" && x.Intervenant != null && x.Id > facture.Id).ToList();
                var examens = _context.Facture.Where(x => x.Numero_dossier == facture.Numero_dossier && x.Type == "examens_assurance" && x.Etat_patient != "Cloturé" && x.Intervenant != "" && x.Intervenant != null && x.Id > facture.Id).ToList();
                if (factures != null)
                {

                    foreach (var item in factures)
                    {
                        facture.Ligne_facturation4 += item.Ligne_facturation4[1..];
                    }
                }
                if (examens != null)
                {

                    foreach (var item in examens)
                    {
                        facture.Ligne_facturation2 += item.Ligne_facturation2[1..];
                    }
                }
            }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in facture.Ligne_facturation1)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation1.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes1 = liste_de_facturation1;

            List<string> liste_de_facturation2 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation2)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation2.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes2 = liste_de_facturation2;

            List<string> liste_de_facturation3 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation3)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation3.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes3 = liste_de_facturation3;

            List<string> liste_de_facturation4 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation4)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation4.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes4 = liste_de_facturation4;

            ViewData["js_fact2"] = "ok";
            ViewBag.examen = _context_examen.Examen.ToList();
            ViewBag.actes = _context_service.Service.Where(x => x.Categorie == "Actes").ToList();
            ViewBag.consultation = _context_service.Service.Where(x => x.Categorie == "Consultation").ToList();
            ViewBag.Pharmacie = _context_stock.Stock.Where(x => x.Lieu_stockage == "PHARMACIE").ToList();
            ViewBag.doctors = _context_doctor.Doctor.ToList();
            ViewBag.assurances = _context_assurance.Assurance.Where(x => x.Actif == true).ToList();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == facture.Numero_dossier).FirstOrDefault();

            ViewData["14"] = "active";
            ViewBag.user = Userconnected();
            return View(facture);
        }

        // GET: Facture/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit3(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var facture = await _context.Facture.FindAsync(id);
            if (facture == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in facture.Ligne_facturation1)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation1.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes1 = liste_de_facturation1;

            List<string> liste_de_facturation2 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation2)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation2.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes2 = liste_de_facturation2;

            List<string> liste_de_facturation3 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation3)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation3.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes3 = liste_de_facturation3;

            List<string> liste_de_facturation4 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation4)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation4.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes4 = liste_de_facturation4;

            ViewData["js_fact2"] = "ok";
            ViewBag.examen = _context_examen.Examen.ToList();
            ViewBag.actes = _context_service.Service.Where(x => x.Categorie == "Actes").ToList();
            ViewBag.consultation = _context_service.Service.Where(x => x.Categorie == "Consultation").ToList();
            ViewBag.Pharmacie = _context_stock.Stock.Where(x => x.Lieu_stockage == "PHARMACIE").ToList();
            ViewBag.doctors = _context_doctor.Doctor.ToList();
            ViewBag.assurances = _context_assurance.Assurance.Where(x => x.Actif == true).ToList();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == facture.Numero_dossier).FirstOrDefault();

            ViewData["14"] = "active";
            ViewBag.user = Userconnected();
            return View(facture);
        }



        // GET: Facture/Edit/5
        [Authorize]
        public async Task<IActionResult> Transform2(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var facture = await _context.Facture.FindAsync(id);
            if (facture == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in facture.Ligne_facturation1)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation1.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes1 = liste_de_facturation1;

            List<string> liste_de_facturation2 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation2)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation2.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes2 = liste_de_facturation2;

            List<string> liste_de_facturation3 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation3)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation3.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes3 = liste_de_facturation3;

            List<string> liste_de_facturation4 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in facture.Ligne_facturation4)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation4.Add(result);
                        result = "";
                    }

                }
                else
                {
                    result += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }
            ViewBag.Lignes4 = liste_de_facturation4;

            ViewData["js_fact2"] = "ok";
            ViewBag.examen = _context_examen.Examen.ToList();
            ViewBag.actes = _context_service.Service.Where(x => x.Categorie == "Actes").ToList();
            ViewBag.consultation = _context_service.Service.Where(x => x.Categorie == "Consultation").ToList();
            ViewBag.Pharmacie = _context_stock.Stock.Where(x => x.Lieu_stockage == "PHARMACIE").ToList();
            ViewBag.doctors = _context_doctor.Doctor.ToList();
            ViewBag.assurances = _context_assurance.Assurance.Where(x => x.Actif == true).ToList();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == facture.Numero_dossier).FirstOrDefault();

            ViewData["14"] = "active";
            ViewBag.user = Userconnected();
            return View(facture);
        }

        // POST: Facture/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Prenom,Phone,Email,Taille,Medecin,Create_date,Date_validite,Derniere_modification,Derniere_impression,DateNaissance,Genre,Profession,Quartier,GroupeSanguin,Intervenant,Ligne_facturation,Numero_de_facture,Moyen_paiement,Total_ht,Tva,Total_ttc,Net_a_payer,Montant_recu,Nombre_affichage,Nombre_impression,Montant_en_lettre,Notes,Etat,Type,Facture_par,Encaisse_par")] Facture facture)
        {
            if (id != facture.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactureExists(facture.Id))
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
            ViewBag.user = Userconnected();
            return View(facture);
        }

        // GET: Facture/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var facture = await _context.Facture
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facture == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user = Userconnected();

            return View(facture);
        }

        [HttpPost]
        [Authorize]
        public async Task<object> SaveFacture([FromBody] Facture MyData)
        {

            if (MyData.Id == 0)
            {
                var patient = await _context_patient.Patient.Where(x => x.Numero_dossier == MyData.Numero_dossier).FirstOrDefaultAsync();

                MyData.Nom = patient.Nom;
                MyData.Prenom = patient.Prenom;
                MyData.Phone = patient.Phone;
                MyData.Email = patient.Email;
                MyData.Taille = patient.Taille;
                MyData.Create_date = DateTime.Now;
                MyData.Date_validite = DateTime.Now;
                MyData.Derniere_modification = DateTime.Now;
                MyData.DateNaissance = patient.DateNaissance;
                MyData.Genre = patient.Genre;
                MyData.Profession = patient.Profession;
                MyData.Quartier = patient.Quartier;
                MyData.GroupeSanguin = patient.GroupeSanguin;
                MyData.Numero_de_facture = "00" + (int.Parse(GetlastFacture()) + 1);
                if (MyData.Type.Contains("proforma"))
                {
                    MyData.Numero_de_facture = "00" + (int.Parse(Getlastproforma()) + 1);
                }
                if (MyData.Type.Contains("caution"))
                {
                    MyData.Numero_de_facture = "00" + (int.Parse(Getlastcaution()) + 1);
                }
                MyData.Montant_recu_patient = 0;
                MyData.Especes = 0;
                MyData.Nombre_affichage = 0;
                MyData.Nombre_impression = 0;
                MyData.Etat_patient = "En attente Paiement";
                MyData.Etat_assurance = "En attente Paiement";
                MyData.Facture_par = Userconnected().nom;
                MyData.Encaisse_par = "";
                MyData.Montant_recu_assurance = 0;
                MyData.Intervenant = "";
                if (MyData.Net_a_payer_patient<0 || MyData.Net_a_payer_assurance<0)
                {
                    return Json("erreur: valeurs incorrectes");
                }

                _context.Add(MyData);
                await _context.SaveChangesAsync();
                return Json(_context.Facture.Where(x => x.Numero_de_facture == MyData.Numero_de_facture && x.Create_date.AddSeconds(10) > DateTime.Now).OrderBy(x => x.Id).LastOrDefault().Id.ToString());
            }
            else
            {

                var patient = await _context_patient.Patient.Where(x => x.Numero_dossier == MyData.Numero_dossier).FirstOrDefaultAsync();
                var facture = await _context.Facture.FirstOrDefaultAsync(m => m.Id == MyData.Id);

                facture.Assureur = MyData.Assureur;
                facture.Nom = patient.Nom;
                facture.Prenom = patient.Prenom;
                facture.Phone = patient.Phone;
                facture.Email = patient.Email;
                facture.Taille = patient.Taille;
                facture.Derniere_modification = DateTime.Now;
                facture.DateNaissance = patient.DateNaissance;
                facture.Genre = patient.Genre;
                facture.Profession = patient.Profession;
                facture.Quartier = patient.Quartier;
                facture.GroupeSanguin = patient.GroupeSanguin;
                facture.Montant_recu_patient = 0;
                facture.Especes = 0;
                facture.Intervenant = "";
                facture.Nombre_affichage += 1;
                facture.Nombre_impression = 0;
                facture.Etat_patient = "En attente Paiement";
                facture.Etat_assurance = "En attente Paiement";
                facture.Facture_par = Userconnected().nom;
                facture.Encaisse_par = "";
                facture.Montant_recu_assurance = 0;
                facture.Net_a_payer_assurance = MyData.Net_a_payer_assurance;
                facture.Net_a_payer_patient = MyData.Net_a_payer_patient;
                facture.Montant_en_lettre = MyData.Montant_en_lettre;
                facture.Montant_en_lettre_assurance = MyData.Montant_en_lettre_assurance;
                facture.Montant_en_lettre_patient = MyData.Montant_en_lettre_patient;
                facture.Medecin = MyData.Medecin;
                facture.Total_ht = MyData.Total_ht;
                facture.Total_ttc = MyData.Total_ht;
                facture.Ligne_facturation1 = MyData.Ligne_facturation1;
                facture.Ligne_facturation2 = MyData.Ligne_facturation2;
                facture.Ligne_facturation3 = MyData.Ligne_facturation3;
                facture.Ligne_facturation4 = MyData.Ligne_facturation4;
                facture.Titre1 = MyData.Titre1;
                facture.Titre2 = MyData.Titre2;
                facture.Titre3 = MyData.Titre3;
                facture.Titre4 = MyData.Titre4;
                facture.Matricule_patient = MyData.Matricule_patient;
                facture.MatriculeADH = MyData.MatriculeADH;
                facture.Societe = MyData.Societe;
                facture.assure_prin = MyData.assure_prin;
                facture.Date_entree = MyData.Date_entree;
                facture.Date_sortie = MyData.Date_sortie;
                facture.Taxe1 = MyData.Taxe1;
                facture.Remise = MyData.Remise;
                if (MyData.Net_a_payer_patient<0 || MyData.Net_a_payer_assurance<0)
                {
                    return Json("erreur: valeurs incorrectes");
                }

                _context.Update(facture);
                await _context.SaveChangesAsync();
                return Json(facture.Id.ToString());
            }



        }



        [HttpPost]
        [Authorize]
        public async Task<object> SaveFacture2([FromBody] Facture MyData)
        {

            if (MyData.Id == 0)
            {
                var patient = await _context_patient.Patient.Where(x => x.Numero_dossier == MyData.Numero_dossier).FirstOrDefaultAsync();
                if (MyData.Assureur != null && MyData.Assureur != "")
                {
                    var assureur = await _context_assurance.Assurance.Where(x => x.Nom == MyData.Assureur).FirstOrDefaultAsync();
                    if (assureur != null)
                    {
                        MyData.Assur_Adresse = assureur.Adresse;
                        MyData.Assur_BP = assureur.Bp;
                        MyData.Assur_Tel = assureur.Telephone;
                        MyData.Assur_NIU = assureur.Niu;
                        MyData.Assur_Rc = assureur.RC;
                    }

                }

                MyData.Nom = patient.Nom;
                MyData.Prenom = patient.Prenom;
                MyData.Phone = patient.Phone;
                MyData.Email = patient.Email;
                MyData.Taille = patient.Taille;
                MyData.Create_date = DateTime.Now;
                MyData.Date_validite = DateTime.Now;
                MyData.Derniere_modification = DateTime.Now;
                MyData.DateNaissance = patient.DateNaissance;
                MyData.Genre = patient.Genre;
                MyData.Profession = patient.Profession;
                MyData.Quartier = patient.Quartier;
                MyData.GroupeSanguin = patient.GroupeSanguin;
                MyData.Numero_de_facture = "00" + (int.Parse(GetlastFacture()) + 1);
                if (MyData.Type.Contains("proforma"))
                {
                    MyData.Numero_de_facture = "00" + (int.Parse(Getlastproforma()) + 1);
                }
                if (MyData.Type.Contains("caution"))
                {
                    MyData.Numero_de_facture = "00" + (int.Parse(Getlastcaution()) + 1);
                }
                MyData.Montant_recu_patient = 0;
                MyData.Especes = 0;
                MyData.Nombre_affichage = 0;
                MyData.Nombre_impression = 0;
                MyData.Etat_patient = "En attente Paiement";
                MyData.Etat_assurance = "En attente Paiement";
                MyData.Facture_par = Userconnected().nom;
                MyData.Encaisse_par = "";
                MyData.Montant_recu_assurance = 0;
                MyData.Etat_assurance = "En attente de paiement";
                MyData.Intervenant = "";
                if (MyData.Net_a_payer_patient<0 || MyData.Net_a_payer_assurance<0)
                {
                    return Json("erreur: valeurs incorrectes");
                }
                _context.Add(MyData);
                await _context.SaveChangesAsync();
                return Json(_context.Facture.Where(x => x.Numero_de_facture == MyData.Numero_de_facture && x.Create_date.AddSeconds(10) > DateTime.Now).OrderBy(x => x.Id).LastOrDefault().Id.ToString());
            }
            else
            {
                var facture = await _context.Facture.FirstOrDefaultAsync(m => m.Id == MyData.Id);

                if (facture.Montant_recu_patient > 0)
                {
                    return Json(facture.Id.ToString());
                }

                if (MyData.Assureur != null && MyData.Assureur != "")
                {
                    var assureur = await _context_assurance.Assurance.Where(x => x.Nom == MyData.Assureur).FirstOrDefaultAsync();
                    facture.Assureur = MyData.Assureur;
                    facture.Assur_Adresse = assureur.Adresse;
                    facture.Assur_BP = assureur.Bp;
                    facture.Assur_Tel = assureur.Telephone;
                    facture.Assur_NIU = assureur.Niu;
                    facture.Assur_Rc = assureur.RC;
                }
                var patient = await _context_patient.Patient.Where(x => x.Numero_dossier == MyData.Numero_dossier).FirstOrDefaultAsync();

                facture.Nom = patient.Nom;
                facture.Prenom = patient.Prenom;
                facture.Phone = patient.Phone;
                facture.Email = patient.Email;
                facture.Taille = patient.Taille;
                facture.Derniere_modification = DateTime.Now;
                facture.DateNaissance = patient.DateNaissance;
                facture.Genre = patient.Genre;
                facture.Profession = patient.Profession;
                facture.Quartier = patient.Quartier;
                facture.GroupeSanguin = patient.GroupeSanguin;
                facture.Montant_recu_patient = 0;
                facture.Especes = 0;
                facture.Nombre_affichage += 1;
                facture.Nombre_impression = 0;
                facture.Etat_patient = "En attente Paiement";
                facture.Etat_assurance = "En attente Paiement";
                facture.Facture_par = Userconnected().nom;
                facture.Encaisse_par = "";
                facture.Montant_recu_assurance = 0;
                facture.Net_a_payer_assurance = MyData.Net_a_payer_assurance;
                facture.Net_a_payer_patient = MyData.Net_a_payer_patient;
                facture.Montant_en_lettre = MyData.Montant_en_lettre;
                facture.Montant_en_lettre_assurance = MyData.Montant_en_lettre_assurance;
                facture.Montant_en_lettre_patient = MyData.Montant_en_lettre_patient;
                facture.Medecin = MyData.Medecin;
                facture.Total_ht = MyData.Total_ht;
                facture.Total_ttc = MyData.Total_ht;
                facture.Ligne_facturation1 = MyData.Ligne_facturation1;
                facture.Ligne_facturation2 = MyData.Ligne_facturation2;
                facture.Ligne_facturation3 = MyData.Ligne_facturation3;
                facture.Ligne_facturation4 = MyData.Ligne_facturation4;
                facture.Titre1 = MyData.Titre1;
                facture.Titre2 = MyData.Titre2;
                facture.Titre3 = MyData.Titre3;
                facture.Titre4 = MyData.Titre4;
                facture.Matricule_patient = MyData.Matricule_patient;
                facture.MatriculeADH = MyData.MatriculeADH;
                facture.Societe = MyData.Societe;
                facture.assure_prin = MyData.assure_prin;
                facture.Date_entree = MyData.Date_entree;
                facture.Date_sortie = MyData.Date_sortie;
                facture.Taxe1 = MyData.Taxe1;
                facture.Remise = MyData.Remise;
                facture.ticketmoderateur = MyData.ticketmoderateur;
                facture.Intervenant = "";

                if (MyData.Net_a_payer_patient<0 || MyData.Net_a_payer_assurance<0)
                {
                    return Json("erreur: valeurs incorrectes");
                }

                _context.Update(facture);
                await _context.SaveChangesAsync();
                return Json(facture.Id.ToString());
            }

        }

        [HttpPost]
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<object> Trans([FromBody] Facture fact)
        {
            var facture = _context.Facture.Where(x => x.Numero_de_facture == fact.Numero_de_facture).FirstOrDefault();
            if (facture != null)
            {
                facture.Medecin = fact.Medecin;
            }

            _context.Update(facture);
            await _context.SaveChangesAsync();
            return Json("ok");

        }









        // POST: Facture/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var facture = await _context.Facture.FindAsync(id);
            _context.Facture.Remove(facture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FactureExists(int id)
        {
            return _context.Facture.Any(e => e.Id == id);
        }
        private string GetlastFacture()
        {
            if (_context.Facture.Count() == 0)
            {
                return "0";
            }
            else
            {
                //return 0;
                return _context.Facture.Where(x => !x.Type.Contains("proforma") && !x.Type.Contains("caution")).OrderBy(x => x.Id).Select(x => x.Numero_de_facture).LastOrDefault();
            }

        }

        private string Getlastproforma()
        {
            if (_context.Facture.Where(x => x.Type.Contains("proforma")).Count() == 0)
            {
                return "0";
            }
            else
            {
                //return 0;
                return _context.Facture.Where(x => x.Type.Contains("proforma")).OrderBy(x => x.Id).Select(x => x.Numero_de_facture).LastOrDefault();
            }

        }

        private string Getlastcaution()
        {
            if (_context.Facture.Where(x => x.Type.Contains("caution")).Count() == 0)
            {
                return "0";
            }
            else
            {
                //return 0;
                return _context.Facture.Where(x => x.Type.Contains("caution")).OrderBy(x => x.Id).Select(x => x.Numero_de_facture).LastOrDefault();
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

         public IActionResult UserAccess(string target)
        {
            var user = from u in _context_utilisateur.User
                    select u;

            user = user.Where(x => x.nom == HttpContext.User.Identity.Name);
            var connectuser = user.First();
            connectuser.Initial = GetInitials(connectuser.nom);
            if (connectuser.Access_module.Contains(target))
            {
                return Json("ok");
            }
            else
            {
                return RedirectToAction("Page405","Stock");
            }
            
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
        public static string Truncate(string s, int length)
        {
            if (s.Length > length)
            {
                return s.Substring(0, length);
            }
            return s;
        }


        public async Task<IActionResult> Pdf(int id, string target, string patient)
        {
            PdfGeneration2 x = new(_context);
            return new FileStreamResult(await x.DetailsFacture(id, target, patient), "application/pdf");
        }
        public async Task<IActionResult> PdfAssur(int id, string target, string patient)
        {
            PdfGeneration02 x = new(_context);
            return new FileStreamResult(await x.DetailsFacture(id, target, patient), "application/pdf");
        }

        public async Task<IActionResult> PdfMatrix(int id, string target, string patient)
        {
            PdfGeneration4 x = new(_context);
            return new FileStreamResult(await x.DetailsFacture(id), "application/pdf");
        }

        public async Task<IActionResult> Pdftiket(int id, string target, string patient)
        {
            PdfGeneration5 x = new(_context);
            return new FileStreamResult(await x.DetailsFacture(id), "application/pdf");
        }
        
        public  async Task<IActionResult>  Pdf_fusion(int id, string target,string patient)
        { 
            PdfGeneration2 x=new(_context);
            return  new FileStreamResult(await x.DetailsFusionFacture(id,target,patient), "application/pdf"); 
        }
    }
}

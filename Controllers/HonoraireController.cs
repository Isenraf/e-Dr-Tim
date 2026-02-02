#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using BANA.Models;
using Microsoft.AspNetCore.Authorization;
using Document = iTextSharp.text.Document;
using BANA.MyFunction;
using BANA.PDF;
using System.Globalization;

namespace BANA.Controllers
{
    
    public class HonoraireController : Controller
    {
        private readonly HonoraireContext _context;
        private readonly UserContext _context_utilisateur;
        private readonly DoctorContext _context_doctor;
        private readonly FactureContext _context_facture;

        public HonoraireController(HonoraireContext context, UserContext context_utilisateur, DoctorContext context_doctor, FactureContext context_facture)
        {
            _context = context;
            _context_utilisateur = context_utilisateur;
            _context_doctor = context_doctor;
            _context_facture = context_facture;

        }

        [Authorize]
        // GET: Honoraire
        public async Task<IActionResult> Index(string tipe, string Etat, string date, string numerofacture)
        {
 
            ViewData["numerofacture"] = numerofacture;
            ViewBag.user = Userconnected();
            ViewData["f000"] = "active";
            ViewBag.doctors = _context_doctor.Doctor.ToList();
            // if (date.Year == 0001)
            // {
            //     date = DateTime.Now;
            // }
            ViewBag.date = DateTime.Parse(date);
            var hono = await _context.Honoraire.Where(x => x.Create_date.Year == DateTime.Parse(date).Year  && x.Create_date.Month == DateTime.Parse(date).Month && x.Create_date.Day == DateTime.Parse(date).Day).OrderBy(x=>x.Create_date).ToListAsync();
            if (numerofacture != null)
            {
                hono = await _context.Honoraire.Where(x =>x.Numerofacture == "00" + numerofacture).ToListAsync();
                if (hono.Count > 0)
                {
                    ViewBag.date = hono.FirstOrDefault().Create_date;
                }
                
            }
            
            int mois = DateTime.Now.Month;
            int jour = DateTime.Now.Day;

            string mois_str = "";
            string jour_str = "";

            if (mois < 10) { mois_str = "0" + mois; } else { mois_str += mois; }
            if (jour < 10) { jour_str = "0" + jour; } else { jour_str += jour; }

            ViewBag.date2 = DateTime.Now.Year + "-" + mois_str + "-" + jour_str;

            List<bool> docteurs = new();
            foreach (var item in hono)
            {
                if (_context_doctor.Doctor.Where(x => x.Nom == item.Medecin).FirstOrDefault()!=null)
                {
                    docteurs.Add(_context_doctor.Doctor.Where(x => x.Nom == item.Medecin).FirstOrDefault().Interne);
                }else
                {
                    docteurs.Add(false);
                }
                
            }
            ViewBag.interne = docteurs;
            return View(hono);
        }
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
         public async Task<IActionResult> Indexx(string tipe, string Etat, string date, string numerofacture)
        {
 
            ViewData["numerofacture"] = numerofacture;
            ViewBag.user = Userconnected();
            ViewData["f000"] = "active";
            ViewBag.doctors = _context_doctor.Doctor.ToList();
            // if (date.Year == 0001)
            // {
            //     date = DateTime.Now;
            // }
            var medecins = _context_doctor.Doctor.Where(x => x.User_Name == Userconnected().nom).ToList();
            var medecin="";
            if (medecins!=null && medecins.Count()==1)
            {
                 medecin = medecins.FirstOrDefault().Nom+" "+medecins.FirstOrDefault().Prenom;
                
            }
            ViewBag.date = DateTime.Parse(date);
            var hono = await _context.Honoraire.Where(x =>x.Medecin.Trim().Replace(" ", "")!="" && x.Medecin.Trim().Replace(" ", "") == medecin.Trim().Replace(" ", "") && x.Create_date.Year == DateTime.Parse(date).Year && x.Create_date.Month == DateTime.Parse(date).Month && x.Create_date.Day == DateTime.Parse(date).Day).OrderBy(x => x.Create_date).ToListAsync();
            
            if (numerofacture != null)
            {
                hono = await _context.Honoraire.Where(x =>x.Medecin.Trim().Replace(" ","")==Userconnected().nom.Trim().Replace(" ","") && x.Numerofacture == "00" + numerofacture).ToListAsync();
                if (hono.Count > 0)
                {
                    ViewBag.date = hono.FirstOrDefault().Create_date;
                }
                
            }
            
            int mois = DateTime.Now.Month;
            int jour = DateTime.Now.Day;

            string mois_str = "";
            string jour_str = "";

            if (mois < 10) { mois_str = "0" + mois; } else { mois_str += mois; }
            if (jour < 10) { jour_str = "0" + jour; } else { jour_str += jour; }

            ViewBag.date2 = DateTime.Now.Year + "-" + mois_str + "-" + jour_str;

            List<bool> docteurs = new();
            foreach (var item in hono)
            {
                if (_context_doctor.Doctor.Where(x => x.Nom == item.Medecin).FirstOrDefault()!=null)
                {
                    docteurs.Add(_context_doctor.Doctor.Where(x => x.Nom == item.Medecin).FirstOrDefault().Interne);
                }else
                {
                    docteurs.Add(false);
                }
                
            }
            ViewBag.interne = docteurs;
            return View(hono);
        }

        [Authorize]
        public async Task<IActionResult> Index0(string tipe, string Etat, DateTime date, string numerofacture)
        {
            List<Stat> datas = new();
            ViewData["14"] = "active";
            var honoraires = await _context.Honoraire.OrderByDescending(x => x.Create_date).ToListAsync();

            var moisAnneesTries = honoraires
            .Select(d => new { d.Create_date.Month, d.Create_date.Year })
            .Distinct()
            .ToList();

            //moisAnneesTries.RemoveAt(0);

            foreach (var ma in moisAnneesTries)
            {

                string nomMois = CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat.GetMonthName(ma.Month);
                string nomMois2 = CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat.GetMonthName(ma.Month + 1);
                if (ma.Month == 12) { nomMois2 = CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat.GetMonthName(0 + 1); }
                Console.WriteLine($"{nomMois} {ma.Year}");
                Stat nouveau = new();
                nouveau.mois = ma.Month;
                nouveau.annee = ma.Year;
                nouveau.date_en_lettre = "21 " + nomMois + " " + ma.Year + " au 20 " + nomMois2 + " " + ma.Year;
                datas.Add(nouveau);

            }

            //datas.AddRange(_context.Honoraire.Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas;
            ViewBag.fini = honoraires.Where(x => x.Etat == "Cloturé").Count();
            ViewBag.encours = honoraires.Where(x => x.Etat != "Cloturé").Count();
            //ViewBag.montanttotal = honoraires.Select(x => (long)x.Montant_medecin).Sum();

            ViewBag.user = Userconnected();
            return View(honoraires);

        }

        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Index00()
        {
            List<Stat> datas = new();
            ViewData["xx3"] = "active current-page";

            var medecins = _context_doctor.Doctor.Where(x => x.User_Name == Userconnected().nom).ToList();
            var medecin="";
            if (medecins!=null && medecins.Count()==1)
            {
                 medecin = medecins.FirstOrDefault().Nom+" "+medecins.FirstOrDefault().Prenom;
                
            }
            var honoraires = await _context.Honoraire.Where(x => x.Medecin.Trim().Replace(" ", "") == medecin.Trim().Replace(" ", "")).OrderByDescending(x => x.Create_date).ToListAsync();

            var moisAnneesTries = honoraires
            .Select(d => new { d.Create_date.Month, d.Create_date.Year })
            .Distinct()
            .ToList();

            //moisAnneesTries.RemoveAt(0);

            foreach (var ma in moisAnneesTries)
            {

                string nomMois = CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat.GetMonthName(ma.Month);
                string nomMois2 = CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat.GetMonthName(ma.Month + 1);
                if (ma.Month == 12) { nomMois2 = CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat.GetMonthName(0 + 1); }
                Console.WriteLine($"{nomMois} {ma.Year}");
                Stat nouveau = new();
                nouveau.mois = ma.Month;
                nouveau.annee = ma.Year;
                nouveau.date_en_lettre = "21 " + nomMois + " " + ma.Year + " au 20 " + nomMois2 + " " + ma.Year;
                datas.Add(nouveau);

            }
            ViewBag.datas = datas;
            ViewBag.fini = honoraires.Where(x => x.Etat == "Cloturé").Count();
            ViewBag.encours = honoraires.Where(x => x.Etat != "Cloturé").Count();
            ViewBag.user = Userconnected();
            return View(honoraires);

        }


        [Authorize]
        public async Task<IActionResult> Index1(int mois, int annee)
        {
            List<string> datas = new();
            ViewData["14"] = "active";
            var honoraires = await _context.Honoraire.Where(x => x.Create_date.Year == annee && x.Create_date.Month == mois && x.Create_date.Day > 20).OrderBy(x => x.Create_date).ToListAsync();
            if (mois == 12) { mois = 0; annee++; }
            honoraires.AddRange(await _context.Honoraire.Where(x => x.Create_date.Year == annee && x.Create_date.Month == mois + 1 && x.Create_date.Day <= 20).OrderBy(x => x.Create_date).ToListAsync());

            var moisAnneesTries = honoraires.Select(d => d.Create_date.ToShortDateString()).Distinct().ToList();
            datas.AddRange(moisAnneesTries);
            ViewBag.datas = datas;
            ViewBag.fini = honoraires.Where(x => x.Etat == "Cloturé").Count();
            ViewBag.encours = honoraires.Where(x => x.Etat != "Cloturé").Count();
            //ViewBag.montanttotal = honoraires.Select(x => (long)x.Montant_medecin).Sum();
            List<string> vecteur = new();
            int i = 0;
            foreach (var item in moisAnneesTries)
            {
                if (honoraires.Where(x => x.Create_date.ToShortDateString() == item).Any(x => x.Autocalcul == true))
                {
                    vecteur.Add("oui");
                    //Console.WriteLine(i + " oui "+item);
                }
                else
                {
                    vecteur.Add("non");
                    //Console.WriteLine(i + " non "+item);
                }
                i++;
            }

            ViewBag.vecteur = vecteur;
            ViewBag.user = Userconnected();
            return View(honoraires);

        }

        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Index11(int mois, int annee)
        {
            List<string> datas = new();
            ViewData["14"] = "active";
            var medecins = _context_doctor.Doctor.Where(x => x.User_Name == Userconnected().nom).ToList();
            var medecin="";
            if (medecins!=null && medecins.Count()==1)
            {
                 medecin = medecins.FirstOrDefault().Nom+" "+medecins.FirstOrDefault().Prenom;
                
            }
            var honoraires = await _context.Honoraire.Where(x =>x.Medecin.Trim().Replace(" ","")==medecin.Trim().Replace(" ","") && x.Create_date.Year == annee && x.Create_date.Month == mois && x.Create_date.Day > 20).OrderBy(x => x.Create_date).ToListAsync();
            if (mois == 12) { mois = 0; annee++; }
            honoraires.AddRange(await _context.Honoraire.Where(x =>x.Medecin.Trim().Replace(" ","")==medecin.Trim().Replace(" ","") && x.Create_date.Year == annee && x.Create_date.Month == mois + 1 && x.Create_date.Day <= 20).OrderBy(x => x.Create_date).ToListAsync());

            var moisAnneesTries = honoraires.Select(d => d.Create_date.ToShortDateString()).Distinct().ToList();
            datas.AddRange(moisAnneesTries);
            ViewBag.datas = datas;
            ViewBag.fini = honoraires.Where(x => x.Etat == "Cloturé").Count();
            ViewBag.encours = honoraires.Where(x => x.Etat != "Cloturé").Count();
            //ViewBag.montanttotal = honoraires.Select(x => (long)x.Montant_medecin).Sum();
            List<string> vecteur = new();
            int i = 0;
            foreach (var item in moisAnneesTries)
            {
                if (honoraires.Where(x => x.Create_date.ToShortDateString() == item).Any(x => x.Autocalcul == true))
                {
                    vecteur.Add("oui");
                    //Console.WriteLine(i + " oui "+item);
                }
                else
                {
                    vecteur.Add("non");
                    //Console.WriteLine(i + " non "+item);
                }
                i++;
            }

            ViewBag.vecteur = vecteur;
            ViewBag.user = Userconnected();
            return View(honoraires);

        }




        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Index(string tipe,string Etat,DateTime date)
        // {
        //     ViewData["tipe"]=tipe;
        //     ViewData["Etat"]=Etat;
        //     ViewBag.user=Userconnected();
        //     ViewData["f000"]="active";
        //     ViewBag.doctors=_context_doctor.Doctor.ToList();
        //     return View(await _context.Honoraire.Where(x=>x.Mode_paiement==tipe && x.Etat==Etat).ToListAsync());
        // }

        // GET: Honoraire/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var honoraire = await _context.Honoraire
                .FirstOrDefaultAsync(m => m.Id == id);
            if (honoraire == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(honoraire);
        }

        // GET: Honoraire/Create
        public IActionResult Create()
        {
            ViewBag.user = Userconnected();
            return View();
        }

        // POST: Honoraire/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Medecin,Telephone,Patient,Mode_paiement,Etat,Create_date,Montant,Pourcentage,Montant_medecin,Montant_paye,Montant_reste")] Honoraire honoraire)
        {
            if (ModelState.IsValid)
            {
                _context.Add(honoraire);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.user = Userconnected();
            return View(honoraire);
        }

        // GET: Honoraire/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var honoraire = await _context.Honoraire.FindAsync(id);
            if (honoraire == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(honoraire);
        }

        // POST: Honoraire/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Medecin,Telephone,Patient,Mode_paiement,Etat,Create_date,Montant,Pourcentage,Montant_medecin,Montant_paye,Montant_reste")] Honoraire honoraire)
        {
            if (id != honoraire.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(honoraire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HonoraireExists(honoraire.Id))
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
            return View(honoraire);
        }

        // GET: Honoraire/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var honoraire = await _context.Honoraire
                .FirstOrDefaultAsync(m => m.Id == id);
            if (honoraire == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();

            return View(honoraire);
        }

        // POST: Honoraire/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var honoraire = await _context.Honoraire.FindAsync(id);
            _context.Honoraire.Remove(honoraire);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Rapport2(DateTime datedebut, DateTime datefin, List<string> docta, List<string> secretaires)
        {


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

            docta.Sort();
            docta.Remove("caution");

            ViewBag.select = docta;
            ViewBag.select2 = secretaires;

            var honoraire00 = await _context.Honoraire.Where(x => x.Etat == "Cloturé" && x.Mode_paiement == "assurance" && x.Create_date >= DateTime.Parse("20/01/2025") && x.Paid_date >= datedebut && x.Paid_date <= datefin && x.Montant_medecin > 0).OrderBy(x => x.Assurance).ThenBy(x => x.Create_date).ToListAsync();
            var honoraire0 = await _context.Honoraire.Where(x => x.Etat == "Cloturé" && x.Mode_paiement == "assurance" && x.Create_date >= DateTime.Parse("20/01/2025") && x.Paid_date.Year == 0001 && x.Montant_medecin > 0 && x.Create_date <= datefin).OrderBy(x => x.Assurance).ThenBy(x => x.Create_date).ToListAsync();
            var honoraire = await _context.Honoraire.Where(x => x.Create_date >= datedebut && x.Create_date <= datefin && x.Create_date >= DateTime.Parse("20/01/2025") && x.Mode_paiement == "cash" && x.Etat == "Cloturé" && x.Montant_medecin > 0).OrderBy(x => x.Id).ToListAsync();
            // if (true)
            // {
            //     honoraire=honoraire.Where(x=>x.Mode_paiement==tipe).ToList();
            // }


            var hono = honoraire.GroupBy(p => new { p.Nom, p.Medecin, p.Patient, p.Create_date.Year, p.Create_date.Month, p.Create_date.Day }).Select(g => g.Last()).ToList();
            var hono1 = honoraire00.GroupBy(p => new { p.Nom, p.Medecin, p.Patient, p.Create_date.Year, p.Create_date.Month, p.Create_date.Day }).Select(g => g.Last()).ToList();
            var hono2 = honoraire0.GroupBy(p => new { p.Nom, p.Medecin, p.Patient, p.Create_date.Year, p.Create_date.Month, p.Create_date.Day }).Select(g => g.Last()).ToList();


            List<Honoraire> finalresult0_1 = new();
            List<Honoraire> finalresult0_2 = new();
            List<Honoraire> finalresult0 = new();

            List<Honoraire> finalresult1_1 = new();
            List<Honoraire> finalresult2_2 = new();
            List<Honoraire> finalresult = new();





            List<string> secretair = new List<string>();
            secretair.AddRange(honoraire.OrderBy(x => x.Medecin).Select(x => x.Observation));
            secretair.AddRange(honoraire0.OrderBy(x => x.Medecin).Select(x => x.Observation));
            secretair.AddRange(honoraire00.OrderBy(x => x.Medecin).Select(x => x.Observation));
            secretair.Sort();
            ViewBag.secretaires = secretair.Distinct();

            honoraire00 = new();
            honoraire0 = new();
            honoraire = new();


            foreach (var item in secretaires)
            {
                honoraire00 = hono1.Where(x => x.Observation == item).ToList();
                finalresult0_1.AddRange(honoraire00);
                honoraire0 = hono2.Where(x => x.Observation == item).ToList();
                finalresult0_2.AddRange(honoraire0);
                honoraire = hono.Where(x => x.Observation == item).ToList();
                finalresult0.AddRange(honoraire);
            }

            List<string> medecins = new List<string>();

            medecins.AddRange(finalresult0_1.OrderBy(x => x.Medecin).Select(x => x.Medecin.Trim()));
            medecins.AddRange(finalresult0_2.OrderBy(x => x.Medecin).Select(x => x.Medecin.Trim()));
            medecins.AddRange(finalresult0.OrderBy(x => x.Medecin).Select(x => x.Medecin.Trim()));
            medecins.Sort();
            if (docta.Count() >= medecins.Count())
            {
                ViewBag.select = medecins;
            }

            List<string> finalmedecins = new();
            finalmedecins.AddRange(medecins.Distinct());

            foreach (var item in medecins.Distinct())
            {
                if (_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault() != null)
                {
                    if (_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault().Telephone != null)
                    {
                        if (_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault().Telephone.Count() < 9)
                        {
                            finalmedecins.Remove(item);
                        }

                    }
                    else
                    {
                        finalmedecins.Remove(item);
                    }
                }
                else
                {
                    finalmedecins.Remove(item);
                }
            }
            ViewBag.medecins = finalmedecins;




            foreach (var item in docta)
            {
                honoraire00 = finalresult0_1.Where(x => x.Medecin.Replace(" ", "") == item.Replace(" ", "")).ToList();
                finalresult1_1.AddRange(honoraire00);
                honoraire0 = finalresult0_2.Where(x => x.Medecin.Replace(" ", "") == item.Replace(" ", "")).ToList();
                finalresult2_2.AddRange(honoraire0);
                honoraire = finalresult0.Where(x => x.Medecin.Replace(" ", "") == item.Replace(" ", "")).ToList();
                finalresult.AddRange(honoraire);
            }



            List<string> tel = new();
            foreach (var item in docta)
            {
                if (_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault() != null)
                    tel.Add(_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault().Telephone);
                else
                    tel.Add("######");
            }
            ViewBag.tel = tel;





            ViewBag.mt = honoraire.Select(x => x.Montant).Sum();
            ViewBag.nf = honoraire.Count();


            ViewBag.date1 = datedebut;
            ViewBag.date2 = datefin;

            ViewBag.total = finalresult.Select(x => x.Montant).Sum();

            foreach (var item in finalresult)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }
            foreach (var item in finalresult2_2)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }

            foreach (var item in finalresult1_1)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }
            foreach (var item in ViewBag.medecins)
            {
                Console.WriteLine(item + " " + item.Length);
            }
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            foreach (var item in docta)
            {
                Console.WriteLine(item);
            }



            ViewBag.honoraireassurance = finalresult2_2;
            ViewBag.honoraireassurance1 = finalresult1_1;
            return View(finalresult);
        }
        
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Rapport3(DateTime datedebut, DateTime datefin, List<string> docta, List<string> secretaires)
        {


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

            docta.Sort();
            docta.Remove("caution");

            ViewBag.select = docta;
            ViewBag.select2 = secretaires;

            var medecinss = _context_doctor.Doctor.Where(x => x.User_Name == Userconnected().nom).ToList();
            var medecin="";
            if (medecinss!=null && medecinss.Count()==1)
            {
                 medecin = medecinss.FirstOrDefault().Nom+" "+medecinss.FirstOrDefault().Prenom;
                
            }

            var honoraire00 = await _context.Honoraire.Where(x => x.Medecin.Trim().Replace(" ","")==medecin.Trim().Replace(" ","") && x.Etat == "Cloturé" && x.Mode_paiement == "assurance" && x.Create_date >= DateTime.Parse("20/01/2025") && x.Paid_date >= datedebut && x.Paid_date <= datefin && x.Montant_medecin > 0).OrderBy(x => x.Assurance).ThenBy(x => x.Create_date).ToListAsync();
            var honoraire0 = await _context.Honoraire.Where(x => x.Medecin.Trim().Replace(" ","")==medecin.Trim().Replace(" ","") && x.Etat == "Cloturé" && x.Mode_paiement == "assurance" && x.Create_date >= DateTime.Parse("20/01/2025") && x.Paid_date.Year == 0001 && x.Montant_medecin > 0 && x.Create_date <= datefin).OrderBy(x => x.Assurance).ThenBy(x => x.Create_date).ToListAsync();
            var honoraire = await _context.Honoraire.Where(x => x.Medecin.Trim().Replace(" ","")==medecin.Trim().Replace(" ","") && x.Create_date >= datedebut && x.Create_date <= datefin && x.Create_date >= DateTime.Parse("20/01/2025") && x.Mode_paiement == "cash" && x.Etat == "Cloturé" && x.Montant_medecin > 0).OrderBy(x => x.Id).ToListAsync();
            // if (true)
            // {
            //     honoraire=honoraire.Where(x=>x.Mode_paiement==tipe).ToList();
            // }


            var hono = honoraire.GroupBy(p => new { p.Nom,p.Medecin, p.Patient, p.Create_date.Year , p.Create_date.Month, p.Create_date.Day}).Select(g => g.Last()).ToList();
            var hono1 = honoraire00.GroupBy(p => new { p.Nom,p.Medecin, p.Patient, p.Create_date.Year , p.Create_date.Month, p.Create_date.Day}).Select(g => g.Last()).ToList();
            var hono2 = honoraire0.GroupBy(p => new { p.Nom,p.Medecin, p.Patient, p.Create_date.Year , p.Create_date.Month, p.Create_date.Day}).Select(g => g.Last()).ToList();


            List<Honoraire> finalresult0_1 = new();
            List<Honoraire> finalresult0_2 = new();
            List<Honoraire> finalresult0 = new();

            List<Honoraire> finalresult1_1 = new();
            List<Honoraire> finalresult2_2 = new();
            List<Honoraire> finalresult = new();





            List<string> secretair = new List<string>();
            secretair.AddRange(honoraire.OrderBy(x => x.Medecin).Select(x => x.Observation));
            secretair.AddRange(honoraire0.OrderBy(x => x.Medecin).Select(x => x.Observation));
            secretair.AddRange(honoraire00.OrderBy(x => x.Medecin).Select(x => x.Observation));
            secretair.Sort();
            ViewBag.secretaires = secretair.Distinct();

            honoraire00 = new();
            honoraire0 = new();
            honoraire = new();


            foreach (var item in secretaires)
            {
                honoraire00 = hono1.Where(x => x.Observation == item).ToList();
                finalresult0_1.AddRange(honoraire00);
                honoraire0 = hono2.Where(x => x.Observation == item).ToList();
                finalresult0_2.AddRange(honoraire0);
                honoraire = hono.Where(x => x.Observation == item).ToList();
                finalresult0.AddRange(honoraire);
            }

            List<string> medecins = new List<string>();

            medecins.AddRange(finalresult0_1.OrderBy(x => x.Medecin).Select(x => x.Medecin.Trim()));
            medecins.AddRange(finalresult0_2.OrderBy(x => x.Medecin).Select(x => x.Medecin.Trim()));
            medecins.AddRange(finalresult0.OrderBy(x => x.Medecin).Select(x => x.Medecin.Trim()));
            medecins.Sort();
            if (docta.Count() >= medecins.Count())
            {
                ViewBag.select = medecins;
            }

            List<string> finalmedecins = new();
            finalmedecins.AddRange(medecins.Distinct());

            foreach (var item in medecins.Distinct())
            {
                if (_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault() != null)
                {
                    if (_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault().Telephone != null)
                    {
                        if (_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault().Telephone.Count() < 9)
                        {
                            finalmedecins.Remove(item);
                        }

                    }
                    else
                    {
                        finalmedecins.Remove(item);
                    }
                }
                else
                {
                    finalmedecins.Remove(item);
                }
            }
            ViewBag.medecins = finalmedecins;




            foreach (var item in docta)
            {
                honoraire00 = finalresult0_1.Where(x => x.Medecin.Replace(" ", "") == item.Replace(" ", "")).ToList();
                finalresult1_1.AddRange(honoraire00);
                honoraire0 = finalresult0_2.Where(x => x.Medecin.Replace(" ", "") == item.Replace(" ", "")).ToList();
                finalresult2_2.AddRange(honoraire0);
                honoraire = finalresult0.Where(x => x.Medecin.Replace(" ", "") == item.Replace(" ", "")).ToList();
                finalresult.AddRange(honoraire);
            }



            List<string> tel = new();
            foreach (var item in docta)
            {
                if (_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault() != null)
                    tel.Add(_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault().Telephone);
                else
                    tel.Add("######");
            }
            ViewBag.tel = tel;





            ViewBag.mt = honoraire.Select(x => x.Montant).Sum();
            ViewBag.nf = honoraire.Count();


            ViewBag.date1 = datedebut;
            ViewBag.date2 = datefin;

            ViewBag.total = finalresult.Select(x => x.Montant).Sum();

            foreach (var item in finalresult)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }
            foreach (var item in finalresult2_2)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }

            foreach (var item in finalresult1_1)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }
            foreach (var item in ViewBag.medecins)
            {
                Console.WriteLine(item + " " + item.Length);
            }
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            foreach (var item in docta)
            {
                Console.WriteLine(item);
            }



            ViewBag.honoraireassurance = finalresult2_2;
            ViewBag.honoraireassurance1 = finalresult1_1;
            return View(finalresult);
        }


        public async Task<IActionResult> Rapport22(DateTime datedebut, DateTime datefin, List<string> docta, List<string> secretaires)
        {
            // foreach (var item in _context.Honoraire.Where(x=>x.Etat=="Cloturé" &&  x.Create_date>=DateTime.Parse("20/01/2025")).ToList())
            // {
            //     if (item.Observation!="NGAH ARLETTE" && item.Observation!="NYUNAI CHRISTINE" && item.Observation != "MARIE SILIKI")
            //     {
            //         item.Paid_date= item.Create_date;
            //         _context.Update(item);
            //         await _context.SaveChangesAsync();
            //     }
            // }
            ViewData["tb0"] = "active";
            ViewData["tb3"] = "active";
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

            docta.Sort();
            docta.Remove("caution");

            ViewBag.select = docta;
            ViewBag.select2 = secretaires;

            var honoraire00 = await _context.Honoraire.Where(x => x.Etat == "Cloturé" && x.Mode_paiement == "assurance" && x.Create_date >= DateTime.Parse("20/01/2025") && x.Paid_date >= datedebut && x.Paid_date <= datefin && x.Montant_medecin > 0).OrderBy(x => x.Assurance).ThenBy(x => x.Create_date).ToListAsync();
            var honoraire0 = await _context.Honoraire.Where(x => x.Etat == "Cloturé" && x.Mode_paiement == "assurance" && x.Create_date >= DateTime.Parse("20/01/2025") && x.Paid_date.Year == 0001 && x.Montant_medecin > 0 && x.Create_date <= datefin).OrderBy(x => x.Assurance).ThenBy(x => x.Create_date).ToListAsync();
            var honoraire = await _context.Honoraire.Where(x => x.Create_date >= datedebut && x.Create_date <= datefin && x.Create_date >= DateTime.Parse("20/01/2025") && x.Mode_paiement == "cash" && x.Etat == "Cloturé" && x.Montant_medecin > 0).OrderBy(x => x.Id).ToListAsync();
            // if (true)
            // {
            //     honoraire=honoraire.Where(x=>x.Mode_paiement==tipe).ToList();
            // }


            var hono = honoraire.GroupBy(p => new { p.Nom, p.Medecin, p.Patient, p.Create_date.Year, p.Create_date.Month, p.Create_date.Day }).Select(g => g.Last()).ToList();
            var hono1 = honoraire00.GroupBy(p => new { p.Nom, p.Medecin, p.Patient, p.Create_date.Year, p.Create_date.Month, p.Create_date.Day }).Select(g => g.Last()).ToList();
            var hono2 = honoraire0.GroupBy(p => new { p.Nom, p.Medecin, p.Patient, p.Create_date.Year, p.Create_date.Month, p.Create_date.Day }).Select(g => g.Last()).ToList();

            List<Honoraire> finalresult0_1 = new();
            List<Honoraire> finalresult0_2 = new();
            List<Honoraire> finalresult0 = new();

            List<Honoraire> finalresult1_1 = new();
            List<Honoraire> finalresult2_2 = new();
            List<Honoraire> finalresult = new();





            List<string> secretair = new List<string>();
            secretair.AddRange(honoraire.OrderBy(x => x.Medecin).Select(x => x.Observation));
            secretair.AddRange(honoraire0.OrderBy(x => x.Medecin).Select(x => x.Observation));
            secretair.AddRange(honoraire00.OrderBy(x => x.Medecin).Select(x => x.Observation));
            secretair.Sort();
            ViewBag.secretaires = secretair.Distinct();

            honoraire00 = new();
            honoraire0 = new();
            honoraire = new();


            foreach (var item in secretaires)
            {
                honoraire00 = hono1.Where(x => x.Observation == item).ToList();
                finalresult0_1.AddRange(honoraire00);
                honoraire0 = hono2.Where(x => x.Observation == item).ToList();
                finalresult0_2.AddRange(honoraire0);
                honoraire = hono.Where(x => x.Observation == item).ToList();
                finalresult0.AddRange(honoraire);
            }

            List<string> medecins = new List<string>();

            medecins.AddRange(finalresult0_1.OrderBy(x => x.Medecin).Select(x => x.Medecin.Trim()));
            medecins.AddRange(finalresult0_2.OrderBy(x => x.Medecin).Select(x => x.Medecin.Trim()));
            medecins.AddRange(finalresult0.OrderBy(x => x.Medecin).Select(x => x.Medecin.Trim()));
            medecins.Sort();
            if (docta.Count() >= medecins.Count())
            {
                ViewBag.select = medecins;
            }
            ViewBag.medecins = medecins.Distinct();




            foreach (var item in docta)
            {
                honoraire00 = finalresult0_1.Where(x => x.Medecin.Replace(" ", "") == item.Replace(" ", "")).ToList();
                finalresult1_1.AddRange(honoraire00);
                honoraire0 = finalresult0_2.Where(x => x.Medecin.Replace(" ", "") == item.Replace(" ", "")).ToList();
                finalresult2_2.AddRange(honoraire0);
                honoraire = finalresult0.Where(x => x.Medecin.Replace(" ", "") == item.Replace(" ", "")).ToList();
                finalresult.AddRange(honoraire);
            }



            List<string> tel = new();
            foreach (var item in docta)
            {
                if (_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault() != null)
                    tel.Add(_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault().Telephone);
                else
                    tel.Add("######");
            }
            ViewBag.tel = tel;





            ViewBag.mt = honoraire.Select(x => x.Montant).Sum();
            ViewBag.nf = honoraire.Count();


            ViewBag.date1 = datedebut;
            ViewBag.date2 = datefin;

            ViewBag.total = finalresult.Select(x => x.Montant).Sum();

            foreach (var item in finalresult)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }
            foreach (var item in finalresult2_2)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }

            foreach (var item in finalresult1_1)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }
            foreach (var item in ViewBag.medecins)
            {
                Console.WriteLine(item + " " + item.Length);
            }
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            Console.WriteLine("--------------------------");
            foreach (var item in docta)
            {
                Console.WriteLine(item);
            }



            ViewBag.honoraireassurance = finalresult2_2;
            ViewBag.honoraireassurance1 = finalresult1_1;


            if (docta.Count > 0)
            {
                EnvoiEtat(datedebut, datefin, docta, finalresult2_2, finalresult1_1, finalresult);

            }
            return View(finalresult);
        }



        [HttpPost]
        //[Authorize]
        public async Task<object> Add([FromBody] Honoraire MyData)
        {
            if (!MedecinExists(MyData.Medecin))
            {
                return Json("medecin");
            }
            var Tous_les_hono = await _context.Honoraire.ToListAsync();

            var honoraire = await _context.Honoraire
                 .FirstOrDefaultAsync(m => m.Id == MyData.Id);


            string targ = honoraire.Etat;
            honoraire.Medecin = MyData.Medecin;
            honoraire.Montant = MyData.Montant;
            honoraire.Pourcentage = MyData.Pourcentage;
            honoraire.Montant_medecin = MyData.Montant_medecin;
            honoraire.Montant_paye = MyData.Montant_paye;
            if (honoraire.Observation == null) { honoraire.Observation = Userconnected().nom; }

            honoraire.Etat = "Cloturé";
            if (honoraire.Observation != "NGAH ARLETTE" && honoraire.Observation != "NYUNAI CHRISTINE" && honoraire.Observation != "MARIE SILIKI")
            {
                honoraire.Paid_date = honoraire.Create_date;
            }
            if (HonoraireCount(honoraire)>0 && targ!="Cloturé")
            {
                return Json("doublons");
            }


            _context.Update(honoraire);
            await _context.SaveChangesAsync();
            return Json("ok");

        }
        [HttpPost]
        //[Authorize]
        public async Task<object> Paid([FromBody] Honoraire MyData)
        {
            var honoraire = await _context.Honoraire
                 .FirstOrDefaultAsync(m => m.Id == MyData.Id);

            foreach (var item in _context.Honoraire.Where(x => x.Numerofacture == honoraire.Numerofacture && x.Medecin == honoraire.Medecin))
            {
                honoraire.Paid_date = MyData.Paid_date;
                //honoraire.Montant_reste=MyData.Montant_medecin-MyData.Montant_paye;
                _context.Update(honoraire);
                await _context.SaveChangesAsync();
            }

            return Json("ok");

        }

        [HttpPost]
        //[Authorize]
        public async Task<object> Clone([FromBody] Honoraire MyData)
        {
            if (!MedecinExists(MyData.Medecin))
            {
                return Json("medecin");
            }
            var honoraire = await _context.Honoraire
                 .FirstOrDefaultAsync(m => m.Id == MyData.Id);



            honoraire.Id = 0;
            honoraire.Medecin = MyData.Medecin;
            honoraire.Pourcentage = MyData.Pourcentage;
            honoraire.Montant_medecin = MyData.Montant_medecin;
            honoraire.Montant_paye = MyData.Montant_paye;
            honoraire.Observation = Userconnected().nom;
            honoraire.Etat = "Cloturé";
            if (honoraire.Observation != "NGAH ARLETTE" && honoraire.Observation != "NYUNAI CHRISTINE" && honoraire.Observation != "MARIE SILIKI")
            {
                honoraire.Paid_date = honoraire.Create_date;
            }

            if (HonoraireCount(honoraire) == 0)
            {
                _context.Add(honoraire);
                await _context.SaveChangesAsync();

            }
            return Json("ok");

        }

        [HttpPost]
        //[Authorize]
        public async Task<object> Auto([FromBody] Honoraire MyData)
        {


            //régularisation
            var factures_maquante = await _context_facture.Facture.Where(x => x.Etat_patient == "Cloturé" && x.Encaisse_par!=""  && x.Create_date.Year == MyData.Create_date.Year && x.Create_date.Month == MyData.Create_date.Month && x.Create_date.Day == MyData.Create_date.Day).ToListAsync();

            string viseur = "λ";
            foreach (var item in factures_maquante)
            {
                string listefacturation = item.Ligne_facturation1 + item.Ligne_facturation2[1..] + item.Ligne_facturation3[1..];
                List<string> liste_de_facturation = new();
                List<string> liste_de_facturation00 = new();
                

                int i = 0;
                int compteur = 0;
                string chaine = "";
                foreach (char c in listefacturation)
                {

                    if (c.Equals('λ'))
                    {
                        if (i != 0)
                        {
                            if (compteur % 7 == 0 && chaine != "")
                            {
                                liste_de_facturation.Add(chaine);
                            }
                            if (compteur % 7 == 6 && chaine != "")
                            {
                                liste_de_facturation00.Add(chaine);
                            }

                            compteur += 1;
                            chaine = "";

                        }

                    }
                    else
                    {
                        chaine += c.ToString();
                    }
                    i++;

                }



                int t = 0;
                foreach (var item0 in liste_de_facturation)
                {
                    Honoraire honoraire = new();
                    honoraire.Create_date = item.Create_date;
                    honoraire.Nom = item0;
                    if (item.Type.Contains("assurance")) { honoraire.Mode_paiement = "assurance"; } else { honoraire.Mode_paiement = "cash"; }
                    honoraire.Numerofacture = item.Numero_de_facture;
                    honoraire.Medecin = item.Medecin;
                    honoraire.Patient = item.Nom + " " + item.Prenom;
                    honoraire.Montant = int.Parse(liste_de_facturation00[t].Replace(" ", ""));
                    honoraire.Assurance = item.Assureur;
                    honoraire.Etat = "En attente";
                    

                    if (HonoraireCount2(honoraire) == 0)
                    {
                        if (item.Type == "hospi_assurance")
                        {
                            honoraire.Medecin = "HOSPITALISATION";

                        }
                        if (item.Type == "generique_0" || item.Type == "generique_assurance")
                        {
                            honoraire.Medecin = "GÉNÉRIQUE";

                        }

                        _context.Add(honoraire);
                       await _context.SaveChangesAsync();
                    }

                    t++;
                }
                if (item.Type=="hospi_assurance")
                {
                    viseur = item.Numero_de_facture +"λ";
                }
                
            }

           
            int mois = MyData.Create_date.Month;
            int annee = MyData.Create_date.Year;
            int jour = MyData.Create_date.Day;
            //if (mois == 12) { mois = 1; annee++; }
            Console.WriteLine(MyData.Create_date);
            Console.WriteLine(MyData.Mode_paiement);

            var honoraires = await _context.Honoraire.Where(x=>x.Create_date.Year==annee && x.Create_date.Month==mois && x.Create_date.Day==jour).OrderBy(x => x.Create_date).ToListAsync();
            var honoraire_a_traiter = honoraires.Where(x=>x.Etat == "En attente" && x.Create_date.Year==annee && x.Create_date.Month==mois && x.Create_date.Day==jour).OrderBy(x => x.Create_date).ToList();
            
            
            foreach (var item in honoraire_a_traiter)
            {
                if (viseur.Contains("λ" + item.Numerofacture + "λ"))
                {
                    item.Medecin = "HOSPITALISATION";
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }

            }
             Console.WriteLine(honoraire_a_traiter.Count());
            int h = 0;
            foreach (var item in honoraire_a_traiter)
            {
                Console.WriteLine(item.Nom);
                Console.WriteLine("XXXXXX");
                //var target = await _context.Honoraire.Where(x => x.Etat == "Cloturé" && x.Medecin.Replace(" ", "") == item.Medecin.Replace(" ", "") && x.Mode_paiement == item.Mode_paiement && x.Nom.Replace(" ", "") == item.Nom.Replace(" ", "")).OrderBy(x => x.Id).LastOrDefaultAsync();
                var target = await _context.Honoraire.Where(x => x.Etat == "Cloturé" && x.Create_date >= DateTime.Parse("20/01/2025") && x.Medecin.Replace(" ", "") == item.Medecin.Replace(" ", "") && x.Nom.Replace(" ", "") == item.Nom.Replace(" ", "") && x.Montant == item.Montant && x.Mode_paiement == item.Mode_paiement).OrderBy(x => x.Id).LastOrDefaultAsync();
                if(h==0){honoraires.FirstOrDefault().Autocalcul = true;_context.Update(honoraires.FirstOrDefault());await _context.SaveChangesAsync();}
                if (target != null)
                {
                    
                    Console.WriteLine("YYYYYY");
                    item.Pourcentage = target.Pourcentage;
                    item.Montant_medecin = target.Montant_medecin;
                    item.Observation = target.Observation;
                    item.Etat = "Cloturé";
                    item.Autocalcul = true;
                    if (item.Observation != "NGAH ARLETTE" && item.Observation != "NYUNAI CHRISTINE" && item.Observation != "MARIE SILIKI")
                    {
                        item.Paid_date = item.Create_date;
                    }

                    if (HonoraireCount(item) == 0 && item.Montant_medecin > 0)
                    {
                        Console.WriteLine(HonoraireCount(item));
                       _context.Update(target);
                       await _context.SaveChangesAsync();
                    }


                }

                h++;
            }

            // var hono = honoraires.Where(x=>x.Etat == "Cloturé" && x.Create_date.Year==annee && x.Create_date.Month==mois && x.Create_date.Day==jour).OrderBy(x => x.Create_date).ToList();
            // foreach (var item in hono)
            // {
            //     // if (item.Observation != "NGAH ARLETTE" && item.Observation != "NYUNAI CHRISTINE" && item.Observation != "MARIE SILIKI")
            //     // {
            //     //     item.Paid_date = item.Create_date;
            //     //     _context.Update(item);
            //     //     await _context.SaveChangesAsync();
            //     // }

            //     // if (item.Observation == "MARIE SILIKI")
            //     // {
            //     //     item.Paid_date = DateTime.MinValue; ;
            //     //     _context.Update(item);
            //     //     await _context.SaveChangesAsync();
            //     // }
                
            // }

            return Json("ok");

        }

        public static string Truncate(string s, int length)
        {
            if (s.Length > length)
            {
                return s.Substring(0, length);
            }
            return s;
        }


        private bool HonoraireExists(int id)
        {
            return _context.Honoraire.Any(e => e.Id == id);
        }
        private bool MedecinExists(string nom)
        {
            return _context_doctor.Doctor.Any(e => e.Nom == nom);
        }
        private bool HonoraireExists2(Honoraire mon_honoraire)
        {
            return _context.Honoraire.Where(x => x.Etat == "Cloturé").Any(e => e.Patient == mon_honoraire.Patient && e.Nom.Replace(" ", "").Trim() == mon_honoraire.Nom.Replace(" ", "").Trim() && e.Create_date.Year == mon_honoraire.Create_date.Year && e.Create_date.Month == mon_honoraire.Create_date.Month && e.Create_date.Day == mon_honoraire.Create_date.Day && e.Medecin == mon_honoraire.Medecin);
        }

        private bool HonoraireExists3(Honoraire mon_honoraire)
        {
            return _context.Honoraire.Where(x => x.Etat == "Cloturé").Any(e => e.Patient == mon_honoraire.Patient && e.Nom.Replace(" ", "").Trim() == mon_honoraire.Nom.Replace(" ", "").Trim() && e.Create_date.Year == mon_honoraire.Create_date.Year && e.Create_date.Month == mon_honoraire.Create_date.Month && e.Create_date.Day == mon_honoraire.Create_date.Day && e.Medecin == mon_honoraire.Medecin);
        }

        private int HonoraireCount(Honoraire mon_honoraire)
        {
            return _context.Honoraire.Where(x => x.Etat == "Cloturé").Count(e => e.Nom.Replace(" ", "").Trim() == mon_honoraire.Nom.Replace(" ", "").Trim() && e.Medecin.Replace(" ", "").Trim() == mon_honoraire.Medecin.Replace(" ", "").Trim() && e.Numerofacture==mon_honoraire.Numerofacture);
        }

         private int HonoraireCount2(Honoraire mon_honoraire)
        {
            return _context.Honoraire.Count(e => e.Patient == mon_honoraire.Patient && e.Nom.Replace(" ", "").Trim() == mon_honoraire.Nom.Replace(" ", "").Trim()  && e.Medecin.Replace(" ", "").Trim() == mon_honoraire.Medecin.Replace(" ", "").Trim() && e.Numerofacture==mon_honoraire.Numerofacture);
        }



        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Pdf([FromBody] ListObject parametre)
        {
            PdfGeneration7 x = new(_context,_context_doctor);
            return new FileStreamResult(await x.HonorairePdf(parametre.debut,parametre.fin,parametre.docta,parametre.Secretaire), "application/pdf");
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

        public void EnvoiEtat(DateTime datedebut, DateTime datefin, List<string> docta, List<Honoraire> hono1, List<Honoraire> hono2, List<Honoraire> hono3)
        {



            foreach (var item0 in docta)
            {


                using (MemoryStream fs = new MemoryStream())
                {
                    var doc1 = new Document(PageSize.A4, 1f, 1f, 1f, 1f);
                    PdfWriter writer = PdfWriter.GetInstance(doc1, fs);


                    doc1.Open();
                    PdfContentByte pcb = writer.DirectContent;


                    BaseFont bf0 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    BaseFont bf00 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);


                    Font font0 = new Font(bf0, 7, Font.NORMAL);
                    Font font00 = new Font(bf00, 7, Font.BOLD);

                    PdfPTable table0 = new PdfPTable(1);
                    table0.WidthPercentage = 99f;
                    //table0.TotalWidth = 300f;

                    PdfPCell charg = new PdfPCell(new Phrase("Rapport des Honoraires Du " + datedebut + "au " + datefin, font00))
                    {
                        BorderWidth = 0.5f,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table0.AddCell(charg);
                    doc1.Add(table0);

                    PdfPTable table1 = new PdfPTable(9);
                    table1.WidthPercentage = 99f;
                    int[] intTable1Width = { 10, 10, 20, 20, 10, 10, 10, 10, 20 };
                    table1.SetWidths(intTable1Width);

                    charg = new PdfPCell(new Phrase("Date", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase("Nºfacure", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase("Patient", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase("Prestation", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase("Montant", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase("Taux", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase("Net a payer", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase("Etat", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase("assurance", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    int somme1 = 0;
                    int somme2 = 0;
                    int somme11 = 0;
                    int somme22 = 0;
                    int somme33 = 0;
                    foreach (var item in hono1)
                    {
                        if (item.Medecin.Replace(" ", "") == item0.Replace(" ", ""))
                        {
                            charg = new PdfPCell(new Phrase(item.Create_date.ToShortDateString(), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(item.Numerofacture, font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(item.Patient, font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(item.Nom, font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(separateur_de_millier(item.Montant.ToString()), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(Math.Round((decimal)item.Pourcentage, 2) + "%", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(separateur_de_millier(item.Montant_medecin.ToString()), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase("EN ATTENTE", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(item.Assurance, font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                            if (item.Montant == null) { item.Montant = 0; somme1 += (int)item.Montant; } else { somme1 += (int)item.Montant; }
                            if (item.Montant_medecin == null) { item.Montant_medecin = 0; somme2 += (int)item.Montant_medecin; } else { somme2 += (int)item.Montant_medecin; }
                        }

                    }

                    if (somme1 != 0)
                    {
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase("Total", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(separateur_de_millier(somme1.ToString()), font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(separateur_de_millier(somme2.ToString()), font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    }

                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);


                    somme11 += somme1; somme1 = 0; somme22 += somme2; somme2 = 0;
                    foreach (var item in hono2)
                    {
                        if (item.Medecin.Replace(" ", "") == item0.Replace(" ", ""))
                        {
                            charg = new PdfPCell(new Phrase(item.Create_date.ToShortDateString(), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(item.Numerofacture, font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(item.Patient, font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(item.Nom, font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(separateur_de_millier(item.Montant.ToString()), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(Math.Round((decimal)item.Pourcentage, 2) + "%", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(separateur_de_millier(item.Montant_medecin.ToString()), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase("PAYÉ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(item.Assurance, font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                            if (item.Montant == null) { item.Montant = 0; somme1 += (int)item.Montant; } else { somme1 += (int)item.Montant; }
                            if (item.Montant_medecin == null) { item.Montant_medecin = 0; somme2 += (int)item.Montant_medecin; } else { somme2 += (int)item.Montant_medecin; }
                        }
                    }

                    if (somme1 != 0)
                    {
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase("Total", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(separateur_de_millier(somme1.ToString()), font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(separateur_de_millier(somme2.ToString()), font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    }

                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);




                    somme33 += somme2; somme11 += somme1; somme1 = 0; somme22 += somme2; somme2 = 0;
                    foreach (var item in hono3)
                    {
                        if (item.Medecin.Replace(" ", "") == item0.Replace(" ", ""))
                        {
                            charg = new PdfPCell(new Phrase(item.Create_date.ToShortDateString(), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(item.Numerofacture, font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(item.Patient, font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(item.Nom, font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(separateur_de_millier(item.Montant.ToString()), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(Math.Round((decimal)item.Pourcentage, 2) + "%", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(separateur_de_millier(item.Montant_medecin.ToString()), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase("CLOTURÉ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                            charg = new PdfPCell(new Phrase(item.Assurance, font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                            if (item.Montant == null) { item.Montant = 0; somme1 += (int)item.Montant; } else { somme1 += (int)item.Montant; }
                            if (item.Montant_medecin == null) { item.Montant_medecin = 0; somme2 += (int)item.Montant_medecin; } else { somme2 += (int)item.Montant_medecin; }
                        }
                    }

                    if (somme1 != 0)
                    {
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase("Total", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(separateur_de_millier(somme1.ToString()), font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(separateur_de_millier(somme2.ToString()), font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                        charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    }
                    somme33 += somme2; somme11 += somme1; somme1 = 0; somme22 += somme2; somme2 = 0;

                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(" ", font0)) { BorderWidth = 0f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);

                    charg = new PdfPCell(new Phrase("MÉDECIN", font00)) { Colspan = 5, BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase("MONTANT ACTE", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_CENTER }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase("MONTANT DÛ ", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase("MONTANT DISPONIBLE", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase("SOLDE", font00)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);

                    charg = new PdfPCell(new Phrase(item0, font0)) { Colspan = 5, BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_LEFT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(separateur_de_millier(somme11.ToString()), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(separateur_de_millier(somme22.ToString()), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(separateur_de_millier(somme33.ToString()), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);
                    charg = new PdfPCell(new Phrase(separateur_de_millier((somme22 - somme33).ToString()), font0)) { BorderWidth = 0.5f, HorizontalAlignment = Element.ALIGN_RIGHT }; table1.AddCell(charg);


                    var qrcode = new BarcodeQRCode("Montant acte:" + somme11 + "Montant a payer" + somme22 + "montant payé" + somme33, 100, 100, null);
                    var image = qrcode.GetImage();
                    var mask = qrcode.GetImage();
                    mask.MakeMask();
                    image.ImageMask = mask; // making the background color transparent

                    var pdfCell = new PdfPCell(image, fit: true);
                    pdfCell.BorderWidth = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;

                    PdfPTable table2 = new PdfPTable(1);
                    table2.DefaultCell.Border = Rectangle.NO_BORDER;
                    table2.WidthPercentage = 15f;
                    table2.TotalWidth = 100f;
                    table2.AddCell(pdfCell);



                    doc1.Add(new Paragraph(" "));
                    doc1.Add(table1);
                    doc1.Add(table2);
                    doc1.Add(new Paragraph("généré automatiquement par NOLAN HOSPITAL le " + DateTime.Now + " par " + Userconnected().nom, font0));
                    doc1.Close();

                    var pdf = Convert.ToBase64String(fs.ToArray());

                    string teldoc = "694217600";
                    var medecin = _context_doctor.Doctor.Where(x => x.Nom == item0).FirstOrDefault();
                    if (medecin != null) { teldoc = medecin.Telephone; }

                    //whatsap
                    Whatsap3 whatsap = new Whatsap3(datedebut, datefin, item0, teldoc, pdf);
                }



            }

        }

        private string separateur_de_millier(string valeuraseparer)
        {
            int i = 0;
            string resultat = "";
            var numbers = new List<string>();
            while (valeuraseparer.Length - i >= 0)
            {
                if (i != 0)
                {


                    numbers.Add(valeuraseparer.Substring(valeuraseparer.Length - i, 3));

                }
                i = i + 3;
            }
            foreach (var item in numbers)
            {
                resultat = item + " " + resultat;
            }
            if (valeuraseparer.Length % 3 != 0)
            {
                resultat = valeuraseparer.Substring(0, valeuraseparer.Length % 3) + " " + resultat;
            }


            return resultat;
        }
        

        // private  <string> separateur_de_millier(string valeuraseparer)
        // {
            
        // }


    }

    

}

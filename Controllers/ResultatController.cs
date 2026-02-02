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
using BANA.MyFunction;
using BANA.PDF;
using System.Text.RegularExpressions;
using NuGet.Protocol;

namespace BANA.Controllers
{
   
    public class ResultatController : Controller
    {
        private readonly ResultatContext _context;
        private readonly UserContext _context_user;
        private readonly PatientContext _context_patient;
        private readonly ExamenContext _context_exam;
        private readonly FactureContext _context_facture;
        private readonly DoctorContext _context_doctor;

        public ResultatController(ResultatContext context, UserContext context_user, ExamenContext context_exam, FactureContext context_facture, PatientContext context_patient, DoctorContext context_doctor)
        {
            _context = context;
            _context_user = context_user;
            _context_exam = context_exam;
            _context_facture = context_facture;
            _context_patient = context_patient;
            _context_doctor = context_doctor;
        }

        // GET: Resultat
         [Authorize]
        public async Task<IActionResult> Index(string tipe, string date, string etat,string target)
        {
            ViewData["Ex0"] = "active";
            ViewBag.user = Userconnected();
            ViewData["tipe"] = tipe;
            var resultats = await _context.Resultat.ToListAsync();
            if (target!=null)
            {
                resultats = await _context.Resultat.Where(x => x.NumeroDossier == target).OrderByDescending(x => x.Id).ToListAsync();
            }
            if (tipe!=null)
            {
                resultats = await _context.Resultat.Where(x => x.Categorie == tipe).OrderByDescending(x => x.Id).ToListAsync();
            }
            if (date != null)
            {
                resultats = resultats.Where(x => x.Create_date >= DateTime.Parse(date) && x.Create_date <= DateTime.Parse(date).AddDays(1).AddSeconds(-1)).ToList();
            }

            if (etat != null)
            {
                resultats = resultats.Where(x => x.valide_par == null && x.fait_par != null).ToList();
            }
            // Lab labo = new();
            // labo.Main();

            return View(resultats);
        }

        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> IndexDmiLabo(string target)
        {
            ViewData["Ex0"] = "active";
            ViewBag.user = Userconnected();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == target).FirstOrDefault();
            var resultats = await _context.Resultat.Where(x => x.NumeroDossier == target && x.Categorie == "Analyse Médicale" && x.valide_par != null).OrderByDescending(x => x.Id).ToListAsync();
            return View(resultats);
        }

        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> IndexDmiAnapath(string target)
        {
            ViewData["Ex0"] = "active";
            ViewBag.user = Userconnected();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == target).FirstOrDefault();
            var resultats = await _context.Resultat.Where(x => x.NumeroDossier == target && x.Categorie == "Anapath" && x.valide_par != null).OrderByDescending(x => x.Id).ToListAsync();
            return View(resultats);
        }

        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> IndexDmiOphta(string target)
        {
            ViewData["Ex0"] = "active";
            ViewBag.user = Userconnected();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == target).FirstOrDefault();
            var resultats = await _context.Resultat.Where(x => x.NumeroDossier == target && x.Categorie == "Ophtalmologie" && x.valide_par != null).OrderByDescending(x => x.Id).ToListAsync();
            return View(resultats);
        }

        

        

         [Authorize]
        public async Task<IActionResult> Labo(string SearchString, bool notUsed)
        {
            ViewData["Ex0"] = "active";
            ViewData["Ex01"] = "active";
            ViewData["6"] = "active";
            if (SearchString != null)
            {
                if (SearchString[..2] == "00") { } else { SearchString = "00" + SearchString; }
            }

            ViewBag.user = Userconnected();
            var resultats = await _context.Resultat.Where(x => x.Categorie == "Analyse Médicale").OrderByDescending(x => x.Id).Take(100).ToListAsync();
            if (String.IsNullOrEmpty(SearchString))
            {
                return View(resultats.Take(100));
            }
            resultats = await _context.Resultat.Where(x => x.NumeroFacture == SearchString).OrderByDescending(x => x.Id).Take(100).ToListAsync();
            return View(resultats.Take(100));
        }

         [Authorize]
        public async Task<IActionResult> Anapath(string SearchString, bool notUsed)
        {
            ViewData["Ex0"] = "active";
            ViewData["Ex01"] = "active";
            ViewData["66"] = "active";
            if (SearchString != null)
            {
                if (SearchString[..2] == "00") { } else { SearchString = "00" + SearchString; }
            }

            ViewBag.user = Userconnected();
            var resultats = await _context.Resultat.Where(x => x.Categorie == "Anapath").OrderByDescending(x => x.Id).Take(100).ToListAsync();
            if (String.IsNullOrEmpty(SearchString))
            {
                return View(resultats.Take(100));
            }
            resultats = await _context.Resultat.Where(x => x.NumeroFacture == SearchString).OrderByDescending(x => x.Id).Take(100).ToListAsync();
            return View(resultats.Take(100));
        }

         [Authorize]
        public async Task<IActionResult> Ophtalmologie(string SearchString, bool notUsed)
        {
            ViewData["666"] = "active";
            if (SearchString != null)
            {
                if (SearchString[..2] == "00") { } else { SearchString = "00" + SearchString; }
            }

            ViewBag.user = Userconnected();
            var resultats = await _context.Resultat.Where(x => x.Categorie == "Ophtalmologie").OrderByDescending(x => x.Id).Take(100).ToListAsync();
            if (String.IsNullOrEmpty(SearchString))
            {
                return View(resultats.Take(100));
            }
            resultats = await _context.Resultat.Where(x => x.NumeroFacture == SearchString).OrderByDescending(x => x.Id).Take(100).ToListAsync();
            return View(resultats.Take(100));
        }

         [Authorize]
        public async Task<IActionResult> Exploration(string SearchString, bool notUsed)
        {
            ViewData["666"] = "active";
            if (SearchString != null)
            {
                if (SearchString[..2] == "00") { } else { SearchString = "00" + SearchString; }
            }

            ViewBag.user = Userconnected();
            var resultats = await _context.Resultat.Where(x => x.Categorie == "Exploration Fonctionnelle").OrderByDescending(x => x.Id).Take(100).ToListAsync();
            if (String.IsNullOrEmpty(SearchString))
            {
                return View(resultats.Take(100));
            }
            resultats = await _context.Resultat.Where(x => x.NumeroFacture == SearchString).OrderByDescending(x => x.Id).Take(100).ToListAsync();
            return View(resultats.Take(100));
        }

        
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> IndexDmiImagerie(string target)
        {
            ViewData["666"] = "active";
           

            ViewBag.user = Userconnected();
            var resultats = await _context.Resultat.Where(x =>  x.NumeroDossier==target && x.Categorie == "Imagerie Médicale").OrderByDescending(x => x.Id).Take(100).ToListAsync();
            ViewBag.patient = _context_patient.Patient.Where(x => x.Numero_dossier == target).FirstOrDefault();
            return View(resultats.Take(100));
        }

        
        
        
         [Authorize]
        public async Task<IActionResult> Validation(string SearchString, bool notUsed)
        {
            ViewData["Ex0"] = "active";
            ViewData["Ex01"] = "active";
            ViewData["6"] = "active";
            if (SearchString != null)
            {
                if (SearchString[..2] == "00") { } else { SearchString = "00" + SearchString; }
            }

            ViewBag.user = Userconnected();

            var resultats = await _context.Resultat.Where(x => x.Categorie == "Analyse Médicale").OrderByDescending(x => x.Id).Take(100).ToListAsync();
            if (String.IsNullOrEmpty(SearchString))
            {
                return View(resultats.Take(100));
            }
            resultats = await _context.Resultat.Where(x => x.NumeroFacture == SearchString).OrderByDescending(x => x.Id).Take(100).ToListAsync();
            return View(resultats.Take(100));
        }

         [Authorize]
        public async Task<IActionResult> Imagerie(string SearchString, bool notUsed)
        {
            ViewData["Ex0"] = "active";
            ViewData["Ex111"] = "active";
            ViewData["06"] = "active";
            if (SearchString != null)
            {
                if (SearchString[..2] == "00") { } else { SearchString = "00" + SearchString; }
            }

            ViewBag.user = Userconnected();
            var resultats = await _context.Resultat.Where(x => x.Categorie == "Imagerie Médicale").OrderByDescending(x => x.Id).Take(100).ToListAsync();
            if (String.IsNullOrEmpty(SearchString))
            {
                return View(resultats.Take(100));
            }
            resultats = await _context.Resultat.Where(x => x.NumeroFacture == SearchString).OrderByDescending(x => x.Id).Take(100).ToListAsync();
            return View(resultats.Take(100));
        }

         [Authorize]
        public async Task<IActionResult> ImpressionLabo(string SearchString, bool notUsed)
        {
            ViewData["Ex0"] = "active";
            ViewData["Ex001"] = "active";
            ViewData["6"] = "active";
            if (SearchString != null)
            {
                if (SearchString[..2] == "00") { } else { SearchString = "00" + SearchString; }
            }
            ViewBag.user = Userconnected();
            var resultats = await _context.Resultat.Where(x => x.Categorie == "Analyse Médicale").OrderByDescending(x => x.Create_date).ToListAsync();
            if (String.IsNullOrEmpty(SearchString))
            {
                return View(resultats.Take(100));
            }
            resultats = await _context.Resultat.Where(x => x.NumeroFacture.ToLower() == SearchString.ToLower()).OrderByDescending(x => x.Create_date).ToListAsync();
            return View(resultats.Take(100));
        }
         [Authorize]
        public async Task<IActionResult> Index0(string tipe)
        {
            ViewData["Ex0"] = "active";
            ViewData["6"] = "active";
            ViewBag.user = Userconnected();
            ViewData["tipe"] = tipe;
            List<string> datas = new List<string>();
            var resultats = await _context.Resultat.Where(x => x.Categorie == tipe).OrderByDescending(x => x.Id).ToListAsync();
            datas.AddRange(resultats.Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct();
            return View(resultats);
        }

         [Authorize]
        public async Task<IActionResult> Index0_0(string tipe)
        {
            ViewData["Ex0"] = "active";
            ViewData["06"] = "active";
            ViewBag.user = Userconnected();
            ViewData["tipe"] = tipe;
            List<string> datas = new List<string>();
            var resultats = await _context.Resultat.Where(x => x.Categorie == tipe).OrderByDescending(x => x.Id).ToListAsync();
            datas.AddRange(resultats.Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct();
            return View(resultats);
        }
        
         [Authorize]
         public async Task<IActionResult> Index0_1(string tipe)
        {
            ViewData["66"] = "active";
            ViewBag.user = Userconnected();
            ViewData["tipe"] = tipe;
            List<string> datas = new List<string>();
            var resultats = await _context.Resultat.Where(x => x.Categorie == tipe).OrderByDescending(x => x.Id).ToListAsync();
            datas.AddRange(resultats.Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct();
            return View(resultats);
        }

         [Authorize]
        public async Task<IActionResult> Index0_2(string tipe)
        {
            ViewData["666"] = "active";
            ViewBag.user = Userconnected();
            ViewData["tipe"] = tipe;
            List<string> datas = new List<string>();
            var resultats = await _context.Resultat.Where(x => x.Categorie == tipe).OrderByDescending(x => x.Id).ToListAsync();
            datas.AddRange(resultats.Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct();
            return View(resultats);
        }

        [Authorize]
        public async Task<IActionResult> Index0_3(string tipe)
        {
            ViewData["666"] = "active";
            ViewBag.user = Userconnected();
            ViewData["tipe"] = tipe;
            List<string> datas = new List<string>();
            var resultats = await _context.Resultat.Where(x => x.Categorie == tipe).OrderByDescending(x => x.Id).ToListAsync();
            datas.AddRange(resultats.Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct();
            return View(resultats);
        }

       
        [Authorize]
        // GET: Resultat/Details/5
        public async Task<IActionResult> Stoneviewer(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var resultat = await _context.Resultat
                .FirstOrDefaultAsync(m => m.Id == id);
           
            
            if (resultat == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

          
            ViewData["qrcode"] = "ok";
            ViewData["Ex0"] = "active";
            if (resultat.Code == null) { resultat.Code = ""; }
            if (resultat.Cote == null) { resultat.Cote = ""; }
            
            
            ViewBag.user = Userconnected();
            

            ViewData["6"] = "active";
            return View(resultat);
        }
        
        
        
         [Authorize]
        public async Task<IActionResult> Validation0(string tipe)
        {
            ViewData["Ex0"] = "active";
            ViewBag.user = Userconnected();
            ViewData["tipe"] = tipe;
            List<string> datas = new List<string>();
            var resultats = await _context.Resultat.Where(x => x.Categorie == tipe && x.valide_par == null && x.fait_par != null).Take(1000).OrderByDescending(x => x.Create_date).ToListAsync();
            datas.AddRange(resultats.Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct();
            return View(resultats);
        }

         [Authorize]
        public async Task<IActionResult> Validation00(string tipe)
        {
            ViewData["06"] = "active";
            ViewBag.user = Userconnected();
            ViewData["tipe"] = tipe;
            List<string> datas = new List<string>();
            var resultats = await _context.Resultat.Where(x => x.Categorie == tipe && x.valide_par == null && x.fait_par != null).Take(1000).OrderByDescending(x => x.Create_date).ToListAsync();
            datas.AddRange(resultats.Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct();
            return View(resultats);
        }

         [Authorize]
         public async Task<IActionResult> Validation0_1(string tipe)
        {
            ViewData["66"] = "active";
            ViewBag.user = Userconnected();
            ViewData["tipe"] = tipe;
            List<string> datas = new List<string>();
            var resultats = await _context.Resultat.Where(x => x.Categorie == tipe && x.valide_par == null && x.fait_par != null).Take(1000).OrderByDescending(x => x.Create_date).ToListAsync();
            datas.AddRange(resultats.Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct();
            return View(resultats);
        }

         [Authorize]
        public async Task<IActionResult> Validation0_2(string tipe)
        {
            ViewData["666"] = "active";
            ViewBag.user = Userconnected();
            ViewData["tipe"] = tipe;
            List<string> datas = new List<string>();
            var resultats = await _context.Resultat.Where(x => x.Categorie == tipe && x.valide_par == null && x.fait_par != null).Take(1000).OrderByDescending(x => x.Create_date).ToListAsync();
            datas.AddRange(resultats.Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct();
            return View(resultats);
        }

         [Authorize]
        public async Task<IActionResult> Validation0_3(string tipe)
        {
            ViewData["666"] = "active";
            ViewBag.user = Userconnected();
            ViewData["tipe"] = tipe;
            List<string> datas = new List<string>();
            var resultats = await _context.Resultat.Where(x => x.Categorie == tipe && x.valide_par == null && x.fait_par != null).Take(1000).OrderByDescending(x => x.Create_date).ToListAsync();
            datas.AddRange(resultats.Select(x => x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas = datas.Distinct();
            return View(resultats);
        }

        [Authorize]
        // GET: Resultat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var resultat = await _context.Resultat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resultat.Contenu == null) { resultat.Contenu = ""; }
            resultat.Contenu = Regex.Replace(resultat.Contenu, "<button.*?</button>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (resultat == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            ViewBag.autres = await _context.Resultat.Where(x => x.NumeroFacture == resultat.NumeroFacture && x.valide_par != null).ToListAsync();
            ViewData["qrcode"] = "ok";
            ViewData["Ex0"] = "active";
            if (resultat.Code == null) { resultat.Code = ""; }
            if (resultat.Cote == null) { resultat.Cote = ""; }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in resultat.Cote)
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
            foreach (char c in resultat.Code)
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
            ViewBag.user = Userconnected();
            if (resultat.Nom.Contains("NFS"))
            {
                resultat.Contenu = resultat.Contenu.Replace("green", "white");
                if (resultat.fait_par == null)
                {

                    var exams = await _context_exam.Examen.Where(x => x.Nom == resultat.Nom && x.Min != null && x.Max != null).ToListAsync();
                    resultat.Contenu = exams.Where(x => x.Min <= (DateTime.Now - resultat.Date_de_naissance).TotalDays && x.Max >= (DateTime.Now - resultat.Date_de_naissance).TotalDays).FirstOrDefault().Contenu;


                }
            }

            ViewData["6"] = "active";
            return View(resultat);
        }

        // GET: Resultat/Details/5
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Details2(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var resultat = await _context.Resultat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resultat.Contenu == null) { resultat.Contenu = ""; }
            resultat.Contenu = Regex.Replace(resultat.Contenu, "<button.*?</button>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (resultat == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            ViewBag.autres = await _context.Resultat.Where(x => x.NumeroFacture == resultat.NumeroFacture && x.valide_par != null).ToListAsync();
            ViewData["qrcode"] = "ok";
            ViewData["Ex0"] = "active";
            if (resultat.Code == null) { resultat.Code = ""; }
            if (resultat.Cote == null) { resultat.Cote = ""; }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in resultat.Cote)
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
            foreach (char c in resultat.Code)
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
            ViewBag.user = Userconnected();
            if (resultat.Nom.Contains("NFS"))
            {
                resultat.Contenu = resultat.Contenu.Replace("green", "white");
                if (resultat.fait_par == null)
                {

                    var exams = await _context_exam.Examen.Where(x => x.Nom == resultat.Nom && x.Min != null && x.Max != null).ToListAsync();
                    resultat.Contenu = exams.Where(x => x.Min <= (DateTime.Now - resultat.Date_de_naissance).TotalDays && x.Max >= (DateTime.Now - resultat.Date_de_naissance).TotalDays).FirstOrDefault().Contenu;


                }
            }

            ViewData["6"] = "active";
            return View(resultat);
        }


        // GET: Resultat/Details/5
         [Authorize]
        public async Task<IActionResult> Couverture(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }


            var resultat = await _context.Resultat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resultat == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            var facture = await _context_facture.Facture.FirstOrDefaultAsync(x => x.Numero_de_facture == resultat.NumeroFacture);
            ViewBag.resultats = _context.Resultat.Where(x => x.NumeroFacture == resultat.NumeroFacture).ToList();

            ViewData["qrcode"] = "ok";
            ViewData["Ex0"] = "active";
            if (resultat.Code == null) { resultat.Code = ""; }
            if (resultat.Cote == null) { resultat.Cote = ""; }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in resultat.Cote)
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


            ViewBag.Lignes2 = liste_de_facturation2;
            ViewBag.user = Userconnected();

            ViewData["6"] = "active";
            return View(resultat);
        }

        // GET: Resultat/Details/5
         [Authorize]
        public async Task<IActionResult> Detailsgroupe(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var resultat = await _context.Resultat
                .FirstOrDefaultAsync(m => m.Id == id);

            if (resultat == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            var resultats = await _context.Resultat.Where(x => x.NumeroFacture == resultat.NumeroFacture && x.Categorie == resultat.Categorie && x.Id != resultat.Id && x.valide_par != "" && x.valide_par != null).ToListAsync();
            List<Resultat> resu = new()
                {
                    resultat
                };
            resu.AddRange(resultats);
            ViewBag.resultats = resu;
            if (resultats.Count > 0)
            {
                foreach (var item in resultats)
                {
                    resultat.Contenu += "<br/>" + item.Contenu;
                    if (item.Code != "λANTIFONGIGRAMMEλ")
                    {
                        resultat.Code = item.Code;
                    }
                    if (item.Cote != "λANTIBIOGRAMMEλ")
                    {
                        resultat.Cote = item.Cote;
                    }


                }
                Console.WriteLine(resultat.Code);
                Console.WriteLine(resultat.Cote);
            }

            ViewData["qrcode"] = "ok";
            ViewData["Ex0"] = "active";
            if (resultat.Code == null) { resultat.Code = ""; }
            if (resultat.Cote == null) { resultat.Cote = ""; }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in resultat.Cote)
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
            foreach (char c in resultat.Code)
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
            ViewBag.user = Userconnected();
            if (resultat.Nom.Contains("NFS"))
            {
                resultat.Contenu = resultat.Contenu.Replace("green", "white");
            }
            ViewData["6"] = "active";
            return View(resultat);
        }

        // GET: Resultat/Details/5
         [Authorize]
        public async Task<IActionResult> Print(string fact, string chaine)
        {
            if (fact == null || chaine == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var resultats = await _context.Resultat.Where(x => x.NumeroFacture == fact && x.Categorie == "Analyse Médicale" && x.valide_par != "" && x.valide_par != null).ToListAsync();
            ViewBag.resultats = resultats;
            Resultat resultat = new() { };
            List<string> machaine = new();
            int i = 0;
            string result = "";
            foreach (char c in chaine)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        machaine.Add(result);
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
            int k = 0;
            foreach (var item in machaine)
            {
                if (k == 0)
                {
                    resultat = resultats.Where(x => x.Nom == item).FirstOrDefault();
                }
                else
                {
                    resultat.Contenu += "<br/>" + resultats.Where(x => x.Nom == item).FirstOrDefault().Contenu;
                    if (resultats.Where(x => x.Nom == item).FirstOrDefault().Code != "λANTIFONGIGRAMMEλ")
                    {
                        resultat.Code = resultats.Where(x => x.Nom == item).FirstOrDefault().Code;
                    }
                    if (resultats.Where(x => x.Nom == item).FirstOrDefault().Cote != "λANTIBIOGRAMMEλ")
                    {
                        resultat.Cote = resultats.Where(x => x.Nom == item).FirstOrDefault().Cote;
                    }
                }
                k++;

            }

            ViewData["qrcode"] = "ok";
            ViewData["Ex0"] = "active";
            if (resultat.Code == null) { resultat.Code = ""; }
            if (resultat.Cote == null) { resultat.Cote = ""; }
            List<string> liste_de_facturation1 = new List<string>();

            i = 0;
            result = "";
            foreach (char c in resultat.Cote)
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
            foreach (char c in resultat.Code)
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
            ViewBag.user = Userconnected();
            if (resultat.Nom.Contains("NFS"))
            {
                resultat.Contenu = resultat.Contenu.Replace("green", "white");
            }
            ViewData["6"] = "active";
            return View(resultat);
        }

        // GET: Resultat/Create
         [Authorize]
        public IActionResult Create()
        {
            ViewBag.user = Userconnected();
            ViewData["6"] = "active";
            return View();
        }

        // POST: Resultat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
         [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Nom,NumeroDossier,NomPatient,Contenu,Categorie,Cote,Code,ExamType,fait_par,valide_par,Create_date,Date_de_finalisation,Montant,resultat_final")] Resultat resultat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resultat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.user = Userconnected();
            ViewData["6"] = "active";
            return View(resultat);
        }

        // GET: Resultat/Edit/5
         [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var resultat = await _context.Resultat.FindAsync(id);
            if (resultat.valide_par != null)
            {
                if (Userconnected().nom != "DR NGOUNGOURE")
                {
                    return RedirectToAction("Page405", "Stock");
                }

            }
            var exam = await _context_exam.Examen.Where(x => x.Nom.Replace(" ", "") == resultat.Nom.Replace(" ", "")).FirstOrDefaultAsync();
            if (resultat == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            if (resultat.Nom.Contains("NFS") && resultat.fait_par == null)
            {

                var exams = await _context_exam.Examen.Where(x => x.Nom.Replace(" ", "") == resultat.Nom.Replace(" ", "") && x.Min != null && x.Max != null).ToListAsync();
                exam = exams.Where(x => x.Min <= (DateTime.Now - resultat.Date_de_naissance).TotalDays && x.Max >= (DateTime.Now - resultat.Date_de_naissance).TotalDays).FirstOrDefault();

            }
            ViewData["richtext"] = "ok";
            ViewData["js_antibio"] = "ok";
            ViewData["Ex0"] = "active";
            if (resultat.fait_par == null) { resultat.Contenu = exam.Contenu; }
            if (resultat.Code == null) { resultat.Code = ""; }
            if (resultat.Cote == null) { resultat.Cote = ""; }



            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result = "";
            foreach (char c in resultat.Cote)
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
            foreach (char c in resultat.Code)
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
            ViewBag.user = Userconnected();
            ViewData["6"] = "active";
            return View(resultat);
        }



        // GET: Resultat/Edit2/5
         [Authorize]
        public async Task<IActionResult> Edit2(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var resultat = await _context.Resultat.FindAsync(id);
            var resul = await _context.Resultat.Where(x => x.Prescripteur.Replace(" ", "") == resultat.Prescripteur.Replace(" ", "") && x.Nom.Replace(" ", "") == resultat.Nom.Replace(" ", "")).OrderBy(x => x.Id).LastOrDefaultAsync();
            if (resultat == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            ViewData["Ex0"] = "active";
            if (resultat.fait_par == null && resul != null)
            {
                resultat.Contenu = resul.Contenu;
            }
            if (resultat.Code == null) { resultat.Code = ""; }
            if (resultat.Cote == null) { resultat.Cote = ""; }


            ViewBag.user = Userconnected();
            ViewData["6"] = "active";
            return View(resultat);
        }


        // GET: Resultat/Edit2/5
         [Authorize]
        public async Task<IActionResult> Edit3(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var resultat = await _context.Resultat.FindAsync(id);
            var resul = await _context.Resultat.Where(x => x.Prescripteur.Replace(" ", "") == resultat.Prescripteur.Replace(" ", "") && x.Nom.Replace(" ", "") == resultat.Nom.Replace(" ", "")).OrderBy(x => x.Id).LastOrDefaultAsync();
            if (resultat == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            ViewData["Ex0"] = "active";
            if (resultat.fait_par == null && resul != null)
            {
                resultat.Contenu = resul.Contenu;
            }
            if (resultat.Code == null) { resultat.Code = ""; }
            if (resultat.Cote == null) { resultat.Cote = ""; }


            ViewBag.user = Userconnected();
            ViewData["6"] = "active";
            return View(resultat);
        }



        // POST: Resultat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
         [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,NumeroDossier,NomPatient,Contenu,Categorie,Cote,Code,ExamType,fait_par,valide_par,Create_date,Date_de_finalisation,Montant,resultat_final")] Resultat resultat)
        {
            if (id != resultat.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resultat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultatExists(resultat.Id))
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
            ViewData["richtext"] = "ok";
            ViewData["6"] = "active";
            return View(resultat);
        }

        // GET: Resultat/Delete/5
         [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var resultat = await _context.Resultat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resultat == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user = Userconnected();
            return View(resultat);
        }

        // POST: Resultat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
         [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resultat = await _context.Resultat.FindAsync(id);
            _context.Resultat.Remove(resultat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
         [Authorize]
        public async Task<object> Validation([FromBody] Resultat MyData)
        {

            var resultat = await _context.Resultat
                .FirstOrDefaultAsync(m => m.Id == MyData.Id);

            resultat.valide_par = Userconnected().nom;
            resultat.Date_de_finalisation = DateTime.Now;
            resultat.resultat_final = MyData.resultat_final;

            if (resultat.Categorie == "Analyse Médicale" && (Userconnected().nom != "DR NGOUNGOURE"))
            {
                if (resultat.Nom == "HP AG")
                {

                }
                else
                {
                    return Json("/stock/405");
                }
            }
            _context.Update(resultat);
            await _context.SaveChangesAsync();

            var resultat2 = await _context.Resultat.Where(x => x.NumeroFacture == resultat.NumeroFacture && x.valide_par == null).ToListAsync();

            PdfGeneration3 rs_patient = new(_context, _context_doctor);
            List<string> liste_examens = new();
            liste_examens.Add("xxxx");

            MemoryStream stream = await rs_patient.DetailsResultat(resultat.Id, liste_examens);
            var pdf = Convert.ToBase64String(stream.ToArray());

            //envoie resultat whatsap
            if (_context_patient.Patient.Where(x => x.Numero_dossier == resultat.NumeroDossier).FirstOrDefault().Taille=="oui")
            {
                Whatsap4 envoi_whatsap = new(resultat.NomPatient, resultat.Nom, resultat.Genre, resultat.Telephone_patient, pdf);
            }

            return Json("ok");

        }

        [HttpPost]
         [Authorize]
        public async Task<object> Add([FromBody] Resultat MyData)
        {
            var resultat = await _context.Resultat
                 .FirstOrDefaultAsync(m => m.Id == MyData.Id);
            resultat.Contenu = MyData.Contenu;
            resultat.fait_par = Userconnected().nom;
            resultat.Cote = MyData.Cote;
            resultat.Code = MyData.Code;

            _context.Update(resultat);
            await _context.SaveChangesAsync();
            return Json("ok");

        }

        [HttpPost]
         [Authorize]
        public async Task<object> Actualiser([FromBody] Resultat MyData)
        {
            var resultat = await _context.Resultat
                 .FirstOrDefaultAsync(m => m.Id == MyData.Id);
            var patient = await _context_patient.Patient.FirstOrDefaultAsync(m => m.Numero_dossier == resultat.NumeroDossier);
            var tous_ses_resultats = await _context.Resultat.Where(x => x.NumeroFacture == resultat.NumeroFacture).ToListAsync();
            foreach (var item in tous_ses_resultats)
            {
                item.NomPatient = patient.Nom + " " + patient.Prenom;
                item.Date_de_naissance = patient.DateNaissance;
                item.Telephone_patient = patient.Phone;
                item.Genre = patient.Genre;
                _context.Update(resultat);
                await _context.SaveChangesAsync();
            }
            return Json("ok");

        }




        private bool ResultatExists(int id)
        {
            return _context.Resultat.Any(e => e.Id == id);
        }




        public User Userconnected()
        {
            
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

        public async Task<IActionResult> Pdf(int id, List<string> target)
        {
            PdfGeneration3 x = new(_context,_context_doctor);
            return new FileStreamResult(await x.DetailsResultat(id, target), "application/pdf");
        }
        
        public  async Task<IActionResult>  CouverturePdf(int id,string target)
        { 
            PdfGeneration6 x=new(_context);
            return  new FileStreamResult(await x.CouvertureResultat(id,target), "application/pdf"); 
        }
    }
}

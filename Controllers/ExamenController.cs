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
using BANA.PDF;
using System.Text.RegularExpressions;

namespace BANA.Controllers
{
    [Authorize]
    public class ExamenController : Controller
    {
        private readonly ExamenContext _context;
        private readonly UserContext _context_user;

        public ExamenController(ExamenContext context,UserContext context_user)
        {
            _context = context;
            _context_user=context_user;
        }

        // GET: Examen
        public async Task<IActionResult> Index()
        {
            ViewData["Ex0"]="active";
            ViewData["Ex1"]="active";
            ViewData["6"] = "active";
            ViewBag.user=Userconnected();
            return View(await _context.Examen.ToListAsync());
        }
        public async Task<IActionResult> Index2(string tipe)
        {
            if (tipe == null)
            {
                 return RedirectToAction("Page404", "Stock");
            }
            ViewData["cat"]=tipe;
            ViewData["6"] = "active";
            ViewData["Analyse Médicale"]=_context.Examen.Where(x=>x.Categorie=="Analyse Médicale").Count();
            ViewData["Imagerie Médicale"]=_context.Examen.Where(x=>x.Categorie=="Imagerie Médicale").Count();
            ViewData["Table des réactifs"]=_context.Examen.Where(x=>x.Categorie=="Table des réactifs").Count();
            ViewBag.user=Userconnected();
            return View(await _context.Examen.Where(x=>x.Categorie==tipe).ToListAsync());
        }
        public async Task<IActionResult> Index0_2(string tipe)
        {
            if (tipe == null)
            {
                 return RedirectToAction("Page404", "Stock");
            }
            ViewData["cat"]=tipe;
            ViewData["06"] = "active";
            ViewData["Analyse Médicale"]=_context.Examen.Where(x=>x.Categorie=="Analyse Médicale").Count();
            ViewData["Imagerie Médicale"]=_context.Examen.Where(x=>x.Categorie=="Imagerie Médicale").Count();
            ViewData["Table des réactifs"]=_context.Examen.Where(x=>x.Categorie=="Table des réactifs").Count();
            ViewBag.user=Userconnected();
            return View(await _context.Examen.Where(x=>x.Categorie==tipe).ToListAsync());
        }
        
        public async Task<IActionResult> Index2_1(string tipe)
        {
            if (tipe == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["cat"] = tipe;
            ViewData["66"] = "active";
            ViewData["Analyse Médicale"] = _context.Examen.Where(x => x.Categorie == "Analyse Médicale").Count();
            ViewData["Imagerie Médicale"] = _context.Examen.Where(x => x.Categorie == "Imagerie Médicale").Count();
            ViewData["Table des réactifs"] = _context.Examen.Where(x => x.Categorie == "Table des réactifs").Count();
            ViewBag.user = Userconnected();
            return View(await _context.Examen.Where(x => x.Categorie == tipe).ToListAsync());
        }
        
        public async Task<IActionResult> Liste(string tipe)
        {
            if (tipe == null)
            {
                 return RedirectToAction("Page404", "Stock");
            }
            ViewData["cat"]=tipe;
            ViewData["table"] = "ok";
            ViewBag.user=Userconnected();
            return View(await _context.Examen.Where(x=>x.Categorie==tipe).ToListAsync());
        }

        public async Task<IActionResult> Index2_2(string tipe)
        {
            if (tipe == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["cat"] = tipe;
            ViewData["666"] = "active";
            ViewData["Analyse Médicale"] = _context.Examen.Where(x => x.Categorie == "Analyse Médicale").Count();
            ViewData["Imagerie Médicale"] = _context.Examen.Where(x => x.Categorie == "Imagerie Médicale").Count();
            ViewData["Table des réactifs"] = _context.Examen.Where(x => x.Categorie == "Table des réactifs").Count();
            ViewBag.user = Userconnected();
            return View(await _context.Examen.Where(x => x.Categorie == tipe).ToListAsync());
        }

        public async Task<IActionResult> Index2_3(string tipe)
        {
            if (tipe == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["cat"] = tipe;
            ViewData["6666"] = "active";
            ViewData["Analyse Médicale"] = _context.Examen.Where(x => x.Categorie == "Analyse Médicale").Count();
            ViewData["Imagerie Médicale"] = _context.Examen.Where(x => x.Categorie == "Imagerie Médicale").Count();
            ViewData["Table des réactifs"] = _context.Examen.Where(x => x.Categorie == "Table des réactifs").Count();
            ViewBag.user = Userconnected();
            return View(await _context.Examen.Where(x => x.Categorie == tipe).ToListAsync());
        }

        // GET: Examen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }


            var examen = await _context.Examen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examen.Contenu == null) { examen.Contenu = ""; }
            examen.Contenu = Regex.Replace(examen.Contenu, "<button.*?</button>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (examen == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user = Userconnected();
            if (examen.Nom.Contains("NFS"))
            {
                examen.Contenu = examen.Contenu.Replace("green", "white");
            }
            ViewData["6"] = "active";
            return View(examen);
        }

        // GET: Examen/Create
        public IActionResult Create(string tipe)
        {
            if (Userconnected().Type_de_compte!="administrateur" && Userconnected().Type_de_compte!="super-Admin")
            {
                return RedirectToAction("Page405","Stock");
            }
            ViewData["richtext"]="ok";
            //ViewData["6"] = "active";
            ViewData["tipe"]=tipe;
            ViewBag.Type= _context.Examen.Where(x=>x.Categorie==tipe).Select(x=>x.ExamType).ToList().Distinct();
            ViewBag.user=Userconnected();
            return View();
        }

        

        // GET: Examen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            if (Userconnected().Type_de_compte!="administrateur" && Userconnected().Type_de_compte!="super-Admin")
            {
                return RedirectToAction("Page405","Stock");
            }

            var examen = await _context.Examen.FindAsync(id);
            if (examen == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["richtext"]="ok";
            //ViewData["6"] = "active";
            ViewBag.user=Userconnected();
            ViewBag.Type=_context.Examen.Select(x=>x.ExamType).ToList().Distinct();
            ViewBag.tipe=_context.Examen.Select(x=>x.Categorie).ToList().Distinct();
            return View(examen);
        }


         // GET: Examen/Edit/5
        public async Task<IActionResult> Edit2(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            if (Userconnected().Type_de_compte!="administrateur" && Userconnected().Type_de_compte!="super-Admin")
            {
                return RedirectToAction("Page405","Stock");
            }

            var examen = await _context.Examen.FindAsync(id);
            if (examen == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["richtext"]="ok";
            //ViewData["6"] = "active";
            ViewBag.user=Userconnected();
            ViewBag.Type=_context.Examen.Select(x=>x.ExamType).ToList().Distinct();
            ViewBag.tipe=_context.Examen.Select(x=>x.Categorie).ToList().Distinct();
            return View(examen);
        }

        // GET: Examen/Edit/5
        public async Task<IActionResult> Edit3(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            if (Userconnected().Type_de_compte!="administrateur" && Userconnected().Type_de_compte!="super-Admin")
            {
                return RedirectToAction("Page405","Stock");
            }

            var examen = await _context.Examen.FindAsync(id);
            if (examen == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["richtext"]="ok";
            //ViewData["6"] = "active";
            ViewBag.user=Userconnected();
            ViewBag.Type=_context.Examen.Select(x=>x.ExamType).ToList().Distinct();
            ViewBag.tipe=_context.Examen.Select(x=>x.Categorie).ToList().Distinct();
            return View(examen);
        }

        // GET: Examen/Edit/5
        public async Task<IActionResult> Edit0(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            if (Userconnected().Type_de_compte!="administrateur" && Userconnected().Type_de_compte!="super-Admin")
            {
                return RedirectToAction("Page405","Stock");
            }

            var examen = await _context.Examen.FindAsync(id);
            if (examen == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["richtext"]="ok";
            ViewData["6"] = "active";
            ViewBag.user=Userconnected();
            ViewBag.Type=_context.Examen.Select(x=>x.ExamType).ToList().Distinct();
            return View(examen);
        }

        // POST: Examen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Contenu,Create_date,Montant")] Examen examen)
        {
            if (id != examen.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamenExists(examen.Id))
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
            ViewData["richtext"]="ok";
            ViewData["6"] = "active";
            ViewBag.user=Userconnected();
            return View(examen);
        }

        // GET: Examen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var examen = await _context.Examen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examen == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user=Userconnected();
            return View(examen);
        }

        [HttpPost]
        //[Authorize]
        public async Task<object> Add([FromBody] Examen MyData)
        {   
           
            if(MyData.Nom==null || MyData.Categorie==null){
                return Json("error"); 
            }
            if(MyData.Id==0){
                MyData.Create_date=DateTime.Now;
                _context.Add(MyData);
            }else
            {
                var examen = await _context.Examen
                .FirstOrDefaultAsync(m => m.Id == MyData.Id);
                
                if(MyData.Contenu!="λλλλ"){
                     examen.Contenu=MyData.Contenu;
                }else{
                    examen.Nom=MyData.Nom;
                    examen.Montant=MyData.Montant;
                    examen.Categorie=MyData.Categorie;
                    examen.Code=MyData.Code;
                    examen.Cote=MyData.Cote;
                    examen.ExamType=MyData.ExamType;
                    examen.Min=MyData.Min;
                    examen.Max=MyData.Max;
                }
                _context.Update(examen);
            }
            
            await _context.SaveChangesAsync();
             return Json("ok"); 
            
        }

        [HttpPost]
        //[Authorize]
        public async Task<object> GetPrice([FromBody] Examen MyExam)
        {   
            
            var monexamen= await _context.Examen.Where(x => x.Nom==MyExam.Nom).FirstOrDefaultAsync();
            return Json(monexamen); 
            
        }

        // POST: Examen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examen = await _context.Examen.FindAsync(id);
            _context.Examen.Remove(examen);
            await _context.SaveChangesAsync();
            return RedirectToAction("Liste",new{tipe=examen.Categorie});
        }

        private bool ExamenNameExists(string nom)
        {
            return _context.Examen.Any(e => e.Nom == nom);
        }

        private bool ExamenExists(int id)
        {
            return _context.Examen.Any(e => e.Id == id);
        }
        
        public async Task<IActionResult> Pdf(int id, List<string> target)
        {
            PdfGeneration03 x = new(_context);
            return new FileStreamResult(await x.DetailsExamen(id), "application/pdf");
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
    }
}

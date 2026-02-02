#nullable disable
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BANA.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using BANA.MyFunction;

namespace BANA.Controllers
{
    
    [Authorize]
    public class StockController : Controller
    {
        private readonly StockContext _context;
        private readonly UserContext _context_utilisateur;
        private readonly EmplacementContext _context_emplacement;
        private readonly TransactionStockContext _context_transaction;
        
        public StockController(StockContext context,UserContext context_utilisateur,EmplacementContext context_emplacement,TransactionStockContext context_transaction)
        {
            _context = context;
            _context_utilisateur=context_utilisateur;
            _context_emplacement=context_emplacement;
            _context_transaction=context_transaction;
            
            
        }

        // GET: Stock
        public async Task<IActionResult> Index( string emplacement)
        {
            int valeur=0;
            
            if (emplacement!=null)
            {
                ViewBag.emplacement=_context_emplacement.Emplacement.Where(x=>x.Nom==emplacement).ToList();
                ViewBag.magasin=_context_emplacement.Emplacement.ToList();
                ViewData["empl"]=emplacement;
                ViewBag.user=Userconnected();
                ViewData["qte"]=_context.Stock.Where(x=>x.Lieu_stockage==emplacement && x.Actif==true && x.Quantitee>0).Select(x=>(int)x.Quantitee).Sum();
                foreach (var item in _context.Stock.Where(x=>x.Lieu_stockage==emplacement && x.Actif==true && x.Quantitee>0).ToList())
                {
                    if (item.Quantitee!=null)
                    {
                        valeur+=(int)(item.Quantitee*item.Prix_vente);
                    }
                }
                ViewData["valeur"]=valeur;
                ViewData["sss1"]="active";
                ViewData["sss0"]="active";
                return View(await _context.Stock.Where(x=>x.Lieu_stockage==emplacement).ToListAsync());
            }else
            {
               return RedirectToAction("Page404", "Stock");
            }
            

        }

         public async Task<IActionResult> Liste( string emplacement)
        {
            int valeur=0;
            
            if (emplacement!=null)
            {
                ViewBag.emplacement=_context_emplacement.Emplacement.Where(x=>x.Nom==emplacement).ToList();
                ViewBag.magasin=_context_emplacement.Emplacement.ToList();
                ViewData["empl"]=emplacement;
                ViewBag.user=Userconnected();
                ViewData["qte"]=_context.Stock.Where(x=>x.Lieu_stockage==emplacement && x.Actif==true && x.Quantitee>0).Select(x=>(int)x.Quantitee).Sum();
                foreach (var item in _context.Stock.Where(x=>x.Lieu_stockage==emplacement && x.Actif==true && x.Quantitee>0).ToList())
                {
                    if (item.Quantitee!=null)
                    {
                        valeur+=(int)(item.Quantitee*item.Prix_vente);
                    }
                }
                ViewData["valeur"]=valeur;
                ViewData["sss1"]="active";
                ViewData["sss0"]="active";
                return View(await _context.Stock.Where(x=>x.Lieu_stockage==emplacement).ToListAsync());
            }else
            {
               return RedirectToAction("Page404", "Stock");
            }

        }

        // GET: Stock/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewBag.user=Userconnected();
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var stock = await _context.Stock
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return RedirectToAction("Page404", "Stock");
                //return RedirectToAction("Page404", "Stock");
            }
            ViewData["sss1"]="active";
            ViewData["sss0"]="active";
            return View(stock);
        }

        // GET: Stock/Details/5
        public IActionResult Caisse()
        {
           // MyData montableau=new MyData(_context);
                 //  montableau.EditJson();
            ViewBag.user=Userconnected();
            return View();
        }

        // GET: Stock/Create
        public IActionResult Create(string emplacement)
        {
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            if (emplacement!=null)
            {
                ViewBag.emplacement=_context_emplacement.Emplacement.Where(x=>x.Nom==emplacement).ToList();
            }else
            {
               ViewBag.emplacement=_context_emplacement.Emplacement.ToList();
            }
            if (Userconnected().Type_de_compte!="administrateur" && Userconnected().Type_de_compte!="super-Admin" && emplacement=="PHARMACIE")
            {
                return RedirectToAction("Page405","Stock");
            }
            ViewBag.Cat=_context.Stock.Select(x =>x.Categorie).Distinct().ToList();
            ViewBag.user=Userconnected();
            ViewData["sss1"]="active";
            ViewData["sss0"]="active";
            ViewData["image"]="ok";
            return View();
        }

        public IActionResult Page404()
        {
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewBag.user=Userconnected();
            return View();
        }
        public IActionResult Page405()
        {
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewBag.user=Userconnected();
            return View();
        }

        // POST: Stock/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom_article,Type,Prix_vente,Prix_achat,Quantitee,In,Out,Vendu,Stock_minimal,chiffre_affaire,Categorie,Reference,codebarre,Lieu_stockage,unité,Actif,Compte_general,Img,CreateBy,Description,Notes,Societe,Compte_general_entree,Compte_general_sortie,Create_date,LastModification")] Stock stock)
        {
            
            if (ModelState.IsValid)
            {
                if (!StockArticleExists(stock.Nom_article,stock.Lieu_stockage))
                {
                    stock.CreateBy=Userconnected().nom;
                    stock.Create_date=DateTime.Now;
                    stock.LastModification=DateTime.Now;
                    stock.Actif=true;
                    stock.Img="noPhotoFound.png";
                    //uploads file
                    var files = HttpContext.Request.Form.Files;
                    int nb=files.Count();
                    List<string> ImageList = new List<string>();          
                    foreach (var Image in files)
                    {        
            
                        if (Image != null && Image.Length > 0 ){
                                var file = Image;
                                //There is an error here
                                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//uploads//img");                      
                                var fileName = Guid.NewGuid().ToString().Replace("-", "") + ".png";
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                    ImageList.Add(fileName.ToString());
                                }

                                                            
                            }
                    }                     
                    if(ImageList.Count()!=0){
                        stock.Img=ImageList.Last();                    
                    }
                    
                    _context.Add(stock);
                    await _context.SaveChangesAsync();

                    TransactionStock trans=new TransactionStock();
                    trans.Id=0;
                    trans.Nom_article=stock.Nom_article;
                    trans.Emplacement=stock.Lieu_stockage;
                    if (stock.Quantitee==null)
                    {
                        stock.Quantitee=0;
                    }
                    trans.Quantitee=(decimal)stock.Quantitee;
                    trans.utilisateur=Userconnected().nom;
                    trans.Observation= "stock initial (nouveau)";
                    trans.TypeOperation="Entrée";
                    trans.Create_date=DateTime.Now;
                    if (stock.Quantitee!=null && stock.Quantitee!=0)
                    {
                        _context_transaction.Add(trans);
                        await _context_transaction.SaveChangesAsync();
                    }
                    

                    return RedirectToAction(nameof(Index),new { emplacement = stock.Lieu_stockage });
                }else
                {
                    ViewData["error"]="error";
                }
                
            }
            ViewBag.emplacement=_context_emplacement.Emplacement.Where(x=>x.Nom==stock.Lieu_stockage).ToList();
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewBag.Cat=_context.Stock.Select(x =>x.Categorie).Distinct().ToList();
            ViewBag.user=Userconnected();
            ViewData["sss1"]="active";
            ViewData["sss0"]="active";
            ViewData["image"]="ok";
            return View(stock);
        }

        // GET: Stock/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var stock = await _context.Stock.FindAsync(id);
            if (stock == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            if (Userconnected().Type_de_compte!="administrateur" && Userconnected().Type_de_compte!="super-Admin" && stock.Lieu_stockage=="PHARMACIE")
            {
                return RedirectToAction("Page405","Stock");
            }
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewBag.emplacement=_context_emplacement.Emplacement.ToList();
            ViewBag.Cat=_context.Stock.Select(x =>x.Categorie).Distinct().ToList();
            ViewBag.user=Userconnected();
            ViewData["sss1"]="active";
            ViewData["sss0"]="active";
            ViewData["image"]="ok";
            return View(stock);
        }

        // POST: Stock/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom_article,Type,Prix_vente,Prix_achat,Quantitee,In,Out,Vendu,Stock_minimal,chiffre_affaire,Categorie,Reference,codebarre,Lieu_stockage,unité,Actif,Compte_general,Img,CreateBy,Description,Notes,Societe,Compte_general_entree,Compte_general_sortie,Create_date,LastModification")] Stock stock)
        {
            if (id != stock.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                stock.LastModification=DateTime.Now;
                stock.Actif=true;
                //uploads file
                var files = HttpContext.Request.Form.Files;
                int nb=files.Count();
                List<string> ImageList = new List<string>();          
                foreach (var Image in files)
                {        
            
                     if (Image != null && Image.Length > 0 ){
                            var file = Image;
                            //There is an error here
                            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//uploads//img");                      
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + ".png";
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                ImageList.Add(fileName.ToString());
                            }

                                                        
                        }
                }                     
                if(ImageList.Count()!=0){
                    stock.Img=ImageList.Last();                  
                }

                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.Id))
                    {
                        return RedirectToAction("Page404", "Stock");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Liste),new { emplacement = stock.Lieu_stockage });
            }
            ViewBag.emplacement=_context_emplacement.Emplacement.Where(x=>x.Nom==stock.Lieu_stockage).ToList();
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewBag.Cat=_context.Stock.Select(x =>x.Categorie).Distinct().ToList();
            ViewBag.user=Userconnected();
            ViewData["sss1"]="active";
            ViewData["sss0"]="active";
            ViewData["image"]="ok";
            return View(stock);
        }

        // GET: Stock/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var stock = await _context.Stock
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewBag.user=Userconnected();
            ViewData["sss1"]="active";
            ViewData["sss0"]="active";
            return View(stock);
        }

        [HttpPost]
        public async Task<object> GetPrice([FromBody] Service MyStock)
        {   
            
            var monarticle= await _context.Stock.Where(x => x.Nom_article==MyStock.Nom && x.Lieu_stockage=="PHARMACIE").FirstOrDefaultAsync();
            return Json(monarticle); 
            
        }

        // POST: Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _context.Stock.FindAsync(id);
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Liste),new { emplacement = stock.Lieu_stockage });
        }

        private bool StockExists(int id)
        {
            return _context.Stock.Any(e => e.Id == id);
        }
        private bool StockArticleExists(string nom, string empl)
        {
            return _context.Stock.Any(e => e.Nom_article == nom && e.Lieu_stockage==empl);
        }

        

        public User Userconnected()
        {
            ViewData["21"]="active";
            var user = from u in _context_utilisateur.User
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

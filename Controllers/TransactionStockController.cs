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

namespace BANA.Controllers
{
    [Authorize]
    public class TransactionStockController : Controller
    {
        private readonly TransactionStockContext _context;
        private readonly StockContext _context_stock;
        private readonly EmplacementContext _context_emplacement;
        private readonly UserContext _context_user;

        public TransactionStockController(TransactionStockContext context,UserContext context_user,StockContext context_stock,EmplacementContext context_emplacement)
        {
             _context = context;
             _context_user = context_user;
             _context_stock = context_stock;
             _context_emplacement=context_emplacement;
        }

        // GET: TransactionStock
        public async Task<IActionResult> Index(string date)
        {
            ViewData["table"]="ok";
            ViewBag.user=Userconnected();
            var mestransactions= await _context.TransactionStock.OrderByDescending(x=>x.Id).Take(300).ToListAsync();
            if (date!=null)
            {
                mestransactions=mestransactions.Where(x=>x.Create_date>=DateTime.Parse(date)&& x.Create_date<=DateTime.Parse(date).AddDays(1).AddSeconds(-1)).ToList();
            }
            return View(mestransactions);
        }
        public async Task<IActionResult> Index0()
        {
            ViewBag.user=Userconnected();
            List<string> datas = new List<string>();
            var resultats=await _context.TransactionStock.OrderByDescending(x=>x.Id).ToListAsync();
            datas.AddRange(resultats.Select(x=>x.Create_date.ToShortDateString()).Distinct());
            ViewBag.datas=datas.Distinct();
            return View(resultats);
        }

        // GET: TransactionStock/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var transactionStock = await _context.TransactionStock
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transactionStock == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user=Userconnected();
            return View(transactionStock);
        }

        // GET: TransactionStock/Create
        public IActionResult Create(string emplacement)
        {
            ViewBag.articles=_context_stock.Stock.Where(x=>x.Lieu_stockage==emplacement).ToList();
            ViewBag.emplacement=_context_emplacement.Emplacement.Where(x=>x.Nom==emplacement).ToList();
            ViewBag.user=Userconnected();
            return View();
        }

        // POST: TransactionStock/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom_article,Emplacement,Quantitee,utilisateur,Observation,TypeOperation,Create_date")] TransactionStock transactionStock)
        {
            transactionStock.Create_date=DateTime.Now;
            transactionStock.utilisateur=Userconnected().nom;
            ViewBag.articles=_context_stock.Stock.Where(x=>x.Lieu_stockage==transactionStock.Emplacement).ToList();
            ViewBag.emplacement=_context_emplacement.Emplacement.Where(x=>x.Nom==transactionStock.Emplacement).ToList();
            ViewBag.user=Userconnected();
            if (ModelState.IsValid)
            {
                if(transactionStock.TypeOperation=="transfert"){
                    var monarticle2= _context_stock.Stock.Where(x=>x.Nom_article==transactionStock.Nom_article && x.Lieu_stockage=="PHARMACIE").FirstOrDefault();
                    if (monarticle2==null)
                    {
                        return RedirectToAction("Page405","Stock");
                    }
                    transactionStock.Emplacement="MAGASIN GÉNÉRAL";
                    transactionStock.TypeOperation="Sortie";
                    _context.Add(transactionStock);
                    await _context.SaveChangesAsync();
                    transactionStock.Emplacement="PHARMACIE";
                    transactionStock.TypeOperation="Entrée";
                    transactionStock.Id=0;
                    _context.Add(transactionStock);
                    await _context.SaveChangesAsync();
                    var monarticle1= _context_stock.Stock.Where(x=>x.Nom_article==transactionStock.Nom_article && x.Lieu_stockage=="MAGASIN GÉNÉRAL").FirstOrDefault();
                    if(monarticle1.Quantitee==null){monarticle1.Quantitee=0;}
                    monarticle1.Quantitee-=Math.Abs(transactionStock.Quantitee);
                    _context_stock.Update(monarticle1);
                    await _context_stock.SaveChangesAsync();

                   
                    if(monarticle2.Quantitee==null){monarticle2.Quantitee=0;}
                    monarticle2.Quantitee+=Math.Abs(transactionStock.Quantitee);
                    _context_stock.Update(monarticle2);
                    
                    await _context_stock.SaveChangesAsync();
                    
                    ViewData["Ancien"]=transactionStock.Nom_article +" a été transféré à l'emplacement "+ transactionStock.Emplacement+" avec succès Quantité:"+transactionStock.Quantitee; 
                }else
                {
                    var monarticle= _context_stock.Stock.Where(x=>x.Nom_article==transactionStock.Nom_article && x.Lieu_stockage==transactionStock.Emplacement).FirstOrDefault();
                    if(monarticle.Quantitee==null){monarticle.Quantitee=0;}
                    if (transactionStock.TypeOperation=="Entrée")
                    {
                        monarticle.Quantitee+=Math.Abs(transactionStock.Quantitee);
                        ViewData["Ancien"]=transactionStock.Nom_article +" a été ajouté à l'emplacement "+ transactionStock.Emplacement+" avec succès Quantité:"+transactionStock.Quantitee; 
                    }else 
                    {
                        monarticle.Quantitee-=Math.Abs(transactionStock.Quantitee);
                        ViewData["Ancien"]=transactionStock.Nom_article +" a été retiré à l'emplacement "+ transactionStock.Emplacement+" avec succès Quantité:"+transactionStock.Quantitee; 
                    }
                    _context.Add(transactionStock);
                    await _context.SaveChangesAsync();
                    _context_stock.Update(monarticle);
                    await _context_stock.SaveChangesAsync();
                
                    return View(transactionStock);
                        
                }

            }
            
            return View(transactionStock);
        }

        // // GET: TransactionStock/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return RedirectToAction("Page404", "Stock");
        //     }

        //     var transactionStock = await _context.TransactionStock.FindAsync(id);
        //     if (transactionStock == null)
        //     {
        //         return RedirectToAction("Page404", "Stock");
        //     }
        //     ViewBag.user=Userconnected();
        //     return View(transactionStock);
        // }

        // POST: TransactionStock/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Id,Nom_article,Quantitee,utilisateur,Observation,TypeOperation,Create_date")] TransactionStock transactionStock)
        // {
        //     if (id != transactionStock.Id)
        //     {
        //         return RedirectToAction("Page404", "Stock");
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(transactionStock);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!TransactionStockExists(transactionStock.Id))
        //             {
        //                 return RedirectToAction("Page404", "Stock");
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewBag.user=Userconnected();
        //     return View(transactionStock);
        // }

        // GET: TransactionStock/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return RedirectToAction("Page404", "Stock");
        //     }

        //     var transactionStock = await _context.TransactionStock
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (transactionStock == null)
        //     {
        //         return RedirectToAction("Page404", "Stock");
        //     }
        //     ViewBag.user=Userconnected();

        //     return View(transactionStock);
        // }

        // POST: TransactionStock/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var transactionStock = await _context.TransactionStock.FindAsync(id);
        //     _context.TransactionStock.Remove(transactionStock);
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }

        private bool TransactionStockExists(int id)
        {
            return _context.TransactionStock.Any(e => e.Id == id);
        }
       

        public User Userconnected()
        {
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

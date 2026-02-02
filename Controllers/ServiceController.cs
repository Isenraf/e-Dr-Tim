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
    public class ServiceController : Controller
    {
        private readonly ServiceContext _context;
        private readonly DepartementContext _context_departement;
        private readonly UserContext _context_user;
        private readonly StockContext _context_stock;

        public ServiceController(ServiceContext context,DepartementContext context_departement,UserContext context_user,StockContext context_stock)
        {
            _context = context;
            _context_departement = context_departement;
            _context_user = context_user;
            _context_stock=context_stock;
        }

        // GET: Service
        public async Task<IActionResult> Index()
        {
            var services=_context.Service.ToList();
            ViewData["total"]=services.Count().ToString();
            ViewData["min"]=services.OrderBy(x => x.Montant).Select(x => (int)x.Montant).FirstOrDefault().ToString();
            ViewData["max"]=services.OrderByDescending(x => x.Montant).Select(x => (int)x.Montant).FirstOrDefault().ToString();
            ViewBag.user=Userconnected();
            return View(await _context.Service.OrderByDescending(x=>x.Id).ToListAsync());
        }

        // GET: Service/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var service = await _context.Service
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            var services=_context.Service.ToList();
            ViewData["total"]=services.Count().ToString();
            ViewData["min"]=services.OrderBy(x => x.Montant).Select(x => (int)x.Montant).FirstOrDefault().ToString();
            ViewData["max"]=services.OrderByDescending(x => x.Montant).Select(x => (int)x.Montant).FirstOrDefault().ToString();
            ViewBag.user=Userconnected();
            return View(service);
        }

        // GET: Service/Create
        public IActionResult Create()
        {
            if (Userconnected().Type_de_compte!="administrateur" && Userconnected().Type_de_compte!="super-Admin")
            {
                return RedirectToAction("Page405","Stock");
            }
            var services=_context.Service.ToList();
            ViewData["total"]=services.Count().ToString();
            ViewData["min"]=services.OrderBy(x => x.Montant).Select(x => x.Montant).FirstOrDefault().ToString();
            ViewData["max"]=services.OrderByDescending(x => x.Montant).Select(x => x.Montant).FirstOrDefault().ToString();
            ViewBag.departements= _context.Service.Select(x => x.Departement).ToList().Distinct();
            ViewBag.user=Userconnected();
            return View();
        }

        // POST: Service/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Cote,Categorie,Description,Departement,Montant,Actif")] Service service)
        {
            ViewBag.departements= _context.Service.Select(x => x.Departement).ToList().Distinct();
            var services=_context.Service.ToList();
            ViewData["total"]=services.Count().ToString();
            ViewData["min"]=services.OrderBy(x => x.Montant).Select(x => x.Montant).FirstOrDefault().ToString();
            ViewData["max"]=services.OrderByDescending(x => x.Montant).Select(x => x.Montant).FirstOrDefault().ToString();
            ViewBag.user=Userconnected();
            if (ServiceNameExists(service.Nom))
            {
                ViewData["error"]="error";
                return View(service);
            }

            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(service);
        }

        // GET: Service/Edit/5
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

            var service = await _context.Service.FindAsync(id);
            if (service == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.departements= _context.Service.Select(x => x.Departement).ToList().Distinct();
            var services=_context.Service.ToList();
            ViewData["total"]=services.Count().ToString();
            ViewData["min"]=services.OrderBy(x => x.Montant).Select(x => x.Montant).FirstOrDefault().ToString();
            ViewData["max"]=services.OrderByDescending(x => x.Montant).Select(x => x.Montant).FirstOrDefault().ToString();
            ViewBag.user=Userconnected();
            return View(service);
        }

        // POST: Service/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Cote,Categorie,Description,Departement,Montant,Actif")] Service service)
        {
            if (id != service.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
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
            ViewBag.departements= _context.Service.Select(x => x.Departement).ToList().Distinct();
            var services=_context.Service.ToList();
            ViewData["total"]=services.Count().ToString();
            ViewData["min"]=services.OrderBy(x => x.Montant).Select(x => x.Montant).FirstOrDefault().ToString();
            ViewData["max"]=services.OrderByDescending(x => x.Montant).Select(x => x.Montant).FirstOrDefault().ToString();
            ViewBag.user=Userconnected();
            return View(service);
        }

        // GET: Service/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var service = await _context.Service
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            var services=_context.Service.ToList();
            ViewData["total"]=services.Count().ToString();
            ViewData["min"]=services.OrderBy(x => x.Montant).Select(x => x.Montant).FirstOrDefault().ToString();
            ViewData["max"]=services.OrderByDescending(x => x.Montant).Select(x => x.Montant).FirstOrDefault().ToString();
            ViewBag.user=Userconnected();
            return View(service);
        }

        [HttpPost]
        public async Task<object> GetPrice([FromBody] Service MyService)
        {   
            
            var monservice= await _context.Service.Where(x => x.Nom==MyService.Nom).FirstOrDefaultAsync();
            if(monservice==null){
             var monarticle= await _context_stock.Stock.Where(x => x.Nom_article==MyService.Nom).FirstOrDefaultAsync();
             return Json(monarticle); 
            }
             return Json(monservice); 
            
        }

        // POST: Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Service.FindAsync(id);
            _context.Service.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Service.Any(e => e.Id == id);
        }
        private bool ServiceNameExists(string name)
        {
            return _context.Service.Any(e => e.Nom == name);
        }
        

        public User Userconnected()
        {
            ViewData["8"] = "active";
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

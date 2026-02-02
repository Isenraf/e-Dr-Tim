#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BANA.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BANA.Controllers
{
    public class UserController : Controller
    {
        private readonly UserContext _context;
        private readonly EmplacementContext _context_emplacement;

        public UserController(UserContext context,EmplacementContext context_emplacement)
        {
            _context = context;
            _context_emplacement=context_emplacement;
            
        }

        // GET: User
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (Userconnected().Type_de_compte!="administrateur" && Userconnected().Type_de_compte!="super-Admin")
            {
                return RedirectToAction("Page405","Stock");
            }
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewData["activemenu5"]="active";
            ViewBag.user=Userconnected();
            return View(await _context.User.ToListAsync());
        }
        public IActionResult Page404()
        {
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewBag.user=Userconnected();
            return View();
        }

        // LOGIN: User/login
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            var hmAuth = await HttpContext.AuthenticateAsync("CookieAuthHm");
            if (hmAuth.Succeeded)
            {
                return RedirectToAction("Account", "User");
            }
            return View();
        }

        public  async Task<IActionResult> Login2(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            var dmiAuth = await HttpContext.AuthenticateAsync("CookieAuthDmi");
            if (dmiAuth.Succeeded)
            {
                return RedirectToAction("Dashboard", "Doctor");
            }
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string? returnUrl,[Bind("nom,Password")] User user)
        {
            
            if (user.nom!=null && user.Password!=null && NameUserExists(user.nom)){
                var utilisateur = await _context.User.Where(m => m.nom == user.nom).FirstOrDefaultAsync();
                if (user.Password==utilisateur.Password)
                {
                    Console.WriteLine(user.Activate);
                    if (utilisateur.Activate==true)
                    {
                        var UserClaim = new List<Claim>()
                        {
                            new Claim( ClaimTypes.Name,utilisateur.nom),
                            new Claim( ClaimTypes.SerialNumber,"HM")
                        };
                        var userIdentity =new ClaimsIdentity(UserClaim,user.nom);
                        var userPrincipal=new ClaimsPrincipal(new[]{userIdentity});
                        await  HttpContext.SignInAsync("CookieAuthHm",userPrincipal);

                        //actualisation derniere connexion utilisateur
                        try
                        {
                            utilisateur.Lastconnetion = DateTime.Now;
                            _context.Update(utilisateur);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!UserExists(user.Id))
                            {
                                return RedirectToAction("Page404", "Stock");
                            }
                            else
                            {
                                throw;
                            }
                        }
                        //var returnUrl = Request.Query["ReturnUrl"].ToString();
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        return RedirectToAction("Index","Departement");
                    }else{
                        ViewData["error"]="Compte désactivé, veuillez contacter l'administrateur";
                        return View(user);
                    }

                }else{
                    ViewData["error"]="Mot de passe incorrect";
                    return View(user);
                }
                
               
            }
                ViewData["error"]="Utilisateur Inexistant";
                return View(user);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login2(string? returnUrl,[Bind("nom,Password")] User user)
        {
            
            if (user.nom!=null && user.Password!=null && NameUserExists(user.nom)){
                var utilisateur = await _context.User.Where(m => m.nom == user.nom).FirstOrDefaultAsync();
                if (user.Password==utilisateur.Password)
                {
                    Console.WriteLine(user.Activate);
                    if (utilisateur.Activate==true)
                    {
                        var UserClaim = new List<Claim>()
                        {
                            new Claim( ClaimTypes.Name,utilisateur.nom),
                            new Claim( ClaimTypes.SerialNumber,"DMI"),
                        };
                        var userIdentity =new ClaimsIdentity(UserClaim,user.nom);
                        var userPrincipal=new ClaimsPrincipal(new[]{userIdentity});
                        await  HttpContext.SignInAsync("CookieAuthDmi",userPrincipal);

                        //actualisation derniere connexion utilisateur
                        try
                        {
                            utilisateur.Lastconnetion = DateTime.Now;
                            _context.Update(utilisateur);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!UserExists(user.Id))
                            {
                                return RedirectToAction("Page404", "Stock");
                            }
                            else
                            {
                                throw;
                            }
                        }

                        //var returnUrl = Request.Query["ReturnUrl"].ToString();
                        Console.WriteLine(returnUrl);
                        Console.WriteLine(Request.QueryString.Value);
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        return RedirectToAction("Dashboard","Doctor");
                    }else{
                        ViewData["error"]="Compte désactivé, veuillez contacter l'administrateur";
                        return View(user);
                    }

                }else{
                    ViewData["error"]="Mot de passe incorrect";
                    return View(user);
                }
                
               
            }
                ViewData["error"]="Utilisateur Inexistant";
                return View(user);
        }


        // GET: User/Details/5
        [Authorize]
        public async Task<IActionResult> Account()
        {
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewData["activemenu5"]="active";
            ViewBag.user=Userconnected();
            if (Userconnected() == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Email == Userconnected().Email);
            if (user == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            
            return View(user);
        }



        // GET: User/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewBag.user=Userconnected();
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            
            ViewData["activemenu5"]="active";
            return View(user);
        }

        // GET: User/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewBag.user=Userconnected();
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Activate,nom,Email,Password,ConfirmPassword,Type_de_compte,Access_module,Img,Genre,Departement,Societe,Create_date,Lastconnetion")] User user)
        {
            if (ModelState.IsValid)
            {
                if (NameUserExists(user.nom))
                {
                    ViewBag.user=Userconnected();
                    ViewData["error"]= user.nom+", existe  existe dejà dans le système";
                    return View(user);
                }
                user.Create_date=DateTime.Now;
                user.Lastconnetion=DateTime.Now;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewData["activemenu5"]="active";
            ViewBag.user=Userconnected();
            return View(user);
        }

        // GET: User/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewBag.user=Userconnected();
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            if ( user.Access_module==null)
            {
                user.Access_module = "";
            }
           
            ViewData["activemenu5"]="active";
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Activate,nom,Email,Password,ConfirmPassword,Type_de_compte,Access_module,Img,Genre,Departement,Societe,Create_date,Lastconnetion")] User user)
        {
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewData["activemenu5"]="active";
            ViewBag.user=Userconnected();
            if (id != user.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            
            return View(user);
        }

        // GET: User/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.magasin=_context_emplacement.Emplacement.ToList();
            ViewData["activemenu5"]="active";
            ViewBag.user=Userconnected();
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
           
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Disconnect()
        {
            var hmAuth = await HttpContext.AuthenticateAsync("CookieAuthHm");
            if (hmAuth.Succeeded)
            {
                await HttpContext.SignOutAsync("CookieAuthHm");
            }
            return RedirectToAction("Login","User");
        }
        
        public async Task<IActionResult> Disconnect2()
        {
            var DmiAuth = await HttpContext.AuthenticateAsync("CookieAuthDmi");
            if (DmiAuth.Succeeded)
            {
                await HttpContext.SignOutAsync("CookieAuthDmi");
            }
            return RedirectToAction("Login2","User");
        }

        
        public User Userconnected()
        {
            ViewData["18"] = "active";
            var user = from u in _context.User
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

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
        private bool NameUserExists(string name)
        {
            return _context.User.Any(e => e.nom == name);
        }
    }
}

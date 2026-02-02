#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BANA.Models;
using BANA.IA;
using Microsoft.AspNetCore.Authorization;

namespace BANA.Controllers
{
    [Authorize]
    public class DepartementController : Controller
    {
        private readonly DepartementContext _context;
        private readonly UserContext _context_user;
        private readonly DoctorContext _context_doctor;
        private readonly IAIService _aiService;

        public DepartementController(DepartementContext context,UserContext context_user,DoctorContext context_doctor, IAIService aiService)
        {
            _context = context;
            _context_user = context_user;
            _context_doctor=context_doctor;
            _aiService = aiService;
        }

        // GET: Departement
        public async Task<IActionResult> Index()
        {
            // IAIService is now injected and ready to use:
            // var response = await _aiService.AskAsync("Hello");
            ViewData["js_dep"]="ok";
            ViewBag.user=Userconnected();
            return View(await _context.Departement.ToListAsync());
        }

        // GET: Departement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var departement = await _context.Departement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departement == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["total"]=_context.Departement.ToList().Count();
            ViewBag.user=Userconnected();
            ViewBag.doctors=_context_doctor.Doctor.Where(x=>x.Specialite==departement.Nom).ToList();
            return View(departement);
        }

        // GET: Departement/Create
        public IActionResult Create()
        {
            ViewBag.user=Userconnected();
            return View();
        }

        // POST: Departement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Description,Image,Actif")] Departement departement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.user=Userconnected();
            return View(departement);
        }

        // GET: Departement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var departement = await _context.Departement.FindAsync(id);
            if (departement == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user=Userconnected();
            return View(departement);
        }

        // POST: Departement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Description,Image,Actif")] Departement departement)
        {
            if (id != departement.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                        int nb=files.Count();

                        foreach (var Image in files)
                        {         
                            if (Image != null && Image.Length > 0 )
                            {
                                var file = Image;
                                //There is an error here
                                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//img1");
                                var fileName = departement.Nom+".png";
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                    departement.Image=fileName.ToString();
                                }

                                                        
                            }
                        }

                    _context.Update(departement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartementExists(departement.Id))
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
            ViewBag.user=Userconnected();
            return View(departement);
        }

        // GET: Departement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var departement = await _context.Departement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departement == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewBag.user=Userconnected();
            return View(departement);
        }

        // POST: Departement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departement = await _context.Departement.FindAsync(id);
            _context.Departement.Remove(departement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        //[Authorize]
        public async Task<object> CreateDep1([FromBody] Departement MyData)
        {
            Console.WriteLine(MyData.Nom);
            Console.WriteLine(MyData.Description);
            Console.WriteLine(MyData.Actif);
            if (MyData.Nom == null || MyData.Description == null)
            {
                return Json("error");
            }
            if (DepartementNameExists(MyData.Nom))
            {
                return Json("existe");
            }
            MyData.Image = MyData.Nom + ".png";
            _context.Add(MyData);
            await _context.SaveChangesAsync();
            var option1 = new CookieOptions();
            option1.Expires = DateTime.Now.AddMinutes(1);
            Response.Cookies.Append("CurrentID", MyData.Nom, option1);
            return Json("ok");

        }
        

        


         [HttpPost]  
        public async Task<object> UploadFiles()  
        {
            var CurrentID = Request.Cookies["CurrentID"];

            if(CurrentID!=null){
                // Checking no of files injected in Request object  
                if (HttpContext.Request.Form.Files.Count > 0)  
                {  
                    try  
                    {  
                        //  Get all files from Request object  
                        
                        var files = HttpContext.Request.Form.Files;
                        Console.WriteLine(files.Count());
                        foreach (var Image in files)
                        {
                                                        
                            if (Image != null && Image.Length > 0 )
                            {
                                    var file = Image;
                                    //There is an error here
                                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//img1");
                                                        
                                    var fileName = CurrentID+".png";
                                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                    {
                                        await file.CopyToAsync(fileStream);
                                        //ImageList.Add(fileName.ToString());
                                    }

                                                            
                            }
                        }
                    return Json("ok");

                    }  
                    catch (Exception ex)  
                    {  
                        return Json("une erreur s'est produite. Détails: " + ex.Message);  
                    }  
                }else  
                {  
                    return Json("Type de fichier incorrect");  
                } 

            }
            return Json("Une erreur s'est produite");  
            
        }

        private bool DepartementExists(int id)
        {
            return _context.Departement.Any(e => e.Id == id);
        }
        private bool DepartementNameExists(string nom)
        {
            return _context.Departement.Any(e => e.Nom == nom);
        }
        

        public User Userconnected()
        {
            ViewData["12"] = "active";
            var user = from u in _context_user.User
                         select u;

            user = user.Where(x => x.nom == HttpContext.User.Identity.Name);
            var connectuser=user.First();
                connectuser.Initial=GetInitials(connectuser.nom);
            ViewData["1"] = "active";
            
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

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
using BANA.Data;

namespace BANA.Controllers
{
    
    public class DoctorController : Controller
    {
        private readonly DoctorContext _context;
        private readonly DepartementContext _context1;
        private readonly UserContext _context_user;
        private readonly FactureContext _context_facture;
        private readonly PatientContext _context_patient;
        private readonly DmiContext _context_dmi;

        public DoctorController(DoctorContext context, DepartementContext context1, UserContext context_user, FactureContext context_facture, PatientContext context_patient, DmiContext context_dmi)
        {
            _context = context;
            _context1 = context1;
            _context_user = context_user;
            _context_facture = context_facture;
            _context_patient = context_patient;
            _context_dmi = context_dmi;
        }

        // GET: Doctor
        [Authorize]
        public async Task<IActionResult> Index(string typ, string target)
        {


            ViewData["d0"] = "active";
            ViewData["d1"] = "active";
            ViewData["Interne"] = _context.Doctor.Where(x => x.Interne == true).Count();
            ViewData["Externe"] = _context.Doctor.Where(x => x.Interne == false).Count();
            ViewData["total"] = _context.Doctor.Count();
            ViewData["typ"] = typ;
            ViewData["target"] = target;
            ViewBag.user = Userconnected();
            var doctors = await _context.Doctor.OrderByDescending(x => x.Id).Take(200).ToListAsync();
            if (typ != null && typ == "interne")
            {
                doctors = _context.Doctor.Where(x => x.Interne == true).ToList();

            }
            else if (typ != null && typ == "externe")
            {

                doctors = await _context.Doctor.Where(x => x.Interne == false).OrderByDescending(x => x.Id).ToListAsync();
            }
            if (target != null)
            {
                doctors = await _context.Doctor.Where(x => x.Nom.ToLower().Contains(target.Trim().ToLower())).OrderByDescending(x => x.Id).ToListAsync();
            }
            return View(doctors);
        }
        
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Index2(int? mois, int? annee)
        {

            Console.WriteLine(mois);
            Console.WriteLine(annee);
            string nom_medecin = "";
            var userNom = Userconnected().nom.ToLower().Trim().Replace(" ", "");
            var medecin = _context.Doctor.Where(x => x.User_Name.ToLower().Trim().Replace(" ", "") == userNom).FirstOrDefault();
            if(medecin!=null){nom_medecin = medecin.Nom.ToLower().Trim().Replace(" ", ""); }
            ViewData["xxx2"] = "active current-page";
            ViewData["total"] = _context.Doctor.Count();
            ViewBag.user = Userconnected();
            var doctors = await _context.Doctor.OrderByDescending(x => x.Id).Take(200).ToListAsync();
            if (mois == null || annee == null)
            {
                var factures = _context_dmi.Dmi.Select(x => x.NumeroDeFacture).ToList();
                ViewBag.factures = _context_facture.Facture.Where(x => x.Medecin.ToLower().Trim().Replace(" ", "") == nom_medecin && !factures.Contains(x.Numero_de_facture) && x.Type.Contains("consultation".ToLower()) && x.Etat_patient == "Cloturé" && x.Create_date.AddDays(2)>DateTime.Now).OrderByDescending(x => x.Create_date).Take(200).ToList();
            }
            else
            {
                ViewBag.factures = _context_facture.Facture.Where(x => x.Medecin.ToLower().Trim().Replace(" ", "") == nom_medecin && x.Type.Contains("consultation".ToLower()) && x.Etat_patient == "Cloturé" && x.Create_date.Year == DateTime.Now.Year && x.Create_date.Month == DateTime.Now.Month).OrderByDescending(x => x.Create_date).ToList();
            }
            ViewBag.medecins = _context.Doctor.Where(x => x.Interne == true).ToList();

            return View(doctors);
        }

        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Index3(string typ, string target)
        {
            ViewData["xx2"] = "active current-page";
            ViewData["total"] = _context.Doctor.Count();
            ViewBag.user = Userconnected();
            var doctors = await _context.Doctor.OrderByDescending(x => x.Id).Take(200).ToListAsync();
            ViewBag.patients = _context_facture.Facture.Where(x => x.Medecin.ToLower().Trim().Replace(" ", "") == Userconnected().nom.ToLower().Trim().Replace(" ", "") && x.Type.Contains("consultation".ToLower()) && x.Etat_patient == "Cloturé").OrderByDescending(x => x.Create_date).ToList();
            return View(doctors);
        }
        
        [Authorize]
        public async Task<IActionResult> Liste(string typ, string target)
        {


            ViewData["d0"] = "active";
            ViewData["d1"] = "active";
            ViewData["Interne"] = _context.Doctor.Where(x => x.Interne == true).Count();
            ViewData["Externe"] = _context.Doctor.Where(x => x.Interne == false).Count();
            ViewData["total"] = _context.Doctor.Count();
            ViewData["typ"] = typ;
            ViewData["target"] = target;
            ViewData["table"] = "ok";
            ViewBag.user = Userconnected();
            var doctors = await _context.Doctor.OrderByDescending(x => x.Id).Take(200).ToListAsync();
            if (typ != null && typ == "interne")
            {
                doctors = _context.Doctor.Where(x => x.Interne == true).ToList();

            }
            else if (typ != null && typ == "externe")
            {

                doctors = await _context.Doctor.Where(x => x.Interne == false).OrderByDescending(x => x.Id).ToListAsync();
            }
            if (target != null)
            {
                doctors = await _context.Doctor.Where(x => x.Nom.ToLower().Contains(target.Trim().ToLower())).OrderByDescending(x => x.Id).ToListAsync();
            }
            return View(doctors);
        }

        // GET: Doctor/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var doctor = await _context.Doctor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["d0"] = "active";
            ViewData["d3"] = "active";
            ViewBag.user = Userconnected();
            ViewBag.patients = _context_facture.Facture.Where(x => x.Medecin == doctor.Nom).ToList();
            return View(doctor);
        }


        // GET: Doctor/Details/5
        [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
        public async Task<IActionResult> Dashboard()
        {

            //var doctor = await _context.Doctor
            //   .FirstOrDefaultAsync(m => m.Id == id);

            ViewData["xx1"] = "active current-page";
            ViewBag.user = Userconnected();
            ViewBag.doctors = _context.Doctor.Where(x => x.Interne == true).ToList();
            ViewBag.patients = _context_facture.Facture.Where(x => x.Medecin.ToLower().Trim().Replace(" ", "") == Userconnected().nom.ToLower().Trim().Replace(" ", "") && x.Type.Contains("consultation".ToLower()) && x.Etat_patient == "Cloturé").OrderByDescending(x => x.Create_date).ToList();
            return View();
        }



        // GET: Doctor/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["d0"] = "active";
            ViewData["d2"] = "active";
            ViewBag.departements = _context1.Departement.ToList();
            ViewBag.users = _context_user.User.Where(x => x.Activate == true).ToList();
            ViewBag.user = Userconnected();
            return View();
        }

        

        // POST: Doctor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Prenom,Date_De_Naissance,CreateDate,Genre,Specialite,Interne,Departement,Telephone,Email,Site_web,Note,User_Name,Password,PasswordConfirm,social1,social2,social3,social4,Actif,ImageFile1,ImageFile2")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                var docteurs = _context.Doctor.Where(x => x.Telephone == doctor.Telephone).ToList();
                ViewBag.doublons = docteurs;

                

                if (docteurs.Count > 0)
                {
                    if (doctor.Telephone == "111111111")
                    {

                        _context.Add(doctor);
                        await _context.SaveChangesAsync();

                        if (doctor.ImageFile1 != null && doctor.ImageFile1.Length > 0)
                        {
                            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/doctors/profiles");

                            if (!Directory.Exists(uploadPath))
                                Directory.CreateDirectory(uploadPath);

                            var fileName = doctor.Id+"_"+doctor.Nom.Replace(" ","")+".png";
                            var filePath = Path.Combine(uploadPath, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await doctor.ImageFile1.CopyToAsync(stream);
                            }
                        }
                        
                        if (doctor.ImageFile2 != null && doctor.ImageFile2.Length > 0)
                        {
                            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/doctors/signatures");

                            if (!Directory.Exists(uploadPath))
                                Directory.CreateDirectory(uploadPath);

                            var fileName = doctor.Id+"_"+doctor.Nom.Replace(" ","")+".png";
                            var filePath = Path.Combine(uploadPath, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await doctor.ImageFile2.CopyToAsync(stream);
                            }
                        }

                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    _context.Add(doctor);
                    await _context.SaveChangesAsync();

                    if (doctor.ImageFile1 != null && doctor.ImageFile1.Length > 0)
                    {
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/doctors/profiles");

                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);

                        var fileName = doctor.Id+"_"+doctor.Nom.Replace(" ","")+".png";
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await doctor.ImageFile1.CopyToAsync(stream);
                        }
                    }
                    
                    if (doctor.ImageFile2 != null && doctor.ImageFile2.Length > 0)
                    {
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/doctors/signatures");

                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);

                       var fileName = doctor.Id+"_"+doctor.Nom.Replace(" ","")+".png";
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await doctor.ImageFile2.CopyToAsync(stream);
                        }
                    }
                    return RedirectToAction(nameof(Index));

                }

                
            }

            ViewData["d0"]="active";
            ViewData["d2"]="active";
            ViewBag.departements=  _context1.Departement.ToList();
            ViewBag.user=Userconnected();
            ViewBag.users = _context_user.User.Where(x => x.Activate == true).ToList();

            return View(doctor);
        }

        // GET: Doctor/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["d0"] = "active";
            ViewData["d4"] = "active";
            ViewBag.departements = _context1.Departement.ToList();
            ViewBag.users = _context_user.User.Where(x => x.Activate == true).ToList();
            ViewBag.user = Userconnected();
            return View(doctor);
        }

        // POST: Doctor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Prenom,Date_De_Naissance,CreateDate,Genre,Specialite,Departement,Interne,Telephone,Email,Site_web,Note,User_Name,Password,PasswordConfirm,social1,social2,social3,social4,Actif,ImageFile1,ImageFile2")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return RedirectToAction("Page404", "Stock");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();

                    if (doctor.ImageFile1 != null && doctor.ImageFile1.Length > 0)
                    {
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/doctors/profiles");

                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);

                        var fileName = doctor.Id+"_"+doctor.Nom.Replace(" ","")+".png";
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await doctor.ImageFile1.CopyToAsync(stream);
                        }
                    }
                    
                    if (doctor.ImageFile2 != null && doctor.ImageFile2.Length > 0)
                    {
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/doctors/signatures");

                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);

                        var fileName = doctor.Id+"_"+doctor.Nom.Replace(" ","")+".png";
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await doctor.ImageFile2.CopyToAsync(stream);
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
                    {
                        return RedirectToAction("Page404", "Stock");
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            ViewData["d0"]="active";
            ViewData["d4"]="active";
            ViewBag.departements=  _context1.Departement.ToList();
            ViewBag.users = _context_user.User.Where(x => x.Activate == true).ToList();
            ViewBag.user=Userconnected();
            return View(doctor);
        }

        // GET: Doctor/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Page404", "Stock");
            }

            var doctor = await _context.Doctor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return RedirectToAction("Page404", "Stock");
            }
            ViewData["d0"] = "active";
            ViewBag.departements = _context1.Departement.ToList();
            ViewBag.user = Userconnected();
            return View(doctor);
        }

        [Authorize]
        public async Task<IActionResult> Disable(int id)
        {
            var contact = await _context.Doctor.FindAsync(id);
            if (contact.Actif == true) { contact.Actif = false; } else { contact.Actif = true; }
            _context.Update(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Doctor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctor.FindAsync(id);
            _context.Doctor.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctor.Any(e => e.Id == id);
        }
        private bool DmiOK(string nf)
        {
            return _context_dmi.Dmi.Any(e => e.NumeroDeFacture == nf);
        }
        

        public User Userconnected()
        {
            ViewData["3"] = "active";
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

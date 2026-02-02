using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BANA.Data;
using BANA.Models;
using Microsoft.AspNetCore.Authorization;

namespace BANA.Controllers
{
    [Authorize(AuthenticationSchemes ="CookieAuthDmi")]
    public class RondeController : Controller
    {
        private readonly RondeContext _context;
        private readonly UserContext _context_user;
        private readonly DoctorContext _context_doctor;
        private readonly PatientContext _context_patient;

        public RondeController(RondeContext context, UserContext context_user, DoctorContext context_doctor, PatientContext context_patient)
        {
            _context = context;
            _context_user = context_user;
            _context_doctor = context_doctor;
            _context_patient = context_patient;
        }

        // GET: Ronde
        public async Task<IActionResult> Index(int id)
        {
            var ronde = await _context.Ronde
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.user = Userconnected();
            return View(await _context.Ronde.Where(x=>x.NumeroDossier==ronde.NumeroDossier).OrderByDescending(x=>x.Id).ToListAsync());
        }

        // GET: Ronde/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ronde = await _context.Ronde
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ronde == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            ViewBag.rondes = _context.Ronde.Where(x => x.NumeroDossier == ronde.NumeroDossier.ToString()).ToList();
            return View(ronde);
        }

        // GET: Ronde/Create
        public IActionResult Create()
        {
            ViewBag.user = Userconnected();
            return View();
        }

        // POST: Ronde/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroDossier,Patient,Pc,CreateDate,DerniereModif,Trumps,Trumps2,Medecin,Etat")] Ronde ronde)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ronde);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ronde);
        }

        // GET: Ronde/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ronde = await _context.Ronde.FindAsync(id);
            if (ronde == null)
            {
                return NotFound();
            }
            ViewBag.user = Userconnected();
            return View(ronde);
        }

        // POST: Ronde/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroDossier,Patient,Pc,CreateDate,DerniereModif,Trumps,Trumps2,Medecin,Etat")] Ronde ronde)
        {
            if (id != ronde.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ronde);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RondeExists(ronde.Id))
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
            return View(ronde);
        }

        // GET: Ronde/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ronde = await _context.Ronde
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ronde == null)
            {
                return NotFound();
            }

            return View(ronde);
        }

        // POST: Ronde/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ronde = await _context.Ronde.FindAsync(id);
            if (ronde != null)
            {
                _context.Ronde.Remove(ronde);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<object> Add([FromBody] Ronde ronde)
        {
            var medecin = _context_doctor.Doctor.FirstOrDefault(m => m.User_Name == Userconnected().nom);
            var patient = _context_patient.Patient.FirstOrDefault(m => m.Numero_dossier == ronde.NumeroDossier);
            if (medecin != null)
            {
                ronde.Medecin = medecin.Nom;
                ronde.CreateDate = DateTime.Now;
                ronde.DerniereModif = DateTime.Now;
                ronde.Patient = patient.Nom + " " + patient.Prenom;
                ronde.Etat = "Encours";
            }
            
            _context.Update(ronde);
            await _context.SaveChangesAsync();
            return Json(ronde.Id);

        }

        [HttpPost]
        public async Task<object> Add2([FromBody] Ronde ronde)
        {

            var rd = await _context.Ronde
                .FirstOrDefaultAsync(m => m.Id == ronde.Id);

            if (rd != null)
            {
                rd.DerniereModif = DateTime.Now;
                rd.Etat = "Encours";
                rd.Taf = ronde.Taf;
                
            }

            _context.Update(rd);
            await _context.SaveChangesAsync();
            return Json(rd.Id);

        }

        [HttpGet]
        public IActionResult ServerTime()
        {
            var now = DateTime.Now; // heure du serveur
            return Json(new
            {
                date = now.ToString("dd/MM/yyyy HH:mm:ss")
            });
        }
        
        [HttpGet]
        public IActionResult Serveruser()
        {
            var usr =Userconnected().nom; // heure du serveur
            return Json(new
            {
                utilisateur = usr
            });
        }

        public User Userconnected()
        {
            ViewData["8"] = "active";
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

        private bool RondeExists(int id)
        {
            return _context.Ronde.Any(e => e.Id == id);
        }
    }
}

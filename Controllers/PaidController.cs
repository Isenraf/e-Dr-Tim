#nullable disable
using System;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BANA.Models;
using Microsoft.AspNetCore.Authorization;
using BANA.MyFunction;
using iTextSharp.text;
using BANA.PDF;




namespace BANA.Controllers
{
    [Authorize]
    public class PaidController : Controller
    {
        private readonly PaidContext _context;
        private readonly FactureContext _context2;
        private readonly UserContext _context_user;
        private readonly ExamenContext _context_examen;
        private readonly ResultatContext _context_resultat;
        private readonly TransactionStockContext _context_transaction;
        private readonly StockContext _context_stock;
        private readonly DoctorContext _context_docteur;
        private readonly HonoraireContext _context_honoraire;
        
        public PaidController(PaidContext context,UserContext context_user,FactureContext context2,ExamenContext context_examen,ResultatContext context_resultat,TransactionStockContext context_transaction,StockContext context_stock,DoctorContext context_docteur,HonoraireContext context_honoraire)
        {
            _context = context;
            _context_user = context_user;
            _context2 = context2;
            _context_examen=context_examen;
            _context_resultat=context_resultat;
            _context_transaction=context_transaction;
            _context_stock=context_stock;
            _context_docteur=context_docteur;
            _context_honoraire=context_honoraire;
        }

        // GET: Paid
        public async Task<IActionResult> Index(string arg1,string arg2,string arg3)
        {
            ViewData["arg1"]=arg1;
            ViewData["arg2"]=arg2;
            ViewData["arg3"]=arg3;
            ViewData["table"]="ok";
            string date=DateTime.Now.ToShortDateString();
            ViewBag.totjr=_context.Paid.Where(x=>x.Create_date>=DateTime.Parse(date).AddHours(-11) && x.Create_date<=DateTime.Parse(date).AddHours(13).AddSeconds(-1)).Select(y=>y.Montant).Sum();
            ViewBag.totmois=_context.Paid.Where(x=>x.Create_date.Month==DateTime.Now.Month && x.Create_date.Year==DateTime.Now.Year).Select(y=>y.Montant).Sum();
            
            if (arg1!=null && arg2!=null && arg3!=null)
            {
                var lignedepaid=_context.Paid.Where(x=>x.Create_date>=DateTime.Parse(arg1).AddHours(-11) && x.Create_date<=DateTime.Parse(arg1).AddHours(13).AddSeconds(-1) && x.Caissier==arg2 && x.Moyen_paiement==arg3);
                ViewBag.user=Userconnected();
                return View(await lignedepaid.OrderByDescending(x=>x.Id).ToListAsync());
            }else if (arg1!=null && arg2!=null && arg3==null)
            {
                var lignedepaid=_context.Paid.Where(x=>x.Create_date>=DateTime.Parse(arg1).AddHours(-11) && x.Create_date<=DateTime.Parse(arg1).AddHours(13).AddSeconds(-1) && x.Caissier==arg2 && (x.Moyen_paiement=="orange money" || x.Moyen_paiement=="momo" || x.Moyen_paiement=="especes" || x.Moyen_paiement=="banque" || x.Moyen_paiement=="autre"));
                ViewBag.user=Userconnected();
                return View(await lignedepaid.OrderByDescending(x=>x.Id).ToListAsync());
            }else if(arg1!=null && arg2==null && arg3==null)
            {
                var lignedepaid=_context.Paid.Where(x=>x.Create_date>=DateTime.Parse(arg1).AddHours(-11) && x.Create_date<=DateTime.Parse(arg1).AddHours(13).AddSeconds(-1) && ((x.Moyen_paiement=="orange money" || x.Moyen_paiement=="momo" || x.Moyen_paiement=="especes" || x.Moyen_paiement=="banque" || x.Moyen_paiement=="autre")));
                ViewBag.user=Userconnected();
                return View(await lignedepaid.OrderByDescending(x=>x.Id).ToListAsync());
            }else
            {
               var lignedepaid=_context.Paid.Where(x=>x.Moyen_paiement=="orange money" || x.Moyen_paiement=="momo" || x.Moyen_paiement=="especes" || x.Moyen_paiement=="banque" || x.Moyen_paiement=="autre");
                ViewBag.user=Userconnected();
                return View(await _context.Paid.OrderByDescending(x=>x.Id).Take(100).ToListAsync());
            }
            
            
        }
        public async Task<IActionResult> comptapaid01()
        {
            ViewData["13"]="active";
            var Paie=await _context.Paid.Where(x=>x.Moyen_paiement!="caution").ToListAsync();

          
            // Create a list of string.
            List<string> dates = new List<string>();
            List<string> dates1 = new List<string>();
            List<Stat> datesfinal = new List<Stat>();
            
           
            foreach (var item in _context.Paid.Where(x=>x.Moyen_paiement!="caution").Select(x=>x.Create_date))
            {
                dates1.Add(item.Month+"-"+item.Year);
                dates.Add(item.ToString("MMMM")+" "+item.Year);
            }
            var dates_dist1=dates.Distinct();
            
            List<string> dates11 = new List<string>();
            dates11.AddRange(dates1.Distinct());
            //Console.WriteLine(dates1[0].IndexOf("-"));
            

            int i=0;
            foreach (var item in dates_dist1)
            {
                Stat ob=new Stat();
                     ob.mois= int.Parse(dates11[i].Substring(0,dates11[i].IndexOf("-")));
                     ob.annee= int.Parse(dates11[i].Substring(dates11[i].IndexOf("-")+1,4));
                     ob.date_en_lettre=item;
                datesfinal.Add(ob);
                i++;
            }
            ViewBag.dates=datesfinal;

            ViewBag.user=Userconnected();
            return View(Paie);
        }
        

        public async Task<IActionResult> comptapaid1(int mois, int annee)
        {
            ViewData["13"] = "active";

            ViewData["table"] = "ok";
            var paie = await _context.Paid.Where(x => x.Moyen_paiement != "caution" && x.Create_date.Month == mois && x.Create_date.Year == annee).ToListAsync();
            ViewBag.user = Userconnected();
            return View(paie);
        }
        

        
        public async Task<IActionResult> Tb()
        {
            //var Lignespaid=_context.Paid.Where(x=>x.Moyen_paiement!="caution").OrderByDescending(x=>x.Id).Take(10000).Select(x=>x.Create_date.ToShortDateString());
            ViewData["day"] = _context.Paid.Where(x => x.Create_date.Year == DateTime.Now.Year && x.Create_date.Month == DateTime.Now.Month && x.Create_date.Day == DateTime.Now.Day).Select(x => x.Montant).Sum();
            ViewData["Month"] = _context.Paid.Where(x => x.Create_date.Year == DateTime.Now.Year && x.Create_date.Month == DateTime.Now.Month).Select(x => x.Montant).Sum();
            //var dates=_context.Paid;

            List<string> mesdates = new List<string>();

            IQueryable<string> DateQuery = from m in _context.Paid
                                           orderby m.Id
                                           select m.Create_date.ToShortDateString();
            mesdates.AddRange(DateQuery.Distinct());
            if (mesdates.Count() != 0)
            {
                mesdates.Add(DateTime.Parse(mesdates.LastOrDefault()).AddDays(1).ToShortDateString());
            }


            ViewData["2"] = "active";

            ViewBag.dates = mesdates.Distinct().Reverse();
            ViewBag.user = Userconnected();
            return View(await _context.Paid.OrderByDescending(x => x.Id).Take(3000).ToListAsync());
        }
        public async Task<IActionResult> Tb1(String madate,string caisse)
        {
            ViewData["tb0"]="active";
            ViewData["tb3"]="active";
            //var Lignespaid=_context.Paid;
            var Lignespaid=_context.Paid.Where(x=>x.Moyen_paiement!="caution" && x.Create_date>=DateTime.Parse(madate).AddHours(-11) && x.Create_date<=DateTime.Parse(madate).AddHours(13).AddSeconds(-1) && (x.Moyen_paiement=="orange money" || x.Moyen_paiement=="momo" || x.Moyen_paiement=="especes" || x.Moyen_paiement=="banque" || x.Moyen_paiement=="autre"));
            ViewBag.caisses=Lignespaid.Select(x=>x.caisse).Distinct();
            if(caisse!=null){
                Lignespaid=Lignespaid.Where(x=>x.caisse==caisse);
            }
            
            List<string> caissiers = new List<string>();
            caissiers.AddRange(Lignespaid.Select(x=>x.Caissier));
            ViewBag.caissiers=caissiers.Distinct();
            ViewBag.user=Userconnected();
            ViewBag.Lignespaid_om=Lignespaid.Where(x=>x.Moyen_paiement=="orange money");
            ViewBag.Lignespaid_momo=Lignespaid.Where(x=>x.Moyen_paiement=="momo");
            ViewBag.Lignespaid_especes=Lignespaid.Where(x=>x.Moyen_paiement=="especes");
            ViewBag.Lignespaid_banque=Lignespaid.Where(x=>x.Moyen_paiement=="banque");
            ViewBag.Lignespaid_autre=Lignespaid.Where(x=>x.Moyen_paiement=="autre");
            ViewBag.date=madate;
            ViewBag.caisse=caisse;
            ViewBag.caissierscount=caissiers.Distinct().Count();
            ViewBag.total=Lignespaid.Select(x=>x.Montant).Sum();
            return View(await Lignespaid.OrderByDescending(x=>x.Id).ToListAsync());
        }
        public async Task<IActionResult> Rapport(String madate,string caisse)
        {
            ViewData["tb0"]="active";
            ViewData["tb3"]="active";
            ViewBag.user=Userconnected();

            var Lignespaid=_context.Paid.Where(x=>x.Moyen_paiement!="caution" && x.Create_date>=DateTime.Parse(madate).AddHours(-11) && x.Create_date<=DateTime.Parse(madate).AddHours(13).AddSeconds(-1) && (x.Moyen_paiement=="orange money" || x.Moyen_paiement=="momo" || x.Moyen_paiement=="especes" || x.Moyen_paiement=="banque" || x.Moyen_paiement=="autre"));
            var Lignespaid_assurance=_context2.Facture.Where(x=>(x.Type=="hospi_assurance" ||x.Type=="consultation_assurance" ||x.Type=="generique_assurance" ||x.Type=="actes_assurance" ||x.Type=="examens_assurance" ) && x.Encaisse_par!=null &&x.Etat_patient=="Cloturé" && x.Derniere_modification>=DateTime.Parse(madate).AddHours(-11) && x.Derniere_modification<=DateTime.Parse(madate).AddHours(13).AddSeconds(-1));

            if(caisse!=null){
                Lignespaid=Lignespaid.Where(x=>x.caisse==caisse);
            }
            // if (DateTime.Parse(madate)>DateTime.Now)
            // {
            //     Lignespaid=null;
            // }

            ViewBag.mt=Lignespaid.Select(x=>x.Montant).Sum();
            ViewBag.nf=Lignespaid.Count();
            List<string> caissiers = new List<string>();
            List<string> caisses = new List<string>();
            caissiers.AddRange(Lignespaid.Select(x=>x.Caissier));
            caisses.AddRange(Lignespaid.Select(x=>x.caisse));
            ViewBag.caissiers=caissiers.Distinct();
            ViewBag.caisses=caisses.Distinct();
            
            ViewBag.Lignespaid_om=Lignespaid.Where(x=>x.Moyen_paiement=="orange money").Select(x=>x.Montant).Sum();
            ViewBag.Lignespaid_momo=Lignespaid.Where(x=>x.Moyen_paiement=="momo").Select(x=>x.Montant).Sum();
            ViewBag.Lignespaid_especes=Lignespaid.Where(x=>x.Moyen_paiement=="especes").Select(x=>x.Montant).Sum();
            ViewBag.Lignespaid_banque=Lignespaid.Where(x=>x.Moyen_paiement=="banque").Select(x=>x.Montant).Sum();
            ViewBag.Lignespaid_autre=Lignespaid_assurance.Select(x=>x.Net_a_payer_assurance).Sum();
            ViewBag.date=madate;
            ViewBag.caisse=caisse;
            ViewBag.caissierscount=caissiers.Distinct().Count();
            ViewBag.total=Lignespaid.Select(x=>x.Montant).Sum();
            return View(await Lignespaid.OrderByDescending(x=>x.Id).ToListAsync());
        }
        public async Task<IActionResult> Rapport2(String madate,string caisse)
        {
            ViewData["tb0"]="active";
            ViewData["tb3"]="active";
            ViewBag.user=Userconnected();

            var Lignespaid=_context.Paid.Where(x=>x.Moyen_paiement!="caution" && x.Create_date.Month==DateTime.Parse(madate).Month && x.Create_date.Year==DateTime.Parse(madate).Year && (x.Moyen_paiement=="orange money" || x.Moyen_paiement=="momo" || x.Moyen_paiement=="especes" || x.Moyen_paiement=="banque" || x.Moyen_paiement=="autre"));
            
            if(caisse!=null){
                Lignespaid=Lignespaid.Where(x=>x.caisse==caisse);
            }
            if (DateTime.Parse(madate)>DateTime.Now)
            {
                Lignespaid=null;
            }
            ViewBag.mt=Lignespaid.Select(x=>x.Montant).Sum();
            ViewBag.nf=Lignespaid.Count();
            List<string> caissiers = new List<string>();
            List<string> caisses = new List<string>();
            caissiers.AddRange(Lignespaid.Select(x=>x.Caissier));
            caisses.AddRange(Lignespaid.Select(x=>x.caisse));
            ViewBag.caissiers=caissiers.Distinct();
            ViewBag.caisses=caisses.Distinct();
            
            ViewBag.Lignespaid_om=Lignespaid.Where(x=>x.Moyen_paiement=="orange money");
            ViewBag.Lignespaid_momo=Lignespaid.Where(x=>x.Moyen_paiement=="momo");
            ViewBag.Lignespaid_especes=Lignespaid.Where(x=>x.Moyen_paiement=="especes");
            ViewBag.Lignespaid_banque=Lignespaid.Where(x=>x.Moyen_paiement=="banque");
            ViewBag.Lignespaid_autre=Lignespaid.Where(x=>x.Moyen_paiement=="autre");
            ViewBag.date=madate;
            ViewBag.caisse=caisse;
            ViewBag.caissierscount=caissiers.Distinct().Count();
            ViewBag.total=Lignespaid.Select(x=>x.Montant).Sum();
            return View(await Lignespaid.OrderByDescending(x=>x.Id).ToListAsync());
        }

        public async Task<IActionResult> Index2()
        {
            ViewData["day"]= _context.Paid.Where(x=>x.Create_date.Year==DateTime.Now.Year && x.Create_date.Month==DateTime.Now.Month && x.Create_date.Day==DateTime.Now.Day && x.Moyen_paiement=="Assurance").Select(x=>x.Montant).Sum();
            ViewData["Month"]= _context.Paid.Where(x=>x.Create_date.Year==DateTime.Now.Year && x.Create_date.Month==DateTime.Now.Month && x.Moyen_paiement=="Assurance").Select(x=>x.Montant).Sum();
            
            ViewBag.user=Userconnected();
            return View(await _context.Paid.OrderByDescending(x=>x.Id).Where(x=>x.Moyen_paiement=="Assurance").ToListAsync());
        }

     

        [HttpPost]
        public async Task<object> SavePayment([FromBody] Paid MyData)
        {   
            var facture = await _context2.Facture.Where(x=>x.Numero_de_facture==MyData.Numero_facture && x.Type==MyData.fact_type).FirstOrDefaultAsync();
            if (facture.Net_a_payer_patient-facture.Montant_recu_patient<=0)
            {
                return Json("ok");
            }
            MyData.Caissier=Userconnected().nom;
            MyData.Create_date=DateTime.Now;
            if (MyData.Moyen_paiement=="caution")
            {
                var caution = await _context2.Facture.Where(x=>x.Numero_de_facture==MyData.Numerocaution.Substring(MyData.Numerocaution.LastIndexOf(" ")+1) && x.Type=="caution").FirstOrDefaultAsync();
                if (MyData.Montant>(caution.Net_a_payer_patient-caution.Nombre_impression))
                {
                    MyData.Montant=caution.Net_a_payer_patient-caution.Nombre_impression;
                }
                caution.Nombre_impression+=MyData.Montant;
                _context2.Update(caution);
                await _context2.SaveChangesAsync();
                
            }

            _context.Add(MyData);
            await _context.SaveChangesAsync();
            string initial="";
            
                facture.Montant_recu_patient+=MyData.Montant;
                facture.Encaisse_par=Userconnected().nom;
                facture.Create_date=DateTime.Now;
                initial=facture.Intervenant;
                //facture.Nombre_impression += 1;
                if (initial == "" || initial == null)
            {
                facture.Intervenant = Userconnected().nom;
            }
                
                if (facture.Net_a_payer_patient-facture.Montant_recu_patient<=0){facture.Etat_patient="Cloturé";}
                else if(facture.Net_a_payer_patient-facture.Montant_recu_patient>0){facture.Etat_patient="Paiement partiel";}
                // if (MyData.Moyen_paiement.Contains("Caution"))
                // {
                //     facture.Etat_patient="Cloturé";
                //     facture.Montant_recu_patient+=facture.Net_a_payer_patient;
                // }
                
            _context2.Update(facture);
            await _context2.SaveChangesAsync();

            Resultat rsult=new Resultat();
            List<string> liste_de_facturation2 = new List<string>();
            List<string> liste_de_facturation22 = new List<string>();
            
            int i=0;
            int compteur=0;
            string chaine="";
            foreach(char c in facture.Ligne_facturation2) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation2.Add(chaine);
                    }
                    if (compteur%7==6 && chaine!="")
                    {
                        liste_de_facturation22.Add(chaine);
                    }
                    
                    compteur+=1;
                      chaine="";

                  }
                  
              }else{
                  chaine+=c.ToString();
              }
              i++;
              //Console.WriteLine(i+"-"+result);
            }
            
            
            if ((initial=="" || initial==null) && facture.Type!="hospi_assurance" && !facture.Type.Contains("proforma"))
           {
                var doctor=_context_docteur.Doctor.Where(x=>x.Nom==facture.Medecin).FirstOrDefault();
                string doctorphone=" ";
                if (doctor!=null)
                {
                    doctorphone=doctor.Telephone;
                }
                foreach (var item in liste_de_facturation2)
                {
                    
                    var monexam=_context_examen.Examen.Where(x=>x.Nom.Replace(" ","")==item.Replace(" ","")).FirstOrDefault();
                    if (monexam!=null)
                    {
                        
                        rsult.Id=0;
                        rsult.Nom=item;
                        rsult.Categorie=monexam.Categorie;
                        rsult.Code=monexam.Code;
                        rsult.Contenu=monexam.Contenu;
                        rsult.Cote=monexam.Cote;
                        rsult.ExamType=monexam.ExamType;
                        rsult.Create_date=DateTime.Now;
                        rsult.NomPatient=facture.Nom +" "+facture.Prenom;
                        rsult.NumeroDossier=facture.Numero_dossier;
                        rsult.NumeroFacture=facture.Numero_de_facture;
                        rsult.Date_de_naissance=facture.DateNaissance;
                        rsult.Telephone_patient=facture.Phone;
                        rsult.Genre=facture.Genre;
                        rsult.Prescripteur=facture.Medecin +", Tel:"+doctorphone;
                        _context_resultat.Add(rsult);
                        await _context_resultat.SaveChangesAsync();
                    }else
                    {
                        rsult.Id=0;
                        rsult.Nom=item;
                        rsult.Categorie="";
                        rsult.Code="";
                        rsult.Contenu="";
                        rsult.Cote="";
                        rsult.ExamType="";
                        rsult.Create_date=DateTime.Now;
                        rsult.NomPatient=facture.Nom +" "+facture.Prenom;
                        rsult.NumeroDossier=facture.Numero_dossier;
                        rsult.NumeroFacture=facture.Numero_de_facture;
                        rsult.Date_de_naissance=facture.DateNaissance;
                        rsult.Telephone_patient=facture.Phone;
                        rsult.Genre=facture.Genre;
                        rsult.Prescripteur=facture.Medecin +", Tel:"+doctorphone;
                        _context_resultat.Add(rsult);
                        await _context_resultat.SaveChangesAsync();
                    }
                    

                }
           } 


            //mouvement stock
            if ((initial=="" || initial==null) && !facture.Type.Contains("proforma") && facture.Type!="hospi_assurance" && facture.Type!="caution" && facture.Montant_recu_patient-facture.Net_a_payer_patient>=0)
            {
                TransactionStock trans=new TransactionStock();
                List<string> liste_de_facturation4 = new List<string>();
                List<string> quantites = new List<string>();
                
                i=0;
                compteur=0;
                int compteur2=0;
                chaine="";
                foreach(char c in facture.Ligne_facturation4) {
                
                if(c.Equals('λ')){
                    if(i!=0){
                        if (compteur%7==0 && chaine!="")
                        {
                            liste_de_facturation4.Add(chaine);
                            compteur2=compteur;
                        }
                        if (compteur==compteur2+5)
                        {
                            quantites.Add(chaine);
                        }
                        compteur+=1;
                        chaine="";

                    }
                    
                }else{
                    chaine+=c.ToString();
                }
                i++;
                
                }

            
                
                int j=0;
                foreach (var item in liste_de_facturation4)
                {
                    trans.Id=0;
                    trans.Nom_article=item;
                    trans.Emplacement="PHARMACIE";
                    trans.Quantitee=Decimal.Parse(quantites[j]);
                    trans.utilisateur=Userconnected().nom;
                    trans.Observation= "Facture:"+facture.Numero_de_facture;
                    trans.TypeOperation="Sortie";
                    trans.Create_date=DateTime.Now;
                

                    var monstck= _context_stock.Stock.Where(x=>x.Nom_article==item).FirstOrDefault();
                    if (monstck!=null)
                    {
                        if (monstck.Quantitee==null)
                        {
                            monstck.Quantitee=0;
                        }
                            monstck.Quantitee-=Decimal.Parse(quantites[j]);

                        _context_transaction.Add(trans);
                        await _context_transaction.SaveChangesAsync();

                        _context_stock.Update(monstck);
                        await _context_stock.SaveChangesAsync();
                    }
                    
                    j++;
                }
            }
            if (facture.Type=="hospi_assurance" && facture.Montant_recu_patient-facture.Net_a_payer_patient>=0)
            {
              var mesfactures = _context2.Facture.Where(x=>x.Numero_dossier==MyData.Numero_dossier &&(x.Type=="medicament_assurance" || x.Type=="examens_assurance" ) && x.Etat_patient!="Cloturé" && x.Intervenant!="" && x.Intervenant!=null).ToList();
              foreach (var item in mesfactures)
              {
                item.Etat_patient="Cloturé";
                 _context2.Update(item);
                await _context2.SaveChangesAsync();
              }

              var mesfactures2 = _context2.Facture.Where(x=>x.Numero_dossier==MyData.Numero_dossier && x.Type=="examens_assurance" &&  x.Etat_patient!="Cloturé").ToList();
              foreach (var item2 in mesfactures2)
              {
                item2.Etat_patient="Cloturé";
                 _context2.Update(item2);
                await _context2.SaveChangesAsync();
              }



            }



            //Honoraires

            List<string> liste_de_facturation1 = new();
            List<string> liste_de_facturation3 = new();
            List<string> liste_de_facturation11 = new();
            List<string> liste_de_facturation33 = new();
                
            i=0;
            compteur=0;
            chaine="";
            foreach(char c in facture.Ligne_facturation1) {
            
            if(c.Equals('λ')){
                if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation1.Add(chaine);
                    }

                    if (compteur%7==6 && chaine!="")
                    {
                        liste_de_facturation11.Add(chaine);
                    }
                    
                    compteur+=1;
                    chaine="";

                }
                
            }else{
                chaine+=c.ToString();
            }
            i++;
            
            }




            i=0;
            compteur=0;
            chaine="";
            foreach(char c in facture.Ligne_facturation3) {
            
            if(c.Equals('λ')){
                if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation3.Add(chaine);
                    }
                    if (compteur%7==6 && chaine!="")
                    {
                        liste_de_facturation33.Add(chaine);
                    }
                    
                    compteur+=1;
                    chaine="";

                }
                
            }else{
                chaine+=c.ToString();
            }
            i++;
            
            }


            liste_de_facturation1.AddRange(liste_de_facturation2);
            liste_de_facturation1.AddRange(liste_de_facturation3);

            liste_de_facturation11.AddRange(liste_de_facturation22);
            liste_de_facturation11.AddRange(liste_de_facturation33);


            i=0;
            foreach (var item in liste_de_facturation1)
            {
                Honoraire honoraire=new();
                          honoraire.Create_date=DateTime.Now;
                          honoraire.Nom=item;
                          if(facture.Type.Contains("assurance")){honoraire.Mode_paiement="assurance";}else{honoraire.Mode_paiement="cash";}
                          honoraire.Numerofacture=facture.Numero_de_facture;
                          honoraire.Medecin=facture.Medecin;
                          honoraire.Patient=facture.Nom+" "+facture.Prenom;
                          honoraire.Montant=int.Parse(liste_de_facturation11[i].Replace(" ",""));
                          honoraire.Assurance=facture.Assureur;
                          honoraire.Etat="En attente";
                            if (facture.Type == "hospi_assurance")
                            {
                                honoraire.Medecin = "HOSPITALISATION";

                            }
                            if (facture.Type == "generique_0" || facture.Type == "generique_assurance")
                            {
                                honoraire.Medecin = "GÉNÉRIQUE";

                            }

                 _context_honoraire.Add(honoraire);
                await _context_honoraire.SaveChangesAsync();
               
                i++;
            }
            

            //Sms
            //Sms msg=new Sms(facture);

            //Whatsap
            Whatsap whatsap=new Whatsap(facture);
                   
             return Json("ok"); 
            
        }

        [HttpPost]
        [Authorize]
        public async Task<object> SaveCloture([FromBody] Paid MyData)
        {  

            MyData.Caissier=Userconnected().nom;
            MyData.Create_date=DateTime.Now;
            
            _context.Add(MyData);
            await _context.SaveChangesAsync();

            var facture = await _context2.Facture.Where(x=>x.Numero_de_facture==MyData.Numero_facture).FirstOrDefaultAsync();
                facture.Montant_recu_patient+=MyData.Montant;
                facture.Encaisse_par=Userconnected().nom;
                string initial="";
            
                facture.Montant_recu_patient+=MyData.Montant;
                facture.Encaisse_par=Userconnected().nom;
                facture.Create_date=DateTime.Now;
                //facture.Nombre_impression += 1;
                initial =facture.Intervenant;
                if (initial=="" || initial==null)
                {
                    facture.Intervenant=Userconnected().nom;
                }
                facture.Intervenant=Userconnected().nom;
                if (facture.Net_a_payer_patient-facture.Montant_recu_patient<=0){facture.Etat_patient="Cloturé";}
                else if(facture.Net_a_payer_patient-facture.Montant_recu_patient>0){facture.Etat_patient="Paiement partiel";}
                
            _context2.Update(facture);
            await _context2.SaveChangesAsync();

            Resultat rsult=new Resultat();
            List<string> liste_de_facturation2 = new List<string>();
            List<string> liste_de_facturation22 = new List<string>();
            
            int i=0;
            int compteur=0;
            string chaine="";
            foreach(char c in facture.Ligne_facturation2) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation2.Add(chaine);
                    }
                    if (compteur%7==6 && chaine!="")
                    {
                        liste_de_facturation22.Add(chaine);
                    }
                    compteur+=1;
                      chaine="";

                  }
                  
              }else{
                  chaine+=c.ToString();
              }
              i++;
              //Console.WriteLine(i+"-"+result);
            }
            
           if ((initial=="" || initial==null) && facture.Type!="hospi_assurance" && !facture.Type.Contains("proforma"))
           {
                var doctor=_context_docteur.Doctor.Where(x=>x.Nom==facture.Medecin).FirstOrDefault();
                string doctorphone=" ";
                if (doctor!=null)
                {
                    doctorphone=doctor.Telephone;
                }
                foreach (var item in liste_de_facturation2)
                {
                    var monexam=_context_examen.Examen.Where(x=>x.Nom.Replace(" ","")==item.Replace(" ","")).FirstOrDefault();
                    if (monexam!=null)
                    {
                        rsult.Id=0;
                        rsult.Nom=item;
                        rsult.Categorie=monexam.Categorie;
                        rsult.Code=monexam.Code;
                        rsult.Contenu=monexam.Contenu;
                        rsult.Cote=monexam.Cote;
                        rsult.ExamType=monexam.ExamType;
                        rsult.Create_date=DateTime.Now;
                        rsult.NomPatient=facture.Nom+" "+facture.Prenom;
                        rsult.NumeroDossier=facture.Numero_dossier;
                        rsult.NumeroFacture=facture.Numero_de_facture;
                        rsult.Date_de_naissance=facture.DateNaissance;
                        rsult.Telephone_patient=facture.Phone;
                        rsult.Genre=facture.Genre;
                        rsult.Prescripteur=facture.Medecin +", Tel:"+doctorphone;
                        _context_resultat.Add(rsult);
                        await _context_resultat.SaveChangesAsync();
                    }else{
                        rsult.Id=0;
                        rsult.Nom=item;
                        rsult.Categorie="";
                        rsult.Code="";
                        rsult.Contenu="";
                        rsult.Cote="";
                        rsult.ExamType="";
                        rsult.Create_date=DateTime.Now;
                        rsult.NomPatient=facture.Nom+" "+facture.Prenom;
                        rsult.NumeroDossier=facture.Numero_dossier;
                        rsult.NumeroFacture=facture.Numero_de_facture;
                        rsult.Date_de_naissance=facture.DateNaissance;
                        rsult.Telephone_patient=facture.Phone;
                        rsult.Genre=facture.Genre;
                        rsult.Prescripteur=facture.Medecin +", Tel:"+doctorphone;
                        _context_resultat.Add(rsult);
                        await _context_resultat.SaveChangesAsync();
                    }
                    

                }
           } 
            

            //mouvement stock
            if ((initial=="" || initial==null) && facture.Type!="hospi_assurance" && !facture.Type.Contains("proforma") && facture.Type!="caution")
            {
                TransactionStock trans=new TransactionStock();
                List<string> liste_de_facturation4 = new List<string>();
                List<string> quantites = new List<string>();
                
                i=0;
                compteur=0;
                int compteur2=0;
                chaine="";
                foreach(char c in facture.Ligne_facturation4) {
                
                if(c.Equals('λ')){
                    if(i!=0){
                        if (compteur%7==0 && chaine!="")
                        {
                            liste_de_facturation4.Add(chaine);
                            compteur2=compteur;
                        }
                        if (compteur==compteur2+5)
                        {
                            quantites.Add(chaine);
                        }
                        compteur+=1;
                        chaine="";

                    }
                    
                }else{
                    chaine+=c.ToString();
                }
                i++;
                
                }

            
                
                int j=0;
                foreach (var item in liste_de_facturation4)
                {
                    trans.Id=0;
                    trans.Nom_article=item;
                    trans.Emplacement="PHARMACIE";
                    trans.Quantitee=Decimal.Parse(quantites[j]);
                    trans.utilisateur=Userconnected().nom;
                    trans.Observation= "Facture:"+facture.Numero_de_facture;
                    trans.TypeOperation="Sortie";
                    trans.Create_date=DateTime.Now;
                

                    var monstck= _context_stock.Stock.Where(x=>x.Nom_article.Replace(" ","")==item.Replace(" ","")).FirstOrDefault();
                    if (monstck!=null)
                    {
                        if (monstck.Quantitee==null)
                        {
                            monstck.Quantitee=0;
                        }
                            monstck.Quantitee-=Decimal.Parse(quantites[j]);

                        _context_transaction.Add(trans);
                        await _context_transaction.SaveChangesAsync();

                        _context_stock.Update(monstck);
                        await _context_stock.SaveChangesAsync();
                    }
                    
                    j++;
                }
            }
            if (facture.Type=="hospi_assurance")
            {
              var mesfactures = _context2.Facture.Where(x=>x.Numero_dossier==MyData.Numero_dossier && (x.Type=="medicament_assurance" || x.Type=="examens_assurance" ) && x.Etat_patient!="Cloturé" && x.Intervenant!="" && x.Intervenant!=null).ToList();
              foreach (var item in mesfactures)
              {
                item.Etat_patient="Cloturé";
                _context2.Update(item);
                await _context2.SaveChangesAsync();
              }

              var mesfactures2 = _context2.Facture.Where(x=>x.Numero_dossier==MyData.Numero_dossier && x.Type=="examens_assurance" &&  x.Etat_patient!="Cloturé").ToList();
              foreach (var item2 in mesfactures2)
              {
                item2.Etat_patient="Cloturé";
                 _context2.Update(item2);
                await _context2.SaveChangesAsync();
              }
            }



            //Honoraires

            List<string> liste_de_facturation1 = new();
            List<string> liste_de_facturation3 = new();
            List<string> liste_de_facturation11 = new();
            List<string> liste_de_facturation33 = new();
                
            i=0;
            compteur=0;
            chaine="";
            foreach(char c in facture.Ligne_facturation1) {
            
            if(c.Equals('λ')){
                if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation1.Add(chaine);
                    }

                    if (compteur%7==6 && chaine!="")
                    {
                        liste_de_facturation11.Add(chaine);
                    }
                    
                    compteur+=1;
                    chaine="";

                }
                
            }else{
                chaine+=c.ToString();
            }
            i++;
            
            }




            i=0;
            compteur=0;
            chaine="";
            foreach(char c in facture.Ligne_facturation3) {
            
            if(c.Equals('λ')){
                if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation3.Add(chaine);
                    }
                    if (compteur%7==6 && chaine!="")
                    {
                        liste_de_facturation33.Add(chaine);
                    }
                    
                    compteur+=1;
                    chaine="";

                }
                
            }else{
                chaine+=c.ToString();
            }
            i++;
            
            }


            liste_de_facturation1.AddRange(liste_de_facturation2);
            liste_de_facturation1.AddRange(liste_de_facturation3);

            liste_de_facturation11.AddRange(liste_de_facturation22);
            liste_de_facturation11.AddRange(liste_de_facturation33);


            i=0;
            foreach (var item in liste_de_facturation1)
            {
                Honoraire honoraire=new();
                          honoraire.Create_date=DateTime.Now;
                          honoraire.Nom=item;
                          if(facture.Type.Contains("assurance")){honoraire.Mode_paiement="assurance";}else{honoraire.Mode_paiement="cash";}
                          honoraire.Numerofacture=facture.Numero_de_facture;
                          honoraire.Medecin=facture.Medecin;
                          honoraire.Patient=facture.Nom+" "+facture.Prenom;
                          honoraire.Montant=int.Parse(liste_de_facturation11[i].Replace(" ",""));
                          honoraire.Assurance=facture.Assureur;
                          honoraire.Etat="En attente";
                            if (facture.Type == "hospi_assurance")
                            {
                                honoraire.Medecin = "HOSPITALISATION";

                            }
                            if (facture.Type == "generique_0" || facture.Type == "generique_assurance")
                            {
                                honoraire.Medecin = "GÉNÉRIQUE";

                            }
               _context_honoraire.Add(honoraire);
                await _context_honoraire.SaveChangesAsync();
                i++;
            }


            //whatsap
            Whatsap whatsap=new Whatsap(facture);
            
             return Json("ok"); 

        }

        [HttpPost]
        [Authorize]
        public async Task<object> Decloturer([FromBody] Paid MyData)
        {  

            var facture = await _context2.Facture.Where(x=>x.Numero_de_facture==MyData.Numero_facture).FirstOrDefaultAsync();
                
                facture.Encaisse_par="";
                facture.Intervenant="";
                facture.Etat_patient="En attente Paiement";
                
            _context2.Update(facture);
            await _context2.SaveChangesAsync();

           
            
             return Json("ok"); 

        }

        [HttpPost]
        [Authorize]
        public async Task<object> livrer([FromBody] Paid MyData)
        {  

            

            var facture = await _context2.Facture.Where(x=>x.Id==MyData.Id).FirstOrDefaultAsync();
                facture.Intervenant=Userconnected().nom;
                //facture.Nombre_impression += 1;
            _context2.Update(facture);
            await _context2.SaveChangesAsync();

            Resultat rsult=new Resultat();
            List<string> liste_de_facturation4 = new List<string>();
            
            int i=0;
            int compteur=0;
            int compteur2=0;
            string chaine="";

            TransactionStock trans=new TransactionStock();
            List<string> quantites = new List<string>();
            foreach(char c in facture.Ligne_facturation4) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation4.Add(chaine);
                        compteur2=compteur;
                    }
                    if (compteur==compteur2+5)
                    {
                        quantites.Add(chaine);
                    }
                    compteur+=1;
                      chaine="";

                  }
                  
              }else{
                  chaine+=c.ToString();
              }
              i++;
             
            }
            int j=0;
            foreach (var item in liste_de_facturation4)
            {
                trans.Id=0;
                trans.Nom_article=item;
                trans.Emplacement="PHARMACIE";
                trans.Quantitee=Decimal.Parse(quantites[j]);
                trans.utilisateur=Userconnected().nom;
                trans.Observation= "Facture:"+facture.Numero_de_facture;
                trans.TypeOperation="Sortie";
                trans.Create_date=DateTime.Now;
             
                var monstck= _context_stock.Stock.Where(x=>x.Nom_article==item).FirstOrDefault();
                if (monstck!=null)
                {
                    if (monstck.Quantitee==null)
                    {
                        monstck.Quantitee=0;
                    }
                        monstck.Quantitee-=Decimal.Parse(quantites[j]);

                    _context_transaction.Add(trans);
                    await _context_transaction.SaveChangesAsync();

                    _context_stock.Update(monstck);
                    await _context_stock.SaveChangesAsync();
                }
                
                j++;
            }
             return Json("ok"); 
        }

        [HttpPost]
        [Authorize]
        public async Task<object> retourner([FromBody] Paid MyData)
        {  

            

            var facture = await _context2.Facture.Where(x=>x.Id==MyData.Id).FirstOrDefaultAsync();
                facture.Intervenant="";
            _context2.Update(facture);
            await _context2.SaveChangesAsync();

            Resultat rsult=new Resultat();
            List<string> liste_de_facturation4 = new List<string>();
            
            int i=0;
            int compteur=0;
            int compteur2=0;
            string chaine="";

            TransactionStock trans=new TransactionStock();
            List<string> quantites = new List<string>();
            foreach(char c in facture.Ligne_facturation4) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation4.Add(chaine);
                        compteur2=compteur;
                    }
                    if (compteur==compteur2+5)
                    {
                        quantites.Add(chaine);
                    }
                    compteur+=1;
                      chaine="";

                  }
                  
              }else{
                  chaine+=c.ToString();
              }
              i++;
             
            }
            int j=0;
            foreach (var item in liste_de_facturation4)
            {
                trans.Id=0;
                trans.Nom_article=item;
                trans.Emplacement="PHARMACIE";
                trans.Quantitee=Decimal.Parse(quantites[j]);
                trans.utilisateur=Userconnected().nom;
                trans.Observation= "Facture:"+facture.Numero_de_facture;
                trans.TypeOperation="Entrée";
                trans.Create_date=DateTime.Now;
             
                var monstck= _context_stock.Stock.Where(x=>x.Nom_article.Replace(" ","")==item.Replace(" ","")).FirstOrDefault();
                if (monstck!=null)
                {
                    if (monstck.Quantitee==null)
                    {
                        monstck.Quantitee=0;
                    }
                        monstck.Quantitee+=Decimal.Parse(quantites[j]);

                    _context_transaction.Add(trans);
                    await _context_transaction.SaveChangesAsync();

                    _context_stock.Update(monstck);
                    await _context_stock.SaveChangesAsync();
                }
                
                j++;
            }
             return Json("ok"); 
        }

        [HttpPost]
        [Authorize]
        public async Task<object> GenererResultat([FromBody] Paid MyData)
        {  

            
                MyData.Create_date=DateTime.Now;
            var facture = await _context2.Facture.Where(x=>x.Id==MyData.Id).FirstOrDefaultAsync();
                facture.Intervenant=Userconnected().nom;
            _context2.Update(facture);
            await _context2.SaveChangesAsync();

            Resultat rsult=new Resultat();
            List<string> liste_de_facturation2 = new List<string>();
            
            int i=0;
            int compteur=0;
            string chaine="";
            foreach(char c in facture.Ligne_facturation2) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation2.Add(chaine);
                    }
                    compteur+=1;
                      chaine="";

                  }
                  
              }else{
                  chaine+=c.ToString();
              }
              i++;
              //Console.WriteLine(i+"-"+result);
            }
            
            var doctor=_context_docteur.Doctor.Where(x=>x.Nom==facture.Medecin).FirstOrDefault();
                string doctorphone=" ";
                if (doctor!=null)
                {
                    doctorphone=doctor.Telephone;
                }
            foreach (var item in liste_de_facturation2)
            {
                var monexam=_context_examen.Examen.Where(x=>x.Nom.Replace(" ","")==item.Replace(" ","")).FirstOrDefault();
                if (monexam!=null)
                {
                    rsult.Id=0;
                    rsult.Nom=item;
                    rsult.Categorie=monexam.Categorie;
                    rsult.Code=monexam.Code;
                    rsult.Contenu=monexam.Contenu;
                    rsult.Cote=monexam.Cote;
                    rsult.ExamType=monexam.ExamType;
                    rsult.Create_date=DateTime.Now;
                    rsult.NomPatient=facture.Nom+" "+facture.Prenom;
                    rsult.NumeroDossier=facture.Numero_dossier;
                    rsult.NumeroFacture=facture.Numero_de_facture;
                    rsult.Date_de_naissance=facture.DateNaissance;
                    rsult.Telephone_patient=facture.Phone;
                    rsult.Genre=facture.Genre;
                    rsult.Prescripteur=facture.Medecin +", Tel:"+doctorphone;
                    _context_resultat.Add(rsult);
                    await _context_resultat.SaveChangesAsync();
                }else{
                    rsult.Id=0;
                    rsult.Nom=item;
                    rsult.Categorie="";
                    rsult.Code="";
                    rsult.Contenu="";
                    rsult.Cote="";
                    rsult.ExamType="";
                    rsult.Create_date=DateTime.Now;
                    rsult.NomPatient=facture.Nom+" "+facture.Prenom;
                    rsult.NumeroDossier=facture.Numero_dossier;
                    rsult.NumeroFacture=facture.Numero_de_facture;
                    rsult.Date_de_naissance=facture.DateNaissance;
                    rsult.Telephone_patient=facture.Phone;
                    rsult.Genre=facture.Genre;
                    rsult.Prescripteur=facture.Medecin +", Tel:"+doctorphone;
                    _context_resultat.Add(rsult);
                    await _context_resultat.SaveChangesAsync();

                }
                

            }

             return Json("ok"); 
        }


        [HttpPost]
        [Authorize]
        public async Task<object> GenererHonoraire2([FromBody] Paid MyData)
        {  


            var facture = await _context2.Facture.Where(x=>x.Id==MyData.Id).FirstOrDefaultAsync();
                facture.Intervenant=Userconnected().nom;
            _context2.Update(facture);
            await _context2.SaveChangesAsync();

            
            //Honoraires

            List<string> liste_de_facturation1 = new();
            List<string> liste_de_facturation2 = new();
            List<string> liste_de_facturation3 = new();
            List<string> liste_de_facturation11 = new();
            List<string> liste_de_facturation22 = new();
            List<string> liste_de_facturation33 = new();
                
            int i=0;
            int compteur=0;
            string chaine="";
            foreach(char c in facture.Ligne_facturation1) {
            
            if(c.Equals('λ')){
                if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation1.Add(chaine);
                    }

                    if (compteur%7==6 && chaine!="")
                    {
                        liste_de_facturation11.Add(chaine);
                    }
                    
                    compteur+=1;
                    chaine="";

                }
                
            }else{
                chaine+=c.ToString();
            }
            i++;
            
            }



            i=0;
            compteur=0;
            chaine="";
            foreach(char c in facture.Ligne_facturation2) {
            
            if(c.Equals('λ')){
                if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation2.Add(chaine);
                    }
                    if (compteur%7==6 && chaine!="")
                    {
                        liste_de_facturation22.Add(chaine);
                    }
                    
                    compteur+=1;
                    chaine="";

                }
                
            }else{
                chaine+=c.ToString();
            }
            i++;
            
            }




            i=0;
            compteur=0;
            chaine="";
            foreach(char c in facture.Ligne_facturation3) {
            
            if(c.Equals('λ')){
                if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation3.Add(chaine);
                    }
                    if (compteur%7==6 && chaine!="")
                    {
                        liste_de_facturation33.Add(chaine);
                    }
                    
                    compteur+=1;
                    chaine="";

                }
                
            }else{
                chaine+=c.ToString();
            }
            i++;
            
            }


            liste_de_facturation1.AddRange(liste_de_facturation2);
            liste_de_facturation1.AddRange(liste_de_facturation3);

            liste_de_facturation11.AddRange(liste_de_facturation22);
            liste_de_facturation11.AddRange(liste_de_facturation33);


            i=0;
            foreach (var item in liste_de_facturation1)
            {
                Honoraire honoraire=new();
                          honoraire.Create_date=MyData.Create_date;
                          honoraire.Nom=item;
                          if(facture.Type.Contains("assurance")){honoraire.Mode_paiement="assurance";}else{honoraire.Mode_paiement="cash";}
                          honoraire.Numerofacture=facture.Numero_de_facture;
                          honoraire.Medecin=facture.Medecin;
                          honoraire.Patient=facture.Nom+" "+facture.Prenom;
                          honoraire.Montant=int.Parse(liste_de_facturation11[i].Replace(" ",""));
                          honoraire.Assurance=facture.Assureur;
                          honoraire.Etat="En attente";
               _context_honoraire.Add(honoraire);
                await _context_honoraire.SaveChangesAsync();
                i++;
            }

             return Json("ok"); 
        }


        [HttpPost]
        [Authorize]
        public async Task<object> GenererResultat2([FromBody] Paid MyData)
        {  

            

            var facture = await _context2.Facture.Where(x=>x.Id==MyData.Id).FirstOrDefaultAsync();
                //facture.Intervenant=Userconnected().nom;
            //_context2.Update(facture);
            //await _context2.SaveChangesAsync();

            Resultat rsult=new Resultat();
            List<string> liste_de_facturation2 = new List<string>();
            
            int i=0;
            int compteur=0;
            string chaine="";
            foreach(char c in facture.Ligne_facturation2) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                    if (compteur%7==0 && chaine!="")
                    {
                        liste_de_facturation2.Add(chaine);
                    }
                    compteur+=1;
                      chaine="";

                  }
                  
              }else{
                  chaine+=c.ToString();
              }
              i++;
              //Console.WriteLine(i+"-"+result);
            }
            
            var doctor=_context_docteur.Doctor.Where(x=>x.Nom==facture.Medecin).FirstOrDefault();
                string doctorphone=" ";
                if (doctor!=null)
                {
                    doctorphone=doctor.Telephone;
                }
                
            foreach (var item in liste_de_facturation2)
            {
                Console.WriteLine(item.Replace(" ",""));
                var monexam=_context_examen.Examen.Where(x=>x.Nom.ToLower().Replace(" ","")==item.ToLower().Replace(" ","")).FirstOrDefault();

                if (monexam!=null)
                {
                    Console.WriteLine(rsult.Nom);
                    rsult.Id=0;
                    rsult.Nom=item;
                    rsult.Categorie=monexam.Categorie;
                    rsult.Code=monexam.Code;
                    rsult.Contenu=monexam.Contenu;
                    rsult.Cote=monexam.Cote;
                    rsult.ExamType=monexam.ExamType;
                    rsult.Create_date=DateTime.Now;
                    rsult.NomPatient=facture.Nom+" "+facture.Prenom;
                    rsult.NumeroDossier=facture.Numero_dossier;
                    rsult.NumeroFacture=facture.Numero_de_facture;
                    rsult.Date_de_naissance=facture.DateNaissance;
                    rsult.Telephone_patient=facture.Phone;
                    rsult.Genre=facture.Genre;
                    rsult.Prescripteur=facture.Medecin +", Tel:"+doctorphone;
                    _context_resultat.Add(rsult);
                    await _context_resultat.SaveChangesAsync();
                }else{
                    rsult.Id=0;
                    rsult.Nom=item;
                    rsult.Categorie="";
                    rsult.Code="";
                    rsult.Contenu="";
                    rsult.Cote="";
                    rsult.ExamType="";
                    rsult.Create_date=DateTime.Now;
                    rsult.NomPatient=facture.Nom+" "+facture.Prenom;
                    rsult.NumeroDossier=facture.Numero_dossier;
                    rsult.NumeroFacture=facture.Numero_de_facture;
                    rsult.Date_de_naissance=facture.DateNaissance;
                    rsult.Telephone_patient=facture.Phone;
                    rsult.Genre=facture.Genre;
                    rsult.Prescripteur=facture.Medecin +", Tel:"+doctorphone;
                    _context_resultat.Add(rsult);
                    await _context_resultat.SaveChangesAsync();

                }
                

            }

             return Json("ok"); 
        }

        


        
        
        

        // // POST: Paid/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var paid = await _context.Paid.FindAsync(id);
        //     _context.Paid.Remove(paid);
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }

        private bool PaidExists(int id)
        {
            return _context.Paid.Any(e => e.Id == id);
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


        //pdf

         public IActionResult Pdf1(string madate,string tipe)
        { 
            Pdf1Generation x=new(_context,_context2);
            return  new FileStreamResult(x.Rapportdecaisse(madate,"jour"), "application/pdf"); 
        }

        public IActionResult Pdf2(string madate)
        { 
            Pdf1Generation x=new(_context,_context2);
            return  new FileStreamResult(x.Rapportdecaisse(madate,"mois"), "application/pdf"); 
        }
        
    }
}

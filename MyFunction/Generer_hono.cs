using System;
using System.IO;
using System.Threading.Tasks;
using BANA.Models;
using RestSharp;




namespace BANA.MyFunction
{
    public class Generer_hono
    {
        public Generer_hono(Facture mafact)
        {

        }

        // public <string> ligne1(string madate, string tip)
        // {
        //     List<string> liste_de_facturation2 = new List<string>();
        //     List<string> liste_de_facturation22 = new List<string>();
            
        //     int i=0;
        //     int compteur=0;
        //     string chaine="";
        //     foreach(char c in facture.Ligne_facturation2) {
              
        //       if(c.Equals('λ')){
        //           if(i!=0){
        //             if (compteur%7==0 && chaine!="")
        //             {
        //                 liste_de_facturation2.Add(chaine);
        //             }
        //             if (compteur%7==6 && chaine!="")
        //             {
        //                 liste_de_facturation22.Add(chaine);
        //             }
                    
        //             compteur+=1;
        //               chaine="";

        //           }
                  
        //       }else{
        //           chaine+=c.ToString();
        //       }
        //       i++;
        //       //Console.WriteLine(i+"-"+result);
        //     }
            
            
        //     if ((initial=="" || initial==null) && facture.Type!="hospi_assurance" && !facture.Type.Contains("proforma"))
        //    {
        //         var doctor=_context_docteur.Doctor.Where(x=>x.Nom==facture.Medecin).FirstOrDefault();
        //         string doctorphone=" ";
        //         if (doctor!=null)
        //         {
        //             doctorphone=doctor.Telephone;
        //         }
        //         foreach (var item in liste_de_facturation2)
        //         {
                    
        //             var monexam=_context_examen.Examen.Where(x=>x.Nom.Replace(" ","")==item.Replace(" ","")).FirstOrDefault();
        //             if (monexam!=null)
        //             {
                        
        //                 rsult.Id=0;
        //                 rsult.Nom=item;
        //                 rsult.Categorie=monexam.Categorie;
        //                 rsult.Code=monexam.Code;
        //                 rsult.Contenu=monexam.Contenu;
        //                 rsult.Cote=monexam.Cote;
        //                 rsult.ExamType=monexam.ExamType;
        //                 rsult.Create_date=DateTime.Now;
        //                 rsult.NomPatient=facture.Nom +" "+facture.Prenom;
        //                 rsult.NumeroDossier=facture.Numero_dossier;
        //                 rsult.NumeroFacture=facture.Numero_de_facture;
        //                 rsult.Date_de_naissance=facture.DateNaissance;
        //                 rsult.Telephone_patient=facture.Phone;
        //                 rsult.Genre=facture.Genre;
        //                 rsult.Prescripteur=facture.Medecin +", Tel:"+doctorphone;
        //                 _context_resultat.Add(rsult);
        //                 await _context_resultat.SaveChangesAsync();
        //             }else
        //             {
        //                 rsult.Id=0;
        //                 rsult.Nom=item;
        //                 rsult.Categorie="";
        //                 rsult.Code="";
        //                 rsult.Contenu="";
        //                 rsult.Cote="";
        //                 rsult.ExamType="";
        //                 rsult.Create_date=DateTime.Now;
        //                 rsult.NomPatient=facture.Nom +" "+facture.Prenom;
        //                 rsult.NumeroDossier=facture.Numero_dossier;
        //                 rsult.NumeroFacture=facture.Numero_de_facture;
        //                 rsult.Date_de_naissance=facture.DateNaissance;
        //                 rsult.Telephone_patient=facture.Phone;
        //                 rsult.Genre=facture.Genre;
        //                 rsult.Prescripteur=facture.Medecin +", Tel:"+doctorphone;
        //                 _context_resultat.Add(rsult);
        //                 await _context_resultat.SaveChangesAsync();
        //             }
                    

        //         }
        //    } 


        //     //mouvement stock
        //     if ((initial=="" || initial==null) && !facture.Type.Contains("proforma") && facture.Type!="hospi_assurance" && facture.Type!="caution" && facture.Montant_recu_patient-facture.Net_a_payer_patient>=0)
        //     {
        //         TransactionStock trans=new TransactionStock();
        //         List<string> liste_de_facturation4 = new List<string>();
        //         List<string> quantites = new List<string>();
                
        //         i=0;
        //         compteur=0;
        //         int compteur2=0;
        //         chaine="";
        //         foreach(char c in facture.Ligne_facturation4) {
                
        //         if(c.Equals('λ')){
        //             if(i!=0){
        //                 if (compteur%7==0 && chaine!="")
        //                 {
        //                     liste_de_facturation4.Add(chaine);
        //                     compteur2=compteur;
        //                 }
        //                 if (compteur==compteur2+5)
        //                 {
        //                     quantites.Add(chaine);
        //                 }
        //                 compteur+=1;
        //                 chaine="";

        //             }
                    
        //         }else{
        //             chaine+=c.ToString();
        //         }
        //         i++;
                
        //         }

            
                
        //         int j=0;
        //         foreach (var item in liste_de_facturation4)
        //         {
        //             trans.Id=0;
        //             trans.Nom_article=item;
        //             trans.Emplacement="PHARMACIE";
        //             trans.Quantitee=Decimal.Parse(quantites[j]);
        //             trans.utilisateur=Userconnected().nom;
        //             trans.Observation= "Facture:"+facture.Numero_de_facture;
        //             trans.TypeOperation="Sortie";
        //             trans.Create_date=DateTime.Now;
                

        //             var monstck= _context_stock.Stock.Where(x=>x.Nom_article==item).FirstOrDefault();
        //             if (monstck!=null)
        //             {
        //                 if (monstck.Quantitee==null)
        //                 {
        //                     monstck.Quantitee=0;
        //                 }
        //                     monstck.Quantitee-=Decimal.Parse(quantites[j]);

        //                 _context_transaction.Add(trans);
        //                 await _context_transaction.SaveChangesAsync();

        //                 _context_stock.Update(monstck);
        //                 await _context_stock.SaveChangesAsync();
        //             }
                    
        //             j++;
        //         }
        //     }
        //     if (facture.Type=="hospi_assurance" && facture.Montant_recu_patient-facture.Net_a_payer_patient>=0)
        //     {
        //       var mesfactures = _context2.Facture.Where(x=>x.Numero_dossier==MyData.Numero_dossier &&(x.Type=="medicament_assurance" || x.Type=="examens_assurance" ) && x.Etat_patient!="Cloturé" && x.Intervenant!="" && x.Intervenant!=null).ToList();
        //       foreach (var item in mesfactures)
        //       {
        //         item.Etat_patient="Cloturé";
        //          _context2.Update(item);
        //         await _context2.SaveChangesAsync();
        //       }

        //       var mesfactures2 = _context2.Facture.Where(x=>x.Numero_dossier==MyData.Numero_dossier && x.Type=="examens_assurance" &&  x.Etat_patient!="Cloturé").ToList();
        //       foreach (var item2 in mesfactures2)
        //       {
        //         item2.Etat_patient="Cloturé";
        //          _context2.Update(item2);
        //         await _context2.SaveChangesAsync();
        //       }



        //     }



        //     //Honoraires

        //     List<string> liste_de_facturation1 = new();
        //     List<string> liste_de_facturation3 = new();
        //     List<string> liste_de_facturation11 = new();
        //     List<string> liste_de_facturation33 = new();
                
        //     i=0;
        //     compteur=0;
        //     chaine="";
        //     foreach(char c in facture.Ligne_facturation1) {
            
        //     if(c.Equals('λ')){
        //         if(i!=0){
        //             if (compteur%7==0 && chaine!="")
        //             {
        //                 liste_de_facturation1.Add(chaine);
        //             }

        //             if (compteur%7==6 && chaine!="")
        //             {
        //                 liste_de_facturation11.Add(chaine);
        //             }
                    
        //             compteur+=1;
        //             chaine="";

        //         }
                
        //     }else{
        //         chaine+=c.ToString();
        //     }
        //     i++;
            
        //     }




        //     i=0;
        //     compteur=0;
        //     chaine="";
        //     foreach(char c in facture.Ligne_facturation3) {
            
        //     if(c.Equals('λ')){
        //         if(i!=0){
        //             if (compteur%7==0 && chaine!="")
        //             {
        //                 liste_de_facturation3.Add(chaine);
        //             }
        //             if (compteur%7==6 && chaine!="")
        //             {
        //                 liste_de_facturation33.Add(chaine);
        //             }
                    
        //             compteur+=1;
        //             chaine="";

        //         }
                
        //     }else{
        //         chaine+=c.ToString();
        //     }
        //     i++;
            
        //     }


        //     liste_de_facturation1.AddRange(liste_de_facturation2);
        //     liste_de_facturation1.AddRange(liste_de_facturation3);

        //     liste_de_facturation11.AddRange(liste_de_facturation22);
        //     liste_de_facturation11.AddRange(liste_de_facturation33);


        //     i=0;
        //     foreach (var item in liste_de_facturation1)
        //     {
        //         Honoraire honoraire=new();
        //                   honoraire.Create_date=DateTime.Now;
        //                   honoraire.Nom=item;
        //                   if(facture.Type.Contains("assurance")){honoraire.Mode_paiement="assurance";}else{honoraire.Mode_paiement="cash";}
        //                   honoraire.Numerofacture=facture.Numero_de_facture;
        //                   honoraire.Medecin=facture.Medecin;
        //                   honoraire.Patient=facture.Nom+" "+facture.Prenom;
        //                   honoraire.Montant=int.Parse(liste_de_facturation11[i].Replace(" ",""));
        //                   honoraire.Assurance=facture.Assureur;
        //                   honoraire.Etat="En attente";
        //        _context_honoraire.Add(honoraire);
        //         await _context_honoraire.SaveChangesAsync();
        //         i++;
        //     }

        // }
        
        // public MemoryStream ligne2(string madate, string tip)
        // { 
            
        // }

        

    }
}




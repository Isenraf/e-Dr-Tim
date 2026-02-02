using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BANA.Models;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Barcodes;
using iText.Layout.Borders;
using iText.Kernel.Geom;
using System.Drawing.Printing;
using Path = System.IO.Path;
using System.Diagnostics;



namespace BANA.PDF
{
    public class PdfGeneration5
    {
        private readonly FactureContext _context;
    
        public PdfGeneration5(FactureContext context)
        { 
          _context=context;
        }


        public async Task<MemoryStream> DetailsFacture(int id){
            

            var facture = await _context.Facture
                .FirstOrDefaultAsync(m => m.Id == id);


            List<string> liste_de_facturation1 = new List<string>();
            int i=0;
            string result="";
            foreach(char c in facture.Ligne_facturation1) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                      liste_de_facturation1.Add(result);
                      result="";
                  }
                  
              }else{
                  result+=c.ToString();
              }
              i++;
            }
            int controla=liste_de_facturation1.Count();
            List<string> liste_de_facturation2 = new List<string>();
             i=0;
             result="";
            foreach(char c in facture.Ligne_facturation2) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                      liste_de_facturation2.Add(result);
                      result="";
                  }
                  
              }else{
                  result+=c.ToString();
              }
              i++;
            }
            List<string> liste_de_facturation3 = new List<string>();
            i=0;
            result="";
            foreach(char c in facture.Ligne_facturation3) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                      liste_de_facturation3.Add(result);
                      result="";
                  }
                  
              }else{
                  result+=c.ToString();
              }
              i++;
            }
            List<string> liste_de_facturation4 = new List<string>();
            i=0;
            result="";
            foreach(char c in facture.Ligne_facturation4) {
              
              if(c.Equals('λ')){
                  if(i!=0){
                      liste_de_facturation4.Add(result);
                      result="";
                  }
                  
              }else{
                  result+=c.ToString();
              }
              i++;
            }
            liste_de_facturation1.AddRange(liste_de_facturation2);
            liste_de_facturation1.AddRange(liste_de_facturation3);
            liste_de_facturation1.AddRange(liste_de_facturation4);
            int total_ligne=liste_de_facturation1.Count()+liste_de_facturation2.Count()+liste_de_facturation3.Count()+liste_de_facturation4.Count();


            using (MemoryStream workStream = new MemoryStream())
        {
            
            // Créer un PdfWriter pour écrire dans le MemoryStream
                PdfWriter writer = new PdfWriter(workStream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                var customSize = new PageSize(227,500);
                Document document = new Document(pdfDocument,customSize,false);
                document.SetMargins(10, 10, 10, 10);


                MemoryStream workStream2 = new MemoryStream();

                PdfFont font1 = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont font2 = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                

                Paragraph p = new Paragraph("POLYCLINIQUE DE POITIERS").SetFontSize(13).SetTextAlignment(TextAlignment.LEFT).SetFont(font1).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);
                p = new Paragraph("BP : 15422 Douala-Cameroun").SetFontSize(9).SetTextAlignment(TextAlignment.LEFT).SetFont(font2).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);
                p = new Paragraph("Tel:690073467/675018191").SetFontSize(9).SetTextAlignment(TextAlignment.LEFT).SetFont(font2).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);
                p = new Paragraph("NIU:M110800026452Z").SetFontSize(9).SetTextAlignment(TextAlignment.LEFT).SetFont(font2).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);
                


                // Données à encoder dans le QR Code
                string qrContent = "https://cm.edoctor-tim.com/dmi/ppoitiers/"+facture.Numero_dossier+"/"+facture.Numero_de_facture;

                // Générer le QR Code
                BarcodeQRCode qrCode = new BarcodeQRCode(qrContent);
                Image qrImage = new Image(qrCode.CreateFormXObject(pdfDocument));

                // Redimensionner (facultatif)
                qrImage.SetWidth(65);
                qrImage.SetHeight(65);
                qrImage.SetMarginLeft(150);
                qrImage.SetMarginTop(-40);

                //qrImage.SetFixedPosition(1, 199, 417);
                
                // Ajouter l'image au document
                document.Add(qrImage);
                
                p = new Paragraph("Nº Facture:"+facture.Numero_de_facture+ " du "+facture.Create_date.ToString()).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font2).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);

                p = new Paragraph(facture.Nom +" "+facture.Prenom).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER).SetFont(font1).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);

                p = new Paragraph("Nº dossier: "+facture.Numero_dossier+" Tel: "+facture.Phone).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER).SetFont(font2).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);

                p = new Paragraph((int)((DateTime.Now-facture.DateNaissance).TotalDays/365)+ " ans "+  (int)((DateTime.Now-facture.DateNaissance).TotalDays%365)/30 +" mois "+ (int)(DateTime.Now-facture.DateNaissance).TotalDays%365%30+ " jours ").SetFontSize(8).SetTextAlignment(TextAlignment.CENTER).SetFont(font2).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);

                p = new Paragraph("Facturé par: "+facture.Facture_par+" Encaissé par: "+facture.Encaisse_par+ "livré par: "+facture.Intervenant ).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER).SetFont(font2).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);
                



                

                // Créer un tableau avec 3 colonnes
                Table table = new Table(new float[] { 100f, 50f, 25f,50F });
                table.SetWidth(UnitValue.CreatePercentValue(100)); // 100% de la largeur de la page
                Cell cell1 = new Cell()
                        .Add(new Paragraph("Désignation"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("PU"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("QTE"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("TOTAL"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table.AddCell(cell1);
                   
                   
                   for (int k = 0; k < liste_de_facturation1.Count-1; k+=7){
                        cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[k]))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[k+4]))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[k+5]))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[k+6]))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table.AddCell(cell1);
                    }

                    cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetFont(font2)
                        .SetPadding(0)
                        .SetBorderTop(Border.NO_BORDER)
                        .SetFontSize(1);
                        table.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetFont(font2)
                        .SetBorderTop(Border.NO_BORDER)
                        .SetPadding(0)
                        .SetFontSize(1);
                        table.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetFont(font2)
                        .SetPadding(0).SetBorderTop(Border.NO_BORDER)
                        .SetFontSize(1);
                        table.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetFont(font2)
                        .SetPadding(0)
                        .SetBorderTop(Border.NO_BORDER)
                        .SetFontSize(1);
                        table.AddCell(cell1);
                    

                document.Add(table);
                // Créer un tableau avec 3 colonnes
                Table table1 = new Table(new float[] { 80f, 80f});
                table1.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
                cell1 = new Cell()
                        .Add(new Paragraph("Total HT"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table1.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Total_ht.ToString())+" FCFA" ))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table1.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("TVA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table1.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Exonéré" ))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table1.AddCell(cell1);

                

                

                if (facture.Type=="medicament_assurance")
                {
                   
                    cell1 = new Cell()
                        .Add(new Paragraph("Net à Payer Assur"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table1.AddCell(cell1);

                    cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Net_a_payer_assurance.ToString())+" FCFA" ))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table1.AddCell(cell1);
                }

                if (facture.Remise>0)
                {
                   cell1 = new Cell()
                        .Add(new Paragraph("Remise"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table1.AddCell(cell1);

                    cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Remise.ToString())+" FCFA" ))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table1.AddCell(cell1);
                }
                
                cell1 = new Cell()
                        .Add(new Paragraph("Net à Payer Patient"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table1.AddCell(cell1);

                    cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Net_a_payer_patient.ToString())+" FCFA" ))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table1.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Mtant payé"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table1.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Montant_recu_patient.ToString())+" FCFA" ))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                        table1.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetFont(font2)
                        .SetPadding(0)
                        .SetBorderTop(Border.NO_BORDER)
                        .SetFontSize(1);
                        table1.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetFont(font2)
                        .SetPadding(0)
                        .SetBorderTop(Border.NO_BORDER)
                        .SetFontSize(1);
                        table1.AddCell(cell1);

                document.Add(new Paragraph(" "));
                document.Add(table1);     

                p = new Paragraph("Nous vous souhaitons une bonne guérison. Visitez notre site web www.polycliniquedepoitiers.com ").SetFontSize(8).SetTextAlignment(TextAlignment.CENTER).SetFont(font1).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p); 


            

            document.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream2.Write(byteInfo, 0, byteInfo.Length);
            workStream2.Position = 0;

            // 2. Sauvegarder dans un fichier temporaire
            string tempPath = Path.Combine(Path.GetTempPath(), "doc_temp2.pdf");
            File.WriteAllBytes(tempPath, workStream2.ToArray());

            // 3. Impression avec SumatraPDF sur imprimante physique
            string sumatraExe = @"C:\Users\administrateur.POITIERS\AppData\Local\SumatraPDF\SumatraPDF.exe"; // À adapter
            string nomImprimante = @"\\192.168.100.163\print_dr_tim2"; // Nom exact de l’imprimante installée

            var psi = new ProcessStartInfo
            {
                FileName = sumatraExe,
                Arguments = $"-print-to \"{nomImprimante}\" \"{tempPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process.Start(psi);

            return workStream2;  
        }
            //return new FileStreamResult(workStream, "application/pdf");  
    }




        private string separateur_de_millier(string valeuraseparer)
        {
            int i = 0;
            string resultat = "";
            var numbers = new List<string>();
            while (valeuraseparer.Length - i >= 0)
            {
                if (i != 0)
                {


                    numbers.Add(valeuraseparer.Substring(valeuraseparer.Length - i, 3));

                }
                i = i + 3;
            }
            foreach (var item in numbers)
            {
                resultat = item + " " + resultat;
            }
            if (valeuraseparer.Length % 3 != 0)
            {
                resultat = valeuraseparer.Substring(0, valeuraseparer.Length % 3) + " " + resultat;
            }


            return resultat;
        }

        

        

    }
}




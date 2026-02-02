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
using iText.Kernel.Pdf.Event;
using iText.Kernel.Pdf.Canvas;
using PdfiumViewer;
using iText.Kernel.Colors;



namespace BANA.PDF
{
    public class PdfGeneration4
    {
        private readonly FactureContext _context;
    
        public PdfGeneration4(FactureContext context)
        { 
          _context=context;
        }

        public class LogoEventHandler : AbstractPdfDocumentEventHandler
        {
            private readonly ImageData logoImage;

            public LogoEventHandler(string logoPath)
            {
                logoImage = ImageDataFactory.Create(logoPath);
            }

            // Implémentation de la méthode abstraite OnAcceptedEvent
            
            protected override void OnAcceptedEvent(AbstractPdfDocumentEvent evt)
            {
                var docEvent = evt as PdfDocumentEvent;
                if (docEvent == null) return;

                var page = docEvent.GetPage();
                var pdfDoc = docEvent.GetDocument();
                var pageSize = page.GetPageSize();

                var canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

                // Position du logo (en haut à droite)

                float x = pageSize.GetRight();
                float y = pageSize.GetTop();

                canvas.AddImageFittedIntoRectangle(logoImage, new Rectangle(x, y), false);
            }
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
                iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(writer);
                Document document = new Document(pdfDocument,iText.Kernel.Geom.PageSize.A4,false);
                document.SetMargins(118, 10, 10, 10);


                MemoryStream workStream2 = new MemoryStream();

                PdfFont font1 = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);
                PdfFont font2 = PdfFontFactory.CreateFont(StandardFonts.COURIER);

                // Charger l'image (ex: PNG, JPG)
                string imagePath = "wwwroot/img1/pppp.jpg";
                if(facture.Type.Contains("dentiste")){imagePath = "wwwroot/img1/00pppp.jpg";}
               
                pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE,new LogoEventHandler(imagePath));
                
    

                Paragraph p = new Paragraph("").SetFontSize(20).SetTextAlignment(TextAlignment.CENTER).SetFont(font1).SetMarginTop(0).SetMarginBottom(0);

                // Créer un tableau avec 3 colonnes
                Table table = new Table(new float[] { 200f, 10f, 200f });
                table.SetWidth(UnitValue.CreatePercentValue(100)); // 100% de la largeur de la page

                
                // Données à encoder dans le QR Code
                string qrContent = "https://cm.edoctor-tim.com/dmi/ppoitiers/"+facture.Numero_dossier+"/"+facture.Numero_de_facture;

                // Générer le QR Code
                BarcodeQRCode qrCode = new BarcodeQRCode(qrContent);
                Image qrImage = new Image(qrCode.CreateFormXObject(pdfDocument));

                // Redimensionner (facultatif)
                qrImage.SetWidth(85);
                qrImage.SetHeight(85);

                qrImage.SetFixedPosition(1, 199, 628);
                
                // Ajouter l'image au document
                document.Add(qrImage);



                // Cellule d’en-tête avec hauteur fixe
                Cell cell1 = new Cell()
                        .Add(new Paragraph("REçU FACT Nº: #".ToUpper()+facture.Numero_de_facture ))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font2)
                        .SetFontSize(12)
                        .SetBorderBottom(Border.NO_BORDER);
                table.AddCell(cell1);
                     cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                string patient=facture.Nom +" "+facture.Prenom;
                cell1 = new Cell()
                        .Add(new Paragraph(patient.ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetBorderBottom(Border.NO_BORDER);
                table.AddCell(cell1);

                // Cellule d’en-tête avec hauteur fixe
                 cell1 = new Cell()
                 .Add(new Paragraph("Dossier: "+facture.Numero_dossier))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font2)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);
                     cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Téléphone: ".ToUpper() + facture.Phone))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetFont(font1)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);

                // Cellule d’en-tête avec hauteur fixe
                 cell1 = new Cell()
                        .Add(new Paragraph("Prescripteur: "+facture.Medecin))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font2)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);
                     cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Age Patient: ".ToUpper() + (int)((DateTime.Now - facture.DateNaissance).TotalDays / 365) + " ans " + (int)((DateTime.Now - facture.DateNaissance).TotalDays % 365) / 30 + " mois " + (int)(DateTime.Now - facture.DateNaissance).TotalDays % 365 % 30 + " jours "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetFont(font1)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(facture.Create_date.ToLongDateString()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font2)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);

                
                cell1 = new Cell()
                        .Add(new Paragraph("SEXE: " + facture.Genre))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetFont(font1)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Adresse: " + facture.Quartier))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetFont(font1)
                        .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);

               document.Add(new Paragraph(" "));
               document.Add(new Paragraph(" "));
               document.Add(table);

               Table table1 = new Table(new float[] { 300f, 50f, 80f, 80f, 100f });
               //ßtable1.SetWidth(100);

               cell1 = new Cell()
                        .Add(new Paragraph("désignation".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font1);
                        table1.AddCell(cell1);

                 cell1 = new Cell()
                        .Add(new Paragraph("cote".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(12)
                        .SetFont(font1);
                        table1.AddCell(cell1);

                 cell1 = new Cell()
                        .Add(new Paragraph("montant".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingRight(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFontSize(12)
                        .SetFont(font1);
                        table1.AddCell(cell1);

                 cell1 = new Cell()
                        .Add(new Paragraph("quantité".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(12)
                        .SetFont(font1);
                        table1.AddCell(cell1);
                
                 cell1 = new Cell()
                        .Add(new Paragraph("total".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingRight(5)
                        .SetFontSize(12)
                        .SetFont(font1);
                        table1.AddCell(cell1);
                document.Add(new Paragraph(" "));
                

                for (int k = 0; k <liste_de_facturation1.Count-1; k+=7)
                {
                    cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[k].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font2)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                        table1.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[k+1].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font2)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                        table1.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[k+4].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingRight(5)
                        .SetFontSize(12)
                        .SetFont(font2)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                        table1.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[k+5].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font2)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                        table1.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[k+6].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingRight(5)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font2)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                        table1.AddCell(cell1);

                }

                cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetPadding(0)
                        .SetFontSize(1)
                        .SetBorderTop(Border.NO_BORDER);
                        table1.AddCell(cell1);
                        cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetPadding(0)
                        .SetFontSize(1)
                        .SetBorderTop(Border.NO_BORDER);
                        table1.AddCell(cell1);
                        cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetPadding(0)
                        .SetFontSize(1)
                        .SetBorderTop(Border.NO_BORDER);
                        table1.AddCell(cell1);
                        cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetPadding(0)
                        .SetFontSize(1)
                        .SetBorderTop(Border.NO_BORDER);
                        table1.AddCell(cell1);
                        cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetPadding(0)
                        .SetFontSize(1)
                        .SetBorderTop(Border.NO_BORDER);
                        table1.AddCell(cell1);

                document.Add(table1);
                document.Add(new Paragraph(" "));
                

                // Créer un code-barres 128
                Barcode128 barcode = new Barcode128(pdfDocument);
                barcode.SetCode(facture.Numero_de_facture);
                barcode.SetCodeType(Barcode128.CODE128);

                // ✅ Créer l’image du code-barres avec tous les paramètres requis
                Image barcodeImage = new Image(barcode.CreateFormXObject(
                    ColorConstants.BLACK,      // Couleur du code-barres
                    ColorConstants.WHITE,      // Fond blanc
                    pdfDocument                        // PdfDocument, requis ici
                ));

                // Ajouter à la page
                //document.Add(new Paragraph("Code-barres :"));
                //document.Add(barcodeImage);
               
                
                



                

                Table table2 = new Table(new float[] { 150f, 147f});
                table2.SetHorizontalAlignment(HorizontalAlignment.RIGHT); // Alignement à droite
                                                                          //table2.SetMarginTop(-100);
                
                Table table02 = new Table(new float[] { 150f, 147f});
                table02.SetHorizontalAlignment(HorizontalAlignment.RIGHT); // Alignement à droite

                cell1 = new Cell()
                        .Add(new Paragraph("TOTAL HT: ".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.LEFT);
                        table2.AddCell(cell1);
                        table02.AddCell(cell1);
                        

                        cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Total_ht.ToString()) + "FCFA"))
                        .SetPadding(0)
                        .SetPaddingRight(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.RIGHT);
                        table2.AddCell(cell1);
                        table02.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph("TVA: ".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.LEFT);
                        table2.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph("Exonérée"))
                        .SetPadding(0)
                        .SetPaddingRight(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.RIGHT);
                        table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("TOTAl TTC: ".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.LEFT);
                        table2.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Total_ht.ToString()) + "FCFA"))
                        .SetPadding(0)
                        .SetPaddingRight(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.RIGHT);
                        table2.AddCell(cell1);

                if (facture.Type!=null)
                {
                    if (facture.Type.Contains("assurance"))
                    {
                        cell1 = new Cell()
                        .Add(new Paragraph("Net à Payer Assur: ".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.LEFT);
                        table2.AddCell(cell1);
                        table02.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Net_a_payer_assurance.ToString()) + "FCFA"))
                        .SetPadding(0)
                        .SetPaddingRight(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.RIGHT);
                        table2.AddCell(cell1);
                        table02.AddCell(cell1);
                    }
                    
                }

                if (facture.Remise>0)
                {
                   cell1 = new Cell()
                        .Add(new Paragraph("Remise: ".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.LEFT);
                        table2.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Remise.ToString()) + "FCFA"))
                        .SetPadding(0)
                        .SetPaddingRight(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.RIGHT);
                        table2.AddCell(cell1);
                }


                if (facture.ticketmoderateur > 0)
                {
                    cell1 = new Cell()
                         .Add(new Paragraph("Ticket Modérateur: ".ToUpper()))
                         .SetPadding(0)
                         .SetPaddingLeft(5)
                         .SetFontSize(12)
                         .SetFont(font1)
                         .SetTextAlignment(TextAlignment.LEFT);
                    table2.AddCell(cell1);
                    table02.AddCell(cell1);

                    cell1 = new Cell()
                    .Add(new Paragraph(separateur_de_millier(facture.ticketmoderateur.ToString()) + "FCFA"))
                    .SetPadding(0)
                    .SetPaddingRight(5)
                    .SetFontSize(12)
                    .SetFont(font1)
                    .SetTextAlignment(TextAlignment.RIGHT);
                    table2.AddCell(cell1);
                    table02.AddCell(cell1);


                    cell1 = new Cell()
                        .Add(new Paragraph("Complément patient: ".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.LEFT);
                    table2.AddCell(cell1);
                    table02.AddCell(cell1);

                    cell1 = new Cell()
                    .Add(new Paragraph(separateur_de_millier((facture.Net_a_payer_patient - facture.ticketmoderateur + facture.Remise).ToString()) + "FCFA"))
                    .SetPadding(0)
                    .SetPaddingRight(5)
                    .SetFontSize(12)
                    .SetFont(font1)
                    .SetTextAlignment(TextAlignment.RIGHT);
                    table2.AddCell(cell1);
                    table02.AddCell(cell1);
                }

                cell1 = new Cell()
                        .Add(new Paragraph("Net à Payer: ".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.LEFT);
                        table2.AddCell(cell1);
                        table02.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Net_a_payer_patient.ToString()) + "FCFA"))
                        .SetPadding(0)
                        .SetPaddingRight(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.RIGHT);
                        table2.AddCell(cell1);
                        table02.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Montant payé: ".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.LEFT);
                        table2.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Montant_recu_patient.ToString()) + "FCFA"))
                        .SetPadding(0)
                        .SetPaddingRight(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.RIGHT);
                        table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Reste à payé: ".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.LEFT);
                        table2.AddCell(cell1);

                        cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier((facture.Net_a_payer_patient-facture.Montant_recu_patient).ToString()) + "FCFA"))
                        .SetPadding(0)
                        .SetPaddingRight(5)
                        .SetFontSize(12)
                        .SetFont(font1)
                        .SetTextAlignment(TextAlignment.RIGHT);
                        table2.AddCell(cell1);



                Table table03 = new([800,500]);
                //table03.SetMarginTop(-75);
                cell1 = new Cell()
                        .SetPadding(0)
                        .SetBorder(Border.NO_BORDER);


                p = new Paragraph(facture.Montant_en_lettre_patient.ToUpper()).SetFontSize(10).SetTextAlignment(TextAlignment.LEFT).SetFont(font1).SetMarginTop(0).SetMarginBottom(0);
                cell1.Add(p);
                p = new Paragraph("Facture générée le "+facture.Create_date.ToString("dd/MM/yyyy")+" "+ facture.Create_date.ToShortTimeString()).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font1).SetMarginTop(0).SetMarginBottom(0);
                cell1.Add(p);
                p = new Paragraph("Facturé par: "+facture.Facture_par).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font1).SetMarginTop(0).SetMarginBottom(0);
                cell1.Add(p);
                p = new Paragraph("Encaissée par: "+facture.Encaisse_par).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font1).SetMarginTop(0).SetMarginBottom(0);
                cell1.Add(p);
                p = new Paragraph("Nous vous souhaitons un prompt retablissement").SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font1).SetMarginTop(0).SetMarginBottom(0);
                cell1.Add(p);
                if(controla>0){
                    p = new Paragraph("Date Validité Consultations: "+facture.Create_date.AddDays(14).ToString("dd/MM/yyyy")+" " +facture.Create_date.AddDays(14).ToShortTimeString()).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font1).SetMarginTop(0).SetMarginBottom(0);
                    cell1.Add(p);
                }else{
                    p = new Paragraph(" "+facture.Create_date.AddDays(14).ToString("dd/MM/yyyy")+" " +facture.Create_date.AddDays(14).ToShortTimeString()).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font1).SetMarginTop(0).SetMarginBottom(0);
                    cell1.Add(p);
                }
                cell1.Add(barcodeImage);

                // Chemin vers l’image (locale ou absolue)
                string imagePath1 = "wwwroot/img1/edr.png";

                // Charger l’image
                ImageData imageData = ImageDataFactory.Create(imagePath1);
                Image image = new Image(imageData);

                // Redimensionner si nécessaire
                image.SetWidth(10);  // ou .ScaleToFit(200, 100);

                // Ajouter l’image au document
                cell1.Add(image);
                p = new Paragraph("© 2025 e-Dr Tim Hospital Management. Tous droits réservés.\nCe document est strictement confidentiel et destiné uniquement à son destinataire.").SetFontSize(5).SetTextAlignment(TextAlignment.LEFT).SetFontColor(ColorConstants.GRAY).SetFont(font1).SetMarginTop(-13).SetMarginLeft(15);
                    cell1.Add(p);
                //document.Add(p);


                table03.AddCell(cell1);
                
                Cell cell2=new();
                cell2.Add(table2)
                .SetBorder(Border.NO_BORDER);
                //cell1.Add(table3);
                table03.AddCell(cell2);

                

                Div blocIndivisible = new Div()
                      .SetKeepTogether(true) // Empêche la coupure
                       .Add(table03);
                document.Add(blocIndivisible);

                

               //liste_de_facturation1
                document.Add(new Paragraph("-----------------------------------------------------------------------------------------------------------------------------------------------"));
                
                document.Add(table);
                document.Add(new Paragraph(" "));
                document.Add(table1);
                if (facture.Type.Contains("assurance"))
                {
                    document.Add(new Paragraph("Assurance: "+facture.Assureur).SetFont(font1));
                }
                document.Add(table02);
                document.Add(barcodeImage);
                

            document.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream2.Write(byteInfo, 0, byteInfo.Length);
            workStream2.Position = 0;
            


            // 2. Sauvegarder dans un fichier temporaire
                string tempPath = Path.Combine(Path.GetTempPath(), "doc_temp.pdf");
                File.WriteAllBytes(tempPath, workStream2.ToArray());

                // 3. Impression avec SumatraPDF sur imprimante physique
                string sumatraExe = @"C:\Users\administrateur.POITIERS\AppData\Local\SumatraPDF\SumatraPDF.exe"; // À adapter
                string nomImprimante = @"\\192.168.100.252\print_dr_tim1"; // Nom exact de l’imprimante installée

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








    




        private  string separateur_de_millier(string valeuraseparer)
        {
            int i=0;
            string resultat="";
            var numbers = new List<string>();
            while (valeuraseparer.Length-i>=0)
            {
               if(i!=0)
               {


                    numbers.Add(valeuraseparer.Substring(valeuraseparer.Length-i,3));
                    
               }
               i=i+3; 
            }
            foreach (var item in numbers)
            {
                 resultat= item+" "+resultat;
            }
            if(valeuraseparer.Length%3!=0){
                resultat=valeuraseparer.Substring(0,valeuraseparer.Length%3)+" "+resultat;
            }
            
           
            return resultat;
        }

        

        

    }
}




using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BANA.Models;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf.Event;
using iText.IO.Image;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.IO.Font.Constants;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Barcodes;
using iText.Layout.Borders;
using iText.Kernel.Colors;


namespace BANA.PDF
{
    public class PdfGeneration2
    {
        private readonly FactureContext _context;
    
        public PdfGeneration2(FactureContext context)
        { 
          _context=context;
        }


        public class LogoEventHandler : AbstractPdfDocumentEventHandler
        {
            private readonly ImageData logoImage;
            private readonly ImageData brouillonImage;

            public LogoEventHandler(string logoPath,string brouillonPath)
           {
                logoImage = ImageDataFactory.Create(logoPath);
                brouillonImage = ImageDataFactory.Create(brouillonPath);
                
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
                canvas.AddImageFittedIntoRectangle(brouillonImage, new Rectangle(200, 400, 150, 45), false);
                
            }
        }


        public class PageNumberingHandler : AbstractPdfDocumentEventHandler
        {
            private readonly PdfFont font;
            private readonly PdfFormXObject placeholder;

            public PageNumberingHandler(PdfFont font, PdfFormXObject placeholder)
            {
                this.font = font;
                this.placeholder = placeholder;
            }

            protected override void OnAcceptedEvent(AbstractPdfDocumentEvent evt)
            {
                var docEvent = evt as PdfDocumentEvent;
                var page = docEvent.GetPage();
                var pageNumber = docEvent.GetDocument().GetPageNumber(page);
                var pageSize = page.GetPageSize();

                float x = (pageSize.GetLeft() + pageSize.GetRight()+40) / 2;
                float y = pageSize.GetBottom() + 20;

                var canvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), docEvent.GetDocument());
                canvas.BeginText()
                    .SetFontAndSize(font, 8)
                    .MoveText(x - 40, y)
                    .ShowText($"Page {pageNumber} / ")
                    .EndText();

                canvas.AddXObjectAt(placeholder, x - 60 + 50, y);
            }
        }


        public async Task<MemoryStream> DetailsFacture(int id,string target,string patient){
            

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
            int total_ligne=liste_de_facturation1.Count()+liste_de_facturation2.Count()+liste_de_facturation3.Count()+liste_de_facturation4.Count();


            using (MemoryStream workStream = new MemoryStream())
        {
            
                PdfWriter writer = new PdfWriter(workStream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                Document document = new Document(pdfDocument,iText.Kernel.Geom.PageSize.A4,false);
                document.SetMargins(130, 10, 20, 10);


                MemoryStream workStream2 = new MemoryStream();

                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var placeholder = new PdfFormXObject(new Rectangle(0, 0, 50, 12));

                // Ajouter le gestionnaire de numérotation
                pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageNumberingHandler(font, placeholder));


                string imagePath = "wwwroot/img1/pppp.jpg"; // Remplace par le chemin de ton image
                string brouillonPath = "wwwroot/img/brouillon.png";
                if (facture.Type.Contains("dentiste")) { imagePath = "wwwroot/img1/00pppp.jpg"; }
                if (facture.Encaisse_par!="" && facture.Encaisse_par!=null) { brouillonPath = "wwwroot/img1/sign2.png"; }
                // Ajout du gestionnaire d'événements pour ajouter un logo sur chaque page
                pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE,new LogoEventHandler(imagePath,brouillonPath));
                

                
                // Données à encoder dans le QR Code
                string qrContent = "https://cm.edoctor-tim.com/dmi/ppoitiers/"+facture.Numero_dossier+"/"+facture.Numero_de_facture;

                // Générer le QR Code
                BarcodeQRCode qrCode = new BarcodeQRCode(qrContent);
                Image qrImage = new Image(qrCode.CreateFormXObject(pdfDocument));

                // Redimensionner (facultatif)
                qrImage.SetWidth(80);
                qrImage.SetHeight(80);

                //qrImage.SetFixedPosition(1, 301, 640);
             

               


            Table table = new Table(new float[] { 100f,100f });
                table.SetWidth(UnitValue.CreatePercentValue(40)); 

            Cell cell1 = new Cell()
                        .Add(new Paragraph("FACTURE Nº:"))
                        .SetPadding(0)
                        .SetFont(boldFont)
                        .SetPaddingLeft(5)
                        .SetFontSize(10);
                        if(facture.Type.Contains("proforma")){
                        cell1 = new Cell()
                        .Add(new Paragraph("PROFORMA Nº:"))
                        .SetPadding(0)
                        .SetFont(boldFont)
                        .SetPaddingLeft(5)
                        .SetFontSize(10);}
                table.AddCell(cell1);

             cell1 = new Cell()
                        .Add(new Paragraph(facture.Numero_de_facture))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10);
                table.AddCell(cell1);

            cell1 = new Cell()
                        .Add(new Paragraph("AGE PATIENT:"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(10);
                table.AddCell(cell1);
            
            cell1 = new Cell()
                        .Add(new Paragraph((int)((DateTime.Now-facture.DateNaissance).TotalDays/365) +" ans"+  (int)(((DateTime.Now-facture.DateNaissance).TotalDays%365)/30) +" mois" +(((int)(DateTime.Now-facture.DateNaissance).TotalDays)%365)%30 +"jours"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10);
                table.AddCell(cell1);

            cell1 = new Cell()
                        .Add(new Paragraph("NUMÉRO DOSSIER:"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(10);
                table.AddCell(cell1);

            cell1 = new Cell()
                        .Add(new Paragraph(facture.Numero_dossier))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10);
                table.AddCell(cell1);

            if(facture.Date_sortie>facture.Date_entree){
                cell1 = new Cell()
                        .Add(new Paragraph("DATE D'ENTRÉE:"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(10);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(DateTime.Parse(facture.Date_entree.ToString()).ToShortDateString() ))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("DATE DE SORTIE:"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(10);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(DateTime.Parse(facture.Date_sortie.ToString()).ToShortDateString()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10);
                table.AddCell(cell1);

            }else{
                cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10)
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10)
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10)
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                        cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10)
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                
                

            }

                Table table1 = new Table(1);
                table1.SetWidth(UnitValue.CreatePercentValue(55)); 
                table1.SetHorizontalAlignment(HorizontalAlignment.RIGHT); // Alignement à droite
                table1.SetFixedPosition(1, 290, 663, 290); // page 1, position x=100, y=500, largeur=200 

                

                cell1 = new Cell()
                        .Add(new Paragraph((facture.Nom + " " + facture.Prenom).ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetFontSize(10)
                        .SetBorderBottom(Border.NO_BORDER);
                table1.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("TÉLÉPHONE: " + facture.Phone + " "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetBorderTop(Border.NO_BORDER)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetFontSize(10);
                table1.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph("Adresse: " + facture.Quartier + " "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetFontSize(10)
                        .SetBorderTop(Border.NO_BORDER);
                table1.AddCell(cell1);

               

                document.Add(table);
                document.Add(table1);

                Paragraph p = new Paragraph("Prescripteur: "+facture.Medecin).SetFontSize(10).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);

                p = new Paragraph(facture.Create_date.ToLongDateString()).SetFontSize(10).SetTextAlignment(TextAlignment.RIGHT).SetFont(font).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);


                

                int st=0;
                Table table2 = new Table(new float[] { 300,55,70,10,100});
                //table2.SetWidth(UnitValue.CreatePercentValue(100)); 
            if(liste_de_facturation1.Count()>0){
                cell1 = new Cell(1,5)
                        .Add(new Paragraph(facture.Titre1.ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                    cell1 = new Cell()
                        .Add(new Paragraph("Désignation".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Cote".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Montant".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Quantité".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Total".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);
            }
                

                for (int t = 0; t < liste_de_facturation1.Count-1; t+=7)
                {
                    cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[t].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[t+1].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(liste_de_facturation1[t + 4])))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[t+5].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[t + 6].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                    st+=int.Parse(liste_de_facturation1[t+6].Replace(" ",""));
                }
                
                if(st>0){
                    cell1 = new Cell(1,4)
                        .Add(new Paragraph("SOUS TOTAL".ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                        table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(st.ToString())))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                        table2.AddCell(cell1);
                }
                
                st=0;



                if(liste_de_facturation2.Count()>0){
                    cell1 = new Cell(1,5)
                        .Add(new Paragraph(facture.Titre2.ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Désignation".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Cote".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Montant".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Quantité".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Total".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);
            }



                for (int t = 0; t < liste_de_facturation2.Count-1; t+=7)
                {
                    cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation2[t].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation2[t+1].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(liste_de_facturation2[t + 4])))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation2[t+5].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation2[t + 6].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                st+=int.Parse(liste_de_facturation2[t+6].Replace(" ",""));
                }

                if(st>0){
                    cell1 = new Cell(1,4)
                        .Add(new Paragraph("SOUS TOTAL".ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                        table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(st.ToString())))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                        table2.AddCell(cell1);
                }

                 
                st=0;




                if(liste_de_facturation3.Count()>0){
                    cell1 = new Cell(1,5)
                        .Add(new Paragraph(facture.Titre3.ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Désignation".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Cote".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Montant".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetPaddingRight(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                        
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Quantité".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Total".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);
            }


                for (int t = 0; t < liste_de_facturation3.Count-1; t+=7)
                {
                    cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation3[t].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation3[t+1].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(liste_de_facturation3[t + 4])))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetPaddingRight(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation3[t+5].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation3[t + 6].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);
                st+=int.Parse(liste_de_facturation3[t+6].Replace(" ",""));

                }

                if(st>0){
                    cell1 = new Cell(1,4)
                        .Add(new Paragraph("SOUS TOTAL".ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                        table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(st.ToString())))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                        table2.AddCell(cell1);
                }
                st=0;



                if(liste_de_facturation4.Count()>0){
                    cell1 = new Cell(1,5)
                        .Add(new Paragraph(facture.Titre4.ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Désignation".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Cote".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Montant".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Quantité".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Total".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);
            }



                for (int t = 0; t < liste_de_facturation4.Count-1; t+=7)
                {
                    cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation4[t].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation4[t+1].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(liste_de_facturation4[t + 4])))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation4[t+5].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation4[t + 6].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                       
                table2.AddCell(cell1);
                st+=int.Parse(liste_de_facturation4[t+6].Replace(" ",""));
                
                }

                if(st>0){
                    cell1 = new Cell(1,4)
                        .Add(new Paragraph("SOUS TOTAL".ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                        table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(st.ToString())))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                        table2.AddCell(cell1);
                }
                st=0;


                document.Add(table2);
                //document.Add(qrImage);

                Table table3 = new Table([160,100]);
                table3.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
                //table3.SetMarginTop(-84);

                cell1 = new Cell()
                        .Add(new Paragraph("TOTAL HT:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Total_ht.ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("TVA:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Exonéré"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("TOTAl TTC:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Total_ht.ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

        if (facture.Type.Contains("assurance"))
          {
                if (facture.Total_ht-facture.Net_a_payer_patient-facture.Remise>0)
                { 
                  cell1 = new Cell()
                        .Add(new Paragraph("Net à Payer Assurance:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier((facture.Total_ht - facture.Net_a_payer_patient - facture.Remise).ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);
                }
                
                
                if (facture.ticketmoderateur>0)
                {
                  cell1 = new Cell()
                        .Add(new Paragraph("Ticket Modérateur:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.ticketmoderateur.ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetPaddingRight(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);
                }


                if (facture.Net_a_payer_patient+facture.Remise-facture.ticketmoderateur > 0)
                { 
                      cell1 = new Cell()
                        .Add(new Paragraph("Complément patient:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                 cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier((facture.Net_a_payer_patient + facture.Remise - facture.ticketmoderateur).ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table3.AddCell(cell1);  
                }
          }

                if (facture.Remise > 0)
                { 
                  cell1 = new Cell()
                        .Add(new Paragraph("Remise:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Remise.ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table3.AddCell(cell1);
                }

                

                cell1 = new Cell()
                        .Add(new Paragraph("Net à Payer Patient:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Net_a_payer_patient.ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Montant payé:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Montant_recu_patient.ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Reste à payé:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier((facture.Net_a_payer_patient - facture.Montant_recu_patient).ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetPaddingRight(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);


                Table table03 = new([800,500]);
                                table03.SetMarginTop(-75);
                cell1 = new Cell()
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                //table03.AddCell(cell1);
                //cell1.Add(qrCode);
                p = new Paragraph("Facture générée le "+facture.Create_date).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginBottom(0).SetMarginLeft(80).SetMarginTop(0);
                
                if(facture.Type.Contains("proforma")){
                        p = new Paragraph("Proforma générée le "+facture.Create_date).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginBottom(0).SetMarginLeft(80).SetMarginTop(0);
                }

                cell1.Add(p)
                .SetBorder(Border.NO_BORDER);
                                
                p = new Paragraph("Éditée par: " + facture.Facture_par).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginBottom(0).SetMarginLeft(80).SetMarginTop(0);
                //document.Add(p);
                cell1.Add(p)
                .SetBorder(Border.NO_BORDER);
                p = new Paragraph("Encaissée par: " +facture.Encaisse_par).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginBottom(0).SetMarginLeft(80).SetMarginTop(0);
                //document.Add(p);
                cell1.Add(p)
                .SetBorder(Border.NO_BORDER);
                p = new Paragraph("Nous vous souhaitons un prompt retablissement").SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginLeft(80).SetMarginTop(0).SetMarginBottom(0);
                //document.Add(p);
                cell1.Add(p)
                .SetBorder(Border.NO_BORDER);
                
                cell1.Add(new Paragraph(". ")).SetBorder(Border.NO_BORDER).SetPadding(7);
                 p = new Paragraph(facture.Montant_en_lettre_patient.ToUpper()).SetFontSize(9).SetTextAlignment(TextAlignment.LEFT).SetFont(boldFont).SetMarginBottom(0);
                //document.Add(p);
                cell1.Add(p)
                .SetBorder(Border.NO_BORDER);
                table03.AddCell(cell1);

                Cell cell2=new();
                cell2.Add(table3)
                .SetBorder(Border.NO_BORDER);
                //cell1.Add(table3);
                table03.AddCell(cell2);



                Div blocIndivisible = new Div()
                      .SetKeepTogether(true) // Empêche la coupure
                      .Add(qrImage)
                       .Add(table03);
                document.Add(blocIndivisible);

                //cell1.Add(blocIndivisible);

                
                

                

                               // document.Add(blocIndivisible);

                                // Écrire le nombre total de pages dans le placeholder
                                var canvas = new PdfCanvas(placeholder, pdfDocument);
                canvas.BeginText()
                    .SetFontAndSize(font, 8)
                    .MoveText(0, 0)
                    .ShowText(pdfDocument.GetNumberOfPages().ToString())
                    .EndText();
                





                     




                                document.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream2.Write(byteInfo, 0, byteInfo.Length);
            workStream2.Position = 0;

            return workStream2;  
        }
            //return new FileStreamResult(workStream, "application/pdf");  
    }




 public async Task<MemoryStream> DetailsFusionFacture(int id,string target,string patient){
            
            List<string> liste_des_factures = new List<string>();
            int k=0;
            string resultat="";
            foreach(char c in target) {
              
              if(c.Equals('λ')){
                  if(k!=0){
                      liste_des_factures.Add(resultat);
                      resultat="";
                  }
                  
              }else{
                  resultat+=c.ToString();
              }
              k++;
            }

            var facture = new Facture();
            if(liste_des_factures.Count()>0){
                facture = await _context.Facture.FirstOrDefaultAsync(m => m.Id == int.Parse(liste_des_factures[0]));
            }
            foreach (var item in liste_des_factures)
            {
                if (item != liste_des_factures[0])
                {
                        facture.Ligne_facturation1 += _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Ligne_facturation1.Substring(1, _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Ligne_facturation1.Length - 1);
                        facture.Ligne_facturation2 += _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Ligne_facturation2.Substring(1, _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Ligne_facturation2.Length - 1);
                        facture.Ligne_facturation3 += _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Ligne_facturation3.Substring(1, _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Ligne_facturation3.Length - 1);
                        facture.Ligne_facturation4 += _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Ligne_facturation4.Substring(1, _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Ligne_facturation4.Length - 1);
                        facture.Net_a_payer_patient += _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Net_a_payer_patient;
                        facture.Net_a_payer_assurance += _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Net_a_payer_assurance;
                        facture.Remise += _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Remise;
                        facture.ticketmoderateur += _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).ticketmoderateur;
                        facture.Total_ht += _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Total_ht;
                        facture.Total_ttc += _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Total_ttc;
                        facture.Taxe1 += _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Taxe1;
                        facture.Montant_recu_patient += _context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Montant_recu_patient;
                        if (facture.Create_date<_context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Create_date)
                        {
                            facture.Create_date=_context.Facture.FirstOrDefault(m => m.Id == int.Parse(item)).Create_date;
                        }
                        
                       
                }
            }

             
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
            //int total_ligne=liste_de_facturation1.Count()+liste_de_facturation2.Count()+liste_de_facturation3.Count()+liste_de_facturation4.Count();
            

            using (MemoryStream workStream = new MemoryStream())
        {
            
                PdfWriter writer = new PdfWriter(workStream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                Document document = new Document(pdfDocument,iText.Kernel.Geom.PageSize.A4,false);
                document.SetMargins(130, 10, 20, 10);


                MemoryStream workStream2 = new MemoryStream();

                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var placeholder = new PdfFormXObject(new Rectangle(0, 0, 50, 12));

                // Ajouter le gestionnaire de numérotation
                pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageNumberingHandler(font, placeholder));


                string imagePath = "wwwroot/img1/pppp.jpg"; // Remplace par le chemin de ton image
                string brouillonPath = "wwwroot/img/brouillon.png";
                if (facture.Type.Contains("dentiste")) { imagePath = "wwwroot/img1/00pppp.jpg"; }
                if (facture.Encaisse_par!="" && facture.Encaisse_par!=null || facture.Type.Contains("proforma")) { brouillonPath = "wwwroot/img1/sign2.png"; }
                
                // Ajout du gestionnaire d'événements pour ajouter un logo sur chaque page
                                pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE,new LogoEventHandler(imagePath,brouillonPath));
                

                
                // Données à encoder dans le QR Code
                string qrContent = "https://cm.edoctor-tim.com/dmi/ppoitiers/"+facture.Numero_dossier+"/"+facture.Numero_de_facture;

                // Générer le QR Code
                BarcodeQRCode qrCode = new BarcodeQRCode(qrContent);
                Image qrImage = new Image(qrCode.CreateFormXObject(pdfDocument));

                // Redimensionner (facultatif)
                qrImage.SetWidth(80);
                qrImage.SetHeight(80);

                //qrImage.SetFixedPosition(1, 301, 640);
             

               


            Table table = new Table(new float[] { 100f,100f });
                table.SetWidth(UnitValue.CreatePercentValue(40)); 

            Cell cell1 = new Cell()
                        .Add(new Paragraph("FACTURE Nº:"))
                        .SetPadding(0)
                        .SetFont(boldFont)
                        .SetPaddingLeft(5)
                        .SetFontSize(10);
                        if(facture.Type.Contains("proforma")){
                        cell1 = new Cell()
                        .Add(new Paragraph("PROFORMA Nº:"))
                        .SetPadding(0)
                        .SetFont(boldFont)
                        .SetPaddingLeft(5)
                        .SetFontSize(10);}
                table.AddCell(cell1);

             cell1 = new Cell()
                        .Add(new Paragraph(facture.Numero_de_facture))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10);
                table.AddCell(cell1);

            cell1 = new Cell()
                        .Add(new Paragraph("AGE PATIENT:"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(10);
                table.AddCell(cell1);
            
            cell1 = new Cell()
                        .Add(new Paragraph((int)((DateTime.Now-facture.DateNaissance).TotalDays/365) +" ans"+  (int)(((DateTime.Now-facture.DateNaissance).TotalDays%365)/30) +" mois" +(((int)(DateTime.Now-facture.DateNaissance).TotalDays)%365)%30 +"jours"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10);
                table.AddCell(cell1);

            cell1 = new Cell()
                        .Add(new Paragraph("NUMÉRO DOSSIER:"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(10);
                table.AddCell(cell1);

            cell1 = new Cell()
                        .Add(new Paragraph(facture.Numero_dossier))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10);
                table.AddCell(cell1);

            if(facture.Date_sortie>facture.Date_entree){
                cell1 = new Cell()
                        .Add(new Paragraph("DATE D'ENTRÉE:"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(10);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(DateTime.Parse(facture.Date_entree.ToString()).ToShortDateString() ))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("DATE DE SORTIE:"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(10);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(DateTime.Parse(facture.Date_sortie.ToString()).ToShortDateString()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10);
                table.AddCell(cell1);

            }else{
                cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10)
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10)
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10)
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                        cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(10)
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                
                

            }

                Table table1 = new Table(1);
                table1.SetWidth(UnitValue.CreatePercentValue(55)); 
                table1.SetHorizontalAlignment(HorizontalAlignment.RIGHT); // Alignement à droite
                table1.SetFixedPosition(1, 290, 663, 290); // page 1, position x=100, y=500, largeur=200 

                

                cell1 = new Cell()
                        .Add(new Paragraph((facture.Nom + " " + facture.Prenom).ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetFontSize(10)
                        .SetBorderBottom(Border.NO_BORDER);
                table1.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("TÉLÉPHONE: " + facture.Phone + " "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetBorderTop(Border.NO_BORDER)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetFontSize(10);
                table1.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph("Adresse: " + facture.Quartier + " "))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetFontSize(10)
                        .SetBorderTop(Border.NO_BORDER);
                table1.AddCell(cell1);

               

                document.Add(table);
                document.Add(table1);

                Paragraph p = new Paragraph("Prescripteur: "+facture.Medecin).SetFontSize(10).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);

                p = new Paragraph(facture.Create_date.ToLongDateString()).SetFontSize(10).SetTextAlignment(TextAlignment.RIGHT).SetFont(font).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);


                

                int st=0;
                Table table2 = new Table(new float[] { 300,55,70,10,100});
                //table2.SetWidth(UnitValue.CreatePercentValue(100)); 
            if(liste_de_facturation1.Count()>0){
                cell1 = new Cell(1,5)
                        .Add(new Paragraph(facture.Titre1.ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                    cell1 = new Cell()
                        .Add(new Paragraph("Désignation".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Cote".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Montant".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Quantité".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Total".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);
            }
                

                for (int t = 0; t < liste_de_facturation1.Count-1; t+=7)
                {
                    cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[t].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[t+1].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(liste_de_facturation1[t + 4])))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[t+5].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation1[t + 6].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                    st+=int.Parse(liste_de_facturation1[t+6].Replace(" ",""));
                }
                
                if(st>0){
                    cell1 = new Cell(1,4)
                        .Add(new Paragraph("SOUS TOTAL".ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                        table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(st.ToString())))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                        table2.AddCell(cell1);
                }
                
                st=0;



                if(liste_de_facturation2.Count()>0){
                    cell1 = new Cell(1,5)
                        .Add(new Paragraph(facture.Titre2.ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Désignation".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Cote".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Montant".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Quantité".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Total".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);
            }



                for (int t = 0; t < liste_de_facturation2.Count-1; t+=7)
                {
                    cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation2[t].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation2[t+1].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(liste_de_facturation2[t + 4])))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation2[t+5].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation2[t + 6].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                st+=int.Parse(liste_de_facturation2[t+6].Replace(" ",""));
                }

                if(st>0){
                    cell1 = new Cell(1,4)
                        .Add(new Paragraph("SOUS TOTAL".ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                        table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(st.ToString())))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                        table2.AddCell(cell1);
                }

                 
                st=0;




                if(liste_de_facturation3.Count()>0){
                    cell1 = new Cell(1,5)
                        .Add(new Paragraph(facture.Titre3.ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Désignation".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Cote".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Montant".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetPaddingRight(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                        
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Quantité".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Total".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);
            }


                for (int t = 0; t < liste_de_facturation3.Count-1; t+=7)
                {
                    cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation3[t].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation3[t+1].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(liste_de_facturation3[t + 4])))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetPaddingRight(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation3[t+5].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation3[t + 6].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);
                st+=int.Parse(liste_de_facturation3[t+6].Replace(" ",""));

                }

                if(st>0){
                    cell1 = new Cell(1,4)
                        .Add(new Paragraph("SOUS TOTAL".ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                        table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(st.ToString())))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                        table2.AddCell(cell1);
                }
                st=0;



                if(liste_de_facturation4.Count()>0){
                    cell1 = new Cell(1,5)
                        .Add(new Paragraph(facture.Titre4.ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Désignation".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Cote".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Montant".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Quantité".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Total".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);
            }



                for (int t = 0; t < liste_de_facturation4.Count-1; t+=7)
                {
                    cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation4[t].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation4[t+1].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(liste_de_facturation4[t + 4])))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation4[t+5].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(liste_de_facturation4[t + 6].ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                       
                table2.AddCell(cell1);
                st+=int.Parse(liste_de_facturation4[t+6].Replace(" ",""));
                
                }

                if(st>0){
                    cell1 = new Cell(1,4)
                        .Add(new Paragraph("SOUS TOTAL".ToUpper()))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                        table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(st.ToString())))
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                        table2.AddCell(cell1);
                }
                st=0;


                document.Add(table2);
                //document.Add(qrImage);

                Table table3 = new Table([160,100]);
                table3.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
                //table3.SetMarginTop(-84);

                cell1 = new Cell()
                        .Add(new Paragraph("TOTAL HT:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Total_ht.ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("TVA:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Exonéré"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("TOTAl TTC:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Total_ht.ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

        if (facture.Type.Contains("assurance"))
          {
                if (facture.Total_ht-facture.Net_a_payer_patient-facture.Remise>0)
                { 
                  cell1 = new Cell()
                        .Add(new Paragraph("Net à Payer Assurance:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier((facture.Total_ht - facture.Net_a_payer_patient - facture.Remise).ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);
                }
                
                
                if (facture.ticketmoderateur>0)
                {
                  cell1 = new Cell()
                        .Add(new Paragraph("Ticket Modérateur:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.ticketmoderateur.ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetPaddingRight(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);
                }


                if (facture.Net_a_payer_patient+facture.Remise-facture.ticketmoderateur > 0)
                { 
                      cell1 = new Cell()
                        .Add(new Paragraph("Complément patient:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                 cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier((facture.Net_a_payer_patient + facture.Remise - facture.ticketmoderateur).ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table3.AddCell(cell1);  
                }
          }

                if (facture.Remise > 0)
                { 
                  cell1 = new Cell()
                        .Add(new Paragraph("Remise:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Remise.ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table3.AddCell(cell1);
                }

                

                cell1 = new Cell()
                        .Add(new Paragraph("Net à Payer Patient:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Net_a_payer_patient.ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Montant payé:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(facture.Montant_recu_patient.ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Reste à payé:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier((facture.Net_a_payer_patient - facture.Montant_recu_patient).ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetPaddingRight(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);


                Table table03 = new([800,500]);
                                table03.SetMarginTop(-75);
                cell1 = new Cell()
                        .SetPadding(0)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                //table03.AddCell(cell1);
                //cell1.Add(qrCode);
                p = new Paragraph("Facture générée le "+facture.Create_date).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginBottom(0).SetMarginLeft(80).SetMarginTop(0);
                
                if(facture.Type.Contains("proforma")){
                        p = new Paragraph("Proforma générée le "+facture.Create_date).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginBottom(0).SetMarginLeft(80).SetMarginTop(0);
                }

                cell1.Add(p)
                .SetBorder(Border.NO_BORDER);
                                
                p = new Paragraph("Éditée par: " + facture.Facture_par).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginBottom(0).SetMarginLeft(80).SetMarginTop(0);
                //document.Add(p);
                cell1.Add(p)
                .SetBorder(Border.NO_BORDER);
                p = new Paragraph("Encaissée par: " +facture.Encaisse_par).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginBottom(0).SetMarginLeft(80).SetMarginTop(0);
                //document.Add(p);
                cell1.Add(p)
                .SetBorder(Border.NO_BORDER);
                p = new Paragraph("Nous vous souhaitons un prompt retablissement").SetFontSize(8).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginLeft(80).SetMarginTop(0).SetMarginBottom(0);
                //document.Add(p);
                cell1.Add(p)
                .SetBorder(Border.NO_BORDER);
                
                cell1.Add(new Paragraph(". ")).SetBorder(Border.NO_BORDER).SetPadding(7);
                 p = new Paragraph(" ").SetFontSize(9).SetTextAlignment(TextAlignment.LEFT).SetFont(boldFont).SetMarginBottom(0);
                //document.Add(p);
                cell1.Add(p)
                .SetBorder(Border.NO_BORDER);
                table03.AddCell(cell1);

                Cell cell2=new();
                cell2.Add(table3)
                .SetBorder(Border.NO_BORDER);
                //cell1.Add(table3);
                table03.AddCell(cell2);



                Div blocIndivisible = new Div()
                      .SetKeepTogether(true) // Empêche la coupure
                      .Add(qrImage)
                       .Add(table03);
                document.Add(blocIndivisible);

                //cell1.Add(blocIndivisible);

                
                

                

                               // document.Add(blocIndivisible);

                                // Écrire le nombre total de pages dans le placeholder
                                var canvas = new PdfCanvas(placeholder, pdfDocument);
                canvas.BeginText()
                    .SetFontAndSize(font, 8)
                    .MoveText(0, 0)
                    .ShowText(pdfDocument.GetNumberOfPages().ToString())
                    .EndText();
                





                     




                                document.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream2.Write(byteInfo, 0, byteInfo.Length);
            workStream2.Position = 0;

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




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
using Microsoft.AspNetCore.ResponseCompression;


namespace BANA.PDF
{
    public class PdfGeneration02
    {
        private readonly FactureContext _context;
    
        public PdfGeneration02(FactureContext context)
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
                document.SetMargins(130, 10, 30, 10);


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
             

               


            Table table = new Table(new float[] { 100f,230f });
                //table.SetWidth(UnitValue.CreatePercentValue(50)); 

            Cell cell1 = new Cell()
                        .Add(new Paragraph("FACTURE Nº:"))
                        .SetPadding(0)
                        .SetFont(boldFont)
                        .SetPaddingLeft(5)
                        .SetFontSize(9);
                if(facture.Type.Contains("proforma")){
                        cell1 = new Cell()
                        .Add(new Paragraph("PROFORMA Nº:"))
                        .SetPadding(0)
                        .SetFont(boldFont)
                        .SetPaddingLeft(5)
                        .SetFontSize(9);}
                table.AddCell(cell1);

             cell1 = new Cell()
                        .Add(new Paragraph(facture.Numero_de_facture))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(9);
                table.AddCell(cell1);

            cell1 = new Cell()
                        .Add(new Paragraph("Assuré Prin:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table.AddCell(cell1);
            
            cell1 = new Cell()
                        .Add(new Paragraph(facture.assure_prin))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(9);
                table.AddCell(cell1);

            cell1 = new Cell()
                        .Add(new Paragraph("Bénéficiaire:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table.AddCell(cell1);
            
            cell1 = new Cell()
                        .Add(new Paragraph(facture.Nom +" "+facture.Prenom))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(9);
                table.AddCell(cell1);

                 cell1 = new Cell()
                        .Add(new Paragraph("SOCIÉTÉ:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table.AddCell(cell1);
            
            cell1 = new Cell()
                        .Add(new Paragraph(facture.Societe))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(9);
                table.AddCell(cell1);
                
            if(facture.Date_sortie>facture.Date_entree){
                cell1 = new Cell()
                        .Add(new Paragraph("DATE D'ENTRÉE:"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(DateTime.Parse(facture.Date_entree.ToString()).ToShortDateString() ))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(9);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("DATE DE SORTIE:"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(DateTime.Parse(facture.Date_sortie.ToString()).ToShortDateString()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(9);
                table.AddCell(cell1);

            }else{
                cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetPadding(7)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(9)
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetPadding(7)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(9)
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetPadding(7)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(9)
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                        cell1 = new Cell()
                        .Add(new Paragraph(" "))
                        .SetPadding(7)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetFontSize(9)
                        .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);
                
                

            }

                Table table1 = new Table(1);
                table1.SetWidth(UnitValue.CreatePercentValue(40)); 
                table1.SetHorizontalAlignment(HorizontalAlignment.RIGHT); // Alignement à droite
                table1.SetMarginTop(-85);
                //table1.SetFixedPosition(1, 310, 695, 270); // page 1, position x=100, y=500, largeur=200 
                                int targ = 0;
              

                

                cell1 = new Cell()
                        .Add(new Paragraph(facture.Assureur.ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(boldFont)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetFontSize(10)
                        .SetBorderBottom(Border.NO_BORDER);
                table1.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(facture.Assur_NIU+""))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetBorderTop(Border.NO_BORDER)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetFontSize(10);
                if (facture.Assur_NIU!= null &&facture.Assur_NIU.Trim().Length>1){table1.AddCell(cell1);}else{ targ++; }
                
                 cell1 = new Cell()
                        .Add(new Paragraph(facture.Assur_Rc+""))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetBorderTop(Border.NO_BORDER)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetFontSize(10);
                if (facture.Assur_Rc!= null && facture.Assur_Rc.Trim().Length>1){table1.AddCell(cell1);}else{ targ++; }

                 cell1 = new Cell()
                        .Add(new Paragraph(facture.Assur_BP+""))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetBorderTop(Border.NO_BORDER)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetFontSize(10);
                if (facture.Assur_BP!= null && facture.Assur_BP.Trim().Length>1){table1.AddCell(cell1);}else{ targ++; }
                 cell1 = new Cell()
                        .Add(new Paragraph(facture.Assur_Tel+""))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetBorderTop(Border.NO_BORDER)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetFontSize(10);
                if (facture.Assur_Tel!= null && facture.Assur_Tel.Trim().Length>1){table1.AddCell(cell1);}else{ targ++; }
                 cell1 = new Cell()
                        .Add(new Paragraph(facture.Assur_Adresse+""))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetBorderTop(Border.NO_BORDER)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetFontSize(10);
                if (facture.Assur_Adresse!= null && facture.Assur_Adresse.Trim().Length>1){table1.AddCell(cell1);}else{ targ++; }
                

                for (int t = 0; t < targ; t++)
                {
                        cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetPadding(7)
                        .SetPaddingLeft(5)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                        table1.AddCell(cell1);
                       
                }
                cell1 = new Cell()
                        .Add(new Paragraph(""))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(font)
                        .SetBorder(new DashedBorder(ColorConstants.BLACK, 0.5f))
                        .SetBorderTop(Border.NO_BORDER)
                        .SetFontSize(10);
                table1.AddCell(cell1);
                

                
                

                document.Add(table);
                document.Add(table1);

                Paragraph p = new Paragraph("MAT PATIENT: "+facture.Matricule_patient +",          MAT ADH:"+facture.MatriculeADH+",          Nº DOSSIER:"+facture.Numero_dossier).SetFontSize(7).SetTextAlignment(TextAlignment.LEFT).SetFont(font).SetMarginTop(0).SetMarginBottom(0);            
                document.Add(p);

                p = new Paragraph(facture.Create_date.ToLongDateString()).SetFontSize(10).SetTextAlignment(TextAlignment.RIGHT).SetFont(font).SetMarginTop(0).SetMarginBottom(0);
                document.Add(p);


                

                int st=0;
                Table table2 = new Table(new float[] { 300,55,10,70,70,10,100});
                //table2.SetWidth(UnitValue.CreatePercentValue(100)); 
            if(liste_de_facturation1.Count()>0){
                cell1 = new Cell(1,7)
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
                        .Add(new Paragraph("Assurance".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph("Plafond".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
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
                        .Add(new Paragraph(liste_de_facturation1[t + 2]+"%".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(liste_de_facturation1[t + 3])))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
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
                    cell1 = new Cell(1,6)
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
                    cell1 = new Cell(1,7)
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
                        .Add(new Paragraph("Assurance".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph("Plafond".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
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
                        .Add(new Paragraph(liste_de_facturation2[t + 2]+"%".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(liste_de_facturation2[t + 3])))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
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
                    cell1 = new Cell(1,6)
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
                    cell1 = new Cell(1,7)
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
                        .Add(new Paragraph("Assurance".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph("Plafond".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
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
                        .Add(new Paragraph(liste_de_facturation3[t + 2]+"%".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(liste_de_facturation3[t + 3])))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
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
                    cell1 = new Cell(1,6)
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
                    cell1 = new Cell(1,7)
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
                        .Add(new Paragraph("Assurance".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table2.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph("Plafond".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
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
                        .Add(new Paragraph(liste_de_facturation4[t + 2]+"%".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table2.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(liste_de_facturation4[t + 3])))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(font)
                        .SetPaddingRight(5)
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
                    cell1 = new Cell(1,6)
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

                Table table3 = new([160,100]);
                table3.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
                //table3.SetMarginTop(-80);

                
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
                
                cell1 = new Cell()
                        .Add(new Paragraph("Net à Payer Patient:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier((facture.Net_a_payer_patient+facture.Remise).ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                if (facture.Taxe1 > 0 || facture.Taxe1 < 0)
                {
                        cell1 = new Cell()
                        .Add(new Paragraph("BASE IMPOSABLE:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier((facture.Total_ht-facture.Net_a_payer_patient-facture.Remise).ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table3.AddCell(cell1);
                cell1 = new Cell()
                        .Add(new Paragraph("TAXE "+facture.Taxe1+" %"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(((int)((facture.Total_ht-facture.Net_a_payer_patient-facture.Remise)*facture.Taxe1*0.01)).ToString()) + " FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFont(boldFont)
                        .SetPaddingRight(5)
                        .SetFontSize(9);
                table3.AddCell(cell1);
                
                
                
                }

                if ((int)(facture.Total_ht - facture.Net_a_payer_patient - facture.Remise + (facture.Total_ht - facture.Net_a_payer_patient - facture.Remise) * facture.Taxe1 * 0.01) > 0)
                {
                        cell1 = new Cell()
                        .Add(new Paragraph("TOTAl TTC:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier((facture.Net_a_payer_assurance+facture.Net_a_payer_patient+facture.Remise).ToString())+" FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);
                }
                   cell1 = new Cell()
                        .Add(new Paragraph("NET À PAYER ASSURANCE:".ToUpper()))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(boldFont)
                        .SetFontSize(9);
                table3.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph(separateur_de_millier(((int)(facture.Total_ht-facture.Net_a_payer_patient-facture.Remise+(facture.Total_ht-facture.Net_a_payer_patient-facture.Remise)*facture.Taxe1*0.01)).ToString())+" FCFA"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetPaddingRight(5)
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
                 p = new Paragraph(facture.Montant_en_lettre_assurance.ToUpper()).SetFontSize(9).SetTextAlignment(TextAlignment.LEFT).SetFont(boldFont).SetMarginBottom(0);
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
                

                
                // Créer les deux mots soulignés
                Text mot1 = new Text("Signature de l'assuré(e)").SetFont(boldFont).SetFontSize(10).SetUnderline();
                Text mot2 = new Text("La direction").SetFont(boldFont).SetFontSize(10).SetUnderline();

                // Les ajouter dans un paragraphe avec un espace entre
                 p = new Paragraph()
                .Add(mot1)
                .Add("                                                                                                                       ") // espace entre les deux mots
                .Add(mot2);
                document.Add(new Paragraph(" "));
                document.Add(p);



               

               
                
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




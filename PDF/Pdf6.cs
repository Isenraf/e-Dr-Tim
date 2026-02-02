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
using iTextSharp.text.pdf.qrcode;
using iText.IO.Font;
using iText.Forms.Fields;


namespace BANA.PDF
{
    public class PdfGeneration6
    {
        private readonly ResultatContext _context;
    
        public PdfGeneration6(ResultatContext context)
        { 
          _context=context;
        }



        public async Task<MemoryStream> CouvertureResultat(int id,string target){
            

            var resultat = await _context.Resultat
                .FirstOrDefaultAsync(m => m.Id == id);
            var resultats= _context.Resultat.Where(x => x.NumeroFacture == resultat.NumeroFacture).ToList();


           
           


            using (MemoryStream workStream = new MemoryStream())
                        {

                                PdfWriter writer = new PdfWriter(workStream);
                                PdfDocument pdfDocument = new PdfDocument(writer);
                                Document document = new Document(pdfDocument, iText.Kernel.Geom.PageSize.A4, false);

                                if (target == "A4")
                                {
                                        document = new Document(pdfDocument, iText.Kernel.Geom.PageSize.A4.Rotate(), false);
                                }
                                document.SetMargins(30, 30, 30, 30);


                                MemoryStream workStream2 = new MemoryStream();

                                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);


                                // Chemin vers l'image
                                string imagePath = "wwwroot/img1/p1.png"; // Exemple : "assets/logo.png"

                                // Charger l'image
                                ImageData imageData = ImageDataFactory.Create(imagePath);
                                Image image = new Image(imageData);

                                // Redimensionner ou positionner si nécessaire
                                image.SetWidth(150); // largeur en points
                                image.SetHorizontalAlignment(HorizontalAlignment.CENTER);




                                PdfPage page = pdfDocument.AddNewPage();
                                PdfCanvas canvas = new PdfCanvas(page);

                                // Dimensions de la cellule simulée
                                float x = 30;
                                float y = 590;
                                float width = 520;
                                float height = 80;
                                float radius = 10;
                                if (target == "A4") { x = 60; y = 320; width = 720; }

                                // Dessiner un rectangle à coins arrondis
                                canvas.SetFillColor(new DeviceRgb(0, 168, 82))
                .RoundRectangle(x, y, width, height, radius)
                .Fill();

                                if (target == "A4")
                                {
                                        if (resultat.Categorie == "Imagerie Médicale")
                                        {
                                                canvas.BeginText()
                                                .SetFontAndSize(font, 25)
                                                .MoveText(x + 10, y + 25) // position interne au rectangle
                                                .SetFillColor(ColorConstants.WHITE)
                                                .ShowText(resultat.Nom.ToUpper())
                                                .EndText();
                                        }
                                        else
                                        {
                                                canvas.BeginText()
                                                .SetFontAndSize(font, 25)
                                                .MoveText(x + 10, y + 45) // position interne au rectangle
                                                .SetFillColor(ColorConstants.WHITE)
                                                .ShowText("RÉSULTAT EXAMEN LABORATOIRE")
                                                .EndText();
                                        }

                                }
                                else
                                {
                                        if (resultat.Categorie == "Imagerie Médicale")
                                        {
                                                canvas.BeginText()
                                                .SetFontAndSize(font, 25)
                                                .MoveText(x + 10, y + 25) // position interne au rectangle
                                                .SetFillColor(ColorConstants.WHITE)
                                                .ShowText(resultat.Nom.ToUpper())
                                                .EndText();
                                        }
                                        else
                                        {
                                                canvas.BeginText()
                                                 .SetFontAndSize(font, 25)
                                                 .MoveText(x + 10, y + 45) // position interne au rectangle
                                                 .SetFillColor(ColorConstants.WHITE)
                                                 .ShowText("RÉSULTAT EXAMEN LABORATOIRE")
                                                 .EndText();
                                        }

                                }


                                document.Add(new Paragraph(" "));
                                document.Add(image);


                                // Données à encoder dans le QR Code
                                string qrContent = "https://cm.edoctor-tim.com/dmi/ppoitiers/" + resultat.NumeroDossier + "/" + resultat.NumeroFacture;

                                // Définir les couleurs (violet sur fond blanc)
                                DeviceRgb violet = new DeviceRgb(128, 0, 128);  // Couleur du QR code
                                DeviceRgb blanc = new DeviceRgb(255, 255, 255); // Couleur du fond

                                // Générer le QR Code
                                BarcodeQRCode qrCode = new BarcodeQRCode(qrContent);
                                Image qrImage = new Image(qrCode.CreateFormXObject(ColorConstants.BLACK, pdfDocument));


                                // Redimensionner (facultatif)
                                qrImage.SetWidth(80);
                                qrImage.SetHeight(80);



                                Table table = new Table(new float[] { 100f, 300f, 100f });
                                table.SetWidth(UnitValue.CreatePercentValue(100)); // 100% de la largeur de la page
                                table.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                                var couleurPerso = new DeviceRgb(0, 168, 82);
                                var couleurPerso2 = new DeviceRgb(232, 240, 255);

                                // Charger une image
                                string imagePath2 = "wwwroot/img/pf.png"; // Par exemple : "logo.png"
                                if (resultat.Genre == "M") { imagePath2 = "wwwroot/img/pm.png"; }
                                ImageData imageData2 = ImageDataFactory.Create(imagePath2);
                                Image image2 = new Image(imageData2);
                                image2.SetWidth(80);

                                // Redimensionner l’image si nécessaire
                                image.SetAutoScale(true); // ou image.ScaleToFit(100, 100);


                                Table table1 = new(3);
                                table1.SetWidth(UnitValue.CreatePercentValue(100));

                                Cell cell11 = new Cell(1, 3)
                                        .Add(new Paragraph(resultat.NomPatient.ToUpper()))
                                        .SetPadding(0)
                                        .SetFont(boldFont)
                                        .SetFontColor(couleurPerso)
                                        .SetBorder(Border.NO_BORDER);
                                table1.AddCell(cell11);
                                cell11 = new Cell()
                                        .Add(new Paragraph("Âge patient(e)"))
                                        .SetPadding(0)
                                        .SetFont(boldFont)
                                        .SetFontColor(ColorConstants.BLACK)
                                        .SetBorder(Border.NO_BORDER);
                                table1.AddCell(cell11);

                                cell11 = new Cell()
                                        .Add(new Paragraph("Sexe"))
                                        .SetPadding(0)
                                        .SetFont(boldFont)
                                        .SetFontColor(ColorConstants.BLACK)
                                        .SetBorder(Border.NO_BORDER);
                                table1.AddCell(cell11);

                                cell11 = new Cell()
                                        .Add(new Paragraph("Téléphone"))
                                        .SetPadding(0)
                                        .SetFont(boldFont)
                                        .SetFontColor(ColorConstants.BLACK)
                                        .SetBorder(Border.NO_BORDER);
                                table1.AddCell(cell11);

                                cell11 = new Cell()
                                        .Add(new Paragraph((int)((DateTime.Now - resultat.Date_de_naissance).TotalDays / 365) + " ans " + (int)((DateTime.Now - resultat.Date_de_naissance).TotalDays % 365) / 30 + " mois " + (int)(DateTime.Now - resultat.Date_de_naissance).TotalDays % 365 % 30 + " jours "))
                                        .SetPadding(0)
                                        .SetFontSize(12)
                                        .SetFontColor(ColorConstants.BLACK)
                                        .SetFont(font)
                                        .SetBorder(Border.NO_BORDER);
                                table1.AddCell(cell11);

                                cell11 = new Cell()
                                        .Add(new Paragraph(resultat.Genre))
                                        .SetPadding(0)
                                        .SetFontSize(12)
                                        .SetFontColor(ColorConstants.BLACK)
                                        .SetFont(font)
                                        .SetBorder(Border.NO_BORDER);
                                table1.AddCell(cell11);

                                cell11 = new Cell()
                                        .Add(new Paragraph(resultat.Telephone_patient))
                                        .SetPadding(0)
                                        .SetFontSize(12)
                                        .SetFontColor(ColorConstants.BLACK)
                                        .SetFont(font)
                                        .SetBorder(Border.NO_BORDER);
                                table1.AddCell(cell11);

                                // Cellule d’en-tête avec hauteur fixe
                                Cell cell1 = new Cell()
                        .Add(image2)
                        .SetPadding(0)
                        .SetFont(font)
                        .SetBorder(Border.NO_BORDER);
                                table.AddCell(cell1);


                                cell1 = new Cell()
                                       .Add(table1)
                                       .SetPadding(5)
                                       .SetFont(boldFont)
                                       .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                                       .SetFontSize(16)
                                       .SetBackgroundColor(couleurPerso2)
                                       .SetBorder(Border.NO_BORDER);
                                table.AddCell(cell1);
                                // Cellule d’en-tête avec hauteur fixe
                                cell1 = new Cell()
                                       .Add(qrImage)
                                       .SetPadding(0)
                                       .SetFont(font)
                                       .SetBorder(Border.NO_BORDER);
                                table.AddCell(cell1);

                                //table.AddCell(cell1);

                                

                                Paragraph p1 = new Paragraph("")
                                .SetFontColor(ColorConstants.BLACK)
                                .SetTextAlignment(TextAlignment.LEFT)
                                .SetFontSize(10);
                                foreach (var item in resultats)
                                {
                                        p1.Add(item.Nom+", "); 
                                }
                                Console.WriteLine(resultat.Categorie);
                                if (resultat.Categorie != "Imagerie Médicale")
                                {
                                        document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));
                                        document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));
                                        document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));
                                        p1.SetMarginLeft(10);
                                        p1.SetMarginRight(10);
                                        if (target == "A4")
                                        {
                                                document.Add(new Paragraph(" "));
                                                document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));
                                                p1.SetMarginLeft(40);
                                                p1.SetMarginRight(40);
                                        }
                                        document.Add(p1);

                                        document.Add(new Paragraph(" ")); document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));
                                        document.Add(new Paragraph(" ")); document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));

                                        document.Add(table);
                                }
                                else
                                {
                                       document.Add(new Paragraph(" ")); document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));
                                        document.Add(new Paragraph(" ")); document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));
                                        document.Add(new Paragraph(" ")); document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));
                                        document.Add(new Paragraph(" ")); document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));
                                        document.Add(new Paragraph(" "));

                                        if (target == "A4")
                                        {
                                                document.Add(new Paragraph(" "));document.Add(new Paragraph(" "));document.Add(new Paragraph(" "));
                                                document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));
                                                
                                        }

                                        document.Add(table); 

                                        

                                }

                                if (target == "A4")
                                {
                                        document.Add(new Paragraph(" "));
                                        document.Add(new Paragraph(" ")); document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));
                                }
                                

                               

                                Paragraph p = new Paragraph("Site Web: www.polycliniquedepoitiers.com, Sise à vallé Trois Boutiques ( entre Feu Rouge BESSENGUE et rond point Deido) Douala-Cameroun.  Tél: 233410151/ 690073467/ 675018191/ 656238323/ 680586697/ 677459997")
                                .SetFontColor(ColorConstants.BLACK)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(9);
                                document.Add(p);



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




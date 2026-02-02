using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BANA.Models;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Html2pdf;
using iText.IO.Image;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Event;
using iText.Commons.Actions;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Barcodes;
using System.Text.RegularExpressions;
using System.Text;





namespace BANA.PDF
{
    public class PdfGeneration3
    {
        private readonly ResultatContext _context;
         private readonly DoctorContext _context_doctor;

        public PdfGeneration3(ResultatContext context, DoctorContext context_doctor)
        {
            _context = context;
            _context_doctor = context_doctor;
        }


        public class LogoEventHandler : AbstractPdfDocumentEventHandler
        {
            private readonly ImageData logoImage;
            private readonly ImageData signature;
            private readonly ImageData brouillon;

            public LogoEventHandler(string logoPath, string logoPath2, string logoPath3)
            {
                logoImage = ImageDataFactory.Create(logoPath);
                signature = ImageDataFactory.Create(logoPath2);
                brouillon = ImageDataFactory.Create(logoPath3);
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
                canvas.AddImageFittedIntoRectangle(signature, new Rectangle(350, 50, 200, 115), false);
                canvas.AddImageFittedIntoRectangle(brouillon, new Rectangle(200, 400,150,45), false);
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

                float x = (pageSize.GetLeft() + pageSize.GetRight() + 40) / 2;
                float y = pageSize.GetBottom() + 43;

                var canvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), docEvent.GetDocument());
                canvas.BeginText()
                    .SetFontAndSize(font, 8)
                    .MoveText(x - 40, y)
                    .ShowText($"Page {pageNumber} / ")
                    .EndText();

                canvas.AddXObjectAt(placeholder, x - 60 + 50, y);
            }
        }


        public class FooterParagraphHandler : AbstractPdfDocumentEventHandler
        {
            private readonly PdfFont font;
            private readonly PdfFormXObject placeholder;
           

            public FooterParagraphHandler(PdfFont font, PdfFormXObject placeholder)
            {
                this.font = font;
                this.placeholder = placeholder;
            }

            protected override void OnAcceptedEvent(AbstractPdfDocumentEvent evt)
            {

                var docEvent = evt as PdfDocumentEvent;
                var pdfDoc = docEvent.GetDocument();
                var page = docEvent.GetPage();
                Rectangle pageSize = page.GetPageSize();

                // Position en bas de page
                float x = pageSize.GetLeft() + 20;
                float y = pageSize.GetBottom() + 705;

                var canvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc);
                canvas.BeginText()
                    .SetFontAndSize(font, 8)
                    .SetFillColor(new DeviceRgb(0, 168, 82))
                    .MoveText(x - 40, y)
                    .ShowText($"")
                    .EndText();

                canvas.AddXObjectAt(placeholder, x - 60 + 50, y);
            }
        }





        public async Task<MemoryStream> DetailsResultat(int id, List<string> target)
        {




            var resultat = await _context.Resultat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resultat.Contenu==null){ resultat.Contenu = ""; }else if(resultat.Nom.ToLower().Contains("nfs")){ resultat.Contenu = resultat.Contenu.Replace("#126aef", "#ffffff"); }
            resultat.Contenu = Regex.Replace(resultat.Contenu, "<button.*?</button>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Console.WriteLine(resultat.Contenu);
            resultat.Contenu = ReplaceEmptyParagraphs(resultat.Contenu);

            if (resultat.Code == null) { resultat.Code = ""; }
            if (resultat.Cote == null) { resultat.Cote = ""; }
            List<string> liste_de_facturation1 = new List<string>();

            int i = 0;
            string result1 = "";
            foreach (char c in resultat.Cote)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation1.Add(result1);
                        result1 = "";
                    }

                }
                else
                {
                    result1 += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }

            List<string> liste_de_facturation2 = new List<string>();

            i = 0;
            result1 = "";
            foreach (char c in resultat.Code)
            {

                if (c.Equals('λ'))
                {
                    if (i != 0)
                    {
                        liste_de_facturation2.Add(result1);
                        result1 = "";
                    }

                }
                else
                {
                    result1 += c.ToString();
                }
                i++;
                //Console.WriteLine(i+"-"+result);
            }


            // Créer un MemoryStream pour générer le PDF en mémoire
            using (MemoryStream memoryStream = new MemoryStream())
            {
                if (target.Count() > 0)
                {
                    if (target[0] == "xxxx")
                    {
                        string ownerPassword = "02@Secure02POITIERS2025"; // mot de passe propriétaire

                        WriterProperties props1 = new WriterProperties()
                        .SetStandardEncryption(
                            null,
                            Encoding.UTF8.GetBytes(ownerPassword),
                            EncryptionConstants.ALLOW_FILL_IN | EncryptionConstants.ALLOW_MODIFY_ANNOTATIONS, // autoriser seulement les champs à remplir
                            EncryptionConstants.ENCRYPTION_AES_128 // type de chiffrement
                        );
                        target.Remove("xxxx");
                    }
                    
                }
                
                
                Console.WriteLine(target.Count());
                

                // Créer un PdfWriter pour écrire dans le MemoryStream
                PdfWriter writer = new PdfWriter(memoryStream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                Document document = new Document(pdfDocument, iText.Kernel.Geom.PageSize.A4, false);
                document.SetMargins(140, 10, 60, 10);
                if (resultat.Categorie=="Imagerie Médicale")
                {
                    document.SetMargins(140, 10, 60, 30);
                }
                var props = new ConverterProperties();


                MemoryStream workStream2 = new MemoryStream();

                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                PdfFont fontbold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var placeholder = new PdfFormXObject(new Rectangle(0, 0, 90, 12));
                var placeholder2 = new PdfFormXObject(new Rectangle(0, 0, 400, 60));

                // Ajouter le gestionnaire de numérotation
                pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageNumberingHandler(font, placeholder));
                pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterParagraphHandler(font, placeholder2));

                string imagePath = "wwwroot/img1/pppp2.jpg"; // Remplace par le chemin de ton image
                string signaturePath = "wwwroot/img1/sign2.png"; // Remplace par le chemin de ton image
                string brouillonPath = "wwwroot/img1/sign2.png"; // Remplace par le chemin de ton image
                                                                 // if (resultat.Categorie == "Analyse Médicale")
                                                                 // {
                                                                 // if (resultat.valide_par == "DR NGOUNGOURE")
                                                                 // {
                                                                 //     signaturePath = "wwwroot/img1/sign.png";
                                                                 // }

                var fileName = "";
                //var medecin = null;
                
                if (resultat.valide_par!=null && resultat.valide_par!="")
                {
                    var medecins = _context_doctor.Doctor.Where(x => x.User_Name == resultat.valide_par).ToList();
                    if (medecins!=null && medecins.Count()==1)
                    {
                        var medecin = medecins.FirstOrDefault();
                        fileName = medecin.Id + "_" + medecin.Nom.Replace(" ", "") + ".png";
                        signaturePath = "wwwroot/doctors/signatures/"+fileName;
                    }
                    
                }
                    
                    
                    

                //}

                if (resultat.valide_par==null)
                {
                    brouillonPath = "wwwroot/img/brouillon.png"; 
                }

                string imagePath2 = "wwwroot/img/rs.jpeg"; // Remplace par le chemin de ton image
                if (resultat.Genre == "F") { imagePath2 = "wwwroot/img/rs.jpeg"; }

                // Ajout du gestionnaire d'événements pour ajouter un logo sur chaque page
                pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, new LogoEventHandler(imagePath,signaturePath,brouillonPath));
                

                // Créer un tableau avec 3 colonnes
                Table table = new Table(new float[] { 200f, 50f, 150f });
                table.SetWidth(UnitValue.CreatePercentValue(100)); // 100% de la largeur de la page


                // Données à encoder dans le QR Code
                string qrContent = "https://cm.edoctor-tim.com/dmi/ppoitiers/" + resultat.NumeroDossier + "/" + resultat.NumeroFacture;

                // Générer le QR Code
                BarcodeQRCode qrCode = new BarcodeQRCode(qrContent);
                Image qrImage = new Image(qrCode.CreateFormXObject(pdfDocument));

                // Redimensionner (facultatif)
                qrImage.SetWidth(80);
                qrImage.SetHeight(80);

                qrImage.SetFixedPosition(1, 301, 630);

                if (resultat.Categorie=="Imagerie Médicale")
                {
                   qrImage.SetFixedPosition(1, 308, 630);
                }

                // Ajouter l'image au document
                document.Add(qrImage);


                // Charger l'image
                ImageData imageData = ImageDataFactory.Create(imagePath2);
                Image image = new Image(imageData);

                // Redimensionner si nécessaire
                image.ScaleToFit(60, 60); // taille max : 100x100 pts

                image.SetFixedPosition(1, 230, 650); // page 1

                // Ajouter l’image
                //document.Add(image);


                // Cellule d’en-tête avec hauteur fixe
                Cell cell1 = new Cell()
                        .Add(new Paragraph("Nom Patient: " + resultat.NomPatient))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                   .Add(new Paragraph(" "))
                   .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Résultat généré le: " + resultat.Date_de_finalisation))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER);
                table.AddCell(cell1);

                // Cellule d’en-tête avec hauteur fixe
                cell1 = new Cell()
                       .Add(new Paragraph("Age Patient: " + (int)((DateTime.Now - resultat.Date_de_naissance).TotalDays / 365) + " ans " + (int)((DateTime.Now - resultat.Date_de_naissance).TotalDays % 365) / 30 + " mois " + (int)(DateTime.Now - resultat.Date_de_naissance).TotalDays % 365 % 30 + " jours "))
                       .SetPadding(0)
                       .SetPaddingLeft(5)
                       .SetFontSize(9)
                       .SetBorderBottom(Border.NO_BORDER)
                       .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                   .Add(new Paragraph(" "))
                   .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Édité par: " + resultat.fait_par))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);

                // Cellule d’en-tête avec hauteur fixe
                cell1 = new Cell()
                       .Add(new Paragraph("Sexe Patient: " + resultat.Genre))
                       .SetPadding(0)
                       .SetPaddingLeft(5)
                       .SetFontSize(9)
                       .SetBorderBottom(Border.NO_BORDER)
                       .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                   .Add(new Paragraph(" "))
                   .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);

                cell1 = new Cell()
                        .Add(new Paragraph("Validé par: " + resultat.valide_par))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(9)
                        .SetBorderBottom(Border.NO_BORDER)
                        .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);

                // Cellule d’en-tête avec hauteur fixe
                cell1 = new Cell()
                       .Add(new Paragraph("Téléphone: " + resultat.Telephone_patient))
                       .SetPadding(0)
                       .SetPaddingLeft(5)
                       .SetFontSize(9)
                       .SetBorderBottom(Border.NO_BORDER)
                       .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                   .Add(new Paragraph(" "))
                   .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);

                cell1 = new Cell(2, 1)
                        .Add(new Paragraph("Prescripteur: " + resultat.Prescripteur))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFontSize(9)
                        .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);

                // Cellule d’en-tête avec hauteur fixe
                cell1 = new Cell()
                       .Add(new Paragraph("Numero dossier: " + resultat.NumeroDossier + ", Numero Facture: " + resultat.NumeroFacture))
                       .SetPadding(0)
                       .SetPaddingLeft(5)
                       .SetFontSize(9)
                       .SetBorderTop(Border.NO_BORDER);
                table.AddCell(cell1);
                cell1 = new Cell()
                   .Add(new Paragraph(" "))
                   .SetBorder(Border.NO_BORDER);
                table.AddCell(cell1);

                document.Add(table);
                document.Add(new Paragraph(" "));




                props.SetFontProvider(new DefaultFontProvider(true, true, true));

                var elements = HtmlConverter.ConvertToElements(resultat.Contenu, props);

                foreach (var element in elements)
                {
                    if (element is IBlockElement block)
                    {
                        // Créer un Div pour encapsuler le paragraphe et éviter la coupure
                        Div blocIndivisible = new Div()
                            .SetKeepTogether(true) // Empêche la coupure
                            .Add(block);
                            if (target.Count()==0)
                            {
                                document.Add(blocIndivisible);
                            }
                        //Console.WriteLine(element.GetOwnProperty(heigh));
                    }
                }

                if (liste_de_facturation1.Count()>1 && target.Count()==0)
                {
                    Table tablefinale = new(new float[] { 200f, 50f, 50f, 50f });

                    cell1 = new Cell(1, 4)
                       .Add(new Paragraph("ANTIBIOGRAMME"))
                       .SetPadding(0)
                       .SetPaddingLeft(5)
                       .SetTextAlignment(TextAlignment.CENTER)
                       .SetFont(font)
                       .SetFontSize(13);
                       tablefinale.AddCell(cell1);

                    cell1 = new Cell()
                        .Add(new Paragraph("Antibiotique"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(fontbold)
                        .SetFontSize(10);
                        tablefinale.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Sensible"))
                        .SetPadding(0)
                        .SetFont(fontbold)
                        .SetPaddingLeft(5)
                        .SetFontSize(10);
                        tablefinale.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Intermédiaire"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(fontbold)
                        .SetFontSize(10);
                        tablefinale.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Résistant"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(fontbold)
                        .SetFontSize(10);
                        tablefinale.AddCell(cell1);

                    for (int k = 1; k < liste_de_facturation1.Count() - 1; k += 2)
                    {
                        if (liste_de_facturation1[k + 1] == "Sensible")
                        {
                            cell1 = new Cell()
                            .Add(new Paragraph(liste_de_facturation1[k]))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph("X"))
                            .SetPadding(0)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                        }


                        if (liste_de_facturation1[k + 1] == "Intermédiaire")
                        {
                            cell1 = new Cell()
                            .Add(new Paragraph(liste_de_facturation1[k]))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph("X"))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                        }


                        if (liste_de_facturation1[k + 1] == "Résistant")
                        {
                            cell1 = new Cell()
                            .Add(new Paragraph(liste_de_facturation1[k]))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph("X"))
                            .SetPadding(0)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                        }
                    }

                    document.Add(tablefinale);
                                            
                    
                }

                document.Add(new Paragraph(" "));

                if (liste_de_facturation2.Count()>1 && target.Count()==0)
                {
                    Table tablefinale = new(new float[] { 200f, 50f, 50f, 50f });

                    cell1 = new Cell(1, 4)
                       .Add(new Paragraph("ANTIFONGIGRAMME"))
                       .SetPadding(0)
                       .SetPaddingLeft(5)
                       .SetTextAlignment(TextAlignment.CENTER)
                       .SetFont(font)
                       .SetFontSize(13);
                       tablefinale.AddCell(cell1);

                    cell1 = new Cell()
                        .Add(new Paragraph("Antibiotique"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(fontbold)
                        .SetFontSize(10);
                        tablefinale.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Sensible"))
                        .SetPadding(0)
                        .SetFont(fontbold)
                        .SetPaddingLeft(5)
                        .SetFontSize(10);
                        tablefinale.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Intermédiaire"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(fontbold)
                        .SetFontSize(10);
                        tablefinale.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Résistant"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(fontbold)
                        .SetFontSize(10);
                        tablefinale.AddCell(cell1);

                    for (int k = 1; k < liste_de_facturation2.Count() - 1; k += 2)
                    {
                        if (liste_de_facturation2[k + 1] == "Sensible")
                        {
                            cell1 = new Cell()
                            .Add(new Paragraph(liste_de_facturation2[k]))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph("X"))
                            .SetPadding(0)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                        }


                        if (liste_de_facturation2[k + 1] == "Intermédiaire")
                        {
                            cell1 = new Cell()
                            .Add(new Paragraph(liste_de_facturation2[k]))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph("X"))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                        }


                        if (liste_de_facturation2[k + 1] == "Résistant")
                        {
                            cell1 = new Cell()
                            .Add(new Paragraph(liste_de_facturation2[k]))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph("X"))
                            .SetPadding(0)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale.AddCell(cell1);

                        }
                    }

                    document.Add(tablefinale);
                                            
                    
                }




                if (target.Count() > 1)
                {
                    //document.Add(new Paragraph("-----------------------------------------------------------------------------------------------------------------------------------------------"));
                }

                int t = 1;
                foreach (var item in target)
                {


                        var result = await _context.Resultat
                        .FirstOrDefaultAsync(m => m.Id == int.Parse(item));
                        if (result.Contenu==null){ result.Contenu = ""; }else if(result.Nom.ToLower().Contains("nfs")){ result.Contenu = result.Contenu.Replace("#126aef", "#ffffff"); }
                        result.Contenu = Regex.Replace(result.Contenu, "<button.*?</button>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        resultat.Contenu = resultat.Contenu.Replace("</p><p></p><p>","<br/><br/><br/>").Replace("</p><p>","<br/>");

                        if (result.Code == null) { result.Code = ""; }
                        if (result.Cote == null) { result.Cote = ""; }

                        liste_de_facturation1 = new List<string>();
                         i = 0;
                         result1 = "";
                        foreach (char c in result.Cote)
                        {

                            if (c.Equals('λ'))
                            {
                                if (i != 0)
                                {
                                    liste_de_facturation1.Add(result1);
                                    result1 = "";
                                }

                            }
                            else
                            {
                                result1 += c.ToString();
                            }
                            i++;
                            //Console.WriteLine(i+"-"+result);
                        }

                        liste_de_facturation2 = new List<string>();

                        i = 0;
                        result1 = "";
                        foreach (char c in result.Code)
                        {

                            if (c.Equals('λ'))
                            {
                                if (i != 0)
                                {
                                    liste_de_facturation2.Add(result1);
                                    result1 = "";
                                }

                            }
                            else
                            {
                                result1 += c.ToString();
                            }
                            i++;
                            //Console.WriteLine(i+"-"+result);
                        }


                        var elements1 = HtmlConverter.ConvertToElements(result.Contenu, props);
                        foreach (var element in elements1)
                        {
                            if (element is IBlockElement block)
                            {
                                // Créer un Div pour encapsuler le paragraphe et éviter la coupure
                                Div blocIndivisible = new Div()
                                    .SetKeepTogether(true) // Empêche la coupure
                                    .Add(block);
                                document.Add(blocIndivisible);
                                //Console.WriteLine(element.GetOwnProperty(heigh));
                            }
                        }


                    if (liste_de_facturation1.Count() > 1)
                    {
                        Table tablefinale0 = new(new float[] { 200f, 50f, 50f, 50f });

                        cell1 = new Cell(1, 4)
                        .Add(new Paragraph("ANTIBIOGRAMME"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(13);
                        tablefinale0.AddCell(cell1);

                        cell1 = new Cell()
                            .Add(new Paragraph("Antibiotique"))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(fontbold)
                            .SetFontSize(10);
                        tablefinale0.AddCell(cell1);
                        cell1 = new Cell()
                            .Add(new Paragraph("Sensible"))
                            .SetPadding(0)
                            .SetFont(fontbold)
                            .SetPaddingLeft(5)
                            .SetFontSize(10);
                        tablefinale0.AddCell(cell1);
                        cell1 = new Cell()
                            .Add(new Paragraph("Intermédiaire"))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(fontbold)
                            .SetFontSize(10);
                        tablefinale0.AddCell(cell1);
                        cell1 = new Cell()
                            .Add(new Paragraph("Résistant"))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(fontbold)
                            .SetFontSize(10);
                        tablefinale0.AddCell(cell1);

                        for (int k = 1; k < liste_de_facturation1.Count() - 1; k += 2)
                        {
                            if (liste_de_facturation1[k + 1] == "Sensible")
                            {
                                cell1 = new Cell()
                                .Add(new Paragraph(liste_de_facturation1[k]))
                                .SetPadding(0)
                                .SetPaddingLeft(5)
                                .SetFont(font)
                                .SetFontSize(10);
                                tablefinale0.AddCell(cell1);

                                cell1 = new Cell()
                                .Add(new Paragraph("X"))
                                .SetPadding(0)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetPaddingLeft(5)
                                .SetFont(font)
                                .SetFontSize(10);
                                tablefinale0.AddCell(cell1);

                                cell1 = new Cell()
                                .Add(new Paragraph(" "))
                                .SetPadding(0)
                                .SetPaddingLeft(5)
                                .SetFont(font)
                                .SetFontSize(10);
                                tablefinale0.AddCell(cell1);

                                cell1 = new Cell()
                                .Add(new Paragraph(" "))
                                .SetPadding(0)
                                .SetPaddingLeft(5)
                                .SetFont(font)
                                .SetFontSize(10);
                                tablefinale0.AddCell(cell1);

                            }


                            if (liste_de_facturation1[k + 1] == "Intermédiaire")
                            {
                                cell1 = new Cell()
                                .Add(new Paragraph(liste_de_facturation1[k]))
                                .SetPadding(0)
                                .SetPaddingLeft(5)
                                .SetFont(font)
                                .SetFontSize(10);
                                tablefinale0.AddCell(cell1);

                                cell1 = new Cell()
                                .Add(new Paragraph(" "))
                                .SetPadding(0)
                                .SetPaddingLeft(5)
                                .SetFont(font)
                                .SetFontSize(10);
                                tablefinale0.AddCell(cell1);

                                cell1 = new Cell()
                                .Add(new Paragraph("X"))
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetPadding(0)
                                .SetPaddingLeft(5)
                                .SetFont(font)
                                .SetFontSize(10);
                                tablefinale0.AddCell(cell1);

                                cell1 = new Cell()
                                .Add(new Paragraph(" "))
                                .SetPadding(0)
                                .SetPaddingLeft(5)
                                .SetFont(font)
                                .SetFontSize(10);
                                tablefinale0.AddCell(cell1);

                            }


                            if (liste_de_facturation1[k + 1] == "Résistant")
                            {
                                cell1 = new Cell()
                                .Add(new Paragraph(liste_de_facturation1[k]))
                                .SetPadding(0)
                                .SetPaddingLeft(5)
                                .SetFont(font)
                                .SetFontSize(10);
                                tablefinale0.AddCell(cell1);

                                cell1 = new Cell()
                                .Add(new Paragraph(" "))
                                .SetPadding(0)
                                .SetPaddingLeft(5)
                                .SetFont(font)
                                .SetFontSize(10);
                                tablefinale0.AddCell(cell1);

                                cell1 = new Cell()
                                .Add(new Paragraph(" "))
                                .SetPadding(0)
                                .SetPaddingLeft(5)
                                .SetFont(font)
                                .SetFontSize(10);
                                tablefinale0.AddCell(cell1);

                                cell1 = new Cell()
                                .Add(new Paragraph("X"))
                                .SetPadding(0)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetPaddingLeft(5)
                                .SetFont(font)
                                .SetFontSize(10);
                                tablefinale0.AddCell(cell1);

                            }

                        }
                        document.Add(tablefinale0);
                    }

                 document.Add(new Paragraph(" "));

                if (liste_de_facturation2.Count()>1)
                {
                    Table tablefinale1 = new(new float[] { 200f, 50f, 50f, 50f });

                    cell1 = new Cell(1, 4)
                       .Add(new Paragraph("ANTIFONGIGRAMME"))
                       .SetPadding(0)
                       .SetPaddingLeft(5)
                       .SetTextAlignment(TextAlignment.CENTER)
                       .SetFont(font)
                       .SetFontSize(13);
                       tablefinale1.AddCell(cell1);

                    cell1 = new Cell()
                        .Add(new Paragraph("Antibiotique"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(fontbold)
                        .SetFontSize(10);
                        tablefinale1.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Sensible"))
                        .SetPadding(0)
                        .SetFont(fontbold)
                        .SetPaddingLeft(5)
                        .SetFontSize(10);
                        tablefinale1.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Intermédiaire"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(fontbold)
                        .SetFontSize(10);
                        tablefinale1.AddCell(cell1);
                    cell1 = new Cell()
                        .Add(new Paragraph("Résistant"))
                        .SetPadding(0)
                        .SetPaddingLeft(5)
                        .SetFont(fontbold)
                        .SetFontSize(10);
                        tablefinale1.AddCell(cell1);

                    for (int k = 1; k < liste_de_facturation2.Count() - 1; k += 2)
                    {
                        if (liste_de_facturation2[k + 1] == "Sensible")
                        {
                            cell1 = new Cell()
                            .Add(new Paragraph(liste_de_facturation2[k]))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale1.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph("X"))
                            .SetPadding(0)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale1.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale1.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale1.AddCell(cell1);

                        }


                        if (liste_de_facturation2[k + 1] == "Intermédiaire")
                        {
                            cell1 = new Cell()
                            .Add(new Paragraph(liste_de_facturation2[k]))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale1.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale1.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph("X"))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale1.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale1.AddCell(cell1);

                        }


                        if (liste_de_facturation2[k + 1] == "Résistant")
                        {
                            cell1 = new Cell()
                            .Add(new Paragraph(liste_de_facturation2[k]))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale1.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale1.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph(" "))
                            .SetPadding(0)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale1.AddCell(cell1);

                            cell1 = new Cell()
                            .Add(new Paragraph("X"))
                            .SetPadding(0)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPaddingLeft(5)
                            .SetFont(font)
                            .SetFontSize(10);
                            tablefinale1.AddCell(cell1);

                        }
                    }

                    document.Add(tablefinale1);                       
                    
                    }


                    if (t < target.Count())
                    {
                        document.Add(new Paragraph("-----------------------------------------------------------------------------------------------------------------------------------------------"));
                    }



                    t++;

                }

                // Assure le rendu des polices + images intégrées
                props.SetFontProvider(new DefaultFontProvider(true, true, true));


                // Écrire le nombre total de pages dans le placeholder
                var canvas = new PdfCanvas(placeholder, pdfDocument);
                canvas.BeginText()
                    .SetFontAndSize(font, 8)
                    .MoveText(0, 0)
                    .ShowText(pdfDocument.GetNumberOfPages().ToString())
                    .EndText();


                // Écrire le nombre total de pages dans le placeholder
                var canvas2 = new PdfCanvas(placeholder2, pdfDocument);
                canvas2.BeginText()
                    .SetFontAndSize(font, 8)
                    .MoveText(0, 0)
                    .ShowText(resultat.NomPatient + ", Dossier: " + resultat.NumeroDossier + ", Facture: " + resultat.NumeroFacture + ", Date: " + resultat.Date_de_finalisation)
                    .EndText();




                // Fermer le document pour finaliser la génération du PDF
                document.Close();

                byte[] byteInfo = memoryStream.ToArray();
                workStream2.Write(byteInfo, 0, byteInfo.Length);
                workStream2.Position = 0;

                return workStream2;
            }

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
        static string ReplaceEmptyParagraphs(string input)
        {
            string resultat = Regex.Replace(input,
            @"(<p[^>]*>(\s|<span[^>]*>(\s|&nbsp;)*<\/span>)*<\/p>)+",
            match =>
            {
                int count = Regex.Matches(match.Value, @"<p[^>]*>(\s|<span[^>]*>(\s|&nbsp;)*<\/span>)*<\/p>").Count;
                return string.Concat(new string[count + 1].Select(_ => "<br/>"));
            },
            RegexOptions.IgnoreCase);

            return resultat;
        }
        

        private Resultat infopatient(int id)
        {
            var resultat = _context.Resultat
                .FirstOrDefault(m => m.Id == id);

            return resultat;
        }

        

        

    }

    
}




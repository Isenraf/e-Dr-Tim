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
using BANA.Data;





namespace BANA.PDF
{
    public class PdfGeneration9
    {
        private readonly FinalFicheContext _context;
        private readonly DoctorContext _context_doctor;

        public PdfGeneration9(FinalFicheContext context, DoctorContext context_doctor)
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
                canvas.AddImageFittedIntoRectangle(brouillon, new Rectangle(200, 400, 150, 45), false);
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





        public async Task<MemoryStream> DetailsFiche(int id)
        {




            var fiche = await _context.FinalFiche
                .FirstOrDefaultAsync(m => m.Id == id);
            fiche.Contenu = ReplaceEmptyParagraphs(fiche.Contenu);


            // Créer un MemoryStream pour générer le PDF en mémoire
            using (MemoryStream memoryStream = new MemoryStream())
            {





                // Créer un PdfWriter pour écrire dans le MemoryStream
                PdfWriter writer = new PdfWriter(memoryStream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                Document document = new Document(pdfDocument, iText.Kernel.Geom.PageSize.A4, false);
                document.SetMargins(140, 30, 60, 30);

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


                var fileName = "";
                var medecin = _context_doctor.Doctor.Where(x => (x.Nom + " " + x.Prenom) == fiche.Trump1).FirstOrDefault();
                if (medecin != null)
                {
                    fileName = medecin.Id + "_" + medecin.Nom.Replace(" ", "") + ".png";
                }

                if (fiche.Etat == "Cloturé")
                {
                    signaturePath = "wwwroot/doctors/signatures/" + fileName;
                }


                if (fiche.Etat != "Cloturé")
                {
                    brouillonPath = "wwwroot/img/brouillon.png";
                }


                // Ajout du gestionnaire d'événements pour ajouter un logo sur chaque page
                pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, new LogoEventHandler(imagePath, signaturePath, brouillonPath));


                // Créer un tableau avec 3 colonnes
                Table table = new Table(new float[] { 200f, 50f, 150f });
                table.SetWidth(UnitValue.CreatePercentValue(100)); // 100% de la largeur de la page


                // Données à encoder dans le QR Code
                string qrContent = "https://cm.edoctor-tim.com/dmi/ppoitiers/" + fiche.Numero_dossier;

                // Générer le QR Code
                BarcodeQRCode qrCode = new BarcodeQRCode(qrContent);
                Image qrImage = new Image(qrCode.CreateFormXObject(pdfDocument));

                // Redimensionner (facultatif)
                qrImage.SetWidth(80);
                qrImage.SetHeight(80);

                qrImage.SetFixedPosition(1, 10, 40);


                // Ajouter l'image au document
                document.Add(qrImage);

                props.SetFontProvider(new DefaultFontProvider(true, true, true));

                var elements = HtmlConverter.ConvertToElements(fiche.Contenu, props);

                foreach (var element in elements)
                {
                    if (element is IBlockElement block)
                    {
                        // Créer un Div pour encapsuler le paragraphe et éviter la coupure
                        Div blocIndivisible = new Div()
                            .SetKeepTogether(true) // Empêche la coupure
                            .Add(block);
                        document.Add(blocIndivisible);

                    }
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
        

        

        

    }

    
}




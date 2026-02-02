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
    public class PdfGeneration7
    {
        private readonly HonoraireContext _context;
        private readonly DoctorContext _context_doctor;

                public PdfGeneration7(HonoraireContext context,DoctorContext context_doctor)
                {
                        _context = context;
                        _context_doctor = context_doctor;
                }



        public async Task<MemoryStream> HonorairePdf(DateTime datedebut, DateTime datefin, List<string> docta,List<string> secretaires){
            

            docta.Sort();
            docta.Remove("caution");

           

            var honoraire00 = await _context.Honoraire.Where(x => x.Etat == "Cloturé" && x.Mode_paiement == "assurance" && x.Create_date >= DateTime.Parse("20/01/2025") && x.Paid_date >= datedebut && x.Paid_date <= datefin && x.Montant_medecin > 0).OrderBy(x => x.Assurance).ThenBy(x => x.Create_date).ToListAsync();
            var honoraire0 = await _context.Honoraire.Where(x => x.Etat == "Cloturé" && x.Mode_paiement == "assurance" && x.Create_date >= DateTime.Parse("20/01/2025") && x.Paid_date.Year == 0001 && x.Montant_medecin > 0 && x.Create_date <= datefin).OrderBy(x => x.Assurance).ThenBy(x => x.Create_date).ToListAsync();
            var honoraire = await _context.Honoraire.Where(x => x.Create_date >= datedebut && x.Create_date <= datefin && x.Create_date >= DateTime.Parse("20/01/2025") && x.Mode_paiement == "cash" && x.Etat == "Cloturé" && x.Montant_medecin > 0).OrderBy(x => x.Id).ToListAsync();


            var hono = honoraire.GroupBy(p => new { p.Nom,p.Medecin, p.Patient, p.Create_date.Year , p.Create_date.Month, p.Create_date.Day}).Select(g => g.Last()).ToList();
            var hono1 = honoraire00.GroupBy(p => new { p.Nom,p.Medecin, p.Patient, p.Create_date.Year , p.Create_date.Month, p.Create_date.Day}).Select(g => g.Last()).ToList();
            var hono2 = honoraire0.GroupBy(p => new { p.Nom,p.Medecin, p.Patient, p.Create_date.Year , p.Create_date.Month, p.Create_date.Day}).Select(g => g.Last()).ToList();
            

            List<Honoraire> finalresult0_1 = new();
            List<Honoraire> finalresult0_2 = new();
            List<Honoraire> finalresult0 = new();

            List<Honoraire> finalresult1_1 = new();
            List<Honoraire> finalresult2_2 = new();
            List<Honoraire> finalresult = new();





            List<string> secretair = new List<string>();
            secretair.AddRange(honoraire.OrderBy(x => x.Medecin).Select(x => x.Observation));
            secretair.AddRange(honoraire0.OrderBy(x => x.Medecin).Select(x => x.Observation));
            secretair.AddRange(honoraire00.OrderBy(x => x.Medecin).Select(x => x.Observation));
            secretair.Sort();

            honoraire00 = new();
            honoraire0 = new();
            honoraire = new();


            foreach (var item in secretaires)
            {
                honoraire00 = hono1.Where(x => x.Observation == item).ToList();
                finalresult0_1.AddRange(honoraire00);
                honoraire0 = hono2.Where(x => x.Observation == item).ToList();
                finalresult0_2.AddRange(honoraire0);
                honoraire = hono.Where(x => x.Observation == item).ToList();
                finalresult0.AddRange(honoraire);
            }

            List<string> medecins = new List<string>();

            medecins.AddRange(finalresult0_1.OrderBy(x => x.Medecin).Select(x => x.Medecin.Trim()));
            medecins.AddRange(finalresult0_2.OrderBy(x => x.Medecin).Select(x => x.Medecin.Trim()));
            medecins.AddRange(finalresult0.OrderBy(x => x.Medecin).Select(x => x.Medecin.Trim()));
            medecins.Sort();
            

            foreach (var item in docta)
            {
                honoraire00 = finalresult0_1.Where(x => x.Medecin.Replace(" ", "") == item.Replace(" ", "")).ToList();
                finalresult1_1.AddRange(honoraire00);
                honoraire0 = finalresult0_2.Where(x => x.Medecin.Replace(" ", "") == item.Replace(" ", "")).ToList();
                finalresult2_2.AddRange(honoraire0);
                honoraire = finalresult0.Where(x => x.Medecin.Replace(" ", "") == item.Replace(" ", "")).ToList();
                finalresult.AddRange(honoraire);
            }



            foreach (var item in finalresult)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }
            foreach (var item in finalresult2_2)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }

            foreach (var item in finalresult1_1)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }
            
            List<string> tel = new();
            foreach (var item in docta)
            {
                if (_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault() != null)
                    tel.Add(_context_doctor.Doctor.Where(x => x.Nom.Replace(" ", "") == item.Replace(" ", "")).FirstOrDefault().Telephone);
                else
                    tel.Add("######");
            }
            
            foreach (var item in finalresult)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }
            foreach (var item in finalresult2_2)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }

            foreach (var item in finalresult1_1)
            {
                item.Patient = Truncate(item.Patient, 15);
                item.Nom = Truncate(item.Nom, 15);
                item.Assurance = Truncate(item.Assurance, 15);
            }
            
            foreach (var item in docta)
            {
                Console.WriteLine(item);
            }






                        using (MemoryStream workStream = new MemoryStream())
                        {

                                PdfWriter writer = new PdfWriter(workStream);
                                PdfDocument pdfDocument = new PdfDocument(writer);
                                Document document = new Document(pdfDocument, iText.Kernel.Geom.PageSize.A4, false);
                                document.SetMargins(30, 30, 30, 30);


                                MemoryStream workStream2 = new MemoryStream();

                                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                                List<int> caisses1 = new List<int>();
                                List<int> caisses2 = new List<int>();
                                List<int> caisses3 = new List<int>();
                                List<int> caisses4 = new List<int>();

                                

                                int t = 0;
                                var couleurPerso = new DeviceRgb(14, 187, 19);

                                foreach (var item in docta)
                                {
                                        int somme1 = 0;
                                        int somme2 = 0;
                                        int somme3 = 0;
                                        int somme4 = 0;

                                        int somme01 = 0;
                                        int somme02 = 0;
                                        int somme03 = 0;
                                        int somme04 = 0;

                                        int somme001 = 0;
                                        int somme002 = 0;
                                        int somme003 = 0;
                                        int somme004 = 0;

                                        Table table0 = new(1);
                                        table0.SetWidth(UnitValue.CreatePercentValue(99));
                                        //table0.TotalWidth = 300f;

                                        Cell charg = new Cell()
                                        .Add(new Paragraph("Rapport des Honoraires Du " + datedebut + " au " + datefin))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table0.AddCell(charg);
                                        charg = new Cell()
                                        .Add(new Paragraph(" "))
                                        .SetFontSize(3)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetBorder(Border.NO_BORDER);
                                        table0.AddCell(charg);


                                        document.Add(table0);



                                        Table table = new(9);
                                        table.SetWidth(UnitValue.CreatePercentValue(99));

                                        charg = new Cell(1, 9)
                                        .Add(new Paragraph(item + " - " + tel[t]))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFontColor(couleurPerso)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table.AddCell(charg);

                                        charg = new Cell()
                                        .Add(new Paragraph("Date"))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table.AddCell(charg);

                                        charg = new Cell()
                                        .Add(new Paragraph("Nºfacure"))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table.AddCell(charg);

                                        charg = new Cell()
                                        .Add(new Paragraph("Patient"))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table.AddCell(charg);

                                        charg = new Cell()
                                        .Add(new Paragraph("Prestation"))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table.AddCell(charg);

                                        charg = new Cell()
                                        .Add(new Paragraph("Montant"))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table.AddCell(charg);

                                        charg = new Cell()
                                        .Add(new Paragraph("Taux"))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table.AddCell(charg);

                                        charg = new Cell()
                                        .Add(new Paragraph("Net à payer"))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table.AddCell(charg);

                                        charg = new Cell()
                                        .Add(new Paragraph("Etat"))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table.AddCell(charg);

                                        charg = new Cell()
                                        .Add(new Paragraph("assurance"))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table.AddCell(charg);



                                        foreach (var item0 in finalresult2_2)
                                        {
                                                if (item0.Medecin.Replace(" ", "") == item.Replace(" ", ""))
                                                {
                                                        charg = new Cell()
                                                        .Add(new Paragraph(item0.Create_date.ToShortDateString()))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(item0.Numerofacture))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(item0.Patient))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.LEFT);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(item0.Nom))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.LEFT);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(separateur_de_millier(item0.Montant.ToString())))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.RIGHT);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                       .Add(new Paragraph(Math.Round((decimal)item0.Pourcentage, 2) + "%"))
                                                       .SetFontSize(7)
                                                       .SetPadding(2)
                                                       .SetFont(font)
                                                       .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(separateur_de_millier(item0.Montant_medecin.ToString())))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.RIGHT);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph("En attente"))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(item0.Assurance))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);





                                                        if (item0.Montant == null) { item0.Montant = 0; }
                                                        if (item0.Montant_medecin == null) { item0.Montant_medecin = 0; }
                                                        if (item0.Montant_paye == null) { item0.Montant_paye = 0; }
                                                        if (item0.Montant_reste == null) { item0.Montant_reste = 0; }

                                                        somme01 += (int)item0.Montant;
                                                        somme02 += (int)item0.Montant_medecin;
                                                        somme03 += (int)item0.Montant_paye;
                                                        somme04 += (int)item0.Montant_reste;


                                                }

                                        }
                                        if (somme01 != 0)
                                        {

                                                charg = new Cell(1, 4)
                                                        .Add(new Paragraph("TOTAL"))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                table.AddCell(charg);

                                                charg = new Cell()
                                                        .Add(new Paragraph(separateur_de_millier(somme01.ToString())))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.RIGHT);
                                                table.AddCell(charg);
                                                charg = new Cell()
                                                        .Add(new Paragraph(" "))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                table.AddCell(charg);
                                                charg = new Cell()
                                                        .Add(new Paragraph(separateur_de_millier(somme02.ToString())))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.RIGHT);
                                                table.AddCell(charg);
                                                charg = new Cell()
                                                        .Add(new Paragraph(" "))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                table.AddCell(charg);
                                                charg = new Cell()
                                                        .Add(new Paragraph(" "))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                table.AddCell(charg);




                                        }

                                        charg = new Cell(1, 9)
                                        .Add(new Paragraph(" "))
                                        .SetFontSize(7)
                                        .SetPadding(10)
                                        .SetFont(font)
                                        .SetBorder(Border.NO_BORDER);
                                        table.AddCell(charg);



                                        foreach (var item0 in finalresult1_1)
                                        {
                                                if (item0.Medecin.Replace(" ", "") == item.Replace(" ", ""))
                                                {
                                                        charg = new Cell()
                                                        .Add(new Paragraph(item0.Create_date.ToShortDateString()))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(item0.Numerofacture))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(item0.Patient))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.LEFT);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(item0.Nom))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.LEFT);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(separateur_de_millier(item0.Montant.ToString())))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.RIGHT);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                       .Add(new Paragraph(Math.Round((decimal)item0.Pourcentage, 2) + "%"))
                                                       .SetFontSize(7)
                                                       .SetPadding(2)
                                                       .SetFont(font)
                                                       .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(separateur_de_millier(item0.Montant_medecin.ToString())))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.RIGHT);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph("PAYÉ".ToUpper()))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(item0.Assurance))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);





                                                        if (item0.Montant == null) { item0.Montant = 0; }
                                                        if (item0.Montant_medecin == null) { item0.Montant_medecin = 0; }
                                                        if (item0.Montant_paye == null) { item0.Montant_paye = 0; }
                                                        if (item0.Montant_reste == null) { item0.Montant_reste = 0; }

                                                        somme001 += (int)item0.Montant;
                                                        somme002 += (int)item0.Montant_medecin;
                                                        somme003 += (int)item0.Montant_paye;
                                                        somme004 += (int)item0.Montant_reste;


                                                }

                                        }
                                        if (somme001 != 0)
                                        {

                                                charg = new Cell(1, 4)
                                                        .Add(new Paragraph("TOTAL"))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                table.AddCell(charg);

                                                charg = new Cell()
                                                        .Add(new Paragraph(separateur_de_millier(somme001.ToString())))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.RIGHT);
                                                table.AddCell(charg);
                                                charg = new Cell()
                                                        .Add(new Paragraph(" "))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                table.AddCell(charg);
                                                charg = new Cell()
                                                        .Add(new Paragraph(separateur_de_millier(somme002.ToString())))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.RIGHT);
                                                table.AddCell(charg);
                                                charg = new Cell()
                                                        .Add(new Paragraph(" "))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                table.AddCell(charg);
                                                charg = new Cell()
                                                        .Add(new Paragraph(" "))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                table.AddCell(charg);




                                        }

                                        charg = new Cell(1, 9)
                                        .Add(new Paragraph(" "))
                                        .SetFontSize(7)
                                        .SetPadding(10)
                                        .SetFont(font)
                                        .SetBorder(Border.NO_BORDER);
                                        table.AddCell(charg);



                                        foreach (var item2 in finalresult)
                                        {
                                                if (item2.Medecin.Replace(" ", "") == item.Replace(" ", ""))
                                                {
                                                        charg = new Cell()
                                                        .Add(new Paragraph(item2.Create_date.ToShortDateString()))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(item2.Numerofacture))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(item2.Patient))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.LEFT);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(item2.Nom))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.LEFT);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(separateur_de_millier(item2.Montant.ToString())))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.RIGHT);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                       .Add(new Paragraph(Math.Round((decimal)item2.Pourcentage, 2) + "%"))
                                                       .SetFontSize(7)
                                                       .SetPadding(2)
                                                       .SetFont(font)
                                                       .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(separateur_de_millier(item2.Montant_medecin.ToString())))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.RIGHT);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph("Cloturé".ToUpper()))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);

                                                        charg = new Cell()
                                                        .Add(new Paragraph(item2.Assurance))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(font)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                        table.AddCell(charg);





                                                        if (item2.Montant == null) { item2.Montant = 0; }
                                                        if (item2.Montant_medecin == null) { item2.Montant_medecin = 0; }
                                                        if (item2.Montant_paye == null) { item2.Montant_paye = 0; }
                                                        if (item2.Montant_reste == null) { item2.Montant_reste = 0; }

                                                        somme1 += (int)item2.Montant;
                                                        somme2 += (int)item2.Montant_medecin;
                                                        somme3 += (int)item2.Montant_paye;
                                                        somme4 += (int)item2.Montant_reste;


                                                }

                                        }
                                        if (somme1 != 0)
                                        {

                                                charg = new Cell(1, 4)
                                                        .Add(new Paragraph("TOTAL"))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                table.AddCell(charg);

                                                charg = new Cell()
                                                        .Add(new Paragraph(separateur_de_millier(somme1.ToString())))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.RIGHT);
                                                table.AddCell(charg);
                                                charg = new Cell()
                                                        .Add(new Paragraph(" "))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                table.AddCell(charg);
                                                charg = new Cell()
                                                        .Add(new Paragraph(separateur_de_millier(somme2.ToString())))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.RIGHT);
                                                table.AddCell(charg);
                                                charg = new Cell()
                                                        .Add(new Paragraph(" "))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                table.AddCell(charg);
                                                charg = new Cell()
                                                        .Add(new Paragraph(" "))
                                                        .SetFontSize(7)
                                                        .SetPadding(2)
                                                        .SetFont(boldFont)
                                                        .SetTextAlignment(TextAlignment.CENTER);
                                                table.AddCell(charg);




                                        }

                                        charg = new Cell(1, 9)
                                        .Add(new Paragraph(" "))
                                        .SetFontSize(7)
                                        .SetPadding(10)
                                        .SetFont(font)
                                        .SetBorder(Border.NO_BORDER);
                                        table.AddCell(charg);

                                        document.Add(table);

                                        t++;
                                        
                                        document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));// Ajouter une nouvelle page (via un saut de page logique)
                                        
                                       
                                        caisses1.Add(somme1+somme001+somme01);
                                        caisses2.Add(somme2+somme002+somme02);
                                        caisses3.Add(somme2+somme002);
                                        
                                        
                                }

                                //document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));// Ajouter une nouvelle page (via un saut de page logique)

                                 Table table1 = new(6);
                                        table1.SetWidth(UnitValue.CreatePercentValue(99));

                                       Cell charg1 = new Cell(1, 6)
                                        .Add(new Paragraph("Recap des Honoraires Du " + datedebut + " au " + datefin))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFontColor(couleurPerso)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table1.AddCell(charg1);

                                        charg1 = new Cell()
                                        .Add(new Paragraph("Médecin".ToUpper()))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.LEFT);
                                        table1.AddCell(charg1);

                                        charg1 = new Cell()
                                        .Add(new Paragraph("Téléphone".ToUpper()))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.LEFT);
                                        table1.AddCell(charg1);

                                        charg1 = new Cell()
                                        .Add(new Paragraph("Montant Acte".ToUpper()))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.LEFT);
                                        table1.AddCell(charg1);

                                        charg1 = new Cell()
                                        .Add(new Paragraph("Montant à payer".ToUpper()))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.LEFT);
                                        table1.AddCell(charg1);

                                        charg1 = new Cell()
                                        .Add(new Paragraph("Montant payé".ToUpper()))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.LEFT);
                                        table1.AddCell(charg1);

                                        charg1 = new Cell()
                                        .Add(new Paragraph("Solde".ToUpper()))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(boldFont)
                                        .SetTextAlignment(TextAlignment.LEFT);
                                        table1.AddCell(charg1);

                                        int i = 0;
                                foreach (var item in docta)
                                {
                                        charg1 = new Cell()
                                        .Add(new Paragraph(item.ToUpper()))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(font)
                                        .SetTextAlignment(TextAlignment.LEFT);
                                        table1.AddCell(charg1);

                                        charg1 = new Cell()
                                        .Add(new Paragraph(tel[i]))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(font)
                                        .SetTextAlignment(TextAlignment.CENTER);
                                        table1.AddCell(charg1);

                                        charg1 = new Cell()
                                        .Add(new Paragraph(separateur_de_millier(caisses1[i].ToString())))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(font)
                                        .SetTextAlignment(TextAlignment.RIGHT);
                                        table1.AddCell(charg1);

                                        charg1 = new Cell()
                                        .Add(new Paragraph(separateur_de_millier(caisses2[i].ToString())))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(font)
                                        .SetTextAlignment(TextAlignment.RIGHT);
                                        table1.AddCell(charg1);

                                        charg1 = new Cell()
                                        .Add(new Paragraph(separateur_de_millier(caisses3[i].ToString())))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(font)
                                        .SetTextAlignment(TextAlignment.RIGHT);
                                        table1.AddCell(charg1);

                                        charg1 = new Cell()
                                        .Add(new Paragraph(separateur_de_millier((caisses2[i]-caisses3[i]).ToString())))
                                        .SetFontSize(7)
                                        .SetPadding(2)
                                        .SetFont(font)
                                        .SetTextAlignment(TextAlignment.RIGHT);
                                        table1.AddCell(charg1);

                                  i++;  
                                }

                                charg1 = new Cell(1,2)
                                .Add(new Paragraph("TOTAL GÉNÉRAL:".ToUpper()))
                                .SetFontSize(7)
                                .SetPadding(2)
                                .SetFont(boldFont)
                                .SetTextAlignment(TextAlignment.CENTER);
                                table1.AddCell(charg1);

                                charg1 = new Cell()
                                .Add(new Paragraph(separateur_de_millier(caisses1.Sum().ToString())))
                                .SetFontSize(7)
                                .SetPadding(2)
                                .SetFont(boldFont)
                                .SetTextAlignment(TextAlignment.RIGHT);
                                table1.AddCell(charg1);

                                charg1 = new Cell()
                                .Add(new Paragraph(separateur_de_millier(caisses2.Sum().ToString())))
                                .SetFontSize(7)
                                .SetPadding(2)
                                .SetFont(boldFont)
                                .SetTextAlignment(TextAlignment.RIGHT);
                                table1.AddCell(charg1);

                                charg1 = new Cell()
                                .Add(new Paragraph(separateur_de_millier(caisses3.Sum().ToString())))
                                .SetFontSize(7)
                                .SetPadding(2)
                                .SetFont(boldFont)
                                .SetTextAlignment(TextAlignment.RIGHT);
                                table1.AddCell(charg1);

                                charg1 = new Cell()
                                .Add(new Paragraph(separateur_de_millier((caisses1.Sum()-caisses3.Sum()).ToString())))
                                .SetFontSize(7)
                                .SetPadding(2)
                                .SetFont(boldFont)
                                .SetTextAlignment(TextAlignment.RIGHT);
                                table1.AddCell(charg1);
                                document.Add(table1);
                                        


                                document.Add(new Paragraph(" "));

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
        
        public static string Truncate(string s, int length)
        {
            if (s.Length > length)
            {
                return s.Substring(0, length);
            }
            return s;
        }

        

        

    }
}




using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BANA.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Mvc;


namespace BANA.PDF
{
    public class Pdf1Generation
    {
        private readonly PaidContext _context;
        private readonly FactureContext _context2;
    
        public Pdf1Generation(PaidContext context,FactureContext context2)
        { 
          _context=context;
          _context2=context2;
        }

        public  MemoryStream Rapportdecaisse(string madate,string tip){

            var Lignespaid=_context.Paid.Where(x=>x.Moyen_paiement!="caution" && x.Create_date.Year==DateTime.Parse(madate).Year && x.Create_date.Month==DateTime.Parse(madate).Month && (x.Moyen_paiement=="orange money" || x.Moyen_paiement=="momo" || x.Moyen_paiement=="especes" || x.Moyen_paiement=="banque" || x.Moyen_paiement=="autre"));
            var Lignespaid_assurance=_context2.Facture.Where(x=>(x.Type=="hospi_assurance" ||x.Type=="consultation_assurance" ||x.Type=="generique_assurance" ||x.Type=="actes_assurance" ||x.Type=="examens_assurance" ) && x.Encaisse_par!=null &&x.Etat_patient=="Cloturé" && x.Derniere_modification.Month==DateTime.Parse(madate).Month && x.Derniere_modification.Year==DateTime.Parse(madate).Year);

            if (tip=="jour")
            {
                Lignespaid=_context.Paid.Where(x=>x.Moyen_paiement!="caution" && x.Create_date>=DateTime.Parse(madate).AddHours(-11) && x.Create_date<=DateTime.Parse(madate).AddHours(13).AddSeconds(-1) && (x.Moyen_paiement=="orange money" || x.Moyen_paiement=="momo" || x.Moyen_paiement=="especes" || x.Moyen_paiement=="banque" || x.Moyen_paiement=="autre"));
                Lignespaid_assurance=_context2.Facture.Where(x=>(x.Type=="hospi_assurance" ||x.Type=="consultation_assurance" ||x.Type=="generique_assurance" ||x.Type=="actes_assurance" ||x.Type=="examens_assurance" ) && x.Encaisse_par!=null &&x.Etat_patient=="Cloturé" && x.Derniere_modification>=DateTime.Parse(madate).AddHours(-11) && x.Derniere_modification<=DateTime.Parse(madate).AddHours(13).AddSeconds(-1));
            }

             

            List<string> caissiers = new List<string>();
            List<string> caisses = new List<string>();
            caissiers.AddRange(Lignespaid.Select(x=>x.Caissier));
            caisses.AddRange(Lignespaid.Select(x=>x.caisse));
            List<int> macaisse = new List<int>();


            MemoryStream workStream = new();
            var doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4,5f, 5f, 10f, 10f);

            iTextSharp.text.pdf.PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            
            //PdfWriter writer =  PdfWriter.GetInstance(document, workStream);
            string papierentete="pppp.jpg";

            iTextSharp.text.Image im1 = iTextSharp.text.Image.GetInstance("wwwroot/img1/"+papierentete);
                im1.ScaleAbsolute(596f, 845f);
                im1.SetAbsolutePosition(0, 0);

            iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA,iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font0 = new(bf, 9, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font font2 = new(bf, 9, iTextSharp.text.Font.BOLD,iTextSharp.text.BaseColor.WHITE);
            iTextSharp.text.Font font1 = new(bf, 7, iTextSharp.text.Font.BOLD,iTextSharp.text.BaseColor.WHITE);
            iTextSharp.text.Font font3 = new(bf, 7, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font font4 = new(bf, 7, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font font5 = new(bf, 9, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font font55 = new(bf, 9, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font font6 = new(bf, 16, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font font7 = new(bf, 9, iTextSharp.text.Font.NORMAL);

            doc.Open();
            doc.Add(im1);
            
            for (var k = 0; k < 7; k++)
            {
                doc.Add(new iTextSharp.text.Paragraph(" "));
            }
            


            iTextSharp.text.pdf.PdfPTable table0 = new(1);
                    table0.WidthPercentage=101f;

            iTextSharp.text.pdf.PdfPCell cell=new();
            if (tip=="jour")
            {
                    cell = new(new iTextSharp.text.Paragraph("RAPPORT DE CAISSE DU "+ DateTime.Parse(madate).AddHours(-11).ToString("dd/MM/yyyy")+ DateTime.Parse(madate).AddHours(-11).ToShortTimeString() +" au "+ @DateTime.Parse(madate).AddHours(13).AddSeconds(-1).ToString("dd/MM/yyyy hh:mm:ss"),font0)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=5f,PaddingBottom=5f};
                    cell.BorderWidth=0.2f;
                    table0.AddCell(cell);
            }else{
                    cell = new(new iTextSharp.text.Paragraph("RAPPORT DE CAISSE DU "+ DateTime.Parse(madate).ToString( "MMMM" )  + DateTime.Parse(madate).Year,font0)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=5f,PaddingBottom=5f};
                    cell.BorderWidth=0.2f;
                    table0.AddCell(cell);
            }
            doc.Add(table0);
            doc.Add(new iTextSharp.text.Paragraph("\n"));

            int[] intTable1Width = {10, 10,30,10,15,10,10};
            iTextSharp.text.pdf.PdfPTable table1 = new(7)
            {
                WidthPercentage = 101f,
                TotalWidth = 100f
            };
            table1.SetWidths(intTable1Width);

            foreach(var item in caisses.Distinct()){
                Console.WriteLine(item);
                cell = new(new iTextSharp.text.Paragraph(item,font0)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.Colspan=7;
                cell.BorderWidth=0.1f;
                table1.AddCell(cell);

                int somme=0;
                cell = new(new iTextSharp.text.Paragraph("Nº FACTURE",font1)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                cell.BorderColor=new iTextSharp.text.BaseColor(165,165,165);
                cell.BackgroundColor=new iTextSharp.text.BaseColor(130,196,10);
                table1.AddCell(cell);

                cell = new(new iTextSharp.text.Paragraph("Nº DOSSIER",font1)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                cell.BorderColor=new iTextSharp.text.BaseColor(165,165,165);
                cell.BackgroundColor=new iTextSharp.text.BaseColor(130,196,10);
                table1.AddCell(cell);

                cell = new(new iTextSharp.text.Paragraph("PATIENT",font1)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                cell.BorderColor=new iTextSharp.text.BaseColor(165,165,165);
                cell.BackgroundColor=new iTextSharp.text.BaseColor(130,196,10);
                table1.AddCell(cell);

                cell = new(new iTextSharp.text.Paragraph("MONTANT",font1)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                cell.BorderColor=new iTextSharp.text.BaseColor(165,165,165);
                cell.BackgroundColor=new iTextSharp.text.BaseColor(130,196,10);
                table1.AddCell(cell);

                cell = new(new iTextSharp.text.Paragraph("MOYEN DE PAIEMENT",font1)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                cell.BorderColor=new iTextSharp.text.BaseColor(165,165,165);
                cell.BackgroundColor=new iTextSharp.text.BaseColor(130,196,10);
                table1.AddCell(cell);

                cell = new(new iTextSharp.text.Paragraph("CAISSIER(e)",font1)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                cell.BorderColor=new iTextSharp.text.BaseColor(165,165,165);
                cell.BackgroundColor=new iTextSharp.text.BaseColor(130,196,10);
                table1.AddCell(cell);

                cell = new(new iTextSharp.text.Paragraph("OBSERVATION",font1)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                cell.BorderColor=new iTextSharp.text.BaseColor(165,165,165);
                cell.BackgroundColor=new iTextSharp.text.BaseColor(130,196,10);
                table1.AddCell(cell);

                // cell.BorderWidthTop=0f;
                //     cell.BorderWidthBottom=0f;
                //     cell.BorderWidthRight=0f;

                foreach(var item2 in Lignespaid.OrderByDescending(x=>x.Id)){
                    if(item2.caisse==item){
                        cell = new(new iTextSharp.text.Paragraph(item2.Numero_facture,font3)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                        cell.BorderWidth=0.1f;
                        table1.AddCell(cell);

                        cell = new(new iTextSharp.text.Paragraph(item2.Numero_dossier,font3)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                        cell.BorderWidth=0.1f;
                        table1.AddCell(cell);

                        cell = new(new iTextSharp.text.Paragraph(item2.Patient,font3)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                        cell.BorderWidth=0.1f;
                        table1.AddCell(cell);

                        cell = new(new iTextSharp.text.Paragraph(item2.Montant.ToString(),font3)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                        cell.BorderWidth=0.1f;
                        table1.AddCell(cell);

                        cell = new(new iTextSharp.text.Paragraph(item2.Moyen_paiement,font3)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                        cell.BorderWidth=0.1f;
                        table1.AddCell(cell);

                        cell = new(new iTextSharp.text.Paragraph(item2.Caissier,font3)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                        cell.BorderWidth=0.1f;
                        table1.AddCell(cell);

                        cell = new(new iTextSharp.text.Paragraph(item2.Observation,font3)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                        cell.BorderWidth=0.1f;
                        table1.AddCell(cell);

                        somme+=item2.Montant;
                    }
                }
                macaisse.Add(somme);

                cell = new(new iTextSharp.text.Paragraph("TOTAL",font3)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.Colspan=3;
                cell.BorderWidth=0.1f;
                table1.AddCell(cell);

                cell = new(new iTextSharp.text.Paragraph(somme.ToString(),font3)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                table1.AddCell(cell);

                cell = new(new iTextSharp.text.Paragraph(" ".ToString(),font0)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.Colspan=3;
                cell.BorderWidth=0.1f;
                table1.AddCell(cell);

                cell = new(new iTextSharp.text.Paragraph(" ",font0)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.Colspan=7;
                cell.BorderWidth=0f;
                table1.AddCell(cell);

            }
            doc.Add(table1);

            
            iTextSharp.text.pdf.PdfPTable table2 = new(caisses.Distinct().Count()+2)
            {
                WidthPercentage = 101f,
                TotalWidth = 100f
            };


            foreach(var item in caisses.Distinct()){
                cell = new(new iTextSharp.text.Paragraph(item,font1)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                cell.BorderColor=new iTextSharp.text.BaseColor(165,165,165);
                cell.BackgroundColor=new iTextSharp.text.BaseColor(130,196,10);
                table2.AddCell(cell);               
            }

                cell = new(new iTextSharp.text.Paragraph("ASSURANCE",font1)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                
                cell.BorderColor=new iTextSharp.text.BaseColor(165,165,165);
                cell.BackgroundColor=new iTextSharp.text.BaseColor(130,196,10);
                table2.AddCell(cell); 

                cell = new(new iTextSharp.text.Paragraph("TOTAL",font1)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                cell.BorderColor=new iTextSharp.text.BaseColor(165,165,165);
                cell.BackgroundColor=new iTextSharp.text.BaseColor(130,196,10);
                table2.AddCell(cell);  
            
            int i=0;
            foreach(var item in macaisse){
                cell = new(new iTextSharp.text.Paragraph(macaisse[i].ToString(),font3)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                table2.AddCell(cell);  
                i++;          
            }

            cell = new(new iTextSharp.text.Paragraph(Lignespaid_assurance.Select(x=>x.Net_a_payer_assurance).Sum().ToString(),font3)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                table2.AddCell(cell);

            cell = new(new iTextSharp.text.Paragraph(macaisse.Sum().ToString(),font3)){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,PaddingTop=2f,PaddingBottom=2f};
                cell.BorderWidth=0.1f;
                table2.AddCell(cell);

            

            doc.Add(table2);


            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return workStream;  
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




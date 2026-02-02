using iText.Kernel.Pdf;
using iText.Forms;
using System.IO;

namespace BANA.PDF
{
    public static class PdfUtils
    {
        public static MemoryStream FlattenPdf(Stream inputPdfStream)
        {
            // Crée le flux de sortie
            var outputStream = new MemoryStream();

            // Laisser les flux ouverts
            var reader = new PdfReader(inputPdfStream);
            reader.SetCloseStream(false);

            var writer = new PdfWriter(outputStream);
            writer.SetCloseStream(false);

            using (var pdfDoc = new PdfDocument(reader, writer))
            {
                var form = PdfAcroForm.GetAcroForm(pdfDoc, true);
                form?.FlattenFields();
            }

            // Important : créer une copie car iText ferme souvent le flux après fermeture du doc
            var finalStream = new MemoryStream(outputStream.ToArray());
            finalStream.Position = 0;

            return finalStream;
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Paid
    {
        public int Id { get; set; }
        public string? Numero_dossier { get; set; }
        public string? Patient { get; set; }
        public string? Numero_facture { get; set; }
        public string? Moyen_paiement { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }
        public int Montant { get; set; }
        public int Frais_retrait { get; set; }
        public string? Numerocaution { get; set; }
        public string? Caissier { get; set; }
        public string? caisse { get; set; }
        public string? fact_type { get; set; }
        public string? Observation { get; set; }

    }
}
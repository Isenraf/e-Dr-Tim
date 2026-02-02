using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Honoraire
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage="Nom requis")]
        public string? Nom { get; set; }
        public string? Numerofacture { get; set; }
        public string? Medecin { get; set; }
        public string? Telephone { get; set; }
        public string? Patient { get; set; }
        public string? Mode_paiement { get; set; }
        public string? Assurance { get; set; }
        public string? Etat { get; set; }
        public string? Observation { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }

        [DataType(DataType.Date)]
        public DateTime Paid_date { get; set; }

        public int? Montant { get; set; }
        public decimal? Pourcentage { get; set; }
        public int? Montant_medecin { get; set; }
        public int? Montant_paye { get; set; }
        public int? Montant_reste { get; set; }
        public bool? Autocalcul { get; set; }
        
    }
}
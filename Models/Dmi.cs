using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    
    public class Dmi
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le numéro de dossier est obligatoire")]
        public string? NumeroDossier { get; set; }
        public string? Patient { get; set; }

        [MaxLength(10)]
        public string? NumeroDeFacture { get; set; }
        public string? Temperature { get; set; }
        public string? Poids { get; set; }
        public string? Taille { get; set; }
        public string? Saturation { get; set; }
        public string? Rpm { get; set; }
        public string? Tsysbg { get; set; }
        public string? Tsysbd { get; set; }
        public string? Tdiasbg { get; set; }
        public string? Tdiasbd { get; set; }
        public string? Pbg { get; set; }
        public string? Pbd { get; set; }
        public string? Glycemie_capillaire { get; set; }
        public string? Pc { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public List<string?> Trumps { get; set; } = new List<string?>();
        public string? Medecin { get; set; }
        public string? Etat { get; set; }
    }
}
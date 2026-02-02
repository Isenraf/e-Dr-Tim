using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    
    public class Fiche3
    {
        public int Id { get; set; }
        public string? NumeroDossier { get; set; }
        public string? Patient { get; set; }
        public string? Pc { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public List<string?> Trumps { get; set; } = new List<string?>();
        public string? infirmere { get; set; }
        public string? Medecin { get; set; }
        public string? Etat { get; set; }
    }
}
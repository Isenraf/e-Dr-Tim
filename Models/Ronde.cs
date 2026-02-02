using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{

    public class Ronde
    {
        public int Id { get; set; }
        public string? NumeroDossier { get; set; }
        public string? NumeroHospi { get; set; }
        public string? Patient { get; set; }
        public string? Pc { get; set; }
        public string? Plainte { get; set; }
        public string? Parametres { get; set; }
        public string? Epj { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime DerniereModif { get; set; }
        public List<string?> Trumps { get; set; } = new List<string?>();
        public List<string?> Trumps2 { get; set; } = new List<string?>();
        public List<string?> Trumps3 { get; set; } = new List<string?>();
        public List<string?> Trumps4 { get; set; } = new List<string?>();
        public string? Medecin { get; set; }
        public string? Etat { get; set; }
        public string? Taf { get; set; }
    }
}


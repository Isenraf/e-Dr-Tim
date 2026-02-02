using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Fiche
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Contenu { get; set; }
        public string? Categorie { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }

        [DataType(DataType.Date)]
        public DateTime Dernieremodification { get; set; }
        public string? Trump1 { get; set; }
        public string? Trump2 { get; set; }
        public string? Trump3 { get; set; }

    }
}
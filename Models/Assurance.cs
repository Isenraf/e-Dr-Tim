using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Assurance
    {
         public int Id { get; set; }

        [Required(ErrorMessage="Nom requis")]
        public string? Nom { get; set; }
        public string? Bp { get; set; }
        public string? Adresse { get; set; }

        [Required(ErrorMessage="NIU requis")]
        public string? Niu { get; set; }
        public string? RC { get; set; }
        public string? Telephone { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Create_date { get; set; }
        public bool Actif { get; set; }


    }
}
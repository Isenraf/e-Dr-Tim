using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Departement
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Nom du département requis")]
        public string? Nom { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public bool Actif { get; set; }
        
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Nom du service requis")]
        public string? Nom { get; set; }

        public string? Cote { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage="Spécifier le département")]
        public string? Departement { get; set; }

        [Required(ErrorMessage="Spécifier la catégorie")]
        public string? Categorie { get; set; }

        [Required(ErrorMessage="Spécifier le montant")]
        public decimal? Montant { get; set; }
        public bool Actif { get; set; }
        
    }
}
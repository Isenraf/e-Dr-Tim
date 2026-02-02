using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Staff
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Nom requis")]
        public string? Nom { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage="Désignation requise")]
        public string? Désignation { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "Chiffres uniquement"),StringLength(9, MinimumLength = 9,ErrorMessage = "entrée incorrecte! 9 charactères requis"),Required(ErrorMessage="Téléphone requis")]
        public string? Phone { get; set; }

        public string? Adresse { get; set; }

        public string? Catégorie { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }
        public string? Description { get; set; }
        public bool Actif { get; set; }
        
    }
}
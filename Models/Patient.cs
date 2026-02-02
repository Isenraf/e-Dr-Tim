using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Nom requis")]
        public string? Nom { get; set; }

        public string? Numero_dossier { get; set; }
        public string? Prenom { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "Chiffres uniquement"),StringLength(9, MinimumLength = 9,ErrorMessage = "entrée incorrecte! 9 charactères requis"),Required(ErrorMessage="Téléphone requis")]
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Img { get; set; }
        public string? Taille { get; set; }
        public string? Description { get; set; }
        public string? Notesimportant { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }

        [Required(ErrorMessage="Date de Naissance requise")]

        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }

        [DataType(DataType.Date)]
        public DateTime Derniere_visite { get; set; }

        [Required(ErrorMessage="Genre requis")]
        public string? Genre { get; set; }
        public string? Profession { get; set; }
        public string? Quartier { get; set; }
        public string? GroupeSanguin { get; set; }
        public string? AutresContact { get; set; }
        public int? apparition { get; set; }
        public string? Intervenant { get; set; }
        public string? Etat { get; set; }
        public bool Acrif { get; set; }
        
    }
}
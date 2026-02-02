using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class User
    {
        public int Id { get; set; }

        public bool Activate { get; set; }
        [StringLength(60, MinimumLength = 4,ErrorMessage = "entrée incorrecte! 4 charactère minimum"),Required(ErrorMessage="Nom d'utilisateur requis")]
        public string? nom { get; set; }

        [RegularExpression(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$",ErrorMessage = "Adresse email incorrect !!"),Required(ErrorMessage="Email requis")]
        public string? Email { get; set; }
        
        [StringLength(60, MinimumLength = 4,ErrorMessage = "entrée incorrecte! 4 charactère minimum"),Required(ErrorMessage="Mot de passe requis")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [StringLength(60, MinimumLength = 4,ErrorMessage = "Vous avez entré deux mots de passe différents"),Required(ErrorMessage="Confirmation Mot de passe requis")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Vous avez entré deux mots de passe différents")]
        public string? ConfirmPassword { get; set; }
        public string? Type_de_compte { get; set; }
        public string? Access_module { get; set; }

        public string? Img { get; set; }

        [Required(ErrorMessage="Genre requis")]
        public string? Genre { get; set; }
        public string? Departement { get; set; }
        public string? Societe { get; set; }
        public string? Initial { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime Lastconnetion { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BANA.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nom du Docteur requis")]
        public string? Nom { get; set; }
        public string? Prenom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date_De_Naissance { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "Genre  requis")]
        public string? Genre { get; set; }

        [Required(ErrorMessage = "Spécialité requise")]
        public string? Specialite { get; set; }
        public string? Departement { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "Chiffres uniquement"), StringLength(9, MinimumLength = 9, ErrorMessage = "entrée incorrecte! 9 charactères requis"), Required(ErrorMessage = "Téléphone requis")]
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public string? Site_web { get; set; }
        public string? Note { get; set; }
        public string? User_Name { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Vous avez entré deux mots de passe différents")]
        [DataType(DataType.Password)]
        public string? PasswordConfirm { get; set; }
        public string? social1 { get; set; }
        public string? contrat { get; set; }
        public string? social2 { get; set; }
        public string? social3 { get; set; }
        public string? social4 { get; set; }
        public bool Actif { get; set; }
        public bool Interne { get; set; }

        [NotMapped]  // Empêche EF Core de mapper ce champ
        public IFormFile? ImageFile1 { get; set; }
        
        [NotMapped]  // Empêche EF Core de mapper ce champ
        public IFormFile? ImageFile2 { get; set; }
        
        
    }
}
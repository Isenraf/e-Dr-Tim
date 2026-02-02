using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Tache
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string? Utilisateur { get; set; }

        [Required(ErrorMessage="Titre requise")]
        public string? titre { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateFin { get; set; }
        public string? Description { get; set; }
        public string? Intervenant { get; set; }
        public string? Etat { get; set; }
        public bool Important { get; set; }
        public bool Corbeille { get; set; }
        
    }
}
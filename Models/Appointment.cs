using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Appointment
    {
        public int Id { get; set; }
         public string? Numero_dossier { get; set; }
        public string? Nom_patient { get; set; }
        public string? Genre { get; set; }
        public string? Medecin { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }
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
using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Archive
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Patient { get; set; }
        public string? Numero_dossier { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatedeNaissance { get; set; }
        public string? Genre { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }
        public string? ajoute_par { get; set; }
        public string? Trump2 { get; set; }
        public string? Trump3 { get; set; }
        public string? Etat { get; set; }

    }
}
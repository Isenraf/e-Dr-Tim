using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class LNME
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? DCI { get; set; }
        public string? Catégorie { get; set; }
        public string? Dosage { get; set; }
        public string? Forme { get; set; }
        public bool actif { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }

    }
}
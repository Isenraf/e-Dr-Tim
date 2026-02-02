using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Examen
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Contenu { get; set; }
        public string? Categorie { get; set; }
        public string? Cote { get; set; }
        public string? Code { get; set; }
        public string? ExamType { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }
        public int Montant { get; set; }

    }
}
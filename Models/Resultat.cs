using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Resultat
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? NumeroDossier { get; set; }
        public string? NumeroFacture { get; set; }
        public string? NomPatient { get; set; }
        public string? Genre { get; set; }
        public string? Telephone_patient { get; set; }
        public string? Age_patient { get; set; }
        public string? Prescripteur { get; set; }
        public string? Contenu { get; set; }
        public string? Categorie { get; set; }
        public string? Cote { get; set; }
        public string? Code { get; set; }
        public string? ExamType { get; set; }
        public string? fait_par { get; set; }
        public string? valide_par { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date_de_finalisation { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date_de_naissance { get; set; }
        public int Montant { get; set; }
        public string? resultat_final { get; set; }

    }
}
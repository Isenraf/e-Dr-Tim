using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class TransactionStock
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Nom de l'article requis")]
        public string? Nom_article { get; set; }

        [Required(ErrorMessage="Emplacement requis")]
        public string? Emplacement { get; set; }
        public decimal Quantitee { get; set; }
        public string? utilisateur { get; set; }
        public string? Observation { get; set; }
        public string? TypeOperation { get; set; }
        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }
        
    }
}
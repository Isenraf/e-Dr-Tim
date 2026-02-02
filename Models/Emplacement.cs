using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Emplacement
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Nom de l'emplacement requis")]
        public string? Nom { get; set; }
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Create_date { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? LastModification { get; set; }
    }
}
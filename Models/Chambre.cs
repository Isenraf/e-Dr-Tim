using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Chambre
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage="Nom requis")]
        public string? Numero_chambre { get; set; }
        public string? Patient { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Create_date { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateDebut { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateFin { get; set; }

        [Required(ErrorMessage="Type de chambre requis")]
        public string? TypeChambre { get; set; }
        public int? Prix { get; set; }
        public bool Busy { get; set; }
        
    }
}
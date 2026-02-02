using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Vaccination
    {
        public int Id { get; set; }
        public string? Numero_Dossier { get; set; }
        public string? Patient { get; set; }
       
        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }
        public string? contenu { get; set; }
        public string? Trump1 { get; set; }
        public string? Trump2 { get; set; }
        public string? Trump3 { get; set; }

    }
}
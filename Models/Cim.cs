using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Cim
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Code { get; set; }
        public string? Maladie_fr { get; set; }
        public string? Maladie_en { get; set; }
        public string? Trump1 { get; set; }
        public string? Trump2 { get; set; }
        public string? Trump3 { get; set; }
        public string? Version { get; set; }

    }
}
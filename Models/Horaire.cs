using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Horaire
    {
        public string? Nom { get; set; }
        public int Lun { get; set; }
        public int Mar { get; set; }
        public int Mer { get; set; }
        public int Jeu { get; set; }
        public int Ven { get; set; }
        public int Total { get; set; }

    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Parametre
    {
        public int Id { get; set; }
        public string? Numero_Dossier { get; set; }
        public string? Numero_Facture { get; set; }
        public string? Patient { get; set; }
        public string? Temperature { get; set; }
        public string? Poids { get; set; }
        public string? Taille { get; set; }
        public string? Saturation { get; set; }
        public string? Rpm { get; set; }
        public string? Tsysbg { get; set; }
        public string? Tsysbd { get; set; }
        public string? Tdiasbg { get; set; }
        public string? Tdiasbd { get; set; }
        public string? Pbg { get; set; }
        public string? Pbd { get; set; }
        public string? Glycemie_capillaire { get; set; }
        public string? Pc { get; set; }
        public string? Infirmier { get; set; }
        public string? Pt { get; set; }
        public string? Pb { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }
        public string? Observation { get; set; }
        
    }
}
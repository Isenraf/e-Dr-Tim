namespace BANA.Models
{
    public class FichierModel
    {
        public string Nom { get; set; } = string.Empty;
        public string CheminRelatif { get; set; } = string.Empty;
        public string? Numero_dossier { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateModification { get; set; }
        public double TailleKo { get; set; }
    }
}

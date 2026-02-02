using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Hospi
    {
        // 1. Informations administratives du patient
        public int? Id { get; set; }
        public string? Nom { get; set; }
        public string? Numero_dossier { get; set; }
        public string? Sexe { get; set; }
        public string? Numero_facture { get; set; }
        public DateTime DateNaissance { get; set; }
        public string? Adresse { get; set; }
        public string? Telephone { get; set; }
        public string? PersonneContactUrgence { get; set; }
        public string? TelephoneContactUrgence { get; set; }
        public string? GroupeSanguin { get; set; }

        // 2. Informations sur l’hospitalisation
        public string? NumeroHospitalisation { get; set; }
        public DateTime DateAdmission { get; set; }
        public string? MotifAdmission { get; set; }
        public string? TypeHospitalisation { get; set; }
        public string? Service { get; set; }
        public string? Chambre { get; set; }
        public string? Lit { get; set; }
        public string? MedecinResponsable { get; set; }
        public string? DiagnosticAdmission { get; set; }

        // 3. Données médicales initiales
        public decimal Temperature { get; set; }
        public string? TensionArterielle { get; set; }
        public int Pouls { get; set; }
        public int FrequenceRespiratoire { get; set; }
        public int SaturationOxygene { get; set; }
        public string? Allergies { get; set; }
        public string? Antecedents { get; set; }
        public string? TraitementsEnCours { get; set; }

        // 4. Suivi du séjour
        public string? Prescriptions { get; set; }
        public string? ActesMedicaux { get; set; }
        public string? ExamensEtResultats { get; set; }
        public string? NotesInfirmieres { get; set; }
        public string? EvolutionClinique { get; set; }

        // 5. Données de sortie
        public DateTime? DateSortie { get; set; }
        public string? DiagnosticSortie { get; set; }
        public string? EtatSortie { get; set; }
        public string? Destination { get; set; }
        public string? ResumeMedical { get; set; }
        public string? OrdonnanceSortie { get; set; }

        // 6. Facturation
        public string? Assurance { get; set; }
        public decimal CoutSejour { get; set; }
        public decimal PaiementEffectue { get; set; }
        public decimal SoldeRestant { get; set; }
    }

}
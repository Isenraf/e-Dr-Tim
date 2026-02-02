using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Facture
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Nom requis")]
        public string? Nom { get; set; }
        public string? Prenom { get; set; }

        [Required(ErrorMessage="Numéro dossier requis")]
        public string? Numero_dossier { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Taille { get; set; }
        public string? Medecin { get; set; }

        [DataType(DataType.Date)]
        public DateTime Create_date { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date_validite { get; set; }

        [DataType(DataType.Date)]
        public DateTime Derniere_modification { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Derniere_impression { get; set; }

        [Required(ErrorMessage="Date de Naissance requise")]

        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }

        [Required(ErrorMessage="Genre requis")]
        public string? Genre { get; set; }

        public string? Profession { get; set; }

        public string? Quartier { get; set; }
        public string? GroupeSanguin { get; set; }
        public string? Intervenant { get; set; }
        public string? Ligne_facturation1 { get; set; }
        public string? Ligne_facturation2 { get; set; }
        public string? Ligne_facturation3 { get; set; }
        public string? Ligne_facturation4 { get; set; }
        public string? Numero_de_facture { get; set; }
        public string? Moyen_paiement { get; set; }
        public string? Assur_Adresse { get; set; }
        public string? Assur_BP { get; set; }
        public string? Assur_Tel { get; set; }
        public string? Assur_NIU { get; set; }
        public string? Assur_Rc { get; set; }
        public int Total_ht { get; set; }
        public int Tva { get; set; }
        public int Remise { get; set; }
        public float Taxe1 { get; set; }
        public int Total_ttc { get; set; }
        public int Net_a_payer_patient { get; set; }
        public int Montant_recu_patient { get; set; }
        public int Montant_recu_assurance { get; set; }
        public string? Assureur { get; set; }
        public int Pourcentage_Assurance { get; set; }
        public int Net_a_payer_assurance { get; set; }
        public int Especes { get; set; }
        public int frais_retrait { get; set; }
        public int Nombre_affichage { get; set; }
        public int Nombre_impression { get; set; }
        public int ticketmoderateur { get; set; }
        public string? Montant_en_lettre { get; set; }
        public string? Montant_en_lettre_assurance { get; set; }
        public string? Montant_en_lettre_patient { get; set; }
        public string? Notes { get; set; }
        public string? Matricule_patient { get; set; }
        public string? MatriculeADH { get; set; }
        public string? Societe { get; set; }
        public string? assure_prin { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date_entree { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date_sortie { get; set; }
        public string? Titre1 { get; set; }
        public string? Titre2 { get; set; }
        public string? Titre3 { get; set; }
        public string? Titre4 { get; set; }
        public string? Etat_patient { get; set; }
        public string? Etat_assurance { get; set; }
        public string? Type { get; set; }
        public string? Facture_par { get; set; }
        public string? Encaisse_par { get; set; }
        
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class Stock
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Nom de l'article requis")]
        public string? Nom_article { get; set; }
        public string? Type { get; set; }
        public decimal Prix_vente { get; set; }
        public decimal? Prix_achat { get; set; }
        public decimal? Quantitee { get; set; }
        public int? In { get; set; }
        public int? Out { get; set; }
        public int? Vendu { get; set; }
        public decimal? Stock_minimal { get; set; }
        public decimal? chiffre_affaire { get; set; }
        public string? Categorie { get; set; }
        public string? Reference { get; set; }
        public string? codebarre { get; set; }
        public string? Lieu_stockage { get; set; }
        public string? unité { get; set; }
        public bool Actif { get; set; }
        public string? Compte_general { get; set; }
        public string? Img { get; set; }
        public string? CreateBy { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public string? Societe { get; set; }
        public string? Compte_general_entree { get; set; }
        public string? Compte_general_sortie { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Create_date { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? LastModification { get; set; }
    }
}
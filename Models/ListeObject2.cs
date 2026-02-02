using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class ListObject2
    {
        public string Departement { get; set; }

        public string designation { get; set; }
        public int montant { get; set; }
        public int quantite { get; set; }

    }
}
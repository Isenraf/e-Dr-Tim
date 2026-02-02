using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class ListObject
    {
        public DateTime debut { get; set; }
        public DateTime fin { get; set; }
        public List<string> docta { get; set; }
        public List<string> Secretaire { get; set; }
        
    }
}
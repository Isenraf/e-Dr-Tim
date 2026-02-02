using System;
using System.ComponentModel.DataAnnotations;

namespace BANA.Models
{
    public class DateFilter
    {
        
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        
    
    }
}
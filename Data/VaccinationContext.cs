using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class VaccinationContext : DbContext
    {
        public VaccinationContext (DbContextOptions<VaccinationContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Vaccination> Vaccination { get; set; } = default!;
    }
}

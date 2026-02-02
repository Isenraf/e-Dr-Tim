using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class FicheAssuranceContext : DbContext
    {
        public FicheAssuranceContext (DbContextOptions<FicheAssuranceContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.FicheAssurance> FicheAssurance { get; set; } = default!;
    }
}

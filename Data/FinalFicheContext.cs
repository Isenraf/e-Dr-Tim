using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class FinalFicheContext : DbContext
    {
        public FinalFicheContext (DbContextOptions<FinalFicheContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.FinalFiche> FinalFiche { get; set; } = default!;
    }
}

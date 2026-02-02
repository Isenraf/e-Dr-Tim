using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class FicheContext : DbContext
    {
        public FicheContext (DbContextOptions<FicheContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Fiche> Fiche { get; set; } = default!;
    }
}

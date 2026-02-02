using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class Fiche4Context : DbContext
    {
        public Fiche4Context (DbContextOptions<Fiche4Context> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Fiche4> Fiche4 { get; set; } = default!;
    }
}

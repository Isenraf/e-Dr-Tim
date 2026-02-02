using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class Fiche1Context : DbContext
    {
        public Fiche1Context (DbContextOptions<Fiche1Context> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Fiche1> Fiche1 { get; set; } = default!;
    }
}

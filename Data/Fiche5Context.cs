using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class Fiche5Context : DbContext
    {
        public Fiche5Context (DbContextOptions<Fiche5Context> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Fiche5> Fiche5 { get; set; } = default!;
    }
}

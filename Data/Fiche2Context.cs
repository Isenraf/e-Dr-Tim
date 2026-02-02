using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class Fiche2Context : DbContext
    {
        public Fiche2Context (DbContextOptions<Fiche2Context> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Fiche2> Fiche2 { get; set; } = default!;
    }
}

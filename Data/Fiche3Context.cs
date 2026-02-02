using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class Fiche3Context : DbContext
    {
        public Fiche3Context (DbContextOptions<Fiche3Context> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Fiche3> Fiche3 { get; set; } = default!;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class LNMEContext : DbContext
    {
        public LNMEContext (DbContextOptions<LNMEContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.LNME> LNME { get; set; } = default!;
    }
}

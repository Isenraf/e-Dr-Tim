using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class DmiContext : DbContext
    {
        public DmiContext (DbContextOptions<DmiContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Dmi> Dmi { get; set; } = default!;
    }
}

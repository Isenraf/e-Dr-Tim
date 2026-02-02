using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class RondeContext : DbContext
    {
        public RondeContext (DbContextOptions<RondeContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Ronde> Ronde { get; set; } = default!;
    }
}

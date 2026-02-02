using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class HospiContext : DbContext
    {
        public HospiContext (DbContextOptions<HospiContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Hospi> Hospi { get; set; } = default!;
    }
}

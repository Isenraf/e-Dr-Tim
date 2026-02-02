using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class ParametreContext : DbContext
    {
        public ParametreContext (DbContextOptions<ParametreContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Parametre> Parametre { get; set; } = default!;
    }
}

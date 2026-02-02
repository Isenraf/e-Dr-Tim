using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class CimContext : DbContext
    {
        public CimContext (DbContextOptions<CimContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Cim> Cim { get; set; } = default!;
    }
}

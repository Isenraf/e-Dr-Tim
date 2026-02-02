using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class ArchiveContext : DbContext
    {
        public ArchiveContext (DbContextOptions<ArchiveContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Archive> Archive { get; set; } = default!;
    }
}

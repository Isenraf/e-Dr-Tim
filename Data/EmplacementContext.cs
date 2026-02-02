#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class EmplacementContext : DbContext
    {
        public EmplacementContext (DbContextOptions<EmplacementContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Emplacement> Emplacement { get; set; }
    }

#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class FactureContext : DbContext
    {
        public FactureContext (DbContextOptions<FactureContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Facture> Facture { get; set; }
    }

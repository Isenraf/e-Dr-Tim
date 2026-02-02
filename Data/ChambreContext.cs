#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class ChambreContext : DbContext
    {
        public ChambreContext (DbContextOptions<ChambreContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Chambre> Chambre { get; set; }
    }

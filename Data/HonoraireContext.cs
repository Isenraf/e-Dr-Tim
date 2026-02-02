#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class HonoraireContext : DbContext
    {
        public HonoraireContext (DbContextOptions<HonoraireContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Honoraire> Honoraire { get; set; }
    }

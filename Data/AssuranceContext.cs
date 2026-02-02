#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class AssuranceContext : DbContext
    {
        public AssuranceContext (DbContextOptions<AssuranceContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Assurance> Assurance { get; set; }
    }

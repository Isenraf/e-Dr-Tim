#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class DepartementContext : DbContext
    {
        public DepartementContext (DbContextOptions<DepartementContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Departement> Departement { get; set; }
    }

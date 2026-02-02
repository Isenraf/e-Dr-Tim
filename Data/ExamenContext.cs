#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class ExamenContext : DbContext
    {
        public ExamenContext (DbContextOptions<ExamenContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Examen> Examen { get; set; }
    }

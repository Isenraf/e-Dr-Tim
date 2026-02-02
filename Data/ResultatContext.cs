#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class ResultatContext : DbContext
    {
        public ResultatContext (DbContextOptions<ResultatContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Resultat> Resultat { get; set; }
    }

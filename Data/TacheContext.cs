#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class TacheContext : DbContext
    {
        public TacheContext (DbContextOptions<TacheContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Tache> Tache { get; set; }
    }

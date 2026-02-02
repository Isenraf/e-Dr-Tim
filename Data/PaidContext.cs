#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class PaidContext : DbContext
    {
        public PaidContext (DbContextOptions<PaidContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Paid> Paid { get; set; }
    }

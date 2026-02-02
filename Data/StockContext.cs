#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class StockContext : DbContext
    {
        public StockContext (DbContextOptions<StockContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Stock> Stock { get; set; }
    }

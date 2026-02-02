#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class TransactionStockContext : DbContext
    {
        public TransactionStockContext (DbContextOptions<TransactionStockContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.TransactionStock> TransactionStock { get; set; }
    }

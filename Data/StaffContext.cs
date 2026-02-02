#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class StaffContext : DbContext
    {
        public StaffContext (DbContextOptions<StaffContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Staff> Staff { get; set; }
    }

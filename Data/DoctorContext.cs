#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class DoctorContext : DbContext
    {
        public DoctorContext (DbContextOptions<DoctorContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Doctor> Doctor { get; set; }
    }

#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

    public class PatientContext : DbContext
    {
        public PatientContext (DbContextOptions<PatientContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Patient> Patient { get; set; }
    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BANA.Models;

namespace BANA.Data
{
    public class AppointmentContext : DbContext
    {
        public AppointmentContext (DbContextOptions<AppointmentContext> options)
            : base(options)
        {
        }

        public DbSet<BANA.Models.Appointment> Appointment { get; set; } = default!;
    }
}

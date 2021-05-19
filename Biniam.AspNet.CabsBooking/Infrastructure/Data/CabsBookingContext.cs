using ApplicationCore.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class CabsBookingContext : DbContext
    {
        public CabsBookingContext(DbContextOptions<CabsBookingContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }

        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<BookingsHistory> BookingsHistories { get; set; }
        public DbSet<CabTypes> CabTypes { get; set; }
        public DbSet<Places> Places { get; set; }
    }
}

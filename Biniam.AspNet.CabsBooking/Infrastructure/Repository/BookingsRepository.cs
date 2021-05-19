using ApplicationCore.Entites;
using ApplicationCore.RepositoryInterfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class BookingsRepository : EFRepository<Bookings>, IBookingsRepository
    {
        public BookingsRepository(CabsBookingContext dbContext) : base(dbContext)
        {

        }
    }
}

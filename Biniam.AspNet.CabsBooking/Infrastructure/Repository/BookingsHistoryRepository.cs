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
    public class BookingsHistoryRepository : EFRepository<BookingsHistory> , IBookingsHistoryRepository
    {
        public BookingsHistoryRepository( CabsBookingContext dbContext):base(dbContext)
        {

        }
    }
}

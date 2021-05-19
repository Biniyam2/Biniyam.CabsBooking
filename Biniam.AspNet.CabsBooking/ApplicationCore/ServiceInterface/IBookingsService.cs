using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ServiceInterface
{
    public interface IBookingsService : IAsyncService<Models.Response.Bookings, Models.Request.Bookings>
    {
    }
}

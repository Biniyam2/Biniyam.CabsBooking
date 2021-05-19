using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models.Response;
using ApplicationCore.Models.Request;

namespace ApplicationCore.ServiceInterface
{
    public interface IBookingsHistoryService : IAsyncService<Models.Response.BookingsHistory, Models.Request.BookingsHistory >
    {
    }
}

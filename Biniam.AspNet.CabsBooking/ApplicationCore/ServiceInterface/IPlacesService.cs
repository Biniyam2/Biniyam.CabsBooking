using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ServiceInterface
{
    public interface IPlacesService : IAsyncService<Models.Response.Places, Models.Request.Places>
    {
      
    }
}

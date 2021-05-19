using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ServiceInterface
{
    public interface ICabTypesService : IAsyncService<Models.Response.CabTypes, Models.Request.CabTypes>
    {
    }
}

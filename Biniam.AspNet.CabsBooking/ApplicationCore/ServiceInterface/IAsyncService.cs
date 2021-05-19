using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ServiceInterface
{
    public interface IAsyncService<Response, Request> where Response : class where Request : class
    {
        Task<IEnumerable<Response>> GetAllAsync();
        Task<Response> GetById(int id);
       // Task<IEnumerable<Response>> GetAsync();
        Task<Response> Add(Request request);
        Task<Response> Edit(Request request);
        Task Delete(int id);

    }
}

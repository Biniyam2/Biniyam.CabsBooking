using ApplicationCore.Entites;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class CabTypesService :ResponseConverter, ICabTypesService
    {
        private readonly ICabTypesRepository _cabTypesRepository;
        public CabTypesService(ICabTypesRepository cabTypesRepository)
        {
            _cabTypesRepository = cabTypesRepository;
        }
        public async Task<ApplicationCore.Models.Response.CabTypes> Add(ApplicationCore.Models.Request.CabTypes request)
        {
            CabTypes cabTypes = new CabTypes()
            {
                CabTypeName = request.CabTypeName
            };
            var cab = await _cabTypesRepository.AddAsync(cabTypes);
            ApplicationCore.Models.Response.CabTypes result = new ApplicationCore.Models.Response.CabTypes()
            {
                CabTypeId = cab.CabTypeId,
                CabTypeName = cab.CabTypeName
            };
            return result;
        }

        public async Task Delete(int id)
        {
           await _cabTypesRepository.DeleteAsync(id);
        }

        public async Task<ApplicationCore.Models.Response.CabTypes> Edit(ApplicationCore.Models.Request.CabTypes request)
        {
            CabTypes cabTypes = new CabTypes()
            {
                CabTypeId = request.CabTypeId,
                CabTypeName = request.CabTypeName
            };
            var cab =await _cabTypesRepository.UpdateAsync(cabTypes);
            ApplicationCore.Models.Response.CabTypes result = new ApplicationCore.Models.Response.CabTypes()
            {
                CabTypeId = cab.CabTypeId,
                CabTypeName = cab.CabTypeName
            };
            return result;
        }

        public async Task<IEnumerable<ApplicationCore.Models.Response.CabTypes>> GetAllAsync()
        {
            var cabs =await _cabTypesRepository.ListAllAsync();
            List<ApplicationCore.Models.Response.CabTypes> cabTypes = new List<ApplicationCore.Models.Response.CabTypes>();
            foreach (var cab in cabs)
            {
                cabTypes.Add(new ApplicationCore.Models.Response.CabTypes {
                    CabTypeId = cab.CabTypeId,
                    CabTypeName = cab.CabTypeName
                });               
            }
            return cabTypes;
        }

        public async Task<ApplicationCore.Models.Response.CabTypes> GetById(int id)
        {
            var cab =await _cabTypesRepository.GetByIdAsync(id);
            ApplicationCore.Models.Response.CabTypes cabTypes = new ApplicationCore.Models.Response.CabTypes()
            {
                CabTypeId = cab.CabTypeId,
                CabTypeName = cab.CabTypeName,
                Bookings = BookingsConverter( cab.Bookings ),
                BookingsHistories =BookingsHistoryConverter(cab.BookingsHistories)
            };
            return cabTypes;
        }
    }
}

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
    public class PlacesService :ResponseConverter, IPlacesService
    {
        private readonly IPlacesRepository _placesRepository;
        //private readonly IBookingsHistoryService bookingsHistoryService;
        //private readonly IBookingsService bookingsService;
        public PlacesService(IPlacesRepository placesRepository)
        {
            _placesRepository = placesRepository;
        }
        public async Task<ApplicationCore.Models.Response.Places> Add(ApplicationCore.Models.Request.Places request)
        {
            Places place = new Places() {
               PlaceName = request.PlaceName
            
            };
            var places =await _placesRepository.AddAsync(place);

            ApplicationCore.Models.Response.Places placesResponse = new ApplicationCore.Models.Response.Places()
            {
                PlaceName = places.PlaceName,
                PlaceId = places.PlaceId
            };
            return placesResponse;
        }

        public async Task Delete(int id)
        {
            
         
             await _placesRepository.DeleteAsync(id);
        }

        public async Task<ApplicationCore.Models.Response.Places> Edit(ApplicationCore.Models.Request.Places request)
        {
            Places place = new Places()
            {
                PlaceId = request.PlaceId,
                PlaceName = request.PlaceName

            };
            var places = await _placesRepository.UpdateAsync(place);

            ApplicationCore.Models.Response.Places placesResponse = new ApplicationCore.Models.Response.Places()
            {
                PlaceName = places.PlaceName,
                PlaceId = places.PlaceId
            };
            return placesResponse;
        }

        public async Task<IEnumerable<ApplicationCore.Models.Response.Places>> GetAllAsync()
        {
            List<ApplicationCore.Models.Response.Places> placesResponses = new List<ApplicationCore.Models.Response.Places>();
            var places = await _placesRepository.ListAllAsync();
            foreach (var item in places)
            {
                placesResponses.Add(new ApplicationCore.Models.Response.Places
                {
                    PlaceName = item.PlaceName,
                    PlaceId = item.PlaceId
                });
            }
            return placesResponses; ;
        }

        public async Task<ApplicationCore.Models.Response.Places> GetById(int id)
        {
            var place = await _placesRepository.GetByIdAsync(id);
            ApplicationCore.Models.Response.Places places = new ApplicationCore.Models.Response.Places()
            {
                PlaceId = place.PlaceId,
                PlaceName = place.PlaceName,
                Bookings =  BookingsConverter( place.Bookings),
                BookingsHistories = BookingsHistoryConverter(place.BookingsHistories)
            };
            return places;
        }



        

    }
}

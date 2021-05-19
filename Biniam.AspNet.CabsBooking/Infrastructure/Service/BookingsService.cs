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
    public class BookingsService : ResponseConverter, IBookingsService
    {
        private readonly IBookingsRepository _bookingsRepository;
        public BookingsService(IBookingsRepository bookingsRepository)
        {
            _bookingsRepository = bookingsRepository;
        }
        public async Task<ApplicationCore.Models.Response.Bookings> Add(ApplicationCore.Models.Request.Bookings request)
        {
            Bookings entity = new Bookings()
            {
                Email = request.Email,
                BookingDate = request.BookingDate,
                BookingTime = request.BookingTime,
                FromPlace = request.FromPlace,
                ToPlace = request.ToPlace,
                PickupAddress = request.PickupAddress,
                LandMark = request.LandMark,
                PickupDate = request.PickupDate,
                PickupTime = request.PickupTime,
                CabTypeId = request.CabTypesId,
                ContactNo = request.ContactNo,
                Status = request.Status
            };
            var book = await _bookingsRepository.AddAsync(entity);
            ApplicationCore.Models.Response.Bookings bookings = new ApplicationCore.Models.Response.Bookings()
            {
                Id = book.Id,
                Email = book.Email,
                BookingDate = book.BookingDate,
                BookingTime = book.BookingTime,
                FromPlace = book.FromPlace,
                ToPlace = book.ToPlace,
                PickupAddress = book.PickupAddress,
                LandMark = book.LandMark,
                PickupDate = book.PickupDate,
                PickupTime = book.PickupTime,
                CabTypesId = book.CabTypeId,
                ContactNo = book.ContactNo,
                Status = book.Status
            };
            return bookings;
        }

        public async Task Delete(int id)
        {
           
           await _bookingsRepository.DeleteAsync(id);
        }

        public async Task<ApplicationCore.Models.Response.Bookings> Edit(ApplicationCore.Models.Request.Bookings request)
        {
            Bookings entity = new Bookings()
            {
                Id = request.Id,
                Email = request.Email,
                BookingDate = request.BookingDate,
                BookingTime = request.BookingTime,
                FromPlace = request.FromPlace,
                ToPlace = request.ToPlace,
                PickupAddress = request.PickupAddress,
                LandMark = request.LandMark,
                PickupDate = request.PickupDate,
                PickupTime = request.PickupTime,
                CabTypeId = request.CabTypesId,
                ContactNo = request.ContactNo,
                Status = request.Status
            };
            var book = await _bookingsRepository.UpdateAsync(entity);
            ApplicationCore.Models.Response.Bookings bookings = new ApplicationCore.Models.Response.Bookings()
            {
                Id = book.Id,
                Email = book.Email,
                BookingDate = book.BookingDate,
                BookingTime = book.BookingTime,
                FromPlace = book.FromPlace,
                ToPlace = book.ToPlace,
                PickupAddress = book.PickupAddress,
                LandMark = book.LandMark,
                PickupDate = book.PickupDate,
                PickupTime = book.PickupTime,
                CabTypesId = book.CabTypeId,
                ContactNo = book.ContactNo,
                Status = book.Status
            };
            return bookings;

        }

        public async Task<IEnumerable<ApplicationCore.Models.Response.Bookings>> GetAllAsync()
        {
            var books = await _bookingsRepository.ListAllAsync();
            List<ApplicationCore.Models.Response.Bookings> bookings = new List<ApplicationCore.Models.Response.Bookings>();
            foreach (var book in books)
            {
                bookings.Add(new ApplicationCore.Models.Response.Bookings {
                    Id = book.Id,
                    Email = book.Email,
                    BookingDate = book.BookingDate,
                    BookingTime = book.BookingTime,
                    FromPlace = book.FromPlace,
                    ToPlace = book.ToPlace,
                    PickupAddress = book.PickupAddress,
                    LandMark = book.LandMark,
                    PickupDate = book.PickupDate,
                    PickupTime = book.PickupTime,
                    CabTypesId = book.CabTypeId,
                    ContactNo = book.ContactNo,
                    Status = book.Status
                });
            }

            return bookings;
        }

        public async Task<ApplicationCore.Models.Response.Bookings> GetById(int id)
        {
            var book = await _bookingsRepository.GetByIdAsync(id);
            ApplicationCore.Models.Response.Bookings bookings = new ApplicationCore.Models.Response.Bookings()
            {
                Id = book.Id,
                Email = book.Email,
                BookingDate = book.BookingDate,
                BookingTime = book.BookingTime,
                FromPlace =   book.FromPlace,
                ToPlace = book.ToPlace,
                PickupAddress = book.PickupAddress,
                LandMark = book.LandMark,
                PickupDate = book.PickupDate,
                PickupTime = book.PickupTime,                
                CabTypesId = book.CabTypeId,
                ContactNo = book.ContactNo,
                Status = book.Status,
               // Places = PlacesConverter( book.Places),
                //CabTypes = book.CabTypes
            };
            return bookings;
        }
    }
}

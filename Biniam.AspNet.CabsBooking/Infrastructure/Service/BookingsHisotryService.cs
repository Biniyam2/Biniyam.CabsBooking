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
    public class BookingsHisotryService : IBookingsHistoryService
    {
        private readonly IBookingsHistoryRepository _bookingsHistoryRepository;
        public BookingsHisotryService(IBookingsHistoryRepository bookingsHistoryRepository)
        {
            _bookingsHistoryRepository = bookingsHistoryRepository;
        }
        public async Task<ApplicationCore.Models.Response.BookingsHistory> Add(ApplicationCore.Models.Request.BookingsHistory request)
        {
            BookingsHistory entity = new BookingsHistory()
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
                Status = request.Status,
                Charge = request.Charge,
                Feedback = request.Feedback,
                Comp_time = request.Comp_time
            };
            var book = await _bookingsHistoryRepository.AddAsync(entity);
            ApplicationCore.Models.Response.BookingsHistory bookings = new ApplicationCore.Models.Response.BookingsHistory()
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
                Status = book.Status,
                Charge = book.Charge,
                Feedback = book.Feedback,
                Comp_time = book.Comp_time
            };
            return bookings;
        }

        public async Task Delete(int id)
        {
             await _bookingsHistoryRepository.DeleteAsync(id);
        }

        public async Task<ApplicationCore.Models.Response.BookingsHistory> Edit(ApplicationCore.Models.Request.BookingsHistory request)
        {
            BookingsHistory entity = new BookingsHistory()
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
                Status = request.Status,
                Charge = request.Charge,
                Feedback = request.Feedback,
                Comp_time = request.Comp_time
            };
            var book = await _bookingsHistoryRepository.UpdateAsync(entity);
            ApplicationCore.Models.Response.BookingsHistory bookings = new ApplicationCore.Models.Response.BookingsHistory()
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
                Status = book.Status,
                Charge = book.Charge,
                Feedback = book.Feedback,
                Comp_time = book.Comp_time
            };
            return bookings;
        }

        public async Task<IEnumerable<ApplicationCore.Models.Response.BookingsHistory>> GetAllAsync()
        {
            var books = await _bookingsHistoryRepository.ListAllAsync();
            List<ApplicationCore.Models.Response.BookingsHistory> bookings = new List<ApplicationCore.Models.Response.BookingsHistory>();
            foreach (var book in books)
            {
                bookings.Add(new ApplicationCore.Models.Response.BookingsHistory
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
                    Status = book.Status,
                    Charge = book.Charge,
                    Feedback = book.Feedback,
                    Comp_time = book.Comp_time
                });
            }

            return bookings;
        }

        public async Task<ApplicationCore.Models.Response.BookingsHistory> GetById(int id)
        {
            var book = await _bookingsHistoryRepository.GetByIdAsync(id);
            ApplicationCore.Models.Response.BookingsHistory bookings = new ApplicationCore.Models.Response.BookingsHistory()
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
                Status = book.Status,
                Charge = book.Charge,
                Feedback = book.Feedback,
                Comp_time = book.Comp_time

                //Places = book.Places,
                //CabTypes = book.CabTypes
            };
            return bookings;
        }
    }
}

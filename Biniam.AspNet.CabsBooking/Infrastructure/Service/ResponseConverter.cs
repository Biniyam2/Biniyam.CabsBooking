using ApplicationCore.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class ResponseConverter
    {
        public IEnumerable< ApplicationCore.Models.Response.Bookings > BookingsConverter(ICollection<Bookings> bookings)         
        {
            List<ApplicationCore.Models.Response.Bookings> bookingsList = new List<ApplicationCore.Models.Response.Bookings>();
            if (bookings != null)
            {
                
                foreach (var book in bookings)
                {
                    bookingsList.Add(new ApplicationCore.Models.Response.Bookings
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
                    });
                }
                return bookingsList;
            }
            return bookingsList;
        }
        public IEnumerable<ApplicationCore.Models.Response.BookingsHistory> BookingsHistoryConverter(ICollection<BookingsHistory> bookings)
        {
            List<ApplicationCore.Models.Response.BookingsHistory> bookingsList = new List<ApplicationCore.Models.Response.BookingsHistory>();
            if (bookings != null)
            {

                foreach (var book in bookings)
                {
                    bookingsList.Add(new ApplicationCore.Models.Response.BookingsHistory
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
                return bookingsList;
            }
            return bookingsList;
        }

        public IEnumerable<ApplicationCore.Models.Response.Places> PlacesConverter(IEnumerable<Places> places)
        {
            List<ApplicationCore.Models.Response.Places> placesResponses = new List<ApplicationCore.Models.Response.Places>();
            if (places != null)
            {

                foreach (var item in places)
                {
                    placesResponses.Add(new ApplicationCore.Models.Response.Places
                    {
                        PlaceName = item.PlaceName,
                        PlaceId = item.PlaceId
                    });
                }
                return placesResponses;
            }
            return placesResponses;
           
        }
        public IEnumerable<ApplicationCore.Models.Response.CabTypes> CabTypesConverter(IEnumerable<CabTypes> cabs)
        {
            List<ApplicationCore.Models.Response.CabTypes> cabTypes = new List<ApplicationCore.Models.Response.CabTypes>();
            if (cabs != null)
            {

                foreach (var cab in cabs)
                {
                    cabTypes.Add(new ApplicationCore.Models.Response.CabTypes
                    {
                        CabTypeId = cab.CabTypeId,
                        CabTypeName = cab.CabTypeName
                    });
                }
                return cabTypes;
            }
            return cabTypes;
        }



    }
}

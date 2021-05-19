using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models.Request
{
    public class BookingsHistory
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingTime { get; set; }
        public int FromPlace { get; set; }
        public int ToPlace { get; set; }
        public string PickupAddress { get; set; }
        public string LandMark { get; set; }
        public DateTime PickupDate { get; set; }
        public string PickupTime { get; set; }
        public int CabTypesId { get; set; }
        public string ContactNo { get; set; }
        public string Status { get; set; }

        public string Comp_time { get; set; }
        public decimal? Charge { get; set; }
        public string Feedback { get; set; }



    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models.Response
{
    public class Places
    {
        public int? PlaceId { get; set; }
        public string PlaceName { get; set; }
        public IEnumerable<Response.Bookings> Bookings { get; set; }
        public IEnumerable<Response.BookingsHistory> BookingsHistories { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models.Response
{
    public class CabTypes
    {
        public int CabTypeId { get; set; }
        public string CabTypeName { get; set; }
        public IEnumerable<Response.Bookings> Bookings { get; set; }
        public IEnumerable<Response.BookingsHistory> BookingsHistories { get; set; }
    }
}

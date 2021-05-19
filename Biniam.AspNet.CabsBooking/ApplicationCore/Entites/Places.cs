using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entites
{
    [Table("Places")]
    public class Places
    {
        [Key, Required]
        public int PlaceId { get; set; }
        [MaxLength(30)]
        public string PlaceName { get; set; }
        public ICollection<Bookings> Bookings { get; set; }
        public ICollection<BookingsHistory> BookingsHistories { get; set; }


    
        //public int PlaceId { get; set; }
        //public string PlaceName { get; set; }
        //public ICollection<Bookings> Bookings { get; set; }
        //public ICollection<BookingsHistory> BookingsHistories { get; set; }

    }
}

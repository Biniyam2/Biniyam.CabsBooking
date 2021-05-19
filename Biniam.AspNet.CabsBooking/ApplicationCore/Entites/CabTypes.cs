using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entites
{
    [Table("CabTypes")]
    public class CabTypes
    {
        [Key, Required]
        public int CabTypeId { get; set; }

        [MaxLength(30)]
        public string CabTypeName { get; set; }
        public ICollection<Bookings> Bookings { get; set; }
        public ICollection<BookingsHistory> BookingsHistories { get; set; }




        //public int CabTypeId { get; set; }
        //public string CabTypeName { get; set; }
        //public ICollection<Bookings> Bookings { get; set; }
        //public ICollection<BookingsHistory> BookingsHistories { get; set; }
    }
}

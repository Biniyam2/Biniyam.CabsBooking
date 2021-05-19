using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entites
{
    [Table("BookingsHistory")]
    public class BookingsHistory
    {
        [Key, Required]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        public DateTime? BookingDate { get; set; }
        [MaxLength(5)]
        public string BookingTime { get; set; }

        //************//******************************************************

        [ForeignKey("Places")]
        public int? FromPlace { get; set; }
        // [ForeignKey("Places")]
        public int? ToPlace { get; set; }
        [ForeignKey("FromPlace")]
        public Places Places { get; set; }

        //************//******************************************************
        [MaxLength(200)]
        public string PickupAddress { get; set; }
        [MaxLength(30)]
        public string LandMark { get; set; }
        public DateTime? PickupDate { get; set; }
        [MaxLength(5)]
        public string PickupTime { get; set; }
        //************//******************************************************

        public int? CabTypeId { get; set; }
        [ForeignKey("CabTypeId")]
        public CabTypes CabTypes { get; set; }
        //************//***********************************************************
        [MaxLength(25)]
        public string ContactNo { get; set; }
        [MaxLength(30)]
        public string Status { get; set; }
        [MaxLength(5)]
        public string Comp_time { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Charge { get; set; }
        [MaxLength(1000)]
        public string Feedback { get; set; }



    }
}

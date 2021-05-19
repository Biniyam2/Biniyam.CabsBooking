using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models.Request
{
    public class CabTypes
    {
        public int CabTypeId { get; set; }
        public string CabTypeName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHolder.Data.DbModels
{
    public class Booking : BasicBusinessDbObject
    {
        public virtual User? User { get; set; }
        public virtual Room? Room { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

    }
}

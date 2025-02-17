using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHolder.Data.DbModels
{
    public class PaymentMethod : BasicBusinessDbObject
    {
        public double fee { get; set; }
    }
}

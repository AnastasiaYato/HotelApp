using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHolder.Data.DbModels
{
    public class RoomStatusDetailsPair : BasicBusinessDbObject
    {
        public virtual int RoomStatusId { get; set; }
        public virtual RoomStatus? RoomStatus { get; set; }
        public virtual int? RoomStatusDetailsId { get; set; }
        public virtual RoomStatusDetails? RoomStatusDetails { get; set; }
    }
}

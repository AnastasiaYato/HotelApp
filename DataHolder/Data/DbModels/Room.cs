using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHolder.Data.DbModels
{
    public class Room : BasicBusinessDbObject
    {
        public string? RoomIdentification { get; set; }
        public double Price { get; set; }
        public virtual int RoomSizeId { get; set; }
        public virtual RoomSize? RoomSize { get; set; }
        public virtual int RoomStatusDetailsPairId { get; set; }
        public virtual RoomStatusDetailsPair? RoomStatusDetailsPair { get; set; }
        public int FloorNo { get; set; }
    }
}

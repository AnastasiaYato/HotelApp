using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHolder.Data.DbModels
{
    public class RoomStatusDetails : BasicBusinessDbObject
    {

        public virtual RoomStatusDetailsPair RoomStatusDetailsPair { get; set; }
        public RoomStatusDetails()
        {

        }
        public RoomStatusDetails(RoomStatusDetails roomStatusDetails)
        {
            Name = roomStatusDetails.Name;
            Description = roomStatusDetails.Description;
        }
    }
}

using DataHolder.Data;
using DataHolder.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_UnitTests.Samples.LogicTestsSamples
{
    /// <summary>
    /// Main idea is that we want to use different data in our tests than in production. Integration tests however should use the same data as production.
    /// </summary>
    public static class PrepareMockData
    {
        public static DbDataContext ReturnMockDbDataContext()
        {
            var options = new DbContextOptionsBuilder<DbDataContext>()
           .UseInMemoryDatabase("TestDb")
           .Options;
            var context = new DbDataContext(options);
            PrepareData(context);
            return context;
        }
        private static void PrepareData(DbDataContext context)
        {
            context.Users.Add(new User
            {
                Name = "Unit",
                Surname = "Tester",
                Email = "U.T@NET.PL",
                PhoneNo = "48102103104"
            });

            context.PaymentMethods.Add(new PaymentMethod
            {
                Name = "Cash",
                Description = "PLN or Euro - Current exchange rate is on our website"
            });

            context.RoomSizes.AddRange(SampleData.RoomSizes());
            context.RoomStatuses.AddRange(SampleData.RoomStatuses());
            context.SaveChanges();
            context.Rooms.AddRange(SampleData.Rooms(context));
            context.SaveChanges();
            context.Bookings.Add(new Booking
            {
                Name = "Test Booking",
                Room = context.Rooms.First(r => r.Id == 2),
                User = context.Users.First(u => u.Id == 1),
                PaymentMethod = context.PaymentMethods.First(pm => pm.Id == 1),
                From = DateTime.Now - TimeSpan.FromDays(1),
                To = DateTime.Now + TimeSpan.FromDays(1),
            });
            context.SaveChanges();
        }
    }
}

using Service_HotelManagement;
using Services_IntegrationTests.Utils;
using System.Net;
using FluentAssertions;
using System.Text.Json.Nodes;
using System.Text;
using Newtonsoft.Json;
using DataHolder.Data.DbModels;
using BasicDataAdder.BasicData;
using DataHolder.Data;
using Microsoft.EntityFrameworkCore;

namespace Services_IntegrationTests.ControllerTests
{
    public class BookingControllerTests
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var factory = new WebAppFactory<Program>();
            _client = factory.CreateClient();
            RegressionDb.PrepareDb();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client?.Dispose();
        }

        [Test]
        public async Task GetAllBookingsShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/Booking/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task GetBookingShouldReturnExistingId()
        {
            int id = 1;
            var response = await _client.GetAsync($"/api/Booking/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task GetBookingShouldFailWhenReturningNonExistingId()
        {
            int id = 200;
            var response = await _client.GetAsync($"/api/Booking/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task BookingUpdateShouldFailWhenUpdatingNonExisting()
        {
            int id = 200;
            var requestObj = new Booking
            {
                Name = "Test"
            };
            var jsonBody = JsonConvert.SerializeObject(requestObj, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/Booking/{id}",httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task BookingUpdateShouldUpdate()
        {
            int id = 1;
            var requestObj = new Booking
            {
                Name = "Test"
            };
            var jsonBody = JsonConvert.SerializeObject(requestObj, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/Booking/{id}", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task BookingRemoveShouldRemove()
        {
            int id = 1;

            var response = await _client.DeleteAsync($"/api/Booking/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task BookingRemoveShouldFailWhenRemovingNonExisting()
        {
            int id = 200;
            var response = await _client.DeleteAsync($"/api/Booking/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task NewBookingShouldBeAdded()
        {
            var requestObj = new Booking
            {
                Name = "Test",
            };
            var jsonBody = JsonConvert.SerializeObject(requestObj, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/Booking/", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task NewBookingShouldNotBeAddedIfNull()
        {
            var jsonBody = JsonConvert.SerializeObject(null, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/Booking/", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task BookingShouldSuccessfullyBook()
        {
            int id = 1;
            var requestObj = new User
            {
                Name = "John",
                Surname = "Tester",
                Email = "J@T.NET",
                PhoneNo = "111222333"
            };
            var jsonBody = JsonConvert.SerializeObject(requestObj, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/Booking/book/{id}", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task BookingShouldFailToBookNonFreeRoom()
        {
            int id = 1;
            var jsonBody = JsonConvert.SerializeObject(null, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/Booking/book/{id}", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task BookingShouldSuccessfullyCheckOut()
        {
            int id = 2;
            var response = await _client.PutAsync($"/api/Booking/check-out/{id}", null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task BookingShouldFailToCheckOutFreeRoom()
        {

            int id = 1;
            var response = await _client.PutAsync($"/api/Booking/check-out/{id}", null);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }
    }
}
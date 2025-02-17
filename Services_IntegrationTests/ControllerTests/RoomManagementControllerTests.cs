using DataHolder.Data.DbModels;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using Service_HotelManagement;
using Service_HotelManagement.Helpers;
using Services_IntegrationTests.Utils;
using System.Net;
using System.Text;

namespace Services_IntegrationTests.ControllerTests
{
    public class RoomManagementControllerTests
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
        public async Task GetAllRoomsShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/RoomManagement/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task GetAllRoomsShouldReturnOkWithFilters()
        {
            var requestObj = new RequestPayload
            {
                Name = "Cozy"
            };

            var jsonBody = JsonConvert.SerializeObject(requestObj, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.GetAsync("/api/RoomManagement/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task GetRoomShouldReturnExistingId()
        {
            int id = 1;
            var response = await _client.GetAsync($"/api/RoomManagement/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task GetRoomShouldFailWhenReturningNonExistingId()
        {
            int id = 200;
            var response = await _client.GetAsync($"/api/RoomManagement/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task RoomUpdateShouldFailWhenUpdatingNonExisting()
        {
            int id = 200;
            var requestObj = new Room
            {
                Name = "Test"
            };
            var jsonBody = JsonConvert.SerializeObject(requestObj, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"api/RoomManagement/{id}", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task RoomUpdateShouldUpdate()
        {
            int id = 1;
            var requestObj = new Room
            {
                Name = "Test",
                Description = "Test",
                RoomIdentification = "Test",
                Price = 12,
                RoomSize = new RoomSize { Id = 1 },
                RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = new RoomStatus { Id = 1 } }
            };
            var jsonBody = JsonConvert.SerializeObject(requestObj, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"api/RoomManagement/{id}", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task RoomRemoveShouldRemove()
        {
            int id = 1;

            var response = await _client.DeleteAsync($"api/RoomManagement/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task RoomRemoveShouldFailWhenRemovingNonExisting()
        {
            int id = 200;
            var response = await _client.DeleteAsync($"api/RoomManagement/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task NewRoomShouldBeAdded()
        {
            var requestObj = new Room
            {
                Name = "Test",
                Description = "Test",
                RoomIdentification = "Test",
                Price = 12,
                RoomSize = new RoomSize { Id = 1 },
                RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = new RoomStatus { Id = 1 } }
            };
            var jsonBody = JsonConvert.SerializeObject(requestObj, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"api/RoomManagement/", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task RoomShouldNotBeAddedIfNull()
        {
            var jsonBody = JsonConvert.SerializeObject(null, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"api/RoomManagement/", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task RoomShouldNotChangeStatusWithoutDetailsForNonAvailable()
        {
            int id = 1;

            var jsonBody = JsonConvert.SerializeObject("", Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"api/RoomManagement/manuallyLock/{id}", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task RoomShouldChangeStatusOnlyWithDetails() // This test covers pretty much every other status change unless there will be a breaking change in RoomManagementController
        {
            int id = 1;
            var details = new StringPair("Broken TV","Some details");

            var jsonBody = JsonConvert.SerializeObject(details, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"api/RoomManagement/manuallyLock/{id}", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task RoomShouldBeSetFreeWithoutDetails()
        {
            int id = 1;

            var jsonBody = JsonConvert.SerializeObject("", Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"api/RoomManagement/markAsFree/{id}", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task RoomShouldBeSetFreeWithAndAcceptDetails()
        {
            int id = 1;
            var details = new StringPair("Fixed TV", "Some details");

            var jsonBody = JsonConvert.SerializeObject(details, Formatting.Indented);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"api/RoomManagement/markAsFree/{id}", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }
    }
}

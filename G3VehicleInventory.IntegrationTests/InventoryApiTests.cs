using System.Net;
using System.Net.Http.Json;
using G3VehicleInventory.Application.DTOs;
using Xunit;

namespace G3VehicleInventory.IntegrationTests
{
    public class InventoryApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public InventoryApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-Internal-Gateway", "MY_GATEWAY_SECRET_KEY");
        }

        [Fact]
        public async Task GetAllVehicles_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/Vehicles");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetVehicleById_WhenVehicleDoesNotExist_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/Vehicles/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateVehicle_WithValidPayload_ReturnsCreated()
        {
            var dto = new CreateVehicleDto
            {
                VehicleCode = "TEST1001",
                LocationId = 1,
                VehicleType = "SUV"
            };

            var response = await _client.PostAsJsonAsync("/api/Vehicles", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateVehicle_WithInvalidPayload_ReturnsBadRequest()
        {
            var dto = new CreateVehicleDto
            {
                VehicleCode = "",
                LocationId = 0,
                VehicleType = ""
            };

            var response = await _client.PostAsJsonAsync("/api/Vehicles", dto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateVehicleStatus_WhenVehicleDoesNotExist_ReturnsNotFound()
        {
            var dto = new UpdateVehicleStatusDto
            {
                Status = "Reserved"
            };

            var response = await _client.PutAsJsonAsync("/api/Vehicles/999999/status", dto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteVehicle_WhenVehicleDoesNotExist_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/Vehicles/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
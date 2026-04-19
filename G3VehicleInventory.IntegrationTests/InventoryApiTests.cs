using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace G3VehicleInventory.IntegrationTests
{
    public class InventoryApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public InventoryApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            // Add this only if your WebAPI requires the gateway header
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
    }
}
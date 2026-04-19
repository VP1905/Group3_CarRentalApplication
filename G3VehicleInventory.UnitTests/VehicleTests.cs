using System;
using Xunit;
using G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate;

namespace G3VehicleInventory.UnitTests
{
    public class VehicleTests
    {
        [Fact]
        public void Constructor_WithValidArguments_SetsPropertiesCorrectly()
        {
            // Arrange
            var vehicleCode = new VehicleCode("V001");
            var vehicleType = VehicleType.SUV;
            var locationId = new LocationId(1);

            // Act
            var vehicle = new Vehicle(vehicleCode, vehicleType, locationId);

            // Assert
            Assert.Equal(vehicleCode, vehicle.VehicleCode);
            Assert.Equal(vehicleType, vehicle.VehicleType);
            Assert.NotNull(vehicle.Inventory);
            Assert.Equal(VehicleStatus.Available, vehicle.Inventory.Status);
        }

        [Fact]
        public void Constructor_WithNullVehicleCode_ThrowsArgumentNullException()
        {
            // Arrange
            VehicleCode vehicleCode = null!;
            var vehicleType = VehicleType.SUV;
            var locationId = new LocationId(1);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new Vehicle(vehicleCode, vehicleType, locationId));
        }

        [Fact]
        public void MarkAvailable_ShouldUpdateInventoryStatusToAvailable()
        {
            // Arrange
            var vehicle = CreateVehicle();

            // Act
            vehicle.MarkAvailable();

            // Assert
            Assert.Equal(VehicleStatus.Available, vehicle.Inventory.Status);
        }

        [Fact]
        public void MarkReserved_ShouldUpdateInventoryStatusToReserved()
        {
            // Arrange
            var vehicle = CreateVehicle();

            // Act
            vehicle.MarkReserved();

            // Assert
            Assert.Equal(VehicleStatus.Reserved, vehicle.Inventory.Status);
        }

        [Fact]
        public void MarkRented_ShouldUpdateInventoryStatusToRented()
        {
            // Arrange
            var vehicle = CreateVehicle();

            // Act
            vehicle.MarkRented();

            // Assert
            Assert.Equal(VehicleStatus.Rented, vehicle.Inventory.Status);
        }

        [Fact]
        public void MarkServiced_ShouldUpdateInventoryStatusToUnderService()
        {
            // Arrange
            var vehicle = CreateVehicle();

            // Act
            vehicle.MarkServiced();

            // Assert
            Assert.Equal(VehicleStatus.UnderService, vehicle.Inventory.Status);
        }

        private Vehicle CreateVehicle()
        {
            var vehicleCode = new VehicleCode("V001");
            var vehicleType = VehicleType.SUV;
            var locationId = new LocationId(1);

            return new Vehicle(vehicleCode, vehicleType, locationId);
        }
    }
}
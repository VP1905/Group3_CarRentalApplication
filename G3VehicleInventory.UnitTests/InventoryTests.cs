using System;
using Xunit;
using G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate;
using G3VehicleInventory.Domain.Exceptions;

namespace G3VehicleInventory.UnitTests
{
    public class InventoryTests
    {
        [Fact]
        public void Constructor_WithValidArguments_SetsPropertiesCorrectly()
        {
            var locationId = new LocationId(1);
            var inventory = new Inventory(locationId, VehicleStatus.Available);

            Assert.Equal(locationId, inventory.LocationId);
            Assert.Equal(VehicleStatus.Available, inventory.Status);
        }

        [Fact]
        public void Constructor_WithNullLocationId_ThrowsArgumentNullException()
        {
            LocationId locationId = null!;

            Assert.Throws<ArgumentNullException>(() =>
                new Inventory(locationId, VehicleStatus.Available));
        }

        [Fact]
        public void MarkReserved_FromAvailable_SetsStatusToReserved()
        {
            var inventory = CreateInventory(VehicleStatus.Available);

            inventory.MarkReserved();

            Assert.Equal(VehicleStatus.Reserved, inventory.Status);
        }

        [Fact]
        public void MarkReserved_FromRented_ThrowsDomainException()
        {
            var inventory = CreateInventory(VehicleStatus.Rented);

            Assert.Throws<VehicleInventoryDomainException>(() =>
                inventory.MarkReserved());
        }

        [Fact]
        public void MarkReserved_FromUnderService_ThrowsDomainException()
        {
            var inventory = CreateInventory(VehicleStatus.UnderService);

            Assert.Throws<VehicleInventoryDomainException>(() =>
                inventory.MarkReserved());
        }

        [Fact]
        public void MarkRented_FromAvailable_SetsStatusToRented()
        {
            var inventory = CreateInventory(VehicleStatus.Available);

            inventory.MarkRented();

            Assert.Equal(VehicleStatus.Rented, inventory.Status);
        }

        [Fact]
        public void MarkRented_FromReserved_ThrowsDomainException()
        {
            var inventory = CreateInventory(VehicleStatus.Reserved);

            Assert.Throws<VehicleInventoryDomainException>(() =>
                inventory.MarkRented());
        }

        [Fact]
        public void MarkRented_FromRented_ThrowsDomainException()
        {
            var inventory = CreateInventory(VehicleStatus.Rented);

            Assert.Throws<VehicleInventoryDomainException>(() =>
                inventory.MarkRented());
        }

        [Fact]
        public void MarkRented_FromUnderService_ThrowsDomainException()
        {
            var inventory = CreateInventory(VehicleStatus.UnderService);

            Assert.Throws<VehicleInventoryDomainException>(() =>
                inventory.MarkRented());
        }

        [Fact]
        public void MarkAvailable_FromAvailable_RemainsAvailable()
        {
            var inventory = CreateInventory(VehicleStatus.Available);

            inventory.MarkAvailable();

            Assert.Equal(VehicleStatus.Available, inventory.Status);
        }

        [Fact]
        public void MarkAvailable_FromReserved_ThrowsDomainException()
        {
            var inventory = CreateInventory(VehicleStatus.Reserved);

            Assert.Throws<VehicleInventoryDomainException>(() =>
                inventory.MarkAvailable());
        }

        [Fact]
        public void MarkServiced_FromAvailable_SetsStatusToUnderService()
        {
            var inventory = CreateInventory(VehicleStatus.Available);

            inventory.MarkServiced();

            Assert.Equal(VehicleStatus.UnderService, inventory.Status);
        }

        [Fact]
        public void MarkServiced_FromRented_ThrowsDomainException()
        {
            var inventory = CreateInventory(VehicleStatus.Rented);

            Assert.Throws<VehicleInventoryDomainException>(() =>
                inventory.MarkServiced());
        }

        private Inventory CreateInventory(VehicleStatus status)
        {
            var locationId = new LocationId(1);
            return new Inventory(locationId, status);
        }
    }
}
using G3VehicleInventory.Application.DTOs;
using G3VehicleInventory.Application.Interfaces;
using G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Application.Services
{
    // Handles business logic related to vehicles.
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto)
        {
            if (!Enum.TryParse<VehicleType>(dto.VehicleType, true, out var vehicleType))
                throw new ArgumentException("Invalid vehicle type.");

            var vehicle = new Vehicle(
                new VehicleCode(dto.VehicleCode),
                vehicleType,
                new LocationId(dto.LocationId));

            _vehicleRepository.Add(vehicle);
            await _vehicleRepository.SaveChangesAsync();

            return Map(vehicle);
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.FindByIdAsync(id);
            return vehicle is null ? null : Map(vehicle);
        }

        public async Task<List<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            return vehicles.Select(Map).ToList();
        }

        public async Task<bool> UpdateVehicleStatusAsync(int id, UpdateVehicleStatusDto dto)
        {
            var vehicle = await _vehicleRepository.FindByIdAsync(id);
            if (vehicle is null)
                return false;

            if (!Enum.TryParse<VehicleStatus>(dto.Status, true, out var newStatus))
                throw new ArgumentException("Invalid vehicle status.");

            switch (newStatus)
            {
                case VehicleStatus.Available:
                    vehicle.MarkAvailable();
                    break;
                case VehicleStatus.Reserved:
                    vehicle.MarkReserved();
                    break;
                case VehicleStatus.Rented:
                    vehicle.MarkRented();
                    break;
                case VehicleStatus.UnderService:
                    vehicle.MarkServiced();
                    break;
                default:
                    throw new ArgumentException("Unsupported vehicle status.");
            }

            _vehicleRepository.Update(vehicle);
            await _vehicleRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteVehicleAsync(int id)
        {
            var vehicle = await _vehicleRepository.FindByIdAsync(id);
            if (vehicle is null)
                return false;

            _vehicleRepository.Delete(vehicle);
            await _vehicleRepository.SaveChangesAsync();
            return true;
        }

        private static VehicleDto Map(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                VehicleCode = vehicle.VehicleCode.Value,
                LocationId = vehicle.Inventory.LocationId.Value,
                VehicleType = vehicle.VehicleType.ToString(),
                Status = vehicle.Inventory.Status.ToString()
            };
        }
    }
}

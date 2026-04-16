using G3VehicleInventory.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Application.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto);
        Task<VehicleDto?> GetVehicleByIdAsync(int id);
        Task<List<VehicleDto>> GetAllVehiclesAsync();
        Task<bool> UpdateVehicleStatusAsync(int id, UpdateVehicleStatusDto dto);
        Task<bool> DeleteVehicleAsync(int id);
    }
}

using G3MaintenanceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace G3MaintenanceAPI.Data
{
    public class GR3MaintenanceDbContext : DbContext
    {
        public GR3MaintenanceDbContext(DbContextOptions<GR3MaintenanceDbContext> options)
            : base(options)
        {
        }

        public DbSet<GR3RepairHistory> RepairHistories { get; set; }
    }
}
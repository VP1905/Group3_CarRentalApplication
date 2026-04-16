using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Domain.SeedWork
{
    // T is constrained to IAggregateRoot, so you can only use this with aggregate roots.
    public interface IRepository<T> where T : IAggregateRoot
    {
        // Persists any pending changes to the database asynchronously
        Task SaveChangesAsync();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Domain.SeedWork
{

    // Base class for all domain entities. Handles identity and equality
    public abstract class Entity
    {
        private int? _requestedHashCode;
        private int _id;

        // The entity's unique id, settable only from within the class or subclasses
        public virtual int Id
        {
            get => _id;
            protected set => _id = value;
        }

        // An entity is "transient" when it hasn't been saved yet.
        public bool IsTransient() => Id == default;

        // Two entities are equal only if they are the same type and share the same Id.
        public override bool Equals(object? obj)
        {
            if (obj is not Entity item)
                return false;

            if (ReferenceEquals(this, item))
                return true;

            if (GetType() != item.GetType())
                return false;

            if (item.IsTransient() || IsTransient())
                return false;

            return item.Id == Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                _requestedHashCode ??= Id.GetHashCode() ^ 31;
                return _requestedHashCode.Value;
            }

            return base.GetHashCode();
        }

        public static bool operator ==(Entity? left, Entity? right) => Equals(left, right);
        public static bool operator !=(Entity? left, Entity? right) => !Equals(left, right);
    }
}

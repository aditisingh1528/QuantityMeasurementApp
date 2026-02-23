using System;

namespace QuantityMeasurementApp.Models
{
    public sealed class Feet
    {
        public double Value { get; }

        public Feet(double value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is null || GetType() != obj.GetType())
                return false;

            Feet other = (Feet)obj;

            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
    
}
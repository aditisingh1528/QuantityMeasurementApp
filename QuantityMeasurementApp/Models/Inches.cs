using System;

namespace QuantityMeasurementApp.Models
{
    public sealed class Inches
    {
        public double Value { get; }

        public Inches(double value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is null || GetType() != obj.GetType())
                return false;

            Inches other = (Inches)obj;

            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
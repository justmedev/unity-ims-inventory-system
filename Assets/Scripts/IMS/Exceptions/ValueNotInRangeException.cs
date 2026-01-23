using System;

namespace IMS.Exceptions
{
    /// <summary>
    ///     Thrown when an argument/value is not in a given range. Outputs value, min and max, and should thus be used
    ///     instead of <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    public class ValueNotInRangeException : Exception
    {
        public ValueNotInRangeException(int value, int min, int max)
            : base($"Value {value} is not in range [{min},{max}]")
        {
        }
    }
}
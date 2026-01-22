using System;

namespace IMS.Exceptions
{
    public class ValueNotInRangeException : Exception
    {
        public ValueNotInRangeException(int value, int min, int max)
            : base($"Value {value} is not in range [{min},{max}]")
        {
        }
    }
}
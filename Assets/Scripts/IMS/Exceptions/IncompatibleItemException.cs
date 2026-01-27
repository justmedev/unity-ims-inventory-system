using System;

namespace IMS.Exceptions
{
    /// <summary>
    ///     Thrown when you try to add items that are not of the same type together.
    /// </summary>
    public class IncompatibleItemException : Exception
    {
        public IncompatibleItemException(IItem item1, IItem item2)
            : base($"{item1.GetName()} is incompatible with {item2.GetName()}")
        {
        }
    }
}
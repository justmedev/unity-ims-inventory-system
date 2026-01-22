using System;

namespace IMS.Exceptions
{
    public class IncompatibleItemException : Exception
    {
        public IncompatibleItemException(IItem item1, IItem item2)
            : base($"{item1.GetName()} is incompatible with {item2.GetName()}") {}
    }
}
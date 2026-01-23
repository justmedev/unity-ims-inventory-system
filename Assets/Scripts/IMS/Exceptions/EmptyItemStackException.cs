using System;

namespace IMS.Exceptions
{
    /// <summary>
    ///     Thrown when the item stack that was tried to operate on was empty.
    /// </summary>
    public class EmptyItemStackException : Exception
    {
        public EmptyItemStackException() : base("The stack supplied was empty!")
        {
        }
    }
}
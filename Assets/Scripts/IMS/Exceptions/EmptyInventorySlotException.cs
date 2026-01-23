using System;

namespace IMS.Exceptions
{
    /// <summary>
    ///     Thrown when the item stack that was tried to operate on was empty.
    /// </summary>
    public class EmptyInventorySlotException : Exception
    {
        public EmptyInventorySlotException() : base("The inventory slot is empty!")
        {
        }
    }
}
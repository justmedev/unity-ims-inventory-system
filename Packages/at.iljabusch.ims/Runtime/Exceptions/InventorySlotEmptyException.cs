using System;

namespace IMS.Exceptions
{
    /// <summary>
    ///     Thrown when the item stack that was tried to operate on was empty.
    /// </summary>
    public class InventorySlotEmptyException : Exception
    {
        public InventorySlotEmptyException(int index) : base($"The inventory slot {index} is empty!")
        {
        }
    }
}
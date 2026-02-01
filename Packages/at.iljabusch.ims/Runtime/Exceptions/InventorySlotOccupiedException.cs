using System;

namespace IMS.Exceptions
{
    /// <summary>
    ///     Thrown when an inventory slot position already has an ItemStack attached to it.
    /// </summary>
    public class InventorySlotOccupiedException : Exception
    {
        public InventorySlotOccupiedException(int index)
            : base($"Inventory slot {index} is already occupied by another stack!")
        {
        }
    }
}
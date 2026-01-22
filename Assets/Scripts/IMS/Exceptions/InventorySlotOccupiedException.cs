using System;

namespace IMS.Exceptions
{
    public class InventorySlotOccupiedException : Exception
    {
        public InventorySlotOccupiedException(int index)
            : base($"Inventory slot {index} is already occupied by another stack!")
        {
        }
    }
}
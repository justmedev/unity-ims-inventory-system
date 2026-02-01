using System;

namespace IMS.Exceptions
{
    /// <summary>
    ///     Thrown when an ItemStack has reached its max capacity of items and cannot add any more items.
    /// </summary>
    public class ItemStackFullException : Exception
    {
        public ItemStackFullException(ItemStack itemStack, IItem item)
            : base(
                $"{item.GetName()} cannot be added to the itemStack! There are already {itemStack.Quantity}/{item.GetMaxQuantity()} items on the stack")
        {
        }
    }
}
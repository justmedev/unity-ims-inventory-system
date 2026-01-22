using System;

namespace IMS.Exceptions
{
    public class ItemStackFullException : Exception
    {
        public ItemStackFullException(ItemStack itemStack, IItem item)
            : base(
                $"{item.GetName()} cannot be added to the itemStack! There are already {itemStack.Quantity}/{item.GetMaxQuantity()} items on the stack")
        {
        }
    }
}
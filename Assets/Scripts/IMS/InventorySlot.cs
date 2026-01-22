using IMS.Exceptions;
using JetBrains.Annotations;

namespace IMS
{
    public class InventorySlot
    {
        [CanBeNull] public ItemStack ItemStack { get; private set; }

        public readonly int Index;

        public InventorySlot(int index)
        {
            Index = index;
        }

        // TODO: Support auto placing same-type item stacks

        /// <summary>
        /// Place an item stack onto the inventory slot.
        /// </summary>
        /// <param name="stack"></param>
        /// <exception cref="InventorySlotOccupiedException">When the slot already has another item stack.</exception>
        public void PlaceItemStack(ItemStack stack)
        {
            if (ItemStack != null) throw new InventorySlotOccupiedException(Index);
            ItemStack = stack;
        }
    }
}
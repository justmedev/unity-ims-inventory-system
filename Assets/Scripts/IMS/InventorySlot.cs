using IMS.Exceptions;
using JetBrains.Annotations;

namespace IMS
{
    /// <summary>
    ///     Represents a single inventory slot with lots of items (an ItemStack).
    /// </summary>
    public class InventorySlot
    {
        /// <summary>
        ///     The position of this slot
        /// </summary>
        public readonly int Index;

        private Logger _logger = new(nameof(InventorySlot));

        /// <summary>
        ///     Represents a single inventory slot with an ItemStack and a position (index). This should be created with
        ///     the inventory and not reused.
        /// </summary>
        /// <param name="index">The position (also called slot) this slot is at.</param>
        internal InventorySlot(int index)
        {
            Index = index;
        }

        /// <summary>
        ///     The ItemStack this slot holds. If empty, then this is null.
        /// </summary>
        [CanBeNull]
        public ItemStack ItemStack { get; private set; }

        /// <summary>
        ///     Check if there are any items on this slot.
        /// </summary>
        public bool IsEmpty => ItemStack == null;

        /// <summary>
        ///     Place an item stack onto the inventory slot.
        /// </summary>
        /// <param name="stack">The stack to place.</param>
        /// <exception cref="InventorySlotOccupiedException">When the slot already has another item stack.</exception>
        public void PlaceItemStack(ItemStack stack)
        {
            if (ItemStack != null) throw new InventorySlotOccupiedException(Index);
            ItemStack = stack;
        }

        /// <summary>
        ///     Remove an ItemStack from the slot.
        /// </summary>
        /// <exception cref="InventorySlotEmptyException">When the slot is empty.</exception>
        /// <returns>The removed <see cref="ItemStack" /></returns>
        public ItemStack RemoveItemStack()
        {
            if (ItemStack == null) throw new InventorySlotEmptyException(Index);
            var stack = ItemStack;
            ItemStack = null;
            return stack;
        }

        /// <summary>
        ///     Return a non-null ImageStack. Throws when the slot is empty.
        /// </summary>
        /// <exception cref="InventorySlotEmptyException">When the slot is empty.</exception>
        /// <returns>A non-null ItemStack</returns>
        [NotNull]
        public ItemStack GetItemStack()
        {
            if (IsEmpty || ItemStack == null) throw new InventorySlotEmptyException(Index);
            return ItemStack;
        }
    }
}
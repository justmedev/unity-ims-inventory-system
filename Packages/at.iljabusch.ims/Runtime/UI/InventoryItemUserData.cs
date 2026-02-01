using UnityEngine.UIElements;

namespace IMS.UI
{
    /// <summary>
    ///     Attached to an items <see cref="VisualElement.userData"/>.
    /// </summary>
    public record InventoryItemUserData
    {
        /// <summary>
        ///     The slot index (inventory position) this item is currently attached to.
        /// </summary>
        public readonly int AttachedSlotIndex;

        /// <summary>
        ///     The id of the inventory that this item is in.
        /// </summary>
        public readonly int InventoryId;

        /// <summary>
        ///     The ItemStack this <see cref="VisualElement"/> holds.
        /// </summary>
        public readonly ItemStack ItemStack;

        public InventoryItemUserData(int inventoryId, int attachedSlotIndex, ItemStack itemStack)
        {
            InventoryId = inventoryId;
            AttachedSlotIndex = attachedSlotIndex;
            ItemStack = itemStack;
        }
    };
}
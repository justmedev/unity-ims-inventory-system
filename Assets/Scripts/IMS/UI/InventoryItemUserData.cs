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
        ///     The ItemStack this <see cref="VisualElement"/> holds.
        /// </summary>
        public readonly ItemStack ItemStack;

        public InventoryItemUserData(int attachedSlotIndex, ItemStack itemStack)
        {
            AttachedSlotIndex = attachedSlotIndex;
            ItemStack = itemStack;
        }
    };
}
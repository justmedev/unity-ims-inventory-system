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

        public InventoryItemUserData(int attachedSlotIndex)
        {
            AttachedSlotIndex = attachedSlotIndex;
        }
    };
}
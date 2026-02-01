using UnityEngine.UIElements;

namespace IMS.UI
{
    /// <summary>
    ///     Attached to a slots <see cref="VisualElement.userData"/>.
    /// </summary>
    public record InventorySlotUserData
    {
        /// <summary>
        ///     The slot index (inventory position) of this slot.
        /// </summary>
        public readonly int Index;

        /// <summary>
        ///     The id of the inventory that this item is in.
        /// </summary>
        public readonly int InventoryId;

        public InventorySlotUserData(int inventoryId, int index)
        {
            InventoryId = inventoryId;
            Index = index;
        }
    }
}
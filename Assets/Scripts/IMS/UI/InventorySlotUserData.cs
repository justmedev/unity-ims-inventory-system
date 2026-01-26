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

        public InventorySlotUserData(int index)
        {
            Index = index;
        }
    }
}
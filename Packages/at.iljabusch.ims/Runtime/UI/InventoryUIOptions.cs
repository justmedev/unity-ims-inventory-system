using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace IMS.UI
{
    /// <summary>
    ///     Supplied to the <see cref="Inventory" /> to customize the UI display of the inventory.
    ///     Classes should be used for more in-depth styling.
    /// </summary>
    public record InventoryUIOptions
    {
        /// <summary>
        ///     The <see cref="VisualElement" /> this inventory UI attaches itself to. Could for example center the inventory.
        /// </summary>
        [NotNull] public VisualElement InventoryRoot;

        /// <summary>
        ///     All the items in the inventory are direct children of this <see cref="VisualElement"/>.
        ///     Make sure this is always in front of everything. Multiple inventories can share a single
        ///     <see cref="ItemRoot"/>.
        /// </summary>
        [CanBeNull] public VisualElement ItemRoot;

        /// <summary>
        ///     The size in pixels a inventory slot has. Inventory slots are always squares (1:1 aspect ratio)
        /// </summary>
        public int SlotSize = 72;

        /// <summary>
        ///     The size around slots and around the slot container of this inventory. Cannot be set in the class because
        ///     there are calculations that have to be made in the UI manager.
        /// </summary>
        public int Spacing = 4;

        /// <summary>
        ///     Options for customizing the UI.
        /// </summary>
        /// <param name="inventoryRoot">The root element this inventory is attached under.</param>
        public InventoryUIOptions(VisualElement inventoryRoot)
        {
            InventoryRoot = inventoryRoot;
        }
    }
}
namespace IMS.UI
{
    /// <summary>
    ///     Defines the class name constants that are added to different inventory attributes.
    /// </summary>
    public static class InventoryUIClasses
    {
        /// <summary>
        ///     The outer container/root of an inventory's window
        /// </summary>
        public const string WindowRoot = "inventory__container";

        /// <summary>
        ///     The container below the label that holds just the slots (inside <see cref="WindowRoot"/>)
        /// </summary>
        public const string SlotContainer = "inventory__slot-container";

        /// <summary>
        ///     A single slot inside the <see cref="SlotContainer"/>
        /// </summary>
        public const string Slot = "inventory__slot";

        /// <summary>
        ///     An item inside a <see cref="Slot"/>
        /// </summary>
        public const string SlotItem = "inventory__slot-item";

        /// <summary>
        ///     The quantity label (per default bottom right) inside the <see cref="SlotItem"/>
        /// </summary>
        public const string SlotItemQuantity = "inventory__slot-item-qty";
    }
}
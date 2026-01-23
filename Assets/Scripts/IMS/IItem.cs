namespace IMS
{
    /// <summary>
    ///     Represents a single item. Should be implemented for each item
    ///     <example>a Scriptable Object can implement this to be usable as an inventory item.</example>
    /// </summary>
    public interface IItem
    {
        /// <summary>
        ///     The player-facing name of this item.
        /// </summary>
        /// <returns>The name of this item</returns>
        public string GetName();

        /// <summary>
        ///     Returns a positive, non-null, >1 number describing the max stack size of this item.
        /// </summary>
        /// <returns>Positive, non-null, > 1 number</returns>
        public int GetMaxQuantity();
    }
}
namespace IMS
{
    public interface IItem
    {
        public string GetName();

        /// <summary>
        /// Returns a positive, non-null, >1 number describing the max stack size of this item.
        /// </summary>
        /// <returns>Positive, non-null, > 1 number</returns>
        public int GetMaxQuantity();
    }
}
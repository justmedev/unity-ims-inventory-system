namespace IMS
{
    public record InventorySlotUserData
    {
        public readonly int Index;

        public InventorySlotUserData(int index)
        {
            Index = index;
        }
    }
}
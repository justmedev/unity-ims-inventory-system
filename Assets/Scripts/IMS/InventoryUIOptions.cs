using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;

namespace IMS
{
    public record InventoryUIOptions
    {
        [NotNull] public VisualElement InventoryRoot;
        public int Spacing = 4;
        public int SlotSize = 72;

        public InventoryUIOptions(VisualElement inventoryRoot)
        {
            InventoryRoot = inventoryRoot;
        }
    }
}
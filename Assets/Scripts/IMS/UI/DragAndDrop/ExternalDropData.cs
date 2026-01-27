using UnityEngine.UIElements;

namespace IMS.UI.DragAndDrop
{
    public record ExternalDropData(
        VisualElement InventoryRoot,
        VisualElement Target,
        IPointerEvent PointerEvent,
        Inventory SrcInventory);
}
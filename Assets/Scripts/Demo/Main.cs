using Demo.Items;
using IMS;
using IMS.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Demo
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private Item item;
        [SerializeField] private UIDocument document;

        private void Start()
        {
            var inventoryRoot = document.rootVisualElement.Q("inventoryRoot");
            var inventory = new Inventory(
                "Inventory",
                6,
                3,
                new InventoryUIOptions(inventoryRoot)
            );

            inventory.SetUIManagerItemVisualElementModifier((ref VisualElement ve) =>
            {
                var dm = new DragManipulator();
                dm.OnDrop += (@event, itemVe) =>
                {
                    if (!InventoryUIUtils.TryGetInventorySlotAtPosition(inventoryRoot, @event.position, out var slotVe))
                        return false;
                    Debug.Log("TryGetInventorySlotAtPosition");
                    if (!InventoryUIUtils.TryGetTypedUserData<InventorySlotUserData>(slotVe, out var slotData))
                        return false;
                    Debug.Log("TryGetTypedUserData<InventorySlotUserData>");
                    // if (!InventoryUIUtils.TryGetTypedUserData<InventoryItemUserData>(itemVe, out var itemData))
                    //     return false;
                    // Debug.Log("TryGetTypedUserData<InventoryItemUserData>");

                    // inventory.PlaceItemStack(slotData.Index, itemData.AttachedSlotIndex);
                    InventoryUIUtils.SnapVisualElementToOtherVisualElement(itemVe, slotVe);
                    return true;
                };
                ve.AddManipulator(dm);
            });
            inventory.PlaceItemStack(0, new ItemStack(item, 5));
            inventory.ModifySlotItemStack(0,
                (ref ItemStack stack) => { Debug.Log(stack.AddStack(new ItemStack(item, 10)).Quantity); });
        }
    }
}
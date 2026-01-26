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
                    if (!InventoryUIUtils.TryGetTypedUserData<InventorySlotUserData>(slotVe, out var slotData))
                        return false;
                    if (!InventoryUIUtils.TryGetTypedUserData<InventoryItemUserData>(itemVe, out var itemData))
                        return false;
                    Debug.Log($"TryGetTypedUserData<InventoryItemUserData> preivous: {itemData.AttachedSlotIndex}");

                    // Remove from previous index
                    if (inventory.TryGetItemStackAt(itemData.AttachedSlotIndex, out _))
                    {
                        inventory.Slots[itemData.AttachedSlotIndex].RemoveItemStack();
                        // TODO: Split stack
                    }

                    // Move into new
                    if (inventory.TryGetItemStackAt(slotData.Index, out _))
                    {
                        inventory.ModifySlotItemStack(slotData.Index, (ref ItemStack itemStack) =>
                        {
                            var overflow = itemStack.AddStack(itemData.ItemStack);
                            if (overflow.Quantity != 0) Debug.Log($"{overflow.Quantity} items overflowed!");
                            // TODO: Handle overflow
                        });
                        itemVe.RemoveFromHierarchy();
                        return true;
                    }

                    inventory.PlaceItemStack(slotData.Index, itemData.ItemStack);
                    itemVe.RemoveFromHierarchy();
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
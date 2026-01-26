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
        private Inventory _inventory;

        private void Start()
        {
            var inventoryRoot = document.rootVisualElement.Q("inventoryRoot");
            _inventory = new Inventory(
                "Inventory",
                6,
                3,
                new InventoryUIOptions(inventoryRoot)
            );

            _inventory.SetUIManagerItemVisualElementModifier((ref VisualElement ve) =>
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
                    if (_inventory.TryGetItemStackAt(itemData.AttachedSlotIndex, out _))
                    {
                        _inventory.Slots[itemData.AttachedSlotIndex].RemoveItemStack();
                        // TODO: Split stack
                    }

                    // Move into new
                    if (_inventory.TryGetItemStackAt(slotData.Index, out _))
                    {
                        _inventory.ModifySlotItemStack(slotData.Index, (ref ItemStack itemStack) =>
                        {
                            var overflow = itemStack.AddStack(itemData.ItemStack);
                            if (overflow.Quantity != 0) Debug.Log($"{overflow.Quantity} items overflowed!");
                            // TODO: Handle overflow
                        });
                        itemVe.RemoveFromHierarchy();
                        return true;
                    }

                    _inventory.PlaceItemStack(slotData.Index, itemData.ItemStack);
                    itemVe.RemoveFromHierarchy();
                    return true;
                };
                ve.AddManipulator(dm);
            });
            _inventory.PlaceItemStack(0, new ItemStack(item, 5));
            _inventory.ModifySlotItemStack(0,
                (ref ItemStack stack) => { Debug.Log(stack.AddStack(new ItemStack(item, 10)).Quantity); });
        }
    }
}
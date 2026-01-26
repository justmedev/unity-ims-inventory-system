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
        private Inventory _hotbar;

        private void Start()
        {
            var inventoryRoot = document.rootVisualElement.Q("inventoryRoot");
            var itemRoot = document.rootVisualElement.Q("itemRoot");
            _inventory = new Inventory(
                "Inventory",
                6,
                3,
                new InventoryUIOptions(inventoryRoot) { ItemRoot = itemRoot }
            );
            _inventory.SetUIManagerItemVisualElementModifier((ref VisualElement ve) =>
                CreateItemVisualElementModifier(_inventory, document.rootVisualElement, ref ve));

            var hotbarRoot = document.rootVisualElement.Q("hotbarRoot");
            _hotbar = new Inventory(
                "Hotbar",
                10,
                1,
                new InventoryUIOptions(hotbarRoot) { ItemRoot = itemRoot }
            );
            _hotbar.SetUIManagerItemVisualElementModifier((ref VisualElement ve) =>
                CreateItemVisualElementModifier(_hotbar, document.rootVisualElement, ref ve));

            _inventory.PlaceItemStack(0, new ItemStack(item, 5));
        }

        private static void CreateItemVisualElementModifier(Inventory inventory, VisualElement inventoryRoot,
            ref VisualElement ve)
        {
            var dm = new DragManipulator();
            dm.OnDrop += (@event, itemVe) =>
            {
                if (!InventoryUIUtils.TryGetInventorySlotAtPosition(inventoryRoot, @event.position,
                        out var slotVe)) return false;
                if (!InventoryUIUtils.TryGetTypedUserData<InventorySlotUserData>(slotVe, out var slotData))
                    return false;
                if (!InventoryUIUtils.TryGetTypedUserData<InventoryItemUserData>(itemVe, out var itemData))
                    return false;

                var dstInventory = InventoryManager.Instance.GetInventoryById(slotData.InventoryId);

                // Remove from previous index
                if (inventory.TryGetItemStackAt(itemData.AttachedSlotIndex, out _))
                {
                    inventory.Slots[itemData.AttachedSlotIndex].RemoveItemStack();
                    // TODO: Split stack
                }

                // Move into new
                if (dstInventory.TryGetItemStackAt(slotData.Index, out _))
                {
                    dstInventory.ModifySlotItemStack(slotData.Index, (ref ItemStack itemStack) =>
                    {
                        var overflow = itemStack.AddStack(itemData.ItemStack);
                        if (overflow.Quantity != 0) Debug.Log($"{overflow.Quantity} items overflowed!");
                        // TODO: Handle overflow
                    });
                    itemVe.RemoveFromHierarchy();
                    return true;
                }

                dstInventory.PlaceItemStack(slotData.Index, itemData.ItemStack);
                itemVe.RemoveFromHierarchy();
                return true;
            };
            ve.AddManipulator(dm);
        }
    }
}
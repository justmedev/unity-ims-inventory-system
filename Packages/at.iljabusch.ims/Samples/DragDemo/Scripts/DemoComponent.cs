using IMS.UI;
using IMS.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.UIElements;

namespace IMS.Demo
{
    /// <summary>
    ///     A simple component that demonstrates the use the IMS inventory system.
    /// </summary>
    public class DemoComponent : MonoBehaviour
    {
        [SerializeField] private Item carrot;
        [SerializeField] private Item cucumber;
        [SerializeField] private UIDocument document;
        private Inventory _hotbar;
        private Inventory _inventory;
        private readonly OnDropHandler _onDropHandler = new();

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

            _inventory.PlaceItemStack(0, new ItemStack(carrot, 5));
            _inventory.PlaceItemStack(2, new ItemStack(carrot, 5));
            _inventory.PlaceItemStack(4, new ItemStack(carrot, 5));
            _inventory.PlaceItemStack(5, new ItemStack(cucumber, 3));
        }

        private void CreateItemVisualElementModifier(Inventory inventory, VisualElement inventoryRoot,
            ref VisualElement ve)
        {
            var dm = new DragManipulator();
            dm.OnDrop += (pointerEvent, target) => _onDropHandler.HandleOnDrop(new ExternalDropData(
                inventoryRoot,
                target,
                pointerEvent,
                inventory
            ));
            ve.AddManipulator(dm);
        }
    }
}
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
            var inventory = new Inventory(
                "Inventory",
                6,
                3,
                new InventoryUIOptions(document.rootVisualElement.Q("inventoryRoot"))
            );

            inventory.SetUIManagerItemVisualElementModifier((ref VisualElement ve) =>
            {
                ve.AddManipulator(new InventoryItemDragManipulator());
            });
            inventory.PlaceItemStack(0, new ItemStack(item, 5));
            inventory.ModifySlotItemStack(0,
                (ref ItemStack stack) => { Debug.Log(stack.AddStack(new ItemStack(item, 10)).Quantity); });
        }
    }
}
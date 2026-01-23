using Demo.Items;
using IMS;
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

            inventory.PlaceItemStack(0, new ItemStack(item));
        }
    }
}
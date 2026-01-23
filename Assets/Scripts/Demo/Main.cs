using IMS;
using UnityEngine;
using UnityEngine.UIElements;

namespace Demo
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private UIDocument document;

        private void Start()
        {
            new Inventory(
                "Inventory",
                6,
                3,
                new InventoryUIOptions(document.rootVisualElement.Q("inventoryRoot"))
            );
        }
    }
}
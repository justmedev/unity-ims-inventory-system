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
            var inventory = new Inventory(document.rootVisualElement.Q("inventoryRoot"), new InventoryUIOptions(), 6,
                3);
        }
    }
}
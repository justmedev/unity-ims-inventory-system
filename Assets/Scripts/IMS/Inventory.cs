using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace IMS
{
    public class Inventory
    {
        public IReadOnlyList<InventorySlot> Slots => _slots;

        [NotNull] private readonly InventoryUIManager _uiManager;
        [NotNull] private readonly List<InventorySlot> _slots;
        public int Cols { get; }

        public Inventory(VisualElement inventoryContainer, InventoryUIOptions options, int cols, int rows)
        {
            Cols = cols;
            _uiManager = new InventoryUIManager(this);
            _slots = new List<InventorySlot>(cols * rows);
            for (var i = 0; i < cols * rows; i++)
            {
                _slots.Add(new InventorySlot(i));
            }

            // Create UI Toolkit
            inventoryContainer.Add(_uiManager.CreateInventory());
        }
    }
}
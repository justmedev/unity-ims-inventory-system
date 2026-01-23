using System.Collections.Generic;
using JetBrains.Annotations;

namespace IMS
{
    public class Inventory
    {
        public IReadOnlyList<InventorySlot> Slots => _slots;

        public string Name { get; }
        [NotNull] private readonly InventoryUIManager _uiManager;
        [NotNull] private readonly List<InventorySlot> _slots;
        public int Cols { get; }

        public Inventory(string name, int cols, int rows, InventoryUIOptions options)
        {
            Name = name;
            Cols = cols;
            _uiManager = new InventoryUIManager(this, options);
            _slots = new List<InventorySlot>(cols * rows);
            for (var i = 0; i < cols * rows; i++)
            {
                _slots.Add(new InventorySlot(i));
            }

            _uiManager.CreateInventory();
        }
    }
}
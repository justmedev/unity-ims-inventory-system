using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace IMS
{
    public sealed class InventoryManager
    {
        static InventoryManager()
        {
        }

        private InventoryManager()
        {
        }

        public static InventoryManager Instance { get; } = new();

        private readonly List<Inventory> _inventories = new();

        /// <summary>
        ///     Register a new inventory with the manager.
        /// </summary>
        /// <remarks>This is called internally and should never be used manually.</remarks>
        /// <param name="inventory">The inventory to register</param>
        internal void RegisterInventory(Inventory inventory)
        {
            _inventories.Add(inventory);
        }

        /// <summary>
        ///     Called internally when an inventory is disposed.
        /// </summary>
        /// <param name="inventory">The inventory to remove from the manager.</param>
        internal void UnregisterInventory(Inventory inventory)
        {
            _inventories.Remove(inventory);
        }

        /// <summary>
        ///     Tries to find an inventory with id <see cref="id"/>
        /// </summary>
        /// <param name="id">The id of the inventory.</param>
        /// <returns>The found inventory, or null, if none is found</returns>
        [CanBeNull]
        public Inventory GetInventoryById(int id)
        {
            return _inventories.FirstOrDefault(i => i.Id == id);
        }
    }
}
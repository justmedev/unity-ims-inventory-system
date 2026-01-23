using System.Collections.Generic;
using IMS.Exceptions;
using JetBrains.Annotations;

namespace IMS
{
    /// <summary>
    ///     Encapsulates the slots and ui with business logic for the inventory. Very customizable: size, name, ...
    /// </summary>
    public class Inventory
    {
        [NotNull] private readonly List<InventorySlot> _slots;

        [NotNull] private readonly InventoryUIManager _uiManager;

        /// <summary>
        ///     Create a new inventory, also creates the slots and UI. The size cannot be changed later.
        /// </summary>
        /// <param name="name">The name of this inventory - this cannot be changed later.</param>
        /// <param name="cols">The amount of columns - this cannot be changed later.</param>
        /// <param name="rows">The amount of rows - this cannot be changed later.</param>
        /// <param name="options">The UI options of this inventory - this cannot be changed later.</param>
        public Inventory(string name, int cols, int rows, InventoryUIOptions options)
        {
            Name = name;
            Cols = cols;
            _uiManager = new InventoryUIManager(this, options);
            _slots = new List<InventorySlot>(cols * rows);
            for (var i = 0; i < cols * rows; i++) _slots.Add(new InventorySlot(i));

            _uiManager.CreateInventory();
        }

        /// <summary>
        ///     All the slots this inventory has. The list index can be used to get a slot at a specific position: it
        ///     matches the inventory slot index.
        /// </summary>
        public IReadOnlyList<InventorySlot> Slots => _slots;

        /// <summary>
        ///     The name of this inventory used for a UI title (not unique), ...
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The amount of columns this inventory holds. The rows can easily be calculated by calculating
        ///     <c>Slots.Count / Cols</c>
        /// </summary>
        public int Cols { get; }

        /// <summary>
        ///     Place and render an item in a slot.
        /// </summary>
        /// <param name="index">The slot position to place the ItemStack into.</param>
        /// <param name="stack">The stack to place onto the slot.</param>
        /// <exception cref="InventorySlotOccupiedException">When the slot already has another item stack.</exception>
        public void PlaceItemStack(int index, [NotNull] ItemStack stack)
        {
            Slots[index].PlaceItemStack(stack);
            _uiManager.Render(index);
        }
    }
}
using System;
using System.Collections.Generic;
using IMS.Exceptions;
using IMS.UI;
using JetBrains.Annotations;

namespace IMS
{
    /// <summary>
    ///     Encapsulates the slots and ui with business logic for the inventory. Very customizable: size, name, ...
    /// </summary>
    public class Inventory : IDisposable
    {
        #region Delegates

        public delegate void SlotModifier([NotNull] ref ItemStack stack);

        #endregion

        [NotNull] private readonly List<InventorySlot> _slots;

        [NotNull] private readonly InventoryUIManager _uiManager;

        /// <summary>
        ///     The unique identifier for this inventory.
        /// </summary>
        public readonly int Id = AtomicIntSequencer.GetNext();

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
            InventoryManager.Instance.RegisterInventory(this);
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

        public static IEqualityComparer<Inventory> IdComparer { get; } = new IdEqualityComparer();

        #region IDisposable Members

        public void Dispose()
        {
            InventoryManager.Instance.UnregisterInventory(this);
        }

        #endregion

        public void SetUIManagerItemVisualElementModifier(InventoryUIManager.ItemVisualElementModifier modifier)
        {
            _uiManager.ItemModifier = modifier;
        }

        /// <summary>
        ///     Try to get an item stack at an index (inventory position).
        /// </summary>
        /// <param name="index">The position in the inventory.</param>
        /// <param name="itemStack">The <see cref="ItemStack" /> at that position.</param>
        /// <returns>Whether a match was found.</returns>
        public bool TryGetItemStackAt(int index, out ItemStack itemStack)
        {
            itemStack = null;
            var slot = Slots[index];
            if (slot.IsEmpty) return false;
            itemStack = slot.ItemStack;

            return true;
        }

        /// <summary>
        ///     Modify slot <see cref="ItemStack" /> data and re-render the UI at once.
        /// </summary>
        /// <remarks>WARNING: ItemStack cannot be null!</remarks>
        /// <param name="index">Position of the slot</param>
        /// <param name="modifier">Lambda with <see cref="ItemStack" /> reference you can modify.</param>
        /// <exception cref="InventorySlotEmptyException">When <see cref="ItemStack" /> is null.</exception>
        public void ModifySlotItemStack(int index, [NotNull] SlotModifier modifier)
        {
            var stack = _slots[index].ItemStack;
            if (stack == null) throw new InventorySlotEmptyException(index);
            modifier.Invoke(ref stack);
            PropagateChange(index);
        }

        /// <summary>
        ///     Place and render an item in a slot.
        /// </summary>
        /// <param name="index">The slot position to place the ItemStack into.</param>
        /// <param name="stack">The stack to place onto the slot.</param>
        /// <exception cref="InventorySlotOccupiedException">When the slot already has another item stack.</exception>
        public void PlaceItemStack(int index, [NotNull] ItemStack stack)
        {
            Slots[index].PlaceItemStack(stack);
            PropagateChange(index);
        }

        /// <summary>
        ///     After directly modifying the <see cref="Slots" /> data (adding/removing stacks, modifying content, ...)
        ///     This should be called to reflect the changes in the UI.
        /// </summary>
        /// <param name="index">The slot position/index you modified.</param>
        public void PropagateChange(int index)
        {
            _uiManager.RebuildHierarchy(index);
        }

        /// <summary>
        ///     After directly modifying multiple <see cref="Slots" />, or <see cref="Slots" /> you do not know the
        ///     index off, call this to re-render all the UI.
        /// </summary>
        public void PropagateChanges()
        {
            _uiManager.RebuildHierarchy();
        }

        #region Nested type: IdEqualityComparer

        private sealed class IdEqualityComparer : IEqualityComparer<Inventory>
        {
            #region IEqualityComparer<Inventory> Members

            public bool Equals(Inventory x, Inventory y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null) return false;
                if (y is null) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(Inventory obj)
            {
                return obj.Id;
            }

            #endregion
        }

        #endregion
    }
}
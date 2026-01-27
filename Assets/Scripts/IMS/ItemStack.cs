using System;
using IMS.Exceptions;
using JetBrains.Annotations;

namespace IMS
{
    /// <summary>
    ///     Holds multiple items with a single item type.
    /// </summary>
    public class ItemStack
    {
        private int _quantity;

        /// <summary>
        ///     Hold multiple items in a stack.
        /// </summary>
        /// <remarks>WARNING: An item stack starts empty, so with this constructor the quantity will be 0!</remarks>
        /// <param name="item">The type of item this stack holds. The type cannot change!</param>
        public ItemStack([NotNull] IItem item)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }

        /// <summary>
        ///     Hold multiple items in a stack.
        /// </summary>
        /// <param name="item">The type of the stack.</param>
        /// <param name="quantity">The amount of items this stack manages.</param>
        /// <exception cref="ValueNotInRangeException">When quantity is not in the range [0;MaxQuantity]</exception>
        public ItemStack(IItem item, int quantity)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
            Quantity = quantity;
        }

        /// <summary>
        ///     Represents the item type this inventory holds.
        /// </summary>
        [NotNull]
        public IItem Item { get; }

        /// <summary>
        ///     Amount of items on the ItemStack
        /// </summary>
        /// <exception cref="ValueNotInRangeException">Must be a positive number in the range of [0,MaxQuantity]</exception>
        public int Quantity
        {
            get => _quantity;
            private set
            {
                if (value < 0 || value > Item.GetMaxQuantity())
                    throw new ValueNotInRangeException(value, 0, Item.GetMaxQuantity());
                _quantity = value;
            }
        }

        /// <summary>
        ///     Returns if this <see cref="ItemStack" /> is empty.
        /// </summary>
        public bool IsEmpty => Quantity == 0;

        /// <summary>
        ///     Add an item to the inventory stack, if possible.
        /// </summary>
        /// <exception cref="ItemStackFullException">
        ///     When the item stack capacity is reached and thus, the item would overflow the
        ///     stack.
        /// </exception>
        public void AddItem()
        {
            if (Item.GetMaxQuantity() < Quantity + 1) throw new ItemStackFullException(this, Item);
            Quantity++;
        }

        /// <summary>
        ///     Remove a single item from the stack and return it.
        /// </summary>
        [CanBeNull]
        public IItem TakeItem()
        {
            if (IsEmpty) return null;
            Quantity--;
            return Item;
        }


        /// <summary>
        ///     Take multiple items of the stack.
        /// </summary>
        /// <param name="quantity">The amount of items to remove/take. Must be greater than 0.</param>
        /// <exception cref="ArgumentOutOfRangeException">When the quantity is not greater than 0.</exception>
        /// <returns>A new stack, with just the items that were removed.</returns>
        public ItemStack TakeItems(int quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            if (quantity > Quantity)
            {
                Quantity = 0;
                return new ItemStack(Item, Quantity);
            }

            Quantity -= quantity;
            return new ItemStack(Item, quantity);
        }

        /// <summary>
        ///     Adds all items from a supplied stack onto this stack and returns the remaining stack
        /// </summary>
        /// <param name="stack">The other stack to add onto this stack.</param>
        /// <exception cref="IncompatibleItemException">The types of items on this stack differ from the stack supplied.</exception>
        /// <returns>A stack with all the items that could not be added (Due to reaching capacity mid-adding).</returns>
        public ItemStack AddStack([NotNull] ItemStack stack)
        {
            if (stack.Item != Item) throw new IncompatibleItemException(Item, stack.Item);
            var overflow = new ItemStack(Item, 0)
            {
                Quantity = Math.Max(0, Quantity + stack.Quantity - Item.GetMaxQuantity())
            };
            Quantity = Math.Min(Quantity + stack.Quantity, Item.GetMaxQuantity());
            return overflow;
        }

        protected bool Equals(ItemStack other)
        {
            return Item.Equals(other.Item);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ItemStack)obj);
        }

        public override int GetHashCode()
        {
            return Item.GetHashCode();
        }
    }
}
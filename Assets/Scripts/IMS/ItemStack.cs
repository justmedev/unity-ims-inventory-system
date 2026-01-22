using System;
using IMS.Exceptions;
using JetBrains.Annotations;

namespace IMS
{
    public class ItemStack
    {
        [NotNull] public IItem Item { get; private set; }
        private int _quantity;

        /// <summary>
        /// Amount of items on the ItemStack
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

        public bool IsEmpty => Quantity == 0;

        /// <summary>
        /// Hold multiple items in a stack.
        /// </summary>
        /// <param name="item">The type of item this stack holds. The type cannot change!</param>
        public ItemStack(IItem item)
        {
            Item = item;
        }

        /// <summary>
        /// Hold multiple items in a stack.
        /// </summary>
        /// <param name="item">The type of the stack.</param>
        /// <param name="quantity">The amount of items this stack manages.</param>
        /// <exception cref="ValueNotInRangeException">When quantity is not in the range [0;MaxQuantity]</exception>
        public ItemStack(IItem item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        /// <summary>
        /// Add an item to the inventory stack, if possible.
        /// </summary>
        /// <param name="item">The item to add. Must be the same type as the items that are already on the stack.</param>
        /// <exception cref="IncompatibleItemException">When adding an item that has a different type than the items on the stack already.</exception>
        /// <exception cref="ItemStackFullException">When the item stack capacity is reached and thus, the item would overflow the stack.</exception>
        public void AddItem([NotNull] IItem item)
        {
            if (!item.Equals(Item)) throw new IncompatibleItemException(Item, item);
            if (Item.GetMaxQuantity() > Quantity + 1) throw new ItemStackFullException(this, item);
            Quantity++;
        }

        /// <summary>
        /// Remove a single item from the stack and return it.
        /// </summary>
        [CanBeNull]
        public IItem TakeItem()
        {
            if (IsEmpty) return null;
            Quantity--;
            return Item;
        }


        /// <summary>
        /// Take multiple items of the stack.
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
        /// Adds all items from a supplied stack onto this stack and returns the remaining stack
        /// </summary>
        /// <param name="stack">The other stack to add onto this stack.</param>
        /// <exception cref="IncompatibleItemException">The types of items on this stack differ from the stack supplied.</exception>
        /// <returns>A stack with all the items that could not be added (Due to reaching capacity mid-adding).</returns>
        public ItemStack AddStack([NotNull] ItemStack stack)
        {
            if (stack.Quantity == 0) return stack;
            for (var i = 0; i < stack.Quantity; i++)
            {
                try
                {
                    var item = stack.TakeItem() ?? throw new Exception("Stack was modified in iteration!");
                    AddItem(item);
                }
                catch (ItemStackFullException)
                {
                    stack.AddItem(stack.Item);
                }
            }

            return stack;
        }
    }
}
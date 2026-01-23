using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;

namespace IMS
{
    /// <summary>
    ///     Manages the inventory's UI display and interaction. Only to be used by the <see cref="Inventory" />.
    /// </summary>
    internal class InventoryUIManager
    {
        private Logger _logger = new(nameof(InventoryUIManager));

        [NotNull] private readonly Inventory _inventory;
        private readonly InventoryUIOptions _options;
        private List<VisualElement> _renderedSlots = new();

        internal InventoryUIManager(Inventory inv, InventoryUIOptions options)
        {
            _inventory = inv;
            _options = options;
        }

        /// <summary>
        ///     Create a new Inventory UI with the following USS classes assigned:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>The container uses inventory__container</description>
        ///         </item>
        ///         <item>
        ///             <description>The slot container uses inventory__slot-container</description>
        ///         </item>
        ///         <item>
        ///             <description>The slots use inventory__slot</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <returns>The full inventory UI</returns>
        internal virtual void CreateInventory()
        {
            var containerVe = new VisualElement
            {
                style =
                {
                    marginLeft = _options.Spacing * 2,
                    marginRight = _options.Spacing * 2,
                    width = (_options.SlotSize + _options.Spacing) * _inventory.Cols + _options.Spacing * 3,
                    flexGrow = 0
                }
            };
            containerVe.AddToClassList("inventory__container");

            var label = new Label
            {
                text = _inventory.Name
            };

            var slotContainerVe = new VisualElement
            {
                style =
                {
                    marginTop = _options.Spacing,
                    marginBottom = _options.Spacing,
                    marginLeft = _options.Spacing,
                    marginRight = _options.Spacing,
                    paddingTop = _options.Spacing / 2,
                    paddingBottom = _options.Spacing / 2,
                    paddingLeft = _options.Spacing / 2,
                    paddingRight = _options.Spacing / 2,
                    flexWrap = new StyleEnum<Wrap>(Wrap.Wrap),
                    flexDirection = FlexDirection.Row,
                    flexGrow = 0
                }
            };
            slotContainerVe.AddToClassList("inventory__slot-container");
            containerVe.Add(label);
            containerVe.Add(slotContainerVe);

            _renderedSlots = new List<VisualElement>();
            foreach (var _ in _inventory.Slots)
            {
                var slotVe = new VisualElement
                {
                    style =
                    {
                        marginTop = _options.Spacing / 2,
                        marginBottom = _options.Spacing / 2,
                        marginLeft = _options.Spacing / 2,
                        marginRight = _options.Spacing / 2,
                        width = _options.SlotSize,
                        height = _options.SlotSize,
                        flexShrink = 0
                    }
                };
                slotVe.AddToClassList("inventory__slot");
                slotContainerVe.Add(slotVe);
                _renderedSlots.Add(slotVe);
            }

            _options.InventoryRoot.Add(containerVe);
        }

        /// <summary>
        ///     Renders all slots. Do not use this when you know what slot index you want to update.
        /// </summary>
        public void Render()
        {
            foreach (var slot in _inventory.Slots)
            {
                Render(slot.Index);
            }
        }

        /// <summary>
        ///     Render a single slot by index (position).
        /// </summary>
        /// <param name="slotIndex">The position of the slot you want to render.</param>
        public void Render(int slotIndex)
        {
            _logger.Info($"Render@{slotIndex}");
            var slot = _inventory.Slots[slotIndex];
            var slotVe = _renderedSlots[slotIndex];

            if (slot.IsEmpty)
            {
                foreach (var child in slotVe.Children())
                {
                    child.RemoveFromHierarchy();
                }

                return;
            }

            var itemVe = new VisualElement
            {
                style =
                {
                    backgroundImage = new StyleBackground(slot.GetImageStack().Item.GetSprite()),
                    width = new StyleLength(new Length(100, LengthUnit.Percent)),
                    height = new StyleLength(new Length(100, LengthUnit.Percent))
                }
            };
            slotVe.Add(itemVe);
        }
    }
}
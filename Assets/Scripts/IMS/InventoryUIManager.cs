using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;

namespace IMS
{
    /// <summary>
    /// Manages the inventory's UI display and interaction. Only to be used by the <see cref="Inventory"/>.
    /// </summary>
    internal class InventoryUIManager
    {
        [NotNull] private readonly Inventory _inventory;
        private readonly InventoryUIOptions _options;

        internal InventoryUIManager(Inventory inv, InventoryUIOptions options)
        {
            _inventory = inv;
            _options = options;
        }

        /// <summary>
        /// Create a new Inventory UI with the following USS classes assigned:
        /// <list type="bullet">
        /// <item>
        /// <description>The container uses inventory__container</description>
        /// </item>
        /// <item>
        /// <description>The slot container uses inventory__slot-container</description>
        /// </item>
        /// <item>
        /// <description>The slots use inventory__slot</description>
        /// </item>
        /// </list>
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
            }

            _options.InventoryRoot.Add(containerVe);
        }
    }
}
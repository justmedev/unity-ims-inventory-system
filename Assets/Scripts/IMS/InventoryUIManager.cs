using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;

namespace IMS
{
    public class InventoryUIManager
    {
        [NotNull] private readonly Inventory _inventory;
        private readonly InventoryUIOptions _options;

        public InventoryUIManager(Inventory inv)
        {
            _inventory = inv;
            _options = new InventoryUIOptions();
        }

        public InventoryUIManager(Inventory inv, InventoryUIOptions options)
        {
            _inventory = inv;
            _options = options;
        }

        // TODO: Everything should be customizable

        /// <summary>
        /// Create a new Inventory VisualElement
        /// </summary>
        /// <returns>The full inventory UI</returns>
        public virtual VisualElement CreateInventory()
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

            var label = new Label
            {
                text = "Inventory"
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

                slotContainerVe.Add(slotVe);
            }

            return containerVe;
        }
    }
}
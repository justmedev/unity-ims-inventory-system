using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace IMS.UI
{
    /// <summary>
    ///     Manages the inventory's UI display and interaction. Only to be used by the <see cref="Inventory" />.
    /// </summary>
    public class InventoryUIManager
    {
        public delegate void ItemVisualElementModifier(ref VisualElement itemVe);

        [CanBeNull] public ItemVisualElementModifier ItemModifier;

        private readonly Logger _logger = new(nameof(InventoryUIManager));

        [System.Diagnostics.CodeAnalysis.NotNull]
        private readonly Inventory _inventory;

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
            var windowVe = new VisualElement
            {
                style =
                {
                    marginLeft = _options.Spacing * 2,
                    marginRight = _options.Spacing * 2,
                    width = (_options.SlotSize + _options.Spacing) * _inventory.Cols + _options.Spacing * 3,
                    flexGrow = 0
                }
            };
            windowVe.AddToClassList(InventoryUIClasses.WindowRoot);

            var label = new Label
            {
                text = _inventory.Name
            };

            var slotContainerVe = new VisualElement
            {
                style =
                {
                    flexWrap = new StyleEnum<Wrap>(Wrap.Wrap),
                    flexDirection = FlexDirection.Row,
                    flexGrow = 0
                }
            };
            slotContainerVe.style.MarginAll(_options.Spacing);
            slotContainerVe.style.PaddingAll(_options.Spacing / 2);
            slotContainerVe.AddToClassList(InventoryUIClasses.SlotContainer);
            windowVe.Add(label);
            windowVe.Add(slotContainerVe);

            _renderedSlots = new List<VisualElement>();
            foreach (var slot in _inventory.Slots)
            {
                var slotVe = new VisualElement
                {
                    style =
                    {
                        width = _options.SlotSize,
                        height = _options.SlotSize,
                        flexShrink = 0
                    },
                    userData = new InventorySlotUserData(_inventory.Id, slot.Index)
                };
                slotVe.style.MarginAll(_options.Spacing / 2);
                slotVe.AddToClassList(InventoryUIClasses.Slot);
                slotContainerVe.Add(slotVe);
                _renderedSlots.Add(slotVe);
            }

            if (_options.ItemRoot == null)
            {
                _options.ItemRoot = new VisualElement();
                _options.InventoryRoot.Add(_options.ItemRoot);
            }

            _options.ItemRoot.pickingMode = PickingMode.Ignore;
            _options.InventoryRoot.Add(windowVe);
        }

        /// <summary>
        ///     Rebuild the hierarchy for all slots. Do not use this when you know what slot index you want to update.
        ///     This will cause a rerender from UI Toolkit at some later point.
        /// </summary>
        internal void RebuildHierarchy()
        {
            foreach (var slot in _inventory.Slots)
            {
                RebuildHierarchy(slot.Index);
            }
        }

        /// <summary>
        ///     Build the visual hierarchy for a single slot by index (position).
        ///     This will cause a rerender from UI Toolkit at some later point.
        /// </summary>
        /// <param name="slotIndex">The position of the slot you want to render.</param>
        internal void RebuildHierarchy(int slotIndex)
        {
            _logger.Info($"Inventory {_inventory.Id}: RebuildHierarchy@{slotIndex}");
            var slot = _inventory.Slots[slotIndex];
            var slotVe = _renderedSlots[slotIndex];

            var child = _options.ItemRoot!.Children().FirstOrDefault(iVe =>
            {
                if (InventoryUIUtils.TryGetTypedUserData<InventoryItemUserData>(iVe, out var itemData))
                {
                    return itemData?.AttachedSlotIndex == slotIndex && itemData.InventoryId == _inventory.Id;
                }

                return false;
            });
            child?.RemoveFromHierarchy();
            if (slot.IsEmpty) return;

            var itemVe = new VisualElement
            {
                style =
                {
                    backgroundImage = new StyleBackground(slot.GetItemStack().Item.GetSprite()),
                    width = _options.SlotSize,
                    height = _options.SlotSize,
                    alignContent = Align.FlexEnd,
                    justifyContent = Justify.FlexEnd,
                    position = Position.Absolute,
                    flexGrow = 0,
                    aspectRatio = 1
                },
                userData = new InventoryItemUserData(_inventory.Id, slotIndex, slot.GetItemStack())
            };
            itemVe.AddToClassList(InventoryUIClasses.SlotItem);
            ItemModifier?.Invoke(ref itemVe);

            var label = new Label
            {
                text = slot.GetItemStack().Quantity.ToString()
            };
            label.AddToClassList(InventoryUIClasses.SlotItemQuantity);
            itemVe.Add(label);
            _options.ItemRoot!.Add(itemVe);

            itemVe.RegisterCallbackOnce<GeometryChangedEvent>(_ =>
            {
                InventoryUIUtils.SnapVisualElementToOtherVisualElement(itemVe, slotVe);
            });
        }
    }
}
using IMS.Exceptions;
using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace IMS.UI.DragAndDrop
{
    /// <summary>
    ///     Each Inventory needs its own <see cref="OnDropHandler"/>. This is responsible for handling the OnDrop
    ///     fired by the <see cref="DragManipulator"/>. This class is as separated as possible, so it should be fairly
    ///     easy to override just small behavior and keep the rest default. 
    /// </summary>
    public class OnDropHandler
    {
        private static readonly Logger Logger = new(nameof(OnDropHandler));
        public const bool AcceptEvent = true;
        public const bool ResetVisualElement = false;

        /// <summary>
        ///     Handle the OnDrop event fired by a <see cref="DragManipulator"/>.
        /// </summary>
        /// <param name="ext">Holds all the data needed by this handler.</param>
        /// <returns>Whether to accept or reject the event. Rejection will cause the <paramref name="ext"/>.Target <see cref="VisualElement"/> to be reset.</returns>
        public bool HandleOnDrop([NotNull] ExternalDropData ext)
        {
            if (!PreflightChecks(ext, out var intern) || intern == null) return ResetVisualElement;
            // Required for multiple inventory instances to interact
            if (!TryGetDestinationInventory(ext, intern, out var dstInventory) || dstInventory == null)
                return ResetVisualElement;

            Logger.Info("Preflight passed!");
            Logger.Info(intern);
            Logger.Info(ext);

            // Since we remove the item from the source here, we need to put it back everywhere below this line, if we
            // reject the event.
            RemoveItemFromSourceInventoryIfPresent(ext, intern);

            // Move the item into the new OCCUPIED slot
            if (dstInventory.TryGetItemStackAt(intern.Slot.Index, out _))
            {
                Logger.Info($"Slot {intern.Slot.Index} already occupied!");
                var handlerStatus = AcceptEvent;
                // If an ItemStack is already present at the destination inventory's location, modify it and try to add
                // the current stack to it.
                dstInventory.ModifySlotItemStack(intern.Slot.Index, (ref ItemStack itemStack) =>
                {
                    try
                    {
                        var overflow = itemStack.AddStack(intern.Item.ItemStack);
                        Logger.Info($"{overflow.Quantity} items overflowed!");

                        // Remove the item, because from here on we either put a new item into the source inventory
                        // or we want it to despawn.
                        ext.Target.RemoveFromHierarchy();
                        if (overflow.Quantity > 0)
                        {
                            Logger.Info(
                                $"Inventory {ext.SrcInventory.Id} [slot: {intern.Item.AttachedSlotIndex}].PlaceItemStack(overflow)");
                            // If we have overflow, put the overflow back into the source inventory slot
                            ext.SrcInventory.PlaceItemStack(intern.Item.AttachedSlotIndex, overflow);
                            handlerStatus = ResetVisualElement;
                        }
                        else
                        {
                            Logger.Info("No overflow!");
                            handlerStatus = AcceptEvent;
                        }
                    }
                    catch (IncompatibleItemException iie)
                    {
                        Logger.Info($"IncompatibleItemException caught and handled: {iie.Message}");
                        ext.SrcInventory.PlaceItemStack(intern.Item.AttachedSlotIndex, intern.Item.ItemStack);
                        handlerStatus = ResetVisualElement;
                    }
                });
                return handlerStatus;
            }

            Logger.Info($"Destination slot not occupied! Inventory: {dstInventory.Id} [slot: {intern.Slot.Index}]");
            // Move the item into the new EMPTY slot
            dstInventory.PlaceItemStack(intern.Slot.Index, intern.Item.ItemStack);

            ext.Target.RemoveFromHierarchy();
            return AcceptEvent;
        }

        /// <summary>
        ///     Responsible for assigning all <see cref="InternalDropData"/> properties.
        /// </summary>
        /// <param name="ext">The external properties.</param>
        /// <param name="intern">The newly assigned internal properties. As long as true is returned, never null!</param>
        /// <returns>Whether assigning <paramref name="intern"/> succeeded. Failure should reset the <see cref="VisualElement"/>!</returns>
        protected static bool PreflightChecks(
            [NotNull] ExternalDropData ext,
            [CanBeNull] out InternalDropData intern)
        {
            intern = null;

            if (!InventoryUIUtils.TryGetInventorySlotAtPosition(
                    ext.InventoryRoot,
                    ext.PointerEvent.position,
                    out var slotVe)) return ResetVisualElement;
            if (!InventoryUIUtils.TryGetTypedUserData<InventorySlotUserData>(slotVe, out var slotData))
                return ResetVisualElement;
            if (!InventoryUIUtils.TryGetTypedUserData<InventoryItemUserData>(ext.Target, out var itemData))
                return ResetVisualElement;

            intern = new InternalDropData(itemData, slotData);
            return AcceptEvent;
        }

        protected static void RemoveItemFromSourceInventoryIfPresent(
            [NotNull] ExternalDropData ext,
            [NotNull] InternalDropData intern)
        {
            if (ext.SrcInventory.TryGetItemStackAt(intern.Item.AttachedSlotIndex, out _))
            {
                ext.SrcInventory.Slots[intern.Item.AttachedSlotIndex].RemoveItemStack();
                // TODO: Split stack
            }
        }

        protected bool TryGetDestinationInventory(
            [NotNull] ExternalDropData ext,
            [NotNull] InternalDropData intern,
            [CanBeNull] out Inventory destinationInventory)
        {
            destinationInventory = InventoryManager.Instance.GetInventoryById(intern.Slot.InventoryId);
            if (destinationInventory != null) return true;

            Logger.Warn($"Destination inventory was not found? Tried with id: {intern.Slot.InventoryId}");
            return false;
        }
    }
}
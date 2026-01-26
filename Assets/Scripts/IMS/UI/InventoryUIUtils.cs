using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace IMS
{
    public static class InventoryUIUtils
    {
        public static bool TryGetInventorySlotAtPosition(VisualElement root, Vector2 position,
            [CanBeNull] out VisualElement slotVe)
        {
            var picked = new List<VisualElement>();
            root.panel.PickAll(position, picked);
            slotVe = picked.Find(ve => ve.ClassListContains(InventoryUIClasses.Slot));
            return slotVe != null;
        }

        public static void SetSlotUserData(VisualElement slot, InventorySlotUserData userData)
        {
            slot.userData = userData;
        }

        [CanBeNull]
        public static InventorySlotUserData GetSlotUserData(VisualElement slot)
        {
            if (slot.userData is InventorySlotUserData data) return data;
            return null;
        }
    }
}
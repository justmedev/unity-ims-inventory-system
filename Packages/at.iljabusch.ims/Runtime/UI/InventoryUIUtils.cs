using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace IMS.UI
{
    /// <summary>
    ///     Contains some UI utils like snapping or querying userData.
    /// </summary>
    public static class InventoryUIUtils
    {
        /// <summary>
        ///     Get an inventory slot at a position. If one is found, true is returned, if none is found, false
        ///     is returned.
        /// </summary>
        /// <param name="root">The VisualElement containing the inventory slots.</param>
        /// <param name="position">The position you want to query.</param>
        /// <param name="slotVe">The slot VisualElement, if found.</param>
        /// <returns>Wheter a match was found.</returns>
        public static bool TryGetInventorySlotAtPosition(VisualElement root, Vector2 position,
            out VisualElement slotVe)
        {
            var picked = new List<VisualElement>();
            root.panel.PickAll(position, picked);
            slotVe = picked.Find(ve => ve.ClassListContains(InventoryUIClasses.Slot));
            return slotVe != null;
        }

        /// <summary>
        ///     Try to get <see cref="userData"/> (with type <see cref="T"/>) of a <see cref="VisualElement"/> <see cref="ve"/>.
        /// </summary>
        /// <param name="ve">The <see cref="VisualElement"/> you want to get the <see cref="VisualElement.userData"/> on.</param>
        /// <param name="userData">The <see cref="VisualElement.userData"/> with type <see cref="T"/> that was found.</param>
        /// <typeparam name="T">The type of <see cref="VisualElement.userData"/> to check.</typeparam>
        /// <returns>Whether a match was found.</returns>
        public static bool TryGetTypedUserData<T>(VisualElement ve, out T userData) where T : class
        {
            userData = null;
            if (ve.userData is not T data) return false;
            userData = data;
            return true;
        }

        /// <summary>
        ///     Snap one <see cref="VisualElement"/> to the other's position.
        /// </summary>
        /// <param name="srcVe">The <see cref="VisualElement"/> to reposition.</param>
        /// <param name="parentVe">The <see cref="VisualElement"/> that should be snapped to.</param>
        public static void SnapVisualElementToOtherVisualElement(VisualElement srcVe, VisualElement parentVe)
        {
            var targetPos = parentVe.ChangeCoordinatesTo(srcVe.parent, Vector2.zero);
            srcVe.style.left = targetPos.x;
            srcVe.style.top = targetPos.y;
        }
    }
}
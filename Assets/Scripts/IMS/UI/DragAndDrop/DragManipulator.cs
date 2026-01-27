using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace IMS.UI.DragAndDrop
{
    /// <summary>
    ///     A <see cref="PointerManipulator"/> that implements basic <see cref="VisualElement"/> dragging with events.
    ///     You can easily extend this class with your own modifications.
    /// </summary>
    public class DragManipulator : PointerManipulator
    {
        /// <summary>
        ///     The position of the <see cref="VisualElement"/> when dragging started (pointer localPosition).
        /// </summary>
        protected Vector3 StartPosition;

        /// <summary>
        ///     The original position in this <see cref="VisualElement"/>'s local position space (top and left)
        /// </summary>
        protected Vector2? StartOffset;

        /// <summary>
        ///     Whether this manipulator is currently dragging an item.
        /// </summary>
        protected bool IsDragging;

        /// <summary>
        ///     The pointer that is currently dragging a VisualElement.
        /// </summary>
        protected int PointerId;

        /// <summary>
        ///     Fired when a <see cref="VisualElement"/> is dropped (mouse button let go).
        /// </summary>
        /// <param name="pointerEvent">The <see cref="PointerUpEvent"/> that was received in the manipulator.</param>
        /// <param name="targetVe">The <see cref="VisualElement"/> this manipulator is attached to.</param>
        /// <returns>Whether to accept the event, or reject it, resetting the <see cref="VisualElement"/>.</returns>
        public delegate bool DropEvent(PointerUpEvent pointerEvent, VisualElement targetVe);

        /// <summary>
        ///     Subscribe to this to receive <see cref="DropEvent"/> events.
        /// </summary>
        [CanBeNull] public DropEvent OnDrop;

        /// <summary>
        ///     Fired when a <see cref="VisualElement"/> is picked up (mouse button clicked).
        /// </summary>
        /// <param name="pointerEvent">The <see cref="PointerDownEvent"/> that was received in the manipulator.</param>
        /// <param name="targetVe">The <see cref="VisualElement"/> this manipulator is attached to.</param>
        /// <returns>Whether to accept the event, or reject it, resetting the <see cref="VisualElement"/>.</returns>
        public delegate bool PickupEvent(PointerDownEvent pointerEvent, VisualElement targetVe);

        /// <summary>
        ///     Subscribe to this to receive <see cref="PickupEvent"/> events.
        /// </summary>
        [CanBeNull] public PickupEvent OnPickup;

        public DragManipulator()
        {
            PointerId = -1;
            IsDragging = false;
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }

        private void OnPointerDown(PointerDownEvent e)
        {
            if (IsDragging)
            {
                e.StopImmediatePropagation();
                return;
            }

            if (!CanStartManipulation(e)) return;
            StartOffset = new Vector2(target.style.left.value.value, target.style.top.value.value);
            StartPosition = e.localPosition;
            PointerId = e.pointerId;

            IsDragging = true;
            target.CapturePointer(PointerId);
            e.StopPropagation();

            target.BringToFront();

            if (OnPickup == null || OnPickup.Invoke(e, target)) return;
            IsDragging = false;
            target.ReleaseMouse();
            e.StopPropagation();
        }

        private void OnPointerMove(PointerMoveEvent e)
        {
            if (!IsDragging || !target.HasPointerCapture(PointerId))
                return;

            var diff = e.localPosition - StartPosition;

            target.style.top = target.layout.y + diff.y;
            target.style.left = target.layout.x + diff.x;

            e.StopPropagation();
        }

        private void OnPointerUp(PointerUpEvent e)
        {
            if (!IsDragging || !target.HasPointerCapture(PointerId) || !CanStopManipulation(e))
                return;

            IsDragging = false;
            target.ReleaseMouse();
            e.StopPropagation();

            if (OnDrop == null || OnDrop.Invoke(e, target) || !StartOffset.HasValue) return;
            target.style.top = StartOffset.Value.y;
            target.style.left = StartOffset.Value.x;
        }
    }
}
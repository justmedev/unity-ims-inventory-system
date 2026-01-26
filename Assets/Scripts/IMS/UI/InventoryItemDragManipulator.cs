using UnityEngine;
using UnityEngine.UIElements;

namespace IMS.UI
{
    public class InventoryItemDragManipulator : PointerManipulator
    {
        private Vector3 _start;
        private bool _active;
        private int _pointerId;
        private Vector2 _startSize;

        public InventoryItemDragManipulator()
        {
            _pointerId = -1;
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            _active = false;
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
            if (_active)
            {
                e.StopImmediatePropagation();
                return;
            }

            if (!CanStartManipulation(e)) return;
            _start = e.localPosition;
            _pointerId = e.pointerId;

            _active = true;
            target.CapturePointer(_pointerId);
            e.StopPropagation();
        }

        private void OnPointerMove(PointerMoveEvent e)
        {
            if (!_active || !target.HasPointerCapture(_pointerId))
                return;

            Vector2 diff = e.localPosition - _start;

            target.style.top = target.layout.y + diff.y;
            target.style.left = target.layout.x + diff.x;

            e.StopPropagation();
        }

        private void OnPointerUp(PointerUpEvent e)
        {
            if (!_active || !target.HasPointerCapture(_pointerId) || !CanStopManipulation(e))
                return;

            _active = false;
            target.ReleaseMouse();
            e.StopPropagation();
        }
    }
}
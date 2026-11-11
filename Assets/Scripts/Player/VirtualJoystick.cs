using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SyberiaEpic
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public RectTransform handle;
        RectTransform rect;
        float radius = 65f;
        void Awake() { rect = transform as RectTransform; }
        public void OnPointerDown(PointerEventData e) => OnDrag(e);
        public void OnDrag(PointerEventData e)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, e.position, e.pressEventCamera, out Vector2 p))
            {
                Vector2 v = Vector2.ClampMagnitude(p / radius, 1f);
                if (handle) handle.anchoredPosition = v * radius;
                PlayerInput.Instance?.SetMobileMove(v);
            }
        }
        public void OnPointerUp(PointerEventData e)
        {
            if (handle) handle.anchoredPosition = Vector2.zero;
            PlayerInput.Instance?.SetMobileMove(Vector2.zero);
        }
    }
}

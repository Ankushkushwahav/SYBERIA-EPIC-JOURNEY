using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SyberiaEpic
{
    public class MobileControls : MonoBehaviour
    {
        public static void Build(Canvas canvas)
        {
            GameObject joystick = Panel(canvas.transform, "Joystick", new Vector2(0,0), new Vector2(170,170), new Vector2(115,115));
            Image jimg = joystick.GetComponent<Image>(); jimg.color = new Color(1,1,1,.18f);
            GameObject handle = Panel(joystick.transform, "Handle", new Vector2(.5f,.5f), new Vector2(65,65), Vector2.zero);
            handle.GetComponent<Image>().color = new Color(1,1,1,.55f);
            var joy = joystick.AddComponent<VirtualJoystick>(); joy.handle = handle.GetComponent<RectTransform>();

            AddAction(canvas.transform, "JUMP", new Vector2(1,0), new Vector2(-95,190), () => PlayerInput.Instance?.PressJump());
            AddAction(canvas.transform, "USE", new Vector2(1,0), new Vector2(-95,105), () => PlayerInput.Instance?.PressInteract());
            AddAction(canvas.transform, "BAG", new Vector2(1,0), new Vector2(-95,20), () => PlayerInput.Instance?.PressInventory());

            GameObject lookZone = new GameObject("CameraTouchZone", typeof(RectTransform), typeof(Image), typeof(LookDrag));
            lookZone.transform.SetParent(canvas.transform, false);
            RectTransform rt = lookZone.GetComponent<RectTransform>(); rt.anchorMin = new Vector2(.45f,.35f); rt.anchorMax = new Vector2(1,1); rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
            Image image = lookZone.GetComponent<Image>(); image.color = new Color(1,1,1,.001f); image.raycastTarget = true;
        }

        static GameObject Panel(Transform parent, string name, Vector2 anchor, Vector2 size, Vector2 pos)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent,false);
            RectTransform rt=go.GetComponent<RectTransform>(); rt.anchorMin=rt.anchorMax=anchor; rt.pivot=anchor; rt.anchoredPosition=pos; rt.sizeDelta=size;
            return go;
        }

        static void AddAction(Transform parent, string label, Vector2 anchor, Vector2 pos, UnityEngine.Events.UnityAction action)
        {
            GameObject go=Panel(parent,label,new Vector2(anchor.x,anchor.y),new Vector2(76,76),pos);
            go.GetComponent<Image>().color=new Color(.08f,.25f,.35f,.8f);
            var button=go.AddComponent<Button>(); button.onClick.AddListener(action);
            GameObject txt=new GameObject("Text",typeof(RectTransform),typeof(Text)); txt.transform.SetParent(go.transform,false);
            RectTransform rt=txt.GetComponent<RectTransform>(); rt.anchorMin=Vector2.zero; rt.anchorMax=Vector2.one; rt.offsetMin=rt.offsetMax=Vector2.zero;
            Text t=txt.GetComponent<Text>(); t.font=Resources.GetBuiltinResource<Font>("Arial.ttf"); t.text=label; t.color=Color.white; t.alignment=TextAnchor.MiddleCenter; t.fontSize=15;
        }
    }

    public class LookDrag : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        Vector2 last;
        public void OnPointerDown(PointerEventData e) { last=e.position; }
        public void OnDrag(PointerEventData e)
        {
            Vector2 delta=e.position-last; last=e.position;
            PlayerInput.Instance?.AddLook(delta);
        }
    }
}

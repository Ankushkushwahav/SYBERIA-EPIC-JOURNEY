using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SyberiaEpic
{
    public class UIHud : MonoBehaviour
    {
        public static UIHud Instance { get; private set; }
        public static bool IsPaused { get; private set; }
        Text missionText, messageText, inventoryText;
        GameObject pausePanel, inventoryPanel;
        float messageTimer;

        void Awake() { Instance = this; }
        void Start() { Refresh(); }

        public void Build(Canvas canvas)
        {
            missionText = MakeText(canvas.transform, "Mission", new Vector2(0, 1), new Vector2(0, 1), new Vector2(20, -20), new Vector2(560, 100), 20, TextAnchor.UpperLeft);
            messageText = MakeText(canvas.transform, "Message", new Vector2(.5f, 0), new Vector2(.5f, 0), new Vector2(0, 90), new Vector2(700, 55), 19, TextAnchor.MiddleCenter);
            inventoryPanel = MakePanel(canvas.transform, "InventoryPanel", new Vector2(.5f,.5f), new Vector2(360,220));
            inventoryText = MakeText(inventoryPanel.transform, "InventoryText", new Vector2(.5f,.5f), new Vector2(.5f,.5f), Vector2.zero, new Vector2(330,180), 20, TextAnchor.MiddleCenter);
            inventoryPanel.SetActive(false);
            pausePanel = MakePanel(canvas.transform, "PausePanel", new Vector2(.5f,.5f), new Vector2(320,180));
            MakeText(pausePanel.transform, "Paused", new Vector2(.5f,.65f), new Vector2(.5f,.65f), Vector2.zero, new Vector2(280,55), 28, TextAnchor.MiddleCenter).text = "PAUSED";
            Button resume = MakeButton(pausePanel.transform, "Resume", "RESUME", new Vector2(.5f,.3f), new Vector2(180,45));
            resume.onClick.AddListener(TogglePause);
            pausePanel.SetActive(false);
        }

        Text MakeText(Transform parent, string name, Vector2 anchor, Vector2 pivot, Vector2 pos, Vector2 size, int fontSize, TextAnchor align)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = pivot; rt.pivot = pivot; rt.anchoredPosition = pos; rt.sizeDelta = size;
            Text t = go.GetComponent<Text>(); t.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); t.fontSize = fontSize; t.color = Color.white; t.alignment = align; t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Overflow;
            return t;
        }

        GameObject MakePanel(Transform parent, string name, Vector2 anchor, Vector2 size)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            RectTransform rt = go.GetComponent<RectTransform>(); rt.anchorMin = rt.anchorMax = anchor; rt.pivot = anchor; rt.anchoredPosition = Vector2.zero; rt.sizeDelta = size;
            go.GetComponent<Image>().color = new Color(.04f,.07f,.1f,.94f);
            return go;
        }

        Button MakeButton(Transform parent, string name, string label, Vector2 anchor, Vector2 size)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            RectTransform rt = go.GetComponent<RectTransform>(); rt.anchorMin = rt.anchorMax = anchor; rt.pivot = anchor; rt.anchoredPosition = Vector2.zero; rt.sizeDelta = size;
            go.GetComponent<Image>().color = new Color(.15f,.35f,.45f,.95f);
            Text txt = MakeText(go.transform, "Label", new Vector2(.5f,.5f), new Vector2(.5f,.5f), Vector2.zero, size, 17, TextAnchor.MiddleCenter); txt.text = label;
            return go.GetComponent<Button>();
        }

        void Update()
        {
            if (messageTimer > 0) { messageTimer -= Time.deltaTime; if (messageTimer <= 0 && messageText) messageText.text = ""; }
        }
        public void Refresh()
        {
            if (!missionText) return;
            var m = MissionManager.Instance;
            missionText.text = m ? "CHAPTER " + (GameState.Instance.Data.currentChapter + 1) + " — " + m.CurrentTitle + "\n" + m.CurrentObjective : "SYBERIA — EPIC JOURNEY";
            if (inventoryText) inventoryText.text = "INVENTORY\n\n" + ((GameState.Instance && GameState.Instance.Data.hasMechanicalKey) ? "• Brass Clockwork Key" : "Empty");
        }
        public void ShowMessage(string message)
        {
            if (messageText) messageText.text = message;
            messageTimer = 4f;
        }
        public void ToggleInventory()
        {
            if (inventoryPanel) { inventoryPanel.SetActive(!inventoryPanel.activeSelf); Refresh(); }
        }
        public void TogglePause()
        {
            IsPaused = !IsPaused;
            Time.timeScale = IsPaused ? 0 : 1;
            if (pausePanel) pausePanel.SetActive(IsPaused);
        }
    }
}

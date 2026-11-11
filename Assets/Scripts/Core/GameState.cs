using UnityEngine;
using System;

namespace SyberiaEpic
{
    [Serializable]
    public class SaveData
    {
        public int currentChapter = 0;
        public int completedChapters = 0;
        public int coins = 0;
        public bool hasMechanicalKey = false;
        public float playerX, playerY, playerZ;
    }

    public class GameState : MonoBehaviour
    {
        public static GameState Instance { get; private set; }
        public SaveData Data { get; private set; } = new SaveData();

        public event Action OnStateChanged;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }

        public void NotifyChanged() => OnStateChanged?.Invoke();

        public void Save(Transform player = null)
        {
            if (player != null)
            {
                Vector3 p = player.position;
                Data.playerX = p.x; Data.playerY = p.y; Data.playerZ = p.z;
            }
            PlayerPrefs.SetString("SYBERIA_SAVE", JsonUtility.ToJson(Data));
            PlayerPrefs.Save();
            NotifyChanged();
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey("SYBERIA_SAVE"))
            {
                var loaded = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SYBERIA_SAVE"));
                if (loaded != null) Data = loaded;
            }
            NotifyChanged();
        }

        public void AddItem(string itemId)
        {
            if (itemId == "mechanical_key") Data.hasMechanicalKey = true;
            Save();
        }

        public void CompleteCurrentChapter()
        {
            Data.completedChapters = Mathf.Max(Data.completedChapters, Data.currentChapter + 1);
            Data.currentChapter = Mathf.Min(7, Data.completedChapters);
            Save();
        }

        public Vector3 SavedPosition => new Vector3(Data.playerX, Data.playerY, Data.playerZ);
    }
}

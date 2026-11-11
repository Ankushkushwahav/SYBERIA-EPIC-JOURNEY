using UnityEngine;

namespace SyberiaEpic
{
    public class MissionManager : MonoBehaviour
    {
        public static MissionManager Instance { get; private set; }
        public string[] chapterNames = {
            "The Brass Key of Old Market", "The Painter's Letter", "The Clockwork Archive",
            "Tracks Beneath the Fog", "The Bridge Mechanism", "The Silent Manor",
            "The Last Steam Departure", "The Fountain's Secret"
        };
        public string[] objectives = {
            "Find the lost brass key in the bazaar.",
            "Find Dana's old letter in the artist's home.",
            "Search the university archive for a gear emblem.",
            "Follow the abandoned railway and inspect its signal.",
            "Activate the bridge counterweight mechanism.",
            "Discover the hidden room inside the manor.",
            "Restore power to the old locomotive.",
            "Bring the collected clues to the fountain."
        };
        bool objectiveComplete;
        void Awake() { Instance = this; }
        public string CurrentTitle => chapterNames[Mathf.Clamp(GameState.Instance ? GameState.Instance.Data.currentChapter : 0, 0, 7)];
        public string CurrentObjective => objectives[Mathf.Clamp(GameState.Instance ? GameState.Instance.Data.currentChapter : 0, 0, 7)];
        public bool IsComplete => objectiveComplete || (GameState.Instance != null && GameState.Instance.Data.hasMechanicalKey);
        public void CompleteObjective()
        {
            objectiveComplete = true;
            UIHud.Instance?.Refresh();
            UIHud.Instance?.ShowMessage("Objective completed! Return to the mission marker.");
        }
        public void FinishChapter()
        {
            if (!IsComplete) { UIHud.Instance?.ShowMessage("Complete the objective first."); return; }
            GameState.Instance?.CompleteCurrentChapter();
            objectiveComplete = false;
            UIHud.Instance?.ShowMessage("Chapter completed! Next chapter unlocked.");
            UIHud.Instance?.Refresh();
        }
    }
}

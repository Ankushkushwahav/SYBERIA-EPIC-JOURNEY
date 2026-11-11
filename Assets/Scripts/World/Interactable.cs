using UnityEngine;

namespace SyberiaEpic
{
    public abstract class Interactable : MonoBehaviour
    {
        public string prompt = "Interact";
        public abstract void Interact();
    }

    public class LostMechanicalKey : Interactable
    {
        bool collected;
        void Start() { prompt = "Press E / INTERACT to collect the brass key"; }
        public override void Interact()
        {
            if (collected) return;
            collected = true;
            GameState.Instance?.AddItem("mechanical_key");
            MissionManager.Instance?.CompleteObjective();
            UIHud.Instance?.ShowMessage("You found the Brass Clockwork Key!");
            gameObject.SetActive(false);
        }
    }

    public static class Interactor
    {
        public static void TryInteract(Transform player)
        {
            Collider[] hits = Physics.OverlapSphere(player.position + Vector3.up, 2.2f);
            Interactable best = null;
            float bestDistance = 999f;
            foreach (var hit in hits)
            {
                var item = hit.GetComponentInParent<Interactable>();
                if (item == null) continue;
                float d = Vector3.Distance(player.position, item.transform.position);
                if (d < bestDistance) { best = item; bestDistance = d; }
            }
            if (best != null) best.Interact();
            else UIHud.Instance?.ShowMessage("Nothing to interact with nearby.");
        }
    }
}

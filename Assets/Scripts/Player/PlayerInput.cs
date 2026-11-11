using UnityEngine;

namespace SyberiaEpic
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput Instance { get; private set; }
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool RunHeld { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool InteractPressed { get; private set; }
        public bool InventoryPressed { get; private set; }

        void Awake() { Instance = this; }

        void Update()
        {
            Vector2 keyboard = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Move = Vector2.ClampMagnitude(keyboard + MobileMove, 1f);
            RunHeld = Input.GetKey(KeyCode.LeftShift) || MobileRun;
            if (Input.GetKeyDown(KeyCode.Space)) JumpPressed = true;
            if (Input.GetKeyDown(KeyCode.E)) InteractPressed = true;
            if (Input.GetKeyDown(KeyCode.I)) InventoryPressed = true;
            if (Input.GetKeyDown(KeyCode.Escape)) UIHud.Instance?.TogglePause();
        }

        Vector2 MobileMove;
        bool MobileRun;

        public void SetMobileMove(Vector2 value) => MobileMove = Vector2.ClampMagnitude(value, 1f);
        public void SetRun(bool value) => MobileRun = value;
        public void PressJump() => JumpPressed = true;
        public void PressInteract() => InteractPressed = true;
        public void PressInventory() => InventoryPressed = true;
        public void AddLook(Vector2 value) => Look += value;

        public bool ConsumeJump() { bool v = JumpPressed; JumpPressed = false; return v; }
        public bool ConsumeInteract() { bool v = InteractPressed; InteractPressed = false; return v; }
        public bool ConsumeInventory() { bool v = InventoryPressed; InventoryPressed = false; return v; }
        public Vector2 ConsumeLook() { Vector2 v = Look; Look = Vector2.zero; return v; }
    }
}

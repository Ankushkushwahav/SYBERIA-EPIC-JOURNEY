using UnityEngine;

namespace SyberiaEpic
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public float walkSpeed = 3.2f, runSpeed = 6f, jumpHeight = 1.4f, gravity = -22f, turnSpeed = 12f;
        CharacterController controller;
        float verticalSpeed;
        Transform cam;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            cam = Camera.main ? Camera.main.transform : null;
            if (GameState.Instance != null && GameState.Instance.Data.playerY > 0.1f)
                transform.position = GameState.Instance.SavedPosition;
        }

        void Update()
        {
            if (UIHud.IsPaused) return;
            Vector2 input = PlayerInput.Instance ? PlayerInput.Instance.Move : Vector2.zero;
            Vector3 forward = cam ? Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized : Vector3.forward;
            Vector3 right = cam ? Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized : Vector3.right;
            Vector3 direction = Vector3.ClampMagnitude(forward * input.y + right * input.x, 1f);
            float speed = PlayerInput.Instance != null && PlayerInput.Instance.RunHeld ? runSpeed : walkSpeed;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion target = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, turnSpeed * Time.deltaTime);
            }

            if (controller.isGrounded && verticalSpeed < 0) verticalSpeed = -2f;
            if (PlayerInput.Instance != null && PlayerInput.Instance.ConsumeJump() && controller.isGrounded)
                verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);

            verticalSpeed += gravity * Time.deltaTime;
            Vector3 motion = direction * speed;
            motion.y = verticalSpeed;
            controller.Move(motion * Time.deltaTime);

            if (PlayerInput.Instance != null && PlayerInput.Instance.ConsumeInteract())
                Interactor.TryInteract(transform);
            if (PlayerInput.Instance != null && PlayerInput.Instance.ConsumeInventory())
                UIHud.Instance?.ToggleInventory();
        }
    }
}

using UnityEngine;

namespace SyberiaEpic
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float distance = 5.5f, height = 2.1f, sensitivity = 0.12f, smooth = 12f;
        float yaw, pitch = 12f;

        void Start()
        {
            if (target) yaw = target.eulerAngles.y;
            Cursor.lockState = CursorLockMode.None;
        }

        void LateUpdate()
        {
            if (!target) return;
            Vector2 look = PlayerInput.Instance ? PlayerInput.Instance.ConsumeLook() : Vector2.zero;
            if (Input.GetMouseButton(1))
            {
                yaw += Input.GetAxis("Mouse X") * 3f;
                pitch -= Input.GetAxis("Mouse Y") * 2f;
            }
            yaw += look.x * sensitivity;
            pitch = Mathf.Clamp(pitch - look.y * sensitivity, -10f, 55f);
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 focus = target.position + Vector3.up * height;
            Vector3 desired = focus - rotation * Vector3.forward * distance;
            if (Physics.Linecast(focus, desired, out RaycastHit hit)) desired = hit.point + hit.normal * 0.2f;
            transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(focus - transform.position), smooth * Time.deltaTime);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Basit birinci sahis (FPS) oyuncu kontrol scripti.
    /// WASD hareket ve mouse ile bakis yonu kontrolu saglar.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class SimplePlayerController : MonoBehaviour
    {
        #region Fields

        private const float k_Gravity = -9.81f;

        [Header("Movement")]
        [SerializeField] private float m_MoveSpeed = 5f;

        [Header("Camera")]
        [SerializeField] private Transform m_CameraHolder;
        [SerializeField] private float m_MouseSensitivity = 0.15f;
        [SerializeField] private float m_MinVerticalAngle = -80f;
        [SerializeField] private float m_MaxVerticalAngle = 80f;

        private CharacterController m_Controller;
        private float m_VerticalRotation;
        private Vector3 m_Velocity;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_Controller = GetComponent<CharacterController>();

            if (m_CameraHolder == null)
            {
                var cam = Camera.main;
                if (cam != null)
                {
                    m_CameraHolder = cam.transform;
                }
                else
                {
                    Debug.LogError("[SimplePlayerController] CameraHolder not assigned and Main Camera not found!");
                }
            }
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMouseLook();
            HandleMovement();
        }

        #endregion

        #region Methods

        private void HandleMouseLook()
        {
            if (m_CameraHolder == null)
                return;

            var mouse = Mouse.current;
            if (mouse == null)
                return;

            Vector2 mouseDelta = mouse.delta.ReadValue();
            float mouseX = mouseDelta.x * m_MouseSensitivity;
            float mouseY = mouseDelta.y * m_MouseSensitivity;

            m_VerticalRotation -= mouseY;
            m_VerticalRotation = Mathf.Clamp(m_VerticalRotation, m_MinVerticalAngle, m_MaxVerticalAngle);

            m_CameraHolder.localRotation = Quaternion.Euler(m_VerticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleMovement()
        {
            if (m_Controller.isGrounded && m_Velocity.y < 0f)
            {
                m_Velocity.y = -2f;
            }

            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            float moveX = 0f;
            float moveZ = 0f;

            if (keyboard.wKey.isPressed) moveZ += 1f;
            if (keyboard.sKey.isPressed) moveZ -= 1f;
            if (keyboard.dKey.isPressed) moveX += 1f;
            if (keyboard.aKey.isPressed) moveX -= 1f;

            Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
            if (moveDirection.sqrMagnitude > 1f)
                moveDirection.Normalize();

            m_Controller.Move(moveDirection * (m_MoveSpeed * Time.deltaTime));

            m_Velocity.y += k_Gravity * Time.deltaTime;
            m_Controller.Move(m_Velocity * Time.deltaTime);
        }

        #endregion
    }
}

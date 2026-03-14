using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionSystem.Runtime.Player
{
    /// <summary>
    /// Birinci şahıs (FPS) oyuncu kontrol sınıfı.
    /// Kamera takılmalarını önlemek için yumuşatılmış (smoothed) hareket ve bakış mekanikleri içerir.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class SimplePlayerController : MonoBehaviour
    {
        #region Fields

        private const float k_Gravity = -9.81f;

        [Header("Movement Settings")]
        [SerializeField] private float m_MoveSpeed = 5f;
        [SerializeField, Range(0.01f, 0.5f)] private float m_MoveSmoothTime = 0.1f;

        [Header("Camera Settings")]
        [SerializeField] private Transform m_CameraHolder;
        [SerializeField] private float m_MouseSensitivity = 0.15f;
        [SerializeField, Range(0.01f, 0.5f)] private float m_MouseSmoothTime = 0.05f;
        [SerializeField] private float m_MinVerticalAngle = -80f;
        [SerializeField] private float m_MaxVerticalAngle = 80f;

        private CharacterController m_Controller;

        // Kamera yumuşatma değişkenleri
        private Vector2 m_TargetMouseLook;
        private Vector2 m_CurrentMouseLook;
        private Vector2 m_MouseSmoothVelocity;

        // Hareket yumuşatma değişkenleri
        private Vector3 m_TargetMoveDirection;
        private Vector3 m_CurrentMoveDirection;
        private Vector3 m_MoveSmoothVelocity;

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
                    Debug.LogError("[SimplePlayerController] CameraHolder atanmamış ve Main Camera sahnede bulunamadı!");
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

        /// <summary>
        /// Fareden gelen girdileri okur ve kamerayı pürüzsüz bir şekilde döndürür.
        /// </summary>
        private void HandleMouseLook()
        {
            if (m_CameraHolder == null)
            {
                return;
            }

            var mouse = Mouse.current;
            if (mouse == null)
            {
                return;
            }

            // Ham input verisini alıyoruz
            Vector2 mouseDelta = mouse.delta.ReadValue() * m_MouseSensitivity;

            // Hedef bakış açısını güncelliyoruz
            m_TargetMouseLook.x += mouseDelta.x;
            m_TargetMouseLook.y += mouseDelta.y;

            // Dikey açıyı kısıtlıyoruz (boyun kırılmasını engellemek için)
            m_TargetMouseLook.y = Mathf.Clamp(m_TargetMouseLook.y, m_MinVerticalAngle, m_MaxVerticalAngle);

            // SmoothDamp ile mevcut bakış açısından hedef açıya pürüzsüz geçiş yapıyoruz
            m_CurrentMouseLook = Vector2.SmoothDamp(
                m_CurrentMouseLook,
                m_TargetMouseLook,
                ref m_MouseSmoothVelocity,
                m_MouseSmoothTime);

            // Dikey rotasyonu kameraya, yatay rotasyonu karakterin gövdesine uyguluyoruz
            m_CameraHolder.localRotation = Quaternion.Euler(-m_CurrentMouseLook.y, 0f, 0f);
            transform.localRotation = Quaternion.Euler(0f, m_CurrentMouseLook.x, 0f);
        }

        /// <summary>
        /// Klavyeden gelen girdileri okur ve karakteri pürüzsüz bir şekilde hareket ettirir.
        /// </summary>
        private void HandleMovement()
        {
            if (m_Controller.isGrounded && m_Velocity.y < 0f)
            {
                // Yere değiyorsak ufak bir eksi değer vererek yokuş aşağı inerken zıplamasını engelliyoruz
                m_Velocity.y = -2f;
            }

            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            // Ham hareket girdisini alıyoruz
            float moveX = 0f;
            float moveZ = 0f;

            if (keyboard.wKey.isPressed) moveZ += 1f;
            if (keyboard.sKey.isPressed) moveZ -= 1f;
            if (keyboard.dKey.isPressed) moveX += 1f;
            if (keyboard.aKey.isPressed) moveX -= 1f;

            Vector3 rawDirection = transform.right * moveX + transform.forward * moveZ;

            if (rawDirection.sqrMagnitude > 1f)
            {
                rawDirection.Normalize();
            }

            m_TargetMoveDirection = rawDirection;

            // Hareketi yumuşatıyoruz (aniden hızlanıp aniden durmayı engeller, momentum hissi katar)
            m_CurrentMoveDirection = Vector3.SmoothDamp(
                m_CurrentMoveDirection,
                m_TargetMoveDirection,
                ref m_MoveSmoothVelocity,
                m_MoveSmoothTime);

            // Karakteri hareket ettiriyoruz
            m_Controller.Move(m_CurrentMoveDirection * (m_MoveSpeed * Time.deltaTime));

            // Yerçekimi uyguluyoruz
            m_Velocity.y += k_Gravity * Time.deltaTime;
            m_Controller.Move(m_Velocity * Time.deltaTime);
        }

        #endregion
    }
}
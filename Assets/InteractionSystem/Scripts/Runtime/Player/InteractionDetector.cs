using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Oyuncunun bakis yonunde IInteractable nesneleri tespit eden ve
    /// etkilesim input'unu yoneten sistem. Oyuncu GameObject'ine eklenir.
    /// </summary>
    public class InteractionDetector : MonoBehaviour
    {
        #region Fields

        private const float k_SphereCastRadius = 0.15f;

        [Header("Detection Settings")]
        [SerializeField] private float m_InteractionRange = 3f;
        [SerializeField] private LayerMask m_InteractionLayer = ~0;
        [SerializeField] private Transform m_RaycastOrigin;

        [Header("Input Settings")]
        [SerializeField] private Key m_InteractionKey = Key.E;

        private IInteractable m_CurrentTarget;
        private float m_HoldTimer;
        private bool m_IsHolding;
        private Camera m_Camera;
        private PlayerInventory m_Inventory;

        #endregion

        #region Events

        /// <summary>
        /// Odaklanan etkilesim hedefi degistiginde tetiklenir.
        /// Parametre null ise hedef kaybolmustur.
        /// </summary>
        public event Action<IInteractable> OnTargetChanged;

        /// <summary>
        /// Hold tipi etkilesimde ilerleme degistiginde tetiklenir.
        /// Parametre 0-1 arasi normalize degerdir.
        /// </summary>
        public event Action<float> OnHoldProgress;

        /// <summary>
        /// Herhangi bir etkilesim tamamlandiginda tetiklenir.
        /// </summary>
        public event Action<IInteractable> OnInteractionCompleted;

        #endregion

        #region Properties

        /// <summary>
        /// Su an odaklanilan IInteractable nesnesi. Yoksa null.
        /// </summary>
        public IInteractable CurrentTarget => m_CurrentTarget;

        /// <summary>
        /// Etkilesim menzili (metre).
        /// </summary>
        public float InteractionRange => m_InteractionRange;

        /// <summary>
        /// Hold etkilesimi devam ediyor mu?
        /// </summary>
        public bool IsHolding => m_IsHolding;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_Camera = Camera.main;
            if (m_Camera == null)
            {
                Debug.LogError("[InteractionDetector] Main Camera not found!");
            }

            m_Inventory = GetComponent<PlayerInventory>();
            if (m_Inventory == null)
            {
                Debug.LogError("[InteractionDetector] PlayerInventory component not found on the same GameObject!");
            }

            if (m_RaycastOrigin == null)
            {
                m_RaycastOrigin = m_Camera != null ? m_Camera.transform : transform;
            }
        }

        private void Update()
        {
            DetectInteractable();
            HandleInput();
        }

        #endregion

        #region Methods

        private void DetectInteractable()
        {
            IInteractable detected = null;

            Ray ray = new Ray(m_RaycastOrigin.position, m_RaycastOrigin.forward);

            if (Physics.SphereCast(ray, k_SphereCastRadius, out RaycastHit hit, m_InteractionRange, m_InteractionLayer))
            {
                detected = hit.collider.GetComponentInParent<IInteractable>();
            }

            if (detected != m_CurrentTarget)
            {
                SetTarget(detected);
            }
        }

        private void SetTarget(IInteractable newTarget)
        {
            if (m_CurrentTarget != null)
            {
                m_CurrentTarget.OnFocusEnd();
            }

            CancelHold();

            m_CurrentTarget = newTarget;

            if (m_CurrentTarget != null)
            {
                m_CurrentTarget.OnFocusBegin();
            }

            OnTargetChanged?.Invoke(m_CurrentTarget);
        }

        private void HandleInput()
        {
            if (m_CurrentTarget == null)
                return;

            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            switch (m_CurrentTarget.InteractionType)
            {
                case InteractionType.Instant:
                case InteractionType.Toggle:
                    HandleInstantInput(keyboard);
                    break;
                case InteractionType.Hold:
                    HandleHoldInput(keyboard);
                    break;
                default:
                    Debug.LogError($"[InteractionDetector] Unknown InteractionType: {m_CurrentTarget.InteractionType}");
                    break;
            }
        }

        private void HandleInstantInput(Keyboard keyboard)
        {
            if (keyboard[m_InteractionKey].wasPressedThisFrame)
            {
                TryInteract();
            }
        }

        private void HandleHoldInput(Keyboard keyboard)
        {
            if (keyboard[m_InteractionKey].isPressed)
            {
                if (!m_CurrentTarget.CanInteract(gameObject))
                {
                    CancelHold();
                    return;
                }

                m_IsHolding = true;
                m_HoldTimer += Time.deltaTime;

                float holdDuration = m_CurrentTarget.HoldDuration;
                if (holdDuration <= 0f)
                {
                    Debug.LogError("[InteractionDetector] Hold interaction has HoldDuration <= 0!");
                    CancelHold();
                    return;
                }

                float progress = Mathf.Clamp01(m_HoldTimer / holdDuration);
                OnHoldProgress?.Invoke(progress);

                if (m_HoldTimer >= holdDuration)
                {
                    m_CurrentTarget.Interact(gameObject);
                    OnInteractionCompleted?.Invoke(m_CurrentTarget);
                    CancelHold();
                }
            }
            else if (keyboard[m_InteractionKey].wasReleasedThisFrame)
            {
                CancelHold();
            }
        }

        private void TryInteract()
        {
            if (m_CurrentTarget == null)
                return;

            if (!m_CurrentTarget.CanInteract(gameObject))
            {
                Debug.Log($"[InteractionDetector] Cannot interact: {m_CurrentTarget.GetPromptMessage()}");
                return;
            }

            m_CurrentTarget.Interact(gameObject);
            OnInteractionCompleted?.Invoke(m_CurrentTarget);
        }

        private void CancelHold()
        {
            if (m_IsHolding)
            {
                OnHoldProgress?.Invoke(0f);
            }

            m_HoldTimer = 0f;
            m_IsHolding = false;
        }

        #endregion
    }
}

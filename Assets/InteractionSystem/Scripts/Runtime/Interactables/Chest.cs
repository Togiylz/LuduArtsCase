using System;

using UnityEngine;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Hold etkilesim ile acilan sandik/konteyner.
    /// Belirli sure basili tutma gerektirir. Acildiktan sonra tekrar acilamaz.
    /// </summary>
    public class Chest : MonoBehaviour, IInteractable
    {
        #region Fields

        private const float k_DefaultHoldDuration = 2f;
        private const float k_LidOpenSpeed = 4f;

        [Header("Chest Settings")]
        [SerializeField] private float m_HoldDuration = k_DefaultHoldDuration;
        [SerializeField] private ItemData m_ContainedItem;

        [Header("Visual")]
        [SerializeField] private Transform m_LidPivot;
        [SerializeField] private float m_LidOpenAngle = -110f;

        private bool m_IsOpened;
        private Quaternion m_ClosedRotation;
        private Quaternion m_OpenRotation;
        private bool m_IsFocused;

        #endregion

        #region Events

        /// <summary>
        /// Sandik acildiginda tetiklenir.
        /// </summary>
        public event Action OnChestOpened;

        /// <summary>
        /// Sandiktan item alindiginda tetiklenir.
        /// </summary>
        public event Action<ItemData> OnItemReceived;

        #endregion

        #region Properties

        /// <summary>
        /// Sandik acildi mi?
        /// </summary>
        public bool IsOpened => m_IsOpened;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_LidPivot != null)
            {
                m_ClosedRotation = m_LidPivot.localRotation;
                m_OpenRotation = m_ClosedRotation * Quaternion.Euler(m_LidOpenAngle, 0f, 0f);
            }
        }

        private void Update()
        {
            if (m_IsOpened && m_LidPivot != null)
            {
                m_LidPivot.localRotation = Quaternion.Slerp(
                    m_LidPivot.localRotation,
                    m_OpenRotation,
                    Time.deltaTime * k_LidOpenSpeed);
            }
        }

        #endregion

        #region Methods

        private void Open(GameObject interactor)
        {
            if (m_IsOpened)
                return;

            m_IsOpened = true;
            OnChestOpened?.Invoke();
            Debug.Log($"[Chest] {gameObject.name} opened.");

            GiveItem(interactor);
        }

        private void GiveItem(GameObject interactor)
        {
            if (m_ContainedItem == null)
            {
                Debug.Log($"[Chest] {gameObject.name} is empty.");
                return;
            }

            var inventory = interactor.GetComponent<PlayerInventory>();
            if (inventory == null)
            {
                Debug.LogError("[Chest] Interactor does not have PlayerInventory!");
                return;
            }

            inventory.AddItem(m_ContainedItem);
            OnItemReceived?.Invoke(m_ContainedItem);
            Debug.Log($"[Chest] {interactor.name} received {m_ContainedItem.ItemName} from {gameObject.name}.");
        }

        #endregion

        #region Interface Implementations

        InteractionType IInteractable.InteractionType => InteractionType.Hold;

        float IInteractable.HoldDuration => m_HoldDuration;

        bool IInteractable.CanInteract(GameObject interactor)
        {
            return !m_IsOpened;
        }

        void IInteractable.Interact(GameObject interactor)
        {
            Open(interactor);
        }

        string IInteractable.GetPromptMessage()
        {
            if (m_IsOpened)
                return "Already Opened";

            return $"Hold E to Open ({m_HoldDuration}s)";
        }

        void IInteractable.OnFocusBegin()
        {
            m_IsFocused = true;
        }

        void IInteractable.OnFocusEnd()
        {
            m_IsFocused = false;
        }

        #endregion
    }
}

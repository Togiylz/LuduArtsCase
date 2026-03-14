using System;

using UnityEngine;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Acilip kapanabilen kapi. Toggle etkilesim ile calisir.
    /// Kilitli ise belirtilen turdeki anahtar gerektirir.
    /// </summary>
    public class Door : MonoBehaviour, IInteractable
    {
        #region Fields

        private const float k_RotationSpeed = 4f;

        [Header("Door Settings")]
        [SerializeField] private float m_OpenAngle = 90f;
        [SerializeField] private Transform m_Pivot;

        [Header("Lock Settings")]
        [SerializeField] private bool m_IsLocked;
        [SerializeField] private KeyType m_RequiredKeyType;
        [SerializeField] private bool m_ConsumeKeyOnUnlock = true;

        [Header("Switch Only")]
        [Tooltip("True ise kapi sadece Switch/Lever ile acilir. Anahtar ve E tusu calismaz.")]
        [SerializeField] private bool m_OpenOnlyBySwitch;

        private bool m_IsOpen;
        private Quaternion m_ClosedRotation;
        private Quaternion m_OpenRotation;
        private Quaternion m_TargetRotation;
        private bool m_IsFocused;
        private InteractionHighlight m_Highlight;
        private InteractionSoundPlayer m_SoundPlayer;

        #endregion

        #region Events

        /// <summary>
        /// Kapi durumu degistiginde tetiklenir. Parametre: acik mi?
        /// </summary>
        public event Action<bool> OnDoorStateChanged;

        /// <summary>
        /// Kapi kilidi acildiginda tetiklenir.
        /// </summary>
        public event Action OnDoorUnlocked;

        #endregion

        #region Properties

        /// <summary>
        /// Kapi su an acik mi?
        /// </summary>
        public bool IsOpen => m_IsOpen;

        /// <summary>
        /// Kapi kilitli mi?
        /// </summary>
        public bool IsLocked => m_IsLocked;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_Highlight = GetComponent<InteractionHighlight>();
            m_SoundPlayer = GetComponent<InteractionSoundPlayer>();

            if (m_Pivot == null)
                m_Pivot = transform;

            m_ClosedRotation = m_Pivot.localRotation;
            m_OpenRotation = m_ClosedRotation * Quaternion.Euler(0f, m_OpenAngle, 0f);
            m_TargetRotation = m_ClosedRotation;
        }

        private void Update()
        {
            m_Pivot.localRotation = Quaternion.Slerp(
                m_Pivot.localRotation,
                m_TargetRotation,
                Time.deltaTime * k_RotationSpeed);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Kapinin kilidini disaridan acar.
        /// </summary>
        public void Unlock()
        {
            if (!m_IsLocked)
                return;

            m_IsLocked = false;
            OnDoorUnlocked?.Invoke();
            Debug.Log($"[Door] {gameObject.name} unlocked.");
        }

        /// <summary>
        /// Kapiyi disaridan acar/kapar (Switch baglantisi icin).
        /// </summary>
        /// <param name="open">True ise ac, false ise kapa.</param>
        public void SetOpen(bool open)
        {
            if (m_IsLocked && !m_OpenOnlyBySwitch)
            {
                Debug.LogWarning($"[Door] {gameObject.name} is locked, cannot set state.");
                return;
            }

            m_IsOpen = open;
            m_TargetRotation = m_IsOpen ? m_OpenRotation : m_ClosedRotation;
            OnDoorStateChanged?.Invoke(m_IsOpen);
        }

        private void Toggle(GameObject interactor)
        {
            if (m_IsLocked)
            {
                if (TryUnlockWithKey(interactor))
                {
                    m_IsLocked = false;
                    OnDoorUnlocked?.Invoke();
                    Debug.Log($"[Door] {gameObject.name} unlocked with {m_RequiredKeyType} key.");
                }
                else
                {
                    if (m_SoundPlayer != null) m_SoundPlayer.PlayDenied();
                    Debug.Log($"[Door] {gameObject.name} is locked. Requires {m_RequiredKeyType} key.");
                    return;
                }
            }

            m_IsOpen = !m_IsOpen;
            m_TargetRotation = m_IsOpen ? m_OpenRotation : m_ClosedRotation;
            OnDoorStateChanged?.Invoke(m_IsOpen);

            if (m_SoundPlayer != null) m_SoundPlayer.PlayInteract();

            Debug.Log($"[Door] {gameObject.name} is now {(m_IsOpen ? "open" : "closed")}.");
        }

        private bool TryUnlockWithKey(GameObject interactor)
        {
            var inventory = interactor.GetComponent<PlayerInventory>();
            if (inventory == null)
            {
                Debug.LogError("[Door] Interactor does not have PlayerInventory!");
                return false;
            }

            if (!inventory.HasKeyOfType(m_RequiredKeyType))
                return false;

            if (m_ConsumeKeyOnUnlock)
                inventory.UseKey(m_RequiredKeyType);

            return true;
        }

        #endregion

        #region Interface Implementations

        InteractionType IInteractable.InteractionType => InteractionType.Toggle;

        float IInteractable.HoldDuration => 0f;

        bool IInteractable.CanInteract(GameObject interactor)
        {
            if (m_OpenOnlyBySwitch)
                return false;

            if (!m_IsLocked)
                return true;

            var inventory = interactor.GetComponent<PlayerInventory>();
            if (inventory == null)
                return false;

            return inventory.HasKeyOfType(m_RequiredKeyType);
        }

        void IInteractable.Interact(GameObject interactor)
        {
            if (m_OpenOnlyBySwitch)
                return;

            Toggle(interactor);
        }

        string IInteractable.GetPromptMessage()
        {
            if (m_OpenOnlyBySwitch)
                return "Use Switch to Open";

            if (m_IsLocked)
                return $"Locked - {m_RequiredKeyType} Key Required";

            return m_IsOpen ? "Press E to Close" : "Press E to Open";
        }

        void IInteractable.OnFocusBegin()
        {
            m_IsFocused = true;
            if (m_Highlight != null) m_Highlight.SetHighlight(true);
            if (m_SoundPlayer != null) m_SoundPlayer.PlayFocus();
        }

        void IInteractable.OnFocusEnd()
        {
            m_IsFocused = false;
            if (m_Highlight != null) m_Highlight.SetHighlight(false);
        }

        #endregion
    }
}

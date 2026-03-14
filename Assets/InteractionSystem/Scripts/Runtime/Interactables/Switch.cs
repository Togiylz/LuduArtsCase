using System;

using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Toggle etkilesim ile baska nesneleri tetikleyen switch/lever.
    /// UnityEvent ile Inspector'dan baglanti kurulabilir.
    /// </summary>
    public class Switch : MonoBehaviour, IInteractable
    {
        #region Fields

        [Header("Switch Settings")]
        [SerializeField] private bool m_IsOn;
        [SerializeField] private bool m_SingleUse;

        [Header("Visual")]
        [SerializeField] private Transform m_LeverPivot;
        [SerializeField] private float m_LeverAngle = 45f;

        [Header("Events")]
        [SerializeField] private UnityEvent m_OnActivated;
        [SerializeField] private UnityEvent m_OnDeactivated;

        private bool m_HasBeenUsed;
        private bool m_IsFocused;

        #endregion

        #region Events

        /// <summary>
        /// Switch durumu degistiginde tetiklenir. Parametre: acik mi?
        /// </summary>
        public event Action<bool> OnSwitchStateChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Switch su an acik mi?
        /// </summary>
        public bool IsOn => m_IsOn;

        #endregion

        #region Unity Methods

        private void Start()
        {
            UpdateVisual();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Switch durumunu disaridan degistirir.
        /// </summary>
        /// <param name="isOn">Yeni durum.</param>
        public void SetState(bool isOn)
        {
            m_IsOn = isOn;
            UpdateVisual();
            OnSwitchStateChanged?.Invoke(m_IsOn);
        }

        private void Toggle()
        {
            m_IsOn = !m_IsOn;
            m_HasBeenUsed = true;

            UpdateVisual();

            if (m_IsOn)
            {
                m_OnActivated?.Invoke();
                Debug.Log($"[Switch] {gameObject.name} activated.");
            }
            else
            {
                m_OnDeactivated?.Invoke();
                Debug.Log($"[Switch] {gameObject.name} deactivated.");
            }

            OnSwitchStateChanged?.Invoke(m_IsOn);
        }

        private void UpdateVisual()
        {
            if (m_LeverPivot == null)
                return;

            float angle = m_IsOn ? m_LeverAngle : -m_LeverAngle;
            m_LeverPivot.localRotation = Quaternion.Euler(angle, 0f, 0f);
        }

        #endregion

        #region Interface Implementations

        InteractionType IInteractable.InteractionType => InteractionType.Toggle;

        float IInteractable.HoldDuration => 0f;

        bool IInteractable.CanInteract(GameObject interactor)
        {
            if (m_SingleUse && m_HasBeenUsed)
                return false;

            return true;
        }

        void IInteractable.Interact(GameObject interactor)
        {
            Toggle();
        }

        string IInteractable.GetPromptMessage()
        {
            if (m_SingleUse && m_HasBeenUsed)
                return "Already Used";

            return m_IsOn ? "Press E to Deactivate" : "Press E to Activate";
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

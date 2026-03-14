using UnityEngine;

using TMPro;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Etkilesim prompt mesajini ve durum geri bildirimini yoneten UI bileşeni.
    /// InteractionDetector event'lerine abone olarak calisir.
    /// </summary>
    public class InteractionPromptUI : MonoBehaviour
    {
        #region Fields

        private const float k_CannotInteractDisplayDuration = 1.5f;

        [Header("References")]
        [SerializeField] private InteractionDetector m_Detector;
        [SerializeField] private GameObject m_PromptPanel;
        [SerializeField] private TextMeshProUGUI m_PromptText;
        [SerializeField] private TextMeshProUGUI m_CannotInteractText;
        [SerializeField] private HoldProgressBarUI m_HoldProgressBar;

        private IInteractable m_LastTarget;
        private float m_CannotInteractTimer;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_Detector == null)
            {
                Debug.LogError("[InteractionPromptUI] InteractionDetector reference is not assigned!");
                return;
            }

            HideAll();
        }

        private void OnEnable()
        {
            if (m_Detector == null)
                return;

            m_Detector.OnTargetChanged += HandleTargetChanged;
            m_Detector.OnHoldProgress += HandleHoldProgress;
            m_Detector.OnInteractionCompleted += HandleInteractionCompleted;
        }

        private void OnDisable()
        {
            if (m_Detector == null)
                return;

            m_Detector.OnTargetChanged -= HandleTargetChanged;
            m_Detector.OnHoldProgress -= HandleHoldProgress;
            m_Detector.OnInteractionCompleted -= HandleInteractionCompleted;
        }

        private void Update()
        {
            UpdatePromptText();
            UpdateCannotInteractTimer();
        }

        #endregion

        #region Methods

        private void HandleTargetChanged(IInteractable target)
        {
            m_LastTarget = target;

            if (target == null)
            {
                HidePrompt();
                return;
            }

            ShowPrompt(target.GetPromptMessage());
        }

        private void HandleHoldProgress(float progress)
        {
            if (m_HoldProgressBar == null)
                return;

            if (progress > 0f)
            {
                m_HoldProgressBar.Show();
                m_HoldProgressBar.SetProgress(progress);
            }
            else
            {
                m_HoldProgressBar.Hide();
            }
        }

        private void HandleInteractionCompleted(IInteractable target)
        {
            if (m_HoldProgressBar != null)
                m_HoldProgressBar.Hide();
        }

        private void UpdatePromptText()
        {
            if (m_LastTarget == null)
                return;

            string message = m_LastTarget.GetPromptMessage();

            if (!m_LastTarget.CanInteract(m_Detector.gameObject))
            {
                ShowCannotInteract(message);
            }
            else
            {
                SetPromptText(message);
            }
        }

        private void UpdateCannotInteractTimer()
        {
            if (m_CannotInteractTimer <= 0f)
                return;

            m_CannotInteractTimer -= Time.deltaTime;

            if (m_CannotInteractTimer <= 0f)
            {
                HideCannotInteract();
            }
        }

        private void ShowPrompt(string message)
        {
            if (m_PromptPanel != null)
                m_PromptPanel.SetActive(true);

            SetPromptText(message);
        }

        private void HidePrompt()
        {
            if (m_PromptPanel != null)
                m_PromptPanel.SetActive(false);

            if (m_HoldProgressBar != null)
                m_HoldProgressBar.Hide();
        }

        private void SetPromptText(string message)
        {
            if (m_PromptText != null)
                m_PromptText.text = message;
        }

        /// <summary>
        /// Etkilesim yapilamadiginda geri bildirim gosterir.
        /// </summary>
        /// <param name="message">Gosterilecek mesaj.</param>
        public void ShowCannotInteract(string message)
        {
            if (m_CannotInteractText == null)
                return;

            m_CannotInteractText.text = message;
            m_CannotInteractText.gameObject.SetActive(true);
            m_CannotInteractTimer = k_CannotInteractDisplayDuration;
        }

        private void HideCannotInteract()
        {
            if (m_CannotInteractText != null)
                m_CannotInteractText.gameObject.SetActive(false);
        }

        private void HideAll()
        {
            HidePrompt();
            HideCannotInteract();
        }

        #endregion
    }
}

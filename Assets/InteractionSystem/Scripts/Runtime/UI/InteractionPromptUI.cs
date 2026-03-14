using UnityEngine;

using TMPro;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Etkilesim prompt mesajini ve durum geri bildirimini yoneten UI bileseni.
    /// InteractionDetector event'lerine abone olarak calisir.
    /// </summary>
    public class InteractionPromptUI : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private InteractionDetector m_Detector;
        [SerializeField] private GameObject m_PromptPanel;
        [SerializeField] private TextMeshProUGUI m_PromptText;
        [SerializeField] private TextMeshProUGUI m_CannotInteractText;
        [SerializeField] private HoldProgressBarUI m_HoldProgressBar;

        [Header("Out of Range")]
        [SerializeField] private TextMeshProUGUI m_OutOfRangeText;

        private IInteractable m_LastTarget;

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
            m_Detector.OnOutOfRangeTargetChanged += HandleOutOfRangeChanged;
            m_Detector.OnHoldProgress += HandleHoldProgress;
            m_Detector.OnInteractionCompleted += HandleInteractionCompleted;
        }

        private void OnDisable()
        {
            if (m_Detector == null)
                return;

            m_Detector.OnTargetChanged -= HandleTargetChanged;
            m_Detector.OnOutOfRangeTargetChanged -= HandleOutOfRangeChanged;
            m_Detector.OnHoldProgress -= HandleHoldProgress;
            m_Detector.OnInteractionCompleted -= HandleInteractionCompleted;
        }

        private void Update()
        {
            UpdatePromptText();
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

            HideOutOfRange();

            if (m_PromptPanel != null)
                m_PromptPanel.SetActive(true);

            UpdatePromptText();
        }

        private void HandleOutOfRangeChanged(IInteractable target)
        {
            if (m_LastTarget != null)
                return;

            if (target == null)
            {
                HideOutOfRange();
                return;
            }

            ShowOutOfRange(target.GetPromptMessage());
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

            UpdatePromptText();
        }

        private void UpdatePromptText()
        {
            if (m_LastTarget == null)
                return;

            string message = m_LastTarget.GetPromptMessage();
            bool canInteract = m_LastTarget.CanInteract(m_Detector.gameObject);

            if (canInteract)
            {
                if (m_PromptText != null)
                {
                    m_PromptText.gameObject.SetActive(true);
                    m_PromptText.text = message;
                }

                if (m_CannotInteractText != null)
                    m_CannotInteractText.gameObject.SetActive(false);
            }
            else
            {
                if (m_CannotInteractText != null)
                {
                    m_CannotInteractText.gameObject.SetActive(true);
                    m_CannotInteractText.text = message;
                }

                if (m_PromptText != null)
                    m_PromptText.gameObject.SetActive(false);
            }
        }

        private void ShowOutOfRange(string objectName)
        {
            if (m_PromptPanel != null)
                m_PromptPanel.SetActive(true);

            if (m_OutOfRangeText != null)
            {
                m_OutOfRangeText.gameObject.SetActive(true);
                m_OutOfRangeText.text = $"Too Far - {objectName}";
            }

            if (m_PromptText != null)
                m_PromptText.gameObject.SetActive(false);

            if (m_CannotInteractText != null)
                m_CannotInteractText.gameObject.SetActive(false);
        }

        private void HideOutOfRange()
        {
            if (m_OutOfRangeText != null)
                m_OutOfRangeText.gameObject.SetActive(false);
        }

        private void HidePrompt()
        {
            if (m_PromptPanel != null)
                m_PromptPanel.SetActive(false);

            if (m_CannotInteractText != null)
                m_CannotInteractText.gameObject.SetActive(false);

            if (m_PromptText != null)
                m_PromptText.gameObject.SetActive(true);

            if (m_HoldProgressBar != null)
                m_HoldProgressBar.Hide();
        }

        private void HideAll()
        {
            HidePrompt();
            HideOutOfRange();
        }

        #endregion
    }
}

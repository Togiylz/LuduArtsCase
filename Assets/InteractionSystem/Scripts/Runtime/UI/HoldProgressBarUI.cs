using UnityEngine;
using UnityEngine.UI;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Hold tipi etkilesimler icin ilerleme cubugu UI bileseni.
    /// Standart UI Slider kullanarak 0-1 arasi ilerleme gosterir.
    /// </summary>
    public class HoldProgressBarUI : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private Slider m_Slider;
        [SerializeField] private GameObject m_BarContainer;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_Slider == null)
            {
                Debug.LogError("[HoldProgressBarUI] Slider is not assigned!");
            }
            else
            {
                m_Slider.interactable = false;
                m_Slider.minValue = 0f;
                m_Slider.maxValue = 1f;
                m_Slider.value = 0f;
            }

            Hide();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Ilerleme cubugunun degerini ayarlar.
        /// </summary>
        /// <param name="progress">0-1 arasi normalize ilerleme degeri.</param>
        public void SetProgress(float progress)
        {
            if (m_Slider == null)
                return;

            m_Slider.value = Mathf.Clamp01(progress);
        }

        /// <summary>
        /// Ilerleme cubugunu gosterir.
        /// </summary>
        public void Show()
        {
            if (m_BarContainer != null)
                m_BarContainer.SetActive(true);
        }

        /// <summary>
        /// Ilerleme cubugunu gizler ve sifirlar.
        /// </summary>
        public void Hide()
        {
            if (m_BarContainer != null)
                m_BarContainer.SetActive(false);

            SetProgress(0f);
        }

        #endregion
    }
}

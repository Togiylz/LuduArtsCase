using UnityEngine;
using UnityEngine.UI;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Hold tipi etkilesimler icin ilerleme cubugu UI bileşeni.
    /// Fill amount ile 0-1 arasi ilerleme gosterir.
    /// </summary>
    public class HoldProgressBarUI : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private Image m_FillImage;
        [SerializeField] private GameObject m_BarContainer;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_FillImage == null)
            {
                Debug.LogError("[HoldProgressBarUI] FillImage is not assigned!");
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
            if (m_FillImage == null)
                return;

            m_FillImage.fillAmount = Mathf.Clamp01(progress);
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

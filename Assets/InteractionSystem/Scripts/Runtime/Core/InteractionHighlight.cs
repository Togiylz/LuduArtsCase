using UnityEngine;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// IInteractable nesnelere eklenen highlight bileseni.
    /// Odaklanildiginda emission rengi veya material degisimi ile gorsel geri bildirim saglar.
    /// </summary>
    public class InteractionHighlight : MonoBehaviour
    {
        #region Fields

        private const string k_EmissionKeyword = "_EMISSION";
        private const string k_EmissionColorProperty = "_EmissionColor";

        [Header("Highlight Settings")]
        [SerializeField] private Color m_HighlightColor = new Color(1f, 0.8f, 0.3f, 1f);
        [SerializeField] private float m_EmissionIntensity = 0.5f;
        [SerializeField] private float m_PulseSpeed = 2f;

        [Header("References")]
        [SerializeField] private Renderer[] m_Renderers;

        private MaterialPropertyBlock m_PropertyBlock;
        private bool m_IsHighlighted;
        private float m_PulseTimer;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_PropertyBlock = new MaterialPropertyBlock();

            if (m_Renderers == null || m_Renderers.Length == 0)
            {
                m_Renderers = GetComponentsInChildren<Renderer>();
            }

            EnableEmission(false);
        }

        private void Update()
        {
            if (!m_IsHighlighted)
                return;

            m_PulseTimer += Time.deltaTime * m_PulseSpeed;
            float pulse = (Mathf.Sin(m_PulseTimer) + 1f) * 0.5f;
            float intensity = Mathf.Lerp(m_EmissionIntensity * 0.3f, m_EmissionIntensity, pulse);

            Color emissionColor = m_HighlightColor * intensity;
            ApplyEmission(emissionColor);
        }

        private void OnDisable()
        {
            SetHighlight(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Highlight'i acar veya kapatir.
        /// </summary>
        /// <param name="enabled">True ise highlight acar.</param>
        public void SetHighlight(bool enabled)
        {
            m_IsHighlighted = enabled;
            m_PulseTimer = 0f;

            if (!enabled)
            {
                ApplyEmission(Color.black);
                EnableEmission(false);
            }
            else
            {
                EnableEmission(true);
            }
        }

        private void ApplyEmission(Color color)
        {
            if (m_Renderers == null)
                return;

            foreach (var rend in m_Renderers)
            {
                if (rend == null)
                    continue;

                rend.GetPropertyBlock(m_PropertyBlock);
                m_PropertyBlock.SetColor(k_EmissionColorProperty, color);
                rend.SetPropertyBlock(m_PropertyBlock);
            }
        }

        private void EnableEmission(bool enabled)
        {
            if (m_Renderers == null)
                return;

            foreach (var rend in m_Renderers)
            {
                if (rend == null)
                    continue;

                foreach (var mat in rend.materials)
                {
                    if (enabled)
                        mat.EnableKeyword(k_EmissionKeyword);
                    else
                        mat.DisableKeyword(k_EmissionKeyword);
                }
            }
        }

        #endregion
    }
}

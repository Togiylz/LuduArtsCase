using UnityEngine;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Etkilesim seslerinin tanimlandigi ScriptableObject.
    /// Her interactable nesne icin farkli ses profili atanabilir.
    /// Birden fazla clip atanirsa rastgele secim yapilir.
    /// </summary>
    [CreateAssetMenu(fileName = "NewInteractionSound", menuName = "InteractionSystem/Interaction Sound Data")]
    public class InteractionSoundData : ScriptableObject
    {
        #region Fields

        [Header("Interaction Sounds")]
        [SerializeField] private AudioClip[] m_InteractClips;
        [SerializeField] [Range(0f, 1f)] private float m_InteractVolume = 1f;
        [SerializeField] [Range(0.8f, 1.2f)] private float m_PitchVariation = 1f;

        [Header("Focus Sounds")]
        [SerializeField] private AudioClip m_FocusClip;
        [SerializeField] [Range(0f, 1f)] private float m_FocusVolume = 0.3f;

        [Header("Denied Sounds")]
        [SerializeField] private AudioClip m_DeniedClip;
        [SerializeField] [Range(0f, 1f)] private float m_DeniedVolume = 0.5f;

        [Header("Hold Sounds")]
        [SerializeField] private AudioClip m_HoldLoopClip;
        [SerializeField] [Range(0f, 1f)] private float m_HoldLoopVolume = 0.4f;
        [SerializeField] private AudioClip m_HoldCompleteClip;
        [SerializeField] [Range(0f, 1f)] private float m_HoldCompleteVolume = 0.8f;

        #endregion

        #region Properties

        /// <summary>
        /// Etkilesim sesi klipleri. Birden fazla ise rastgele secilir.
        /// </summary>
        public AudioClip[] InteractClips => m_InteractClips;

        /// <summary>
        /// Etkilesim ses seviyesi.
        /// </summary>
        public float InteractVolume => m_InteractVolume;

        /// <summary>
        /// Ses pitch varyasyonu. 1.0 = normal.
        /// </summary>
        public float PitchVariation => m_PitchVariation;

        /// <summary>
        /// Odaklanma sesi.
        /// </summary>
        public AudioClip FocusClip => m_FocusClip;

        /// <summary>
        /// Odaklanma ses seviyesi.
        /// </summary>
        public float FocusVolume => m_FocusVolume;

        /// <summary>
        /// Etkilesim reddedildiginde calacak ses.
        /// </summary>
        public AudioClip DeniedClip => m_DeniedClip;

        /// <summary>
        /// Reddedilme ses seviyesi.
        /// </summary>
        public float DeniedVolume => m_DeniedVolume;

        /// <summary>
        /// Hold sirasinda calacak loop ses.
        /// </summary>
        public AudioClip HoldLoopClip => m_HoldLoopClip;

        /// <summary>
        /// Hold loop ses seviyesi.
        /// </summary>
        public float HoldLoopVolume => m_HoldLoopVolume;

        /// <summary>
        /// Hold tamamlandiginda calacak ses.
        /// </summary>
        public AudioClip HoldCompleteClip => m_HoldCompleteClip;

        /// <summary>
        /// Hold tamamlanma ses seviyesi.
        /// </summary>
        public float HoldCompleteVolume => m_HoldCompleteVolume;

        #endregion

        #region Methods

        /// <summary>
        /// Interact kliplerinden rastgele birini dondurur.
        /// </summary>
        /// <returns>Rastgele AudioClip veya null.</returns>
        public AudioClip GetRandomInteractClip()
        {
            if (m_InteractClips == null || m_InteractClips.Length == 0)
                return null;

            return m_InteractClips[Random.Range(0, m_InteractClips.Length)];
        }

        /// <summary>
        /// Rastgele pitch degeri dondurur.
        /// </summary>
        /// <returns>PitchVariation merkezinde rastgele deger.</returns>
        public float GetRandomPitch()
        {
            float variation = m_PitchVariation - 1f;
            return 1f + Random.Range(-variation, variation);
        }

        #endregion
    }
}

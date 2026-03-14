using UnityEngine;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Interactable nesnelere eklenen ses oynatma bileseni.
    /// ScriptableObject tabanli ses profili kullanir.
    /// AudioSource pooling ile performansli calisir.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class InteractionSoundPlayer : MonoBehaviour
    {
        #region Fields

        [Header("Sound Profile")]
        [SerializeField] private InteractionSoundData m_SoundData;

        private AudioSource m_AudioSource;
        private AudioSource m_LoopSource;

        #endregion

        #region Properties

        /// <summary>
        /// Atanmis ses profili.
        /// </summary>
        public InteractionSoundData SoundData => m_SoundData;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
            m_AudioSource.playOnAwake = false;
            m_AudioSource.spatialBlend = 1f;

            SetupLoopSource();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Etkilesim sesini calar. Birden fazla clip varsa rastgele secer.
        /// </summary>
        public void PlayInteract()
        {
            if (m_SoundData == null)
                return;

            var clip = m_SoundData.GetRandomInteractClip();
            if (clip == null)
                return;

            m_AudioSource.pitch = m_SoundData.GetRandomPitch();
            m_AudioSource.PlayOneShot(clip, m_SoundData.InteractVolume);
        }

        /// <summary>
        /// Odaklanma sesini calar.
        /// </summary>
        public void PlayFocus()
        {
            if (m_SoundData == null || m_SoundData.FocusClip == null)
                return;

            m_AudioSource.pitch = 1f;
            m_AudioSource.PlayOneShot(m_SoundData.FocusClip, m_SoundData.FocusVolume);
        }

        /// <summary>
        /// Etkilesim reddedilme sesini calar.
        /// </summary>
        public void PlayDenied()
        {
            if (m_SoundData == null || m_SoundData.DeniedClip == null)
                return;

            m_AudioSource.pitch = 1f;
            m_AudioSource.PlayOneShot(m_SoundData.DeniedClip, m_SoundData.DeniedVolume);
        }

        /// <summary>
        /// Hold loop sesini baslatir.
        /// </summary>
        public void StartHoldLoop()
        {
            if (m_SoundData == null || m_SoundData.HoldLoopClip == null || m_LoopSource == null)
                return;

            if (m_LoopSource.isPlaying)
                return;

            m_LoopSource.clip = m_SoundData.HoldLoopClip;
            m_LoopSource.volume = m_SoundData.HoldLoopVolume;
            m_LoopSource.loop = true;
            m_LoopSource.Play();
        }

        /// <summary>
        /// Hold loop sesini durdurur.
        /// </summary>
        public void StopHoldLoop()
        {
            if (m_LoopSource == null)
                return;

            m_LoopSource.Stop();
            m_LoopSource.clip = null;
        }

        /// <summary>
        /// Hold tamamlanma sesini calar.
        /// </summary>
        public void PlayHoldComplete()
        {
            StopHoldLoop();

            if (m_SoundData == null || m_SoundData.HoldCompleteClip == null)
                return;

            m_AudioSource.pitch = 1f;
            m_AudioSource.PlayOneShot(m_SoundData.HoldCompleteClip, m_SoundData.HoldCompleteVolume);
        }

        private void SetupLoopSource()
        {
            var loopObj = new GameObject("LoopAudioSource");
            loopObj.transform.SetParent(transform);
            loopObj.transform.localPosition = Vector3.zero;

            m_LoopSource = loopObj.AddComponent<AudioSource>();
            m_LoopSource.playOnAwake = false;
            m_LoopSource.spatialBlend = 1f;
            m_LoopSource.loop = true;
        }

        #endregion
    }
}

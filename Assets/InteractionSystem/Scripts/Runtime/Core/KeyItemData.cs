using UnityEngine;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Anahtar item'inin tanim verisi. Hangi kilidi actigi bilgisini icerir.
    /// </summary>
    [CreateAssetMenu(fileName = "NewKey", menuName = "InteractionSystem/Key Item Data")]
    public class KeyItemData : ItemData
    {
        #region Fields

        [SerializeField] private KeyType m_KeyType = KeyType.Red;
        [SerializeField] private Color m_KeyColor = Color.red;

        #endregion

        #region Properties

        /// <summary>
        /// Bu anahtarin turu. Ayni turdeki kilitlerle eslesir.
        /// </summary>
        public KeyType KeyType => m_KeyType;

        /// <summary>
        /// Anahtarin gorsel rengi.
        /// </summary>
        public Color KeyColor => m_KeyColor;

        #endregion
    }
}

using UnityEngine;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Oyun icerisindeki item'larin temel tanim verisi.
    /// </summary>
    [CreateAssetMenu(fileName = "NewItem", menuName = "InteractionSystem/Item Data")]
    public class ItemData : ScriptableObject
    {
        #region Fields

        [SerializeField] private string m_ItemName = "New Item";
        [SerializeField] private string m_Description = "";
        [SerializeField] private Sprite m_Icon;

        #endregion

        #region Properties

        /// <summary>
        /// Item'in gorunen adi.
        /// </summary>
        public string ItemName => m_ItemName;

        /// <summary>
        /// Item aciklamasi.
        /// </summary>
        public string Description => m_Description;

        /// <summary>
        /// Item ikonu.
        /// </summary>
        public Sprite Icon => m_Icon;

        #endregion
    }
}

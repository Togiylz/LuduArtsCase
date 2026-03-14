using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Envanter listesindeki tek bir item slotunun UI bileseni.
    /// Icon ve isim gosterir.
    /// </summary>
    public class InventorySlotUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Image m_Icon;
        [SerializeField] private TextMeshProUGUI m_ItemNameText;

        #endregion

        #region Methods

        /// <summary>
        /// Slotu belirtilen item verisiyle doldurur.
        /// </summary>
        /// <param name="item">Gosterilecek item verisi.</param>
        public void Setup(ItemData item)
        {
            if (item == null)
            {
                Debug.LogError("[InventorySlotUI] Setup called with null item!");
                return;
            }

            if (m_ItemNameText != null)
                m_ItemNameText.text = item.ItemName;

            if (m_Icon != null)
            {
                if (item.Icon != null)
                {
                    m_Icon.sprite = item.Icon;
                    m_Icon.enabled = true;
                }
                else
                {
                    m_Icon.enabled = false;
                }
            }
        }

        #endregion
    }
}

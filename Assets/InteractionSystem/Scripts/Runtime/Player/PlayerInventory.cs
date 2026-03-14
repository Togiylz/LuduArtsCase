using System;
using System.Collections.Generic;

using UnityEngine;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Oyuncunun basit envanter sistemi. Toplanan item'lari saklar ve sorgular.
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        #region Fields

        private readonly List<ItemData> m_Items = new List<ItemData>();

        #endregion

        #region Events

        /// <summary>
        /// Envantere item eklendiginde tetiklenir.
        /// </summary>
        public event Action<ItemData> OnItemAdded;

        /// <summary>
        /// Envanterden item cikarildiginda tetiklenir.
        /// </summary>
        public event Action<ItemData> OnItemRemoved;

        #endregion

        #region Properties

        /// <summary>
        /// Envanterdeki tum item'larin salt-okunur listesi.
        /// </summary>
        public IReadOnlyList<ItemData> Items => m_Items;

        #endregion

        #region Methods

        /// <summary>
        /// Envantere yeni bir item ekler.
        /// </summary>
        /// <param name="item">Eklenecek item verisi.</param>
        public void AddItem(ItemData item)
        {
            if (item == null)
            {
                Debug.LogError("[PlayerInventory] AddItem: item is null!");
                return;
            }

            m_Items.Add(item);
            OnItemAdded?.Invoke(item);
            Debug.Log($"[PlayerInventory] Item added: {item.ItemName}");
        }

        /// <summary>
        /// Envanterden bir item cikarir.
        /// </summary>
        /// <param name="item">Cikarilacak item verisi.</param>
        /// <returns>Basariyla cikarildiysa true.</returns>
        public bool RemoveItem(ItemData item)
        {
            if (item == null)
            {
                Debug.LogError("[PlayerInventory] RemoveItem: item is null!");
                return false;
            }

            if (!m_Items.Remove(item))
            {
                Debug.LogWarning($"[PlayerInventory] Item not found in inventory: {item.ItemName}");
                return false;
            }

            OnItemRemoved?.Invoke(item);
            return true;
        }

        /// <summary>
        /// Envanterde belirtilen item var mi kontrol eder.
        /// </summary>
        /// <param name="item">Kontrol edilecek item.</param>
        /// <returns>Item envanterde varsa true.</returns>
        public bool HasItem(ItemData item)
        {
            if (item == null)
            {
                Debug.LogError("[PlayerInventory] HasItem: item is null!");
                return false;
            }

            return m_Items.Contains(item);
        }

        /// <summary>
        /// Envanterde belirtilen turdeki anahtar var mi kontrol eder.
        /// </summary>
        /// <param name="keyType">Aranan anahtar turu.</param>
        /// <returns>Uygun anahtar varsa true.</returns>
        public bool HasKeyOfType(KeyType keyType)
        {
            foreach (var item in m_Items)
            {
                if (item is KeyItemData keyItem && keyItem.KeyType == keyType)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Belirtilen turdeki anahtari envanterden kullanir (cikarir).
        /// </summary>
        /// <param name="keyType">Kullanilacak anahtar turu.</param>
        /// <returns>Anahtar bulunup kullanildiysa true.</returns>
        public bool UseKey(KeyType keyType)
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i] is KeyItemData keyItem && keyItem.KeyType == keyType)
                {
                    var removedItem = m_Items[i];
                    m_Items.RemoveAt(i);
                    OnItemRemoved?.Invoke(removedItem);
                    Debug.Log($"[PlayerInventory] Key used: {removedItem.ItemName} ({keyType})");
                    return true;
                }
            }

            Debug.LogWarning($"[PlayerInventory] No key of type {keyType} found in inventory.");
            return false;
        }

        #endregion
    }
}

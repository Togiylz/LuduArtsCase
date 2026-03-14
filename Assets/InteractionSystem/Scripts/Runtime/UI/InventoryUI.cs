using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Oyuncunun envanterindeki item'lari listeleyen basit UI bileseni.
    /// PlayerInventory event'lerine abone olarak otomatik guncellenir.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private PlayerInventory m_Inventory;
        [SerializeField] private GameObject m_Panel;
        [SerializeField] private Transform m_ItemListParent;
        [SerializeField] private GameObject m_ItemSlotPrefab;

        [Header("Settings")]
        [SerializeField] private Key m_ToggleKey = Key.Tab;

        private readonly List<GameObject> m_SpawnedSlots = new List<GameObject>();
        private bool m_IsVisible;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_Inventory == null)
            {
                Debug.LogError("[InventoryUI] PlayerInventory reference is not assigned!");
            }

            if (m_Panel != null)
                m_Panel.SetActive(false);
        }

        private void OnEnable()
        {
            if (m_Inventory == null)
                return;

            m_Inventory.OnItemAdded += HandleItemChanged;
            m_Inventory.OnItemRemoved += HandleItemChanged;
        }

        private void OnDisable()
        {
            if (m_Inventory == null)
                return;

            m_Inventory.OnItemAdded -= HandleItemChanged;
            m_Inventory.OnItemRemoved -= HandleItemChanged;
        }

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            if (keyboard[m_ToggleKey].wasPressedThisFrame)
            {
                TogglePanel();
            }
        }

        #endregion

        #region Methods

        private void HandleItemChanged(ItemData item)
        {
            RefreshList();
        }

        private void TogglePanel()
        {
            m_IsVisible = !m_IsVisible;

            if (m_Panel != null)
                m_Panel.SetActive(m_IsVisible);

            if (m_IsVisible)
                RefreshList();
        }

        private void RefreshList()
        {
            ClearSlots();

            if (m_Inventory == null || m_ItemListParent == null || m_ItemSlotPrefab == null)
                return;

            foreach (var item in m_Inventory.Items)
            {
                var slotObj = Instantiate(m_ItemSlotPrefab, m_ItemListParent);
                var slotText = slotObj.GetComponentInChildren<TextMeshProUGUI>();

                if (slotText != null)
                    slotText.text = item.ItemName;

                m_SpawnedSlots.Add(slotObj);
            }
        }

        private void ClearSlots()
        {
            foreach (var slot in m_SpawnedSlots)
            {
                if (slot != null)
                    Destroy(slot);
            }

            m_SpawnedSlots.Clear();
        }

        #endregion
    }
}

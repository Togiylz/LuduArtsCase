using System;
using System.Collections.Generic;

using UnityEngine;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Interactable nesnelerin durumlarini kaydedip yukleyen sistem.
    /// JSON formatinda PlayerPrefs'e kayit yapar.
    /// Her interactable nesne sahnede benzersiz bir ID ile tanimlanir.
    /// </summary>
    public class InteractionSaveSystem : MonoBehaviour
    {
        #region Fields

        private const string k_SaveKey = "InteractionSystemSave";

        private static InteractionSaveSystem s_Instance;

        #endregion

        #region Properties

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static InteractionSaveSystem Instance => s_Instance;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Debug.LogWarning("[InteractionSaveSystem] Duplicate instance found, destroying.");
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
        }

        private void OnDestroy()
        {
            if (s_Instance == this)
                s_Instance = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tum interactable durumlarini kaydeder.
        /// </summary>
        public void SaveAll()
        {
            var saveData = new SaveData();

            var doors = FindObjectsByType<Door>(FindObjectsSortMode.None);
            foreach (var door in doors)
            {
                var id = GetObjectId(door.gameObject);
                saveData.DoorStates[id] = new DoorSaveState
                {
                    IsOpen = door.IsOpen,
                    IsLocked = door.IsLocked
                };
            }

            var chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
            foreach (var chest in chests)
            {
                var id = GetObjectId(chest.gameObject);
                saveData.ChestStates[id] = chest.IsOpened;
            }

            var switches = FindObjectsByType<Switch>(FindObjectsSortMode.None);
            foreach (var sw in switches)
            {
                var id = GetObjectId(sw.gameObject);
                saveData.SwitchStates[id] = sw.IsOn;
            }

            string json = JsonUtility.ToJson(saveData, true);
            PlayerPrefs.SetString(k_SaveKey, json);
            PlayerPrefs.Save();

            Debug.Log($"[InteractionSaveSystem] Saved {doors.Length} doors, {chests.Length} chests, {switches.Length} switches.");
        }

        /// <summary>
        /// Kaydedilmis interactable durumlarini yukler.
        /// </summary>
        public void LoadAll()
        {
            if (!PlayerPrefs.HasKey(k_SaveKey))
            {
                Debug.Log("[InteractionSaveSystem] No save data found.");
                return;
            }

            string json = PlayerPrefs.GetString(k_SaveKey);
            var saveData = JsonUtility.FromJson<SaveData>(json);

            if (saveData == null)
            {
                Debug.LogError("[InteractionSaveSystem] Failed to parse save data!");
                return;
            }

            var doors = FindObjectsByType<Door>(FindObjectsSortMode.None);
            foreach (var door in doors)
            {
                var id = GetObjectId(door.gameObject);
                if (saveData.DoorStates.TryGetValue(id, out var state))
                {
                    if (!state.IsLocked && door.IsLocked)
                        door.Unlock();

                    door.SetOpen(state.IsOpen);
                }
            }

            var chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
            foreach (var chest in chests)
            {
                var id = GetObjectId(chest.gameObject);
                if (saveData.ChestStates.TryGetValue(id, out bool isOpened) && isOpened)
                {
                    ((IInteractable)chest).Interact(gameObject);
                }
            }

            var switches = FindObjectsByType<Switch>(FindObjectsSortMode.None);
            foreach (var sw in switches)
            {
                var id = GetObjectId(sw.gameObject);
                if (saveData.SwitchStates.TryGetValue(id, out bool isOn))
                {
                    sw.SetState(isOn);
                }
            }

            Debug.Log("[InteractionSaveSystem] Load complete.");
        }

        /// <summary>
        /// Kayit verisini siler.
        /// </summary>
        public void ClearSave()
        {
            PlayerPrefs.DeleteKey(k_SaveKey);
            PlayerPrefs.Save();
            Debug.Log("[InteractionSaveSystem] Save data cleared.");
        }

        private string GetObjectId(GameObject obj)
        {
            return $"{obj.scene.name}_{obj.name}_{obj.transform.GetSiblingIndex()}";
        }

        #endregion

        #region Nested Types

        [Serializable]
        private class SaveData
        {
            public SerializableDictionary<string, DoorSaveState> DoorStates = new SerializableDictionary<string, DoorSaveState>();
            public SerializableDictionary<string, bool> ChestStates = new SerializableDictionary<string, bool>();
            public SerializableDictionary<string, bool> SwitchStates = new SerializableDictionary<string, bool>();
        }

        [Serializable]
        private struct DoorSaveState
        {
            public bool IsOpen;
            public bool IsLocked;
        }

        [Serializable]
        private class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
        {
            [SerializeField] private List<TKey> m_Keys = new List<TKey>();
            [SerializeField] private List<TValue> m_Values = new List<TValue>();

            public void OnBeforeSerialize()
            {
                m_Keys.Clear();
                m_Values.Clear();

                foreach (var kvp in this)
                {
                    m_Keys.Add(kvp.Key);
                    m_Values.Add(kvp.Value);
                }
            }

            public void OnAfterDeserialize()
            {
                Clear();

                int count = Mathf.Min(m_Keys.Count, m_Values.Count);
                for (int i = 0; i < count; i++)
                {
                    this[m_Keys[i]] = m_Values[i];
                }
            }
        }

        #endregion
    }
}

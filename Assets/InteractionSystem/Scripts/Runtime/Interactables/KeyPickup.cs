using UnityEngine;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Oyuncunun toplayabilecegi anahtar nesnesi. Instant etkilesim ile calisir.
    /// Toplandiginda envantere eklenir ve sahneden kaldirilir.
    /// </summary>
    public class KeyPickup : MonoBehaviour, IInteractable
    {
        #region Fields

        [Header("Key Settings")]
        [SerializeField] private KeyItemData m_KeyData;

        [Header("Visual")]
        [SerializeField] private float m_RotateSpeed = 90f;
        [SerializeField] private float m_BobSpeed = 2f;
        [SerializeField] private float m_BobHeight = 0.15f;

        private Vector3 m_StartPosition;
        private bool m_IsFocused;
        private bool m_IsCollected;

        #endregion

        #region Properties

        /// <summary>
        /// Bu pickup'in temsil ettigi anahtar verisi.
        /// </summary>
        public KeyItemData KeyData => m_KeyData;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_KeyData == null)
            {
                Debug.LogError($"[KeyPickup] {gameObject.name}: KeyData is not assigned!");
            }
        }

        private void Start()
        {
            m_StartPosition = transform.position;
        }

        private void Update()
        {
            AnimateIdle();
        }

        #endregion

        #region Methods

        private void AnimateIdle()
        {
            transform.Rotate(Vector3.up, m_RotateSpeed * Time.deltaTime, Space.World);

            float newY = m_StartPosition.y + Mathf.Sin(Time.time * m_BobSpeed) * m_BobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        private void Collect(GameObject interactor)
        {
            if (m_IsCollected)
                return;

            var inventory = interactor.GetComponent<PlayerInventory>();
            if (inventory == null)
            {
                Debug.LogError("[KeyPickup] Interactor does not have PlayerInventory!");
                return;
            }

            m_IsCollected = true;
            inventory.AddItem(m_KeyData);
            Debug.Log($"[KeyPickup] {m_KeyData.ItemName} collected by {interactor.name}.");

            Destroy(gameObject);
        }

        #endregion

        #region Interface Implementations

        InteractionType IInteractable.InteractionType => InteractionType.Instant;

        float IInteractable.HoldDuration => 0f;

        bool IInteractable.CanInteract(GameObject interactor)
        {
            if (m_IsCollected)
                return false;

            if (m_KeyData == null)
            {
                Debug.LogError($"[KeyPickup] {gameObject.name}: KeyData is null, cannot interact!");
                return false;
            }

            return true;
        }

        void IInteractable.Interact(GameObject interactor)
        {
            Collect(interactor);
        }

        string IInteractable.GetPromptMessage()
        {
            if (m_KeyData == null)
                return "Error: No Key Data";

            return $"Press E to Pick Up {m_KeyData.ItemName}";
        }

        void IInteractable.OnFocusBegin()
        {
            m_IsFocused = true;
        }

        void IInteractable.OnFocusEnd()
        {
            m_IsFocused = false;
        }

        #endregion
    }
}

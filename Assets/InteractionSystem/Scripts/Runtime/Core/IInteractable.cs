using UnityEngine;

namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Oyuncunun etkilesime gecebilecegi tum nesnelerin uygulamasi gereken sozlesme.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Bu nesnenin etkilesim turu.
        /// </summary>
        InteractionType InteractionType { get; }

        /// <summary>
        /// Bu nesneyle etkilesim mumkun mu?
        /// </summary>
        /// <param name="interactor">Etkilesimi baslatan GameObject (oyuncu).</param>
        /// <returns>Etkilesim yapilabilirse true.</returns>
        bool CanInteract(GameObject interactor);

        /// <summary>
        /// Etkilesimi gerceklestirir.
        /// </summary>
        /// <param name="interactor">Etkilesimi baslatan GameObject (oyuncu).</param>
        void Interact(GameObject interactor);

        /// <summary>
        /// UI'da gosterilecek prompt mesajini dondurur.
        /// </summary>
        /// <returns>Ornegin "Press E to Open".</returns>
        string GetPromptMessage();

        /// <summary>
        /// Hold tipi etkilesimler icin gereken sure (saniye).
        /// Instant ve Toggle icin 0 dondurulmeli.
        /// </summary>
        float HoldDuration { get; }

        /// <summary>
        /// Nesne oyuncunun odagina girdiginde cagrilir.
        /// Highlight efekti vb. icin kullanilabilir.
        /// </summary>
        void OnFocusBegin();

        /// <summary>
        /// Nesne oyuncunun odagindan ciktiginda cagrilir.
        /// </summary>
        void OnFocusEnd();

        /// <summary>
        /// Bu nesnenin Transform referansi. Mesafe hesabi icin kullanilir.
        /// </summary>
        Transform transform { get; }
    }
}

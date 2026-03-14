namespace InteractionSystem.Runtime
{
    /// <summary>
    /// Etkilesim turlerini tanimlar.
    /// </summary>
    public enum InteractionType
    {
        /// <summary>
        /// Tek tus basimi ile aninda gerceklesen etkilesim. (Ornek: item toplama)
        /// </summary>
        Instant,

        /// <summary>
        /// Belirli sure basili tutma gerektiren etkilesim. (Ornek: sandik acma)
        /// </summary>
        Hold,

        /// <summary>
        /// Acik/kapali durumlar arasinda gecis yapan etkilesim. (Ornek: kapi, switch)
        /// </summary>
        Toggle
    }
}

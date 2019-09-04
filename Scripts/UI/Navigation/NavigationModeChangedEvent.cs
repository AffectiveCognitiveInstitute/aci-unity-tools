namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    ///     Global event called when a change in navigation was made.
    /// </summary>
    public struct NavigationModeChangedEvent
    {
        /// <summary>
        ///     The screen id who has been navigated to or away from.
        /// </summary>
        public string screenId { get; }

        /// <summary>
        ///     The type of <see cref="NavigationMode"/> that was used.
        /// </summary>
        public NavigationMode navigationMode { get; }

        /// <summary>
        ///     Creates an instance of <see cref="NavigationCompletedEvent"/>.
        /// </summary>
        /// <param name="screenId">The screen id.</param>
        /// <param name="navigationMode">The <see cref="NavigationMode"/>.</param>
        public NavigationModeChangedEvent(string screenId, NavigationMode navigationMode)
        {
            this.screenId = screenId;
            this.navigationMode = navigationMode;
        }
    }
}

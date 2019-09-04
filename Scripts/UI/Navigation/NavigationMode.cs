namespace Aci.Unity.UI.Navigation
{
    public enum NavigationMode
    {
        /// <summary>
        ///     When a new page is pushed onto the stack
        /// </summary>
        Entering,

        /// <summary>
        ///     Page was popped, now we're returning to the page
        /// </summary>
        Returning,

        /// <summary>
        ///     A new page was pushed, the page is being left
        /// </summary>
        Leaving,

        /// <summary>
        ///     The page was popped
        /// </summary>
        Removed
    }
}

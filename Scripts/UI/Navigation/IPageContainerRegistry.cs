namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Registry for <see cref="IPageContainer"/>
    /// </summary>
    public interface IPageContainerRegistry
    {
        /// <summary>
        /// Gets a page container by page id <see cref="IPageContainer.Id"/>
        /// </summary>
        /// <param name="pageId">The id of the page</param>
        /// <returns>Returns the <see cref="IPageContainer"/></returns>
        IPageContainer GetContainer(string pageId);

        /// <summary>
        /// Tries to get an <see cref="IPageContainer"/>
        /// </summary>
        /// <param name="pageId">The id of the page</param>
        /// <param name="container">The returned <see cref="IPageContainer"/></param>
        /// <returns></returns>
        bool TryGetContainer(string pageId, out IPageContainer container);

        /// <summary>
        /// Registers an instance of <see cref="IPageContainer"/>
        /// </summary>
        /// <param name="container">The page container to be registered</param>
        void Register(IPageContainer container);

        /// <summary>
        /// Unregisters a page container by id
        /// </summary>
        /// <param name="pageId">The page id</param>
        void Unregister(string pageId);
    }
}


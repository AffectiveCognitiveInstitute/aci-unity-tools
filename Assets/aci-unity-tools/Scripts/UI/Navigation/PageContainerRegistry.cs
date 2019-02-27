using System;
using System.Collections.Generic;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Implementation of <see cref="IPageContainerRegistry"/>. Contains a mapping of page id to <see cref="IPageContainer"/>
    /// </summary>
    public class PageContainerRegistry : IPageContainerRegistry
    {
        private Dictionary<string, IPageContainer> m_Mapping = new Dictionary<string, IPageContainer>();

        /// <inheritdoc />
        public IPageContainer GetContainer(string pageId)
        {
            if (string.IsNullOrEmpty(pageId))
                throw new ArgumentNullException(pageId);

            return m_Mapping[pageId];
        }

        /// <inheritdoc />
        public void Register(IPageContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            m_Mapping.Add(container.Id, container);
        }

        /// <inheritdoc />
        public bool TryGetContainer(string pageId, out IPageContainer container)
        {
            if (string.IsNullOrEmpty(pageId))
                throw new ArgumentNullException(pageId);

            return m_Mapping.TryGetValue(pageId, out container);
        }

        /// <inheritdoc />
        public void Unregister(string pageId)
        {
            if (string.IsNullOrEmpty(pageId))
                throw new ArgumentNullException(pageId);

            m_Mapping.Remove(pageId);
        }
    }
}

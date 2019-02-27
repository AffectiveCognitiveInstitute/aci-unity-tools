using System;
using System.Collections.Generic;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Implementation of <see cref="INavigationService"/>
    /// </summary>
    public class NavigationService : INavigationService
    {
        private IPageContainerRegistry m_Registry;
        private Stack<IPageContainer> m_NavigationStack = new Stack<IPageContainer>();

        private IPageContainer m_Current;

        /// <summary>
        /// Creates an instance of <see cref="NavigationService"/>
        /// </summary>
        /// <param name="registry">The registry for <see cref="IPageContainer"/></param>
        public NavigationService(IPageContainerRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException("registry");

            m_Registry = registry;
        }

        /// <inheritdoc />
        public int StackCount => m_NavigationStack.Count;

        /// <inheritdoc />
        public event Action Navigated;

        /// <inheritdoc />
        public void Pop(Action callback = null)
        {
            if (m_NavigationStack.Count == 0)
            {
                if (callback != null)
                    callback();
            }

            IPageContainer newContainer = m_NavigationStack.Pop();
            m_Current.OnNavigatedAway(null);
            m_Current.Hide(newContainer.Id, () =>
            {
                m_Current.Destroy();
                m_Current = newContainer;
                m_Current.Display(() =>
                {
                    m_Current.OnNavigatedBack(null);
                    if (callback != null)
                        callback();
                });

                Navigated?.Invoke();
            });
        }

        /// <inheritdoc />
        public void Pop(INavigationParameters parameters, Action navigationCompleted = null)
        {
            if (m_NavigationStack.Count == 0)
            {
                if (navigationCompleted != null)
                    navigationCompleted();

                return;
            }

            IPageContainer newContainer = m_NavigationStack.Pop();
            m_Current.OnNavigatedAway(parameters);
            m_Current.Hide(newContainer.Id, () =>
            {
                m_Current.Destroy();
                m_Current = newContainer;
                m_Current.OnNavigatedBack(parameters);
                if (navigationCompleted != null)
                    navigationCompleted();

                Navigated?.Invoke();
            });
        }

        /// <inheritdoc />
        public void Push(string page,
                         bool addToStack = true,
                         Action navigationCompleted = null)
        {
            if (string.IsNullOrEmpty(page))
                throw new ArgumentNullException("page");

            IPageContainer container = null;
            if (!m_Registry.TryGetContainer(page, out container))
                throw new PageNotFoundException("page");

            if (addToStack)
                m_NavigationStack.Push(m_Current);

            if (m_Current != null)
            {
                container.PrepareDisplay();
                container.OnNavigatingTo(null);
                m_Current.OnNavigatedAway(null);
                m_Current.Hide(page, () =>
                {
                    container.Display(() =>
                    {
                        container.OnNavigatedTo(null);
                        if (navigationCompleted != null)
                            navigationCompleted();

                        Navigated?.Invoke();
                    });
                });
            }
            else
            {
                container.PrepareDisplay();
                container.OnNavigatingTo(null);
                container.Display(() =>
                {
                    container.OnNavigatedTo(null);
                    if (navigationCompleted != null)
                        navigationCompleted();

                    Navigated?.Invoke();
                });
                
            }

            m_Current = container;
        }

        /// <inheritdoc />
        public void Push(string page,
                         INavigationParameters parameters,
                         bool addToStack = true,
                         Action navigationCompleted = null)
        {
            if (string.IsNullOrEmpty(page))
                throw new ArgumentNullException("page");

            IPageContainer container = null;
            if (!m_Registry.TryGetContainer(page, out container))
                throw new PageNotFoundException("page");

            if (addToStack)
                m_NavigationStack.Push(m_Current);

            if (m_Current != null)
            {
                container.PrepareDisplay();
                m_Current.OnNavigatedAway(parameters);
                m_Current.Hide(page, () =>
                {
                    container.OnNavigatingTo(parameters);
                    container.Display(() =>
                    {
                        container.OnNavigatedTo(parameters);
                        if (navigationCompleted != null)
                            navigationCompleted.Invoke();

                        Navigated?.Invoke();
                    });
                });
            }
            else
            {
                container.PrepareDisplay();
                container.OnNavigatingTo(parameters);
                container.Display(()=>
                {
                    container.OnNavigatedTo(parameters);
                    if (navigationCompleted != null)
                        navigationCompleted.Invoke();

                    Navigated?.Invoke();
                });
            }

            m_Current = container;
        }
    }
}

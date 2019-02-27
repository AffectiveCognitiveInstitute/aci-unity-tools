using System;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Interface for navigating within an application
    /// </summary>
    public interface INavigationService 
    {
        /// <summary>
        /// Event triggered when navigation has occurred
        /// </summary>
        event Action Navigated;

        /// <summary>
        /// The number of items on the navigation stack
        /// </summary>
        int StackCount { get; }

        /// <summary>
        /// ops a page from the navigation stack. Returns to a previous page
        /// </summary>
        /// <param name="callback">A callback when any animation has completed</param>
        void Pop(Action callback = null);

        /// <summary>
        /// Pops a page from the navigation stack. Returns to a previous page
        /// </summary>
        /// <param name="parameters">Parameters passed to the previous page</param>
        /// <param name="navigationCompleted">Callback when navigation has completed, i.e. animation has completed</param>
        void Pop(INavigationParameters parameters, Action navigationCompleted = null);

        /// <summary>
        /// Pushes a page onto the navigation stack. Opens a new page
        /// </summary>
        /// <param name="page">The identifier of the page. See <see cref="IPageContainer"/></param>
        /// <param name="addToStack">Should the page be added to the stack, when pushing another page afterwards?</param>
        /// <param name="navigationCompleted">Callback when navigation has completed, i.e. animation has completed</param>
        void Push(string page, bool addToStack = true, Action navigationCompleted = null);

        /// <summary>
        /// Pushes a page onto the navigation stack. Opens a new page
        /// </summary>
        /// <param name="page">The identifier of the page. See <see cref="IPageContainer"/></param>
        /// <param name="parameters">Parameters passed to the page</param>
        /// <param name="addToStack">Should the page be added to the stack, when pushing another page afterwards?</param>
        /// <param name="navigationCompleted">Callback when navigation has completed, i.e. animation has completed</param>
        void Push(string page, INavigationParameters parameters, bool addToStack = true, Action navigationCompleted = null);
    }
}


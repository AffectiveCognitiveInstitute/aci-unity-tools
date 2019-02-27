using System;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Interface for transitions
    /// </summary>
    public interface IPageTransition
    {
        /// <summary>
        /// Called when page has been pushed onto navigation stack
        /// </summary>
        /// <param name="transitionEnded">A callback when the animation has been completed</param>
        void OnPagePushed(Action transitionEnded = null);

        /// <summary>
        /// Called when a page has been popped from the navigation stack
        /// </summary>
        /// <param name="transitionEnded">A callback when the animation has completed</param>
        void OnPagePopped(Action transitionEnded = null);
    }
}


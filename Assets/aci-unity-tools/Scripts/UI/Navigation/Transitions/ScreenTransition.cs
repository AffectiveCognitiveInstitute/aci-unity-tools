using System.Threading.Tasks;
using UnityEngine;

namespace Aci.Unity.UI.Navigation
{
    public abstract class ScreenTransition : MonoBehaviour, IScreenTransition
    {
        public virtual Task MakeTransitionAsync(NavigationMode navigationMode)
        {
            switch (navigationMode)
            {
                case NavigationMode.Entering:
                    return OnScreenEntering();
                case NavigationMode.Leaving:
                    return OnScreenLeaving();
                case NavigationMode.Returning:
                    return OnScreenReturning();
                case NavigationMode.Removed:
                    return OnScreenBeingRemoved();
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(navigationMode));
            }
        }

        public abstract Task OnScreenEntering();
        public abstract Task OnScreenLeaving();
        public abstract Task OnScreenReturning();
        public abstract Task OnScreenBeingRemoved();
        public abstract void DisplayImmediately();
    }
}
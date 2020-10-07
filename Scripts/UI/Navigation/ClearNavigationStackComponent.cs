using UnityEngine;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Component that clears the navigation stack.
    /// </summary>
    public class ClearNavigationStackComponent : MonoBehaviour
    {
        private INavigationService _navigationService;

        [Zenject.Inject]
        private void Construct(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        /// <summary>
        /// Removes all screens from the navigation stack.
        /// </summary>
        public void Execute()
        {
            _navigationService.Clear();
        }
    }
}
using System.Collections.Generic;

namespace Aci.Unity.UI.Navigation
{
    public struct NavigationCompletedEvent
    {
        public string current { get; }
        public IReadOnlyCollection<IScreenController> navigationStack { get; }

        public NavigationCompletedEvent(string current, IReadOnlyCollection<IScreenController> stack)
        {
            this.current = current;
            this.navigationStack = stack;
        }
    }
}

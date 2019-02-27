using System;

namespace Aci.Unity.UI.Navigation
{
    public interface IPageContainer : INavigationAware
    {
        string Id { get; }
        void PrepareDisplay();
        void Hide(string newPageId, Action callback = null);
        void Display(Action callback = null);
        void Destroy();
    }
}


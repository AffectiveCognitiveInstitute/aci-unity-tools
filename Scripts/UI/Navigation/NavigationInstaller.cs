using Zenject;

namespace Aci.Unity.UI.Navigation
{ 
    public class NavigationInstaller : MonoInstaller<NavigationInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<INavigationService>().To<NavigationService>().AsCached().NonLazy();
            Container.Bind<IScreenRegistry>().To<ScreenRegistry>().AsCached().NonLazy();
        }
    }
}
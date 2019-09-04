using Zenject;

namespace Aci.Unity.UI.Navigation
{ 
    public class NavigationInstaller : MonoInstaller<NavigationInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<INavigationService>().To<NavigationService>().AsSingle().NonLazy();
            Container.Bind<IScreenRegistry>().To<ScreenRegistry>().AsSingle().NonLazy();
        }
    }
}
using Zenject;

namespace Aci.Unity.UI.Dialog
{
    public class DialogInstaller : MonoInstaller<DialogInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IDialogService>().To<DialogService>().AsSingle();
            Container.Bind(typeof(IDialogRequestReceiver), typeof(IDialogDestroyer)).To<DialogManager>().AsCached();
        }
    }
}
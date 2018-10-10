using Aci.Unity.Events;
using Aci.Unity.UI.Localization;
using UnityEngine;
using Zenject;

public class LocalizationInstaller : MonoInstaller<LocalizationInstaller>
{
    public LocalizationManager locMan;

    public override void InstallBindings()
    {
        Container.Bind<IAciEventManager>().FromInstance(new AciEventManager());
        //Container.QueueForInject(eventBroker);

        Container.BindInstance<ILocalizationManager>(locMan);
        Container.QueueForInject(locMan);
    }
}
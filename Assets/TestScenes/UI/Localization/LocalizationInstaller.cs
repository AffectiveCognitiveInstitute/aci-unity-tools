using Aci.Unity.Events;
using Aci.Unity.UI.Localization;
using UnityEngine;
using Zenject;

public class LocalizationInstaller : MonoInstaller<LocalizationInstaller>
{
    public AciEventBroker eventBroker;
    public LocalizationManager locMan;

    public override void InstallBindings()
    {
        Container.BindInstance(eventBroker);
        Container.QueueForInject(eventBroker);

        Container.BindInstance<ILocalizationManager>(locMan);
        Container.QueueForInject(locMan);
    }
}
using System.Collections;
using System.Collections.Generic;
using Aci.Unity.Events;
using Aci.Unity.Sensor;
using UnityEngine;
using Zenject;

public class WebcamInstaller : MonoInstaller
{
    public WebcamProvider webcamProvider;

    public override void InstallBindings()
    {
        Container.Bind<IAciEventManager>().FromInstance(new AciEventManager());

        Container.BindInstance<WebcamProvider>(webcamProvider);
        Container.QueueForInject(webcamProvider);
    }
}

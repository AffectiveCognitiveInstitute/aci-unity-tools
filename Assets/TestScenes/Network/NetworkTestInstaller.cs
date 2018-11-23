using System.Collections;
using System.Collections.Generic;
using Aci.Unity.Network;
using Zenject;

public class NetworkTestInstaller : MonoInstaller<NetworkTestInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<INetworkPackage>().FromInstance(new EmotionPackage());
    }
}

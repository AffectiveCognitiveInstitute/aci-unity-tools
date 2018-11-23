using System.Collections;
using System.Collections.Generic;
using Aci.Unity.Network;
using UnityEngine;

public class EmotionPackage : INetworkPackage
{
    /// <inheritdoc />
    public string call => "emotion";

    public int emotiveState;
}

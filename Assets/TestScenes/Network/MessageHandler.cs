using System;
using System.Collections;
using System.Collections.Generic;
using Aci.Unity.Logging;
using Aci.Unity.Network;
using UnityEngine;

public class MessageHandler : NetworkPackageHandlerBase<EmotionPackage> {
    
    public override void Handle(EmotionPackage package)
    {
        AciLog.Log("MessageHandler", "I handled a message!");
    }
}

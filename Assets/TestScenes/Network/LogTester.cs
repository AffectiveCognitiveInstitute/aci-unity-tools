using System;
using System.Collections;
using System.Collections.Generic;
using Aci.Unity.Logging;
using UnityEngine;

public class LogTester : MonoBehaviour
{
    public void SendLog()
    {
        AciLog.Log("LogTester", "This is a test message.");
    }

    public void SendWarning()
    {
        AciLog.LogWarning("LogTester", "This is a test warning.");
    }

    public void SendError()
    {
        AciLog.LogError("LogTester", "This is a test error.");
    }

    public void SendException()
    {
        AciLog.LogException(new Exception("This is a test exception."));
    }
}

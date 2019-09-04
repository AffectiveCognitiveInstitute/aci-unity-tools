using Aci.Unity.UI.Navigation;
using UnityEngine;

public class PopScreenByInput : MonoBehaviour
{
    [SerializeField]
    private PopScreenComponent m_PopScreenComponent;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            m_PopScreenComponent.Execute();
    }
}

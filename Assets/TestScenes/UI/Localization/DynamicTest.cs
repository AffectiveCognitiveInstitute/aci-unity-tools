using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aci.Unity.Events;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DynamicTest : MonoBehaviour
                         , IAciEventHandler<LocalizationChangedArgs>

{
    public int[] values;

    private IAciEventManager eventMan;
    private DynamicTextMesh dynamicText;

    [Inject]
    public IAciEventManager eventManager
    {
        get { return eventMan; }
        set
        {
            if(eventMan != null)
                UnregisterFromEvents();
            eventMan = value;
            RegisterForEvents();
        }
    }

    /// <inheritdoc />
    public void RegisterForEvents()
    {
        eventMan.AddHandler<LocalizationChangedArgs>(this);
    }

    /// <inheritdoc />
    public void UnregisterFromEvents()
    {
        eventMan.RemoveHandler<LocalizationChangedArgs>(this);
    }

    /// <inheritdoc />
    public async void OnEvent(LocalizationChangedArgs arg)
    {
        dynamicText = GetComponent<DynamicTextMesh>();
        if (dynamicText == null)
            return;
        await Task.Delay(2000);
        dynamicText.UpdateDynamicContent(values[0], values[1], values[2]);
    }
}

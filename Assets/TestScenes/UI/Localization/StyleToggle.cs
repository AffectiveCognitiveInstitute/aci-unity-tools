using System.Collections;
using System.Collections.Generic;
using Aci.Unity.Events;
using Aci.Unity.UI.Localization;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StyleToggle : MonoBehaviour
{
    public Text buttonText;

    [Inject]
    private AciEventBroker _broker;

    [Inject]
    private ILocalizationManager _locMan;

    List<string> replacementPatterns = new List<string>()
    {
        "{{(.+?)}}",
        "##(.+?)##",
        "<<(.+?)>>"
    };

    [Inject]
    private ILocalizationManager locMan
    {
        get { return _locMan; }
        set
        {
            _locMan = value;
            buttonText.text = _locMan.replacementPattern;
        }
    }

    public void Toggle()
    {
        int index = (replacementPatterns.IndexOf(_locMan.replacementPattern) + 1) % replacementPatterns.Count;
        _locMan.replacementPattern = replacementPatterns[index];
        buttonText.text = _locMan.replacementPattern;
        _broker.localizationEvent.Invoke(null, null);
    }
}

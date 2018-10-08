using System.Collections;
using System.Collections.Generic;
using Aci.Unity.UI.Localization;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LanguageToggle : MonoBehaviour
{
    public Text buttonText;

    private ILocalizationManager _locMan;

    [Inject]
    private ILocalizationManager locMan
    {
        get { return _locMan; }
        set
        {
            _locMan = value;
            buttonText.text = _locMan.currentLocalizationDecorator;
        }
    }

    public void Toggle()
    {
        List<string> languages = _locMan.GetCapabilities();
        int index = (languages.IndexOf(_locMan.currentLocalization) + 1) % languages.Count;
        _locMan.currentLocalization = languages[index];
        buttonText.text = _locMan.currentLocalizationDecorator;
    }
}

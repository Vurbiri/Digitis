using UnityEngine;
using UnityEngine.UI;

public class LanguageSwitch : ToggleGroup
{
    [SerializeField] private LanguageItem _langPrefab;
    [Space]
    [SerializeField] private bool _isSave = false;

    protected override void Awake()
    {
        base.Awake();
        
        allowSwitchOff = false;
        foreach (var item in Localization.Instance.Languages)
            Instantiate(_langPrefab, transform).Setup(item, this, _isSave);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class LanguageSwatch : ToggleGroup
{
    [SerializeField] private LanguageItem langPrefab;
    [Space]
    [SerializeField] private bool _isSave = false;

    protected override void Awake()
    {
        base.Awake();
        
        this.allowSwitchOff = false;
        foreach (var item in Localization.Instance.Languages)
            Instantiate(langPrefab, transform).Setup(item, this, _isSave);
    }
}

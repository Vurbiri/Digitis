using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class LanguageItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;

    private bool _isSave;
    private int _id = -1;
    private Localization _localization;
    private Settings _settings;
    private Toggle _thisToggle;

    private void Awake()
    {
        _localization = Localization.InstanceF;
        _settings = Settings.InstanceF;
        _thisToggle = GetComponent<Toggle>();
    }

    public void Setup(Localization.LanguageType languageType, ToggleGroup toggleGroup, bool isSave) 
    {

        _icon.sprite = languageType.Sprite;
        _name.text = languageType.Name;
        _id = languageType.Id;
        _isSave = isSave;

        _thisToggle.isOn = _localization.CurrentIdLang == _id;
        _thisToggle.group = toggleGroup;
        _thisToggle.onValueChanged.AddListener(OnSelect);
        _localization.EventSwitchLanguage += OnSwitchLanguage;
    }

    private void OnSelect(bool isOn)
    {
        if(!isOn) return;

        _localization.SwitchLanguage(_id);
        if(_isSave) _settings.Save();
    }

    private void OnSwitchLanguage()
    {
        _thisToggle.onValueChanged.RemoveListener(OnSelect);
        _thisToggle.isOn = _localization.CurrentIdLang == _id;
        _thisToggle.onValueChanged.AddListener(OnSelect);
    }

    private void OnDestroy()
    {
        if(Localization.Instance != null)
            Localization.Instance.EventSwitchLanguage -= OnSwitchLanguage;
    }
}

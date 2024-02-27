using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageItem : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Animator _animator;
    [Space]
    [SerializeField] private RuntimeAnimatorController _controllerClockwise;
    [SerializeField] private RuntimeAnimatorController _controllerCounterclockwise;

    private bool _isSave;
    private int _id = -1;
    private Localization _localization;
    private SettingsGame _settings;
    

    private void Awake()
    {
        _localization = Localization.InstanceF;
        _settings = SettingsGame.InstanceF;
    }

    public void Setup(Localization.LanguageType languageType, ToggleGroup toggleGroup, bool isSave) 
    {

        _icon.sprite = languageType.Sprite;
        _name.text = languageType.Name;
        _id = languageType.Id;
        _isSave = isSave;

        _animator.runtimeAnimatorController = _id % 2 == 0 ? _controllerCounterclockwise : _controllerClockwise;

        _toggle.isOn = _localization.CurrentIdLang == _id;
        _toggle.group = toggleGroup;
        _toggle.onValueChanged.AddListener(OnSelect);
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
        _toggle.onValueChanged.RemoveListener(OnSelect);
        _toggle.isOn = _localization.CurrentIdLang == _id;
        _toggle.onValueChanged.AddListener(OnSelect);
    }

    private void OnDestroy()
    {
        if(Localization.Instance != null)
            Localization.Instance.EventSwitchLanguage -= OnSwitchLanguage;
    }
}

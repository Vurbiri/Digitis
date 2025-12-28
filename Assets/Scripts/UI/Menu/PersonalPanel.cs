using TMPro;
using UnityEngine;

public class PersonalPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;

    public string Name => _name.text;

    private void Start()
    {
        _name.text = Localization.Instance.GetText("Guest");

        Localization.Instance.EventSwitchLanguage += SetLocalizationName;
    }

    private void SetLocalizationName()
    {
        _name.text = Localization.Instance.GetText("Guest");
    }

    private void OnDestroy()
    {
        if (Localization.Instance != null)
            Localization.Instance.EventSwitchLanguage -= SetLocalizationName;
    }
}

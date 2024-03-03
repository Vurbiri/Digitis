using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonalPanel : MonoBehaviour
{
    [SerializeField] private string _keyGuestName = "Guest";
    [SerializeField] private string _keyAnonymName = "Anonym";
    [Space]
    [SerializeField] private Texture _avatarGuest;
    [SerializeField] private Texture _avatarAnonym;
    [SerializeField] private AvatarSize _avatarSize = AvatarSize.Medium;
    [Space]
    [SerializeField] private RawImage _avatar;
    [SerializeField] private TMP_Text _name;

    public Texture Avatar => _avatar.texture;
    public string Name => _name.text;

    private YandexSDK _ysdk;
    private Localization _localization;

    private void Start()
    {
        _ysdk = YandexSDK.InstanceF;
        _localization = Localization.InstanceF;

        Personalization().Forget();

        async UniTaskVoid Personalization()
        {
            if (_ysdk.IsLogOn)
            {
                string name = _ysdk.PlayerName;
                if (!string.IsNullOrEmpty(name))
                    _name.text = name;
                else
                    _name.text = _localization.GetText(_keyAnonymName);

                var texture = await _ysdk.GetPlayerAvatar(_avatarSize);
                if (texture.Result)
                    _avatar.texture = texture.Value;
                else
                    _avatar.texture = _avatarAnonym;

            }
            else
            {
                _name.text = _localization.GetText(_keyGuestName);
                _avatar.texture = _avatarGuest;
            }
            _localization.EventSwitchLanguage += SetLocalizationName;
        }
    }

    private void SetLocalizationName()
    {
        if (_ysdk.IsLogOn)
        {
            string name = _ysdk.PlayerName;
            if (!string.IsNullOrEmpty(name))
                _name.text = name;
            else
                _name.text = _localization.GetText(_keyAnonymName);
        }
        else
        {
            _name.text = _localization.GetText(_keyGuestName);
        }
    }

    private void OnDestroy()
    {
        if (Localization.Instance != null)
            _localization.EventSwitchLanguage -= SetLocalizationName;
    }
}

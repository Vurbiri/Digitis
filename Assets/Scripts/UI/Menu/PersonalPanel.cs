using System.Collections;
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
    [SerializeField] private Texture _avatarError;
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

        StartCoroutine(Personalization());

        IEnumerator Personalization(bool isFirst = true)
        {
            bool isRepeat = false;

            if (_ysdk.IsLogOn)
            {
                string name = _ysdk.PlayerName;
                if (!(isRepeat = string.IsNullOrEmpty(name)))
                    _name.text = name;
                else
                    _name.text = _localization.GetText(_keyAnonymName);

                if (isRepeat)
                    _avatar.texture = _avatarAnonym;
                else
                    StartCoroutine(Storage.TryLoadTextureWeb(_avatar, _avatarError, _ysdk.GetPlayerAvatarURL(_avatarSize)));
            }
            else
            {
                _name.text = _localization.GetText(_keyGuestName);
                _avatar.texture = _avatarGuest;
            }

            if (isFirst && isRepeat)
            {
                yield return new WaitForSecondsRealtime(3f);
                StartCoroutine(Personalization(false));
                yield break;
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

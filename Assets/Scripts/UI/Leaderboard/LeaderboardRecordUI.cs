using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LeaderboardRecordUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _rankText;
    [SerializeField] private RawImage _avatarRawImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Color _colorText;
    [Space]
    [SerializeField] private string _keyAnonymName = "Anonym";
    [SerializeField] private Texture _avatarAnonym;
    [SerializeField] private Texture _avatarError;
    [Space]
    [SerializeField] private Image _fonImage;
    [SerializeField] private Image _maskAvatar;
    [SerializeField] private Color _fonNormal;
    [SerializeField] private Color _fonPlayer;
    [Space]
    [SerializeField] private TypeRecord _normal;
    [Space]
    [SerializeField] private TypeRecord[] _ranks;

    public void Setup(LeaderboardRecord record, bool isPlayer = false)
    {
        bool isNameEmpty;

        SetText(_rankText, record.Rank.ToString());
        SetText(_scoreText, record.Score.ToString());
        if (!(isNameEmpty = string.IsNullOrEmpty(record.Name)))
            SetText(_nameText, record.Name);
        else
            SetText(_nameText, Localization.Instance.GetText(_keyAnonymName));

        if (isNameEmpty)
            _avatarRawImage.texture = _avatarAnonym;
        else
            StartCoroutine(Storage.TryLoadTextureWeb(_avatarRawImage, _avatarError, record.AvatarURL));

        if (isPlayer)
            SetFonColor(_fonPlayer);
        else
            SetFonColor(_fonNormal);

        if (record.Rank <= _ranks.Length)
            SetRecord(_ranks[record.Rank - 1]);
        else
            SetRecord(_normal);

        gameObject.SetActive(true);

        #region Local Functions
        void SetText(TMP_Text text, string str)
        {
            text.text = str;
            text.color = _colorText;
        }

        void SetFonColor(Color color)
        {
            _fonImage.color = color;
            _maskAvatar.color = color;
        }

        void SetRecord(TypeRecord type)
        {
            Image thisImage = GetComponent<Image>();

            thisImage.color = type.Color;
            thisImage.rectTransform.sizeDelta = _fonImage.rectTransform.sizeDelta + Vector2.one * type.OffsetDistance;
        }
        #endregion
    }

    [System.Serializable]
    private class TypeRecord
    {
        [SerializeField] private Color _color;
        [SerializeField] private float _offsetSize;

        public Color Color => _color;
        public float OffsetDistance => _offsetSize;
    }
}

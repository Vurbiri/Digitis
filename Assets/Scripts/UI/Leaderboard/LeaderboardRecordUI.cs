using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
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
        SetText(_rankText, record.Rank.ToString());
        SetText(_scoreText, record.Score.ToString());
        if(!string.IsNullOrEmpty(record.Name))
            SetText(_nameText, record.Name);
        else
            SetText(_nameText, Localization.Instance.GetText(_keyAnonymName));

        SetAvatar(record.AvatarURL).Forget();

        if(isPlayer)
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

        async UniTaskVoid SetAvatar(string url)
        {
            var texture = await Storage.TryLoadTextureWeb(url);
            if (texture.Result)
                _avatarRawImage.texture = texture.Value;
            else
                _avatarRawImage.texture = _avatarAnonym;

        }

        void SetFonColor(Color color)
        {
            _fonImage.color = color;
            _maskAvatar.color = color;
        }

        void SetRecord(TypeRecord type)
        {
            Outline thisOutline = GetComponent<Outline>();

            thisOutline.effectColor = type.Color;
            thisOutline.effectDistance = Vector2.one * type.OffsetDistance;
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

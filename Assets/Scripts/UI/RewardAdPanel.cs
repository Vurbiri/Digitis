using UnityEngine;
using UnityEngine.UI;

public class RewardAdPanel : MonoBehaviour
{
    [SerializeField] private TextFormatLocalization _label;
    [SerializeField] private Toggle _toggle;
    [Space]
    [SerializeField] private Image _iconButton;
    [SerializeField] private Sprite _spriteEmpty;
    [SerializeField] private Sprite _spriteAd;

    public bool IsOn => _toggle.isOn && _isShow; 
    
    public bool Show 
    { 
        set 
        {
            if (!_isShow)
                return;

            gameObject.SetActive(value);
            _iconButton.sprite = value && _toggle.isOn ? _spriteAd : _spriteEmpty;
        } 
    }
    
    private bool _isShow = false;

    public void Initialize()
    {
        _isShow = DataGame.Instance.CountBonusBombs == 0;
        _label.Setup(YMoney.Instance.BombsRewardedAd);
        _toggle.onValueChanged.AddListener((b) => _iconButton.sprite = b ? _spriteAd : _spriteEmpty);
        _toggle.isOn = false;
        gameObject.SetActive(_isShow);
    }
}

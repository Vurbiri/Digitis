using UnityEngine;
using UnityEngine.UI;

public class RewardAdPanel : MonoBehaviour
{
    [SerializeField] private TextFormatLocalization _label;
    [SerializeField] private Toggle _toggle;

    public bool IsOn => _toggle.isOn && _isShow; 
    
    public bool Show { set { if (_isShow) gameObject.SetActive(value && _isShow); } }
    
    private bool _isShow = false;

    public void Initialize()
    {
        _isShow = DataGame.Instance.CountBonusBombs == 0;
        _label.Setup(YMoney.Instance.BombsRewardedAd);
        _toggle.isOn = false;
        gameObject.SetActive(_isShow);
    }
}

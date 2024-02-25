using UnityEngine;

public class RewardAdPanel : MonoBehaviour
{
    [SerializeField] private ToggleFullInteractable _toggle;

    public bool IsOn => _toggle.IsOn && _isShow; 
    
    public bool Interactable
    { 
        set
        {
            if (!_isShow) return;

            gameObject.SetActive(value);
            _toggle.Interactable = value;
            //_toggle.IsOn = value ? _toggle.IsOn : false;
        }
    }
    private bool _isShow = false;

    public void Initialize()
    {
        _isShow = DataGame.Instance.CountBonusBombs == 0;
        _toggle.IsOn = false;
        gameObject.SetActive(_isShow);
    }
}

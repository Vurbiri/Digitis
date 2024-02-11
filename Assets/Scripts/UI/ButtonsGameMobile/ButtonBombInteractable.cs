using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBombInteractable : MonoBehaviour
{
    [SerializeField] private Game _game;
    [Space]
    [SerializeField] private TMP_Text _countBomb;
    [SerializeField] private ButtonClick _buttonClick;
    [SerializeField] private Image[] _images;
    [SerializeField] private Animator[] _animators;
    [Space]
    [SerializeField] private Color _normalColor = new(0.03921569f, 0.8705882f, 1f, 1f);
    [SerializeField] private Color _disabledColor = new(0.03921569f, 0.8705882f, 1f, 0.5f);

    private bool _enabled = true;

    private void Start()
    {
        SetStatus(_game.CountBombs);
        _game.EventChangeCountBombs += SetStatus;

        void SetStatus(int countBomb)
        {
            _countBomb.text = countBomb.ToString();

            if (countBomb <= 0)
            {
                if (_enabled)
                    OnOff(false, _disabledColor);
            }
            else
            {
                if (!_enabled)
                    OnOff(true, _normalColor);
            }

            void OnOff(bool enabled, Color color)
            {
                _enabled = enabled;
                _buttonClick.enabled = enabled;
                _countBomb.color = color;
                foreach (var animator in _animators)
                    animator.enabled = enabled;
                foreach (var image in _images)
                    image.color = color;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class CustomTargetGraphic : MonoBehaviour
{
    [SerializeField] private RectTransform _thisRectTransform;
    [Space]
    [SerializeField] private Image _icon;
    [SerializeField] private Color _iconNormalColor = new(10, 222, 255, 255);
    [SerializeField] private Color _iconDisabledColor = new(10, 222, 255, 100);
    [SerializeField] private bool _isIconResize = false;
    [Space]
    [SerializeField] private Image[] _targetImages;
    [SerializeField] private Animator[] _targetAnimators;
    [Space]
    [SerializeField] private StateBlock _stateBlock = StateBlock.defaultStateBlock;

    private RectTransform _iconRectTransform;

    private Vector2 _size;
    private Vector2 _sizeIcon;

    private bool _isInteractable = true;

    public void Initialize(bool isInteractable)
    {
        if (_thisRectTransform == null)
            _thisRectTransform = GetComponent<RectTransform>();

        _size = _thisRectTransform.sizeDelta;

        _iconRectTransform = _icon.rectTransform;
        _sizeIcon = _iconRectTransform.sizeDelta;

        if (isInteractable)
            SetNormalState();
        else
            SetDisabledState();
    }

    public void SetNormalState()
    {
        SetState(_stateBlock.normal, true);
    }

    public void SetHighlightedState()
    {
        SetState(_stateBlock.highlighted, true);
    }
    public void SetPressedState()
    {
        SetState(_stateBlock.pressed, true);
    }

    public void SetSelectedState()
    {
        SetState(_stateBlock.selected, true);
    }

    public void SetDisabledState()
    {
        SetState(_stateBlock.disabled, false);
    }

    private void SetState(State targetState, bool isInteractable)
    {
        SetColor(targetState.color);
        SetSpeed(targetState.speed);
        _thisRectTransform.sizeDelta = _size * targetState.scale;
        if (_isIconResize)
            _iconRectTransform.sizeDelta = _sizeIcon * targetState.scale;

        if (_isInteractable != isInteractable)
        {
            _isInteractable = isInteractable;
            _icon.color = isInteractable ? _iconNormalColor : _iconDisabledColor;
        }

        #region Local Functions
        void SetColor(Color targetColor)
        {
            foreach (var image in _targetImages)
                image.color = targetColor;
        }
        void SetSpeed(float speed)
        {
            foreach (var anim in _targetAnimators)
                anim.speed = speed;
        }
        #endregion
    }

    [System.Serializable]
    private struct StateBlock
    {
        public State normal;
        public State highlighted;
        public State pressed;
        public State selected;
        public State disabled;

        public static StateBlock defaultStateBlock;
        static StateBlock()
        {
            defaultStateBlock = new StateBlock()
            {
                normal = new(1f, new(10, 222, 255, 255), 1f),
                highlighted = new(4f, new(60, 160, 255, 255), 1.1f),
                pressed = new(0.5f, new(60, 160, 255, 122), 1.025f),
                selected = new(2f, new(140, 140, 225, 255), 1.05f),
                disabled = new(0f, new(10, 222, 255, 100), 1f),
            };
        }
    }

    [System.Serializable]
    private struct State
    {
        public float speed;
        public Color color;
        public float scale;

        public State(float speed, Color32 color, float scale)
        {
            this.speed = speed;
            this.color = color;
            this.scale = scale;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class CustomButtonTargetGraphic : MonoBehaviour
{
    [SerializeField] private Image _targetIcon;
    [SerializeField] private Image[] _targetImages;
    [SerializeField] private Animator[] _targetAnimators;
    [Space]
    [SerializeField] private Vector2 _size;
    [Space]
    [SerializeField] private StateBlock _stateBlock = StateBlock.defaultStateBlock;

    private RectTransform _thisRectTransform;
    private bool _isInteractable;

    public void Initialize(bool isInteractable)
    {
        _thisRectTransform ??= GetComponent<RectTransform>();
        _size = _thisRectTransform.sizeDelta;

        _isInteractable = isInteractable;
        _targetIcon.color = isInteractable ? _stateBlock.normal.color : _stateBlock.disabled.color;
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

        if(_isInteractable != isInteractable)
        {
            _isInteractable = isInteractable;
            _targetIcon.color = isInteractable ? _stateBlock.normal.color : _stateBlock.disabled.color;
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
                highlighted = new(4f, new(60, 160, 255, 255), 1.125f),
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

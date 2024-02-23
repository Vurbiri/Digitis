public class InputDesktopController : AInputController
{
    public override bool ControlEnable
    {
        get => base.ControlEnable;
        set
        { 
            base.ControlEnable = value;
            foreach (var button in _buttonsInteractable)
                button.IsInteractable = value;
        }
    }

    private IButtonInteractable[] _buttonsInteractable;

    protected override void Awake()
    {
        _buttonsInteractable = new IButtonInteractable[]
           {
                _buttonLeft as IButtonInteractable,
                _buttonRight as IButtonInteractable,
                _buttonDown as IButtonInteractable,
                _buttonRotation as IButtonInteractable,
                _buttonBomb as IButtonInteractable,
                _buttonPause as IButtonInteractable
           };

        foreach (var button in _buttonsInteractable)
            button.Initialize(ControlEnable);

        base.Awake();
    }
}

using NaughtyAttributes;
using System;
using UnityEngine;

public class InputDesktopController : AInputController
{
    [Space]
    [SerializeField, InputAxis] private string _gamepadAxis;
    [SerializeField, InputAxis] private string _mouse;
    [SerializeField] private KeyCode[] _gamepadButtons = new KeyCode[] { KeyCode.JoystickButton0, KeyCode.JoystickButton1, KeyCode.JoystickButton2, KeyCode.JoystickButton3, KeyCode.JoystickButton4, KeyCode.JoystickButton5, KeyCode.JoystickButton6, KeyCode.JoystickButton7, KeyCode.JoystickButton8, KeyCode.JoystickButton9, KeyCode.JoystickButton10 };

    private Device _device = Device.MouseKeyboard;
    public Device CurrentDevice
    {
        get => _device;
        private set 
        {
            if (_device == value) return;

            _device = value;
            EventSwitchingDevice?.Invoke(value);
        }
    }

    public event Action<Device> EventSwitchingDevice;

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

    private void Update() 
    {
        if (Input.anyKeyDown)
        {
            foreach(var button in _gamepadButtons)
            {
                if (Input.GetKey(button))
                {
                    CurrentDevice = Device.Gamepad;
                    return;
                }
            }

            CurrentDevice = Device.MouseKeyboard;
            return;
        }

        if (Input.GetAxis(_mouse) != 0)
        {
            CurrentDevice = Device.MouseKeyboard;
            return;
        }

        if (Input.GetAxis(_gamepadAxis) != 0)
        {
            CurrentDevice = Device.Gamepad;
            return;
        }
    }
}

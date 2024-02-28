using UnityEngine;

public class HelpControlPanel : MonoBehaviour
{
    [SerializeField] private InputDesktopController _inputDesktopController;
    [Space]
    [SerializeField] private GameObject _keyboardPanel;
    [SerializeField] private GameObject _gamepadPanel;


    private void Awake()
    {
        OnSwitchingDevice(_inputDesktopController.CurrentDevice);
        _inputDesktopController.EventSwitchingDevice += OnSwitchingDevice;

        void OnSwitchingDevice(Device device)
        {
            bool isGamepad = device == Device.Gamepad;

            _keyboardPanel.SetActive(!isGamepad);
            _gamepadPanel.SetActive(isGamepad);
        }
    }
}

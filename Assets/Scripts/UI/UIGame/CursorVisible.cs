using UnityEngine;

public class CursorVisible : MonoBehaviour
{
    [SerializeField] private InputDesktopController _inputController;

    private void Start()
    {
        Cursor.visible = _inputController.CurrentDevice == Device.MouseKeyboard;
        _inputController.EventSwitchingDevice += (d) => Cursor.visible = d == Device.MouseKeyboard;
    }
}

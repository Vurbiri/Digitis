using UnityEngine;
using UnityEngine.UI;

public class MenuDesktop : MonoBehaviour
{
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private Selectable _startButton;
    [Space]
    [SerializeField] private GameObject _helpMenu;
    [SerializeField] private Selectable _helpButton;

    private void Awake()
    {
        bool isFirst = SettingsGame.Instance.IsFirstStart;

        _startMenu.SetActive(!isFirst);
        _helpMenu.SetActive(isFirst);

        if (isFirst)
            _helpButton.Select();
        else
            _startButton.Select();
    }
}

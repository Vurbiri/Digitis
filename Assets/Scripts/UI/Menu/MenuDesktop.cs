using UnityEngine;
using UnityEngine.UI;

public class MenuDesktop : MonoBehaviour
{
    [SerializeField] private GameObject _helpMenu;
    [SerializeField] private Selectable _helpButton;

    private void Awake()
    {
        if (!SettingsGame.Instance.IsFirstStart)
            return;

        _helpMenu.SetActive(true);
        _helpButton.Select();
    }
}

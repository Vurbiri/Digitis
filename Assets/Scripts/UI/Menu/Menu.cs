using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _helpMenu;
    
    private void Awake()
    {
        bool isFirst = SettingsGame.Instance.IsFirstStart;

        _mainMenu.SetActive(!isFirst);
        _helpMenu.SetActive(isFirst);
    }
}

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsControllerMobile : ButtonsControllerDesktop
{
    [Space]
    [SerializeField] private ButtonClick _buttonToMenu;
    [Scene, Space]
    [SerializeField] private int _sceneMenu = 1;

    protected override void Awake()
    {
        base.Awake();

        _buttonToMenu.EventButtonClick += () => SceneManager.LoadSceneAsync(_sceneMenu);
    }
}

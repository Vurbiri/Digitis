using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsControllerMobile : MonoBehaviour
{
    [SerializeField] private Game _game;
    [Space]
    [SerializeField] private GameObject _panelGame;
    [SerializeField] private GameObject _panelGameOver;
    [Space]
    [SerializeField] private ButtonClick _buttonToMenu;
    [Scene, Space]
    [SerializeField] private int _sceneMenu = 1;



    private void Awake()
    {
        _panelGame.SetActive(true);
        _panelGameOver.SetActive(false);

        _buttonToMenu.EventButtonClick += () => SceneManager.LoadSceneAsync(_sceneMenu);

        _game.EventGameOver += () => { _panelGame.SetActive(false); _panelGameOver.SetActive(true); };
    }
}

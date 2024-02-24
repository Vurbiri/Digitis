using UnityEngine;

public class ButtonsControllerDesktop : MonoBehaviour
{
    [SerializeField] private Game _game;
    [Space]
    [SerializeField] private GameObject _panelGame;
    [SerializeField] private GameObject _panelGameOver;

    protected virtual void Awake()
    {
        SetActiveButtonsGame(true);
        _game.EventLeaderboard += SetActiveButtonsGame;

        void SetActiveButtonsGame(bool active)
        {
            _panelGame.SetActive(active);
            _panelGameOver.SetActive(!active);
        }
    }
}

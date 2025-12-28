using UnityEngine;

public class ButtonsControllerDesktop : MonoBehaviour
{
	[SerializeField] private Game _game;
	[Space]
	[SerializeField] private GameObject _panelGame;
	[SerializeField] private GameObject _panelGameOver;

	protected virtual void Awake()
	{
		_panelGame.SetActive(true);
		_panelGameOver.SetActive(false);

		_game.EventGameOver += SetActiveButtonsGame;

		void SetActiveButtonsGame()
		{
			_panelGame.SetActive(false);
			_panelGameOver.SetActive(true);
		}
	}
}

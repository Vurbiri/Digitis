using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuDesktop : MonoBehaviour
{
    [Space]
    [SerializeField] private Game _game;
    [SerializeField] private InputDesktopController _inputController;
    [Space]
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _cullingMaskGame;
    [SerializeField] private LayerMask _cullingMaskPause;
    [Space]
    [SerializeField] private GameObject _pauseUI;
    [Space]
    [SerializeField] private Title _title;
    [SerializeField] private string _key = "Digitis";
    [Space]
    [SerializeField] private GameObject _leaderboardMenu;
    [SerializeField] private Selectable _leaderboardButton;
    [SerializeField] private LeaderboardUI _leaderboardUI;
    [Space]
    [SerializeField] private GameObject _buttonPlay;
    [SerializeField] private GameObject _buttonToMenu;
    


    private void Awake()
    {
        _pauseUI.SetActive(false);
        SetActiveButtons(true);

        _inputController.EventPause += () => SetPause(true);
        _inputController.EventUnPause += () => SetPause(false);
        _game.EventLeaderboard += () => OnLeaderboardAsync().Forget();


        void SetPause(bool pause)
        {
            SetCullingMask(pause);
            _pauseUI.SetActive(pause);
        }

        async UniTaskVoid OnLeaderboardAsync()
        {
            _title.Key = _key;
            await _leaderboardUI.TryReward(true);

            _leaderboardMenu.SetActive(true);
            SetActiveButtons(false);

            SetCullingMask(true);
            _pauseUI.SetActive(true);
            _leaderboardButton.Select();
        }

        void SetActiveButtons(bool active)
        {
            _buttonPlay.SetActive(active);
            _buttonToMenu.SetActive(!active);
        }

        void SetCullingMask(bool pause)
        {
            _camera.cullingMask = pause ? _cullingMaskPause : _cullingMaskGame;
        }
    }
}

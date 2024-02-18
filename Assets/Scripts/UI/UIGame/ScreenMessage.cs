using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScreenMessage : MonoBehaviour
{
    [SerializeField] private Game _game;
    [Space]
    [SerializeField] private AudioSource _audioSource;
    [Space]
    [SerializeField] private AudioClip[] _clipsCountdown;
    [SerializeField] private AudioClip _clipStart;
    [SerializeField] private AudioClip _clipLevelUp;
    [SerializeField] private AudioClip _gameOver;

    private TMP_Text _thisText;

    private Localization _localization;

    private readonly WaitForSecondsRealtime pauseOneSecond = new(1f);
    private const string KEY_START = "Start";
    private const string KEY_LEVEL = "Level";
    private const string KEY_GAMEOVER = "GameOver";

    private void Awake()
    {
        _localization = Localization.Instance;
        _thisText = GetComponent<TMP_Text>();
        _thisText.text = string.Empty;
        gameObject.SetActive(false);

        _game.EventCountdown += () => { gameObject.SetActive(true); StartCoroutine(CountdownCoroutine()); };
        _game.EventStartGame += () => { _thisText.text = string.Empty; gameObject.SetActive(false); };
        _game.EventGameOver += OnGameOver;
        DataGame.Instance.EventChangeLevel += LevelUp;

        #region Local Functions
        IEnumerator CountdownCoroutine()
        {
            _thisText.text = string.Empty;
            yield return pauseOneSecond;
            for (int i = _clipsCountdown.Length - 1; i >= 0; i--)
            {
                _audioSource.PlayOneShot(_clipsCountdown[i]);
                _thisText.text = (i + 1).ToString();
                yield return pauseOneSecond;
            }
            _audioSource.PlayOneShot(_clipStart);
            _thisText.text = _localization.GetText(KEY_START) + "!";
        }

        void OnGameOver()
        {
            StopCoroutine(LevelUpCoroutine(0));
            _thisText.text = _localization.GetText(KEY_GAMEOVER);
            gameObject.SetActive(true); 
            _audioSource.PlayOneShot(_gameOver);
        }
        #endregion
    }

    private void LevelUp(int level)
    {
        gameObject.SetActive(true); 
        StartCoroutine(LevelUpCoroutine(level));
    }
    IEnumerator LevelUpCoroutine(int level)
    {
        _audioSource.PlayOneShot(_clipLevelUp);
        _thisText.text = _localization.GetText(KEY_LEVEL) + " " + level.ToString();
        yield return pauseOneSecond;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if(DataGame.Instance != null)
            DataGame.Instance.EventChangeLevel -= LevelUp;
    }
}

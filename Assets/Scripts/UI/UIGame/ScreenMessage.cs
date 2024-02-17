using System.Collections;
using TMPro;
using UnityEditor.Localization.Editor;
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
        SetActive(false);

        _game.EventCountdown += () => { SetActive(true); StartCoroutine(CountdownCoroutine()); };
        _game.EventStartGame += () => { _thisText.text = string.Empty; SetActive(false); };
        _game.EventGameOver += () => { _thisText.text = _localization.GetText(KEY_GAMEOVER); SetActive(true); };
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
        #endregion
    }

    private void LevelUp(int level)
    {
        SetActive(true); 
        StartCoroutine(LevelUpCoroutine());

        #region Local Functions
        IEnumerator LevelUpCoroutine()
        {
            _audioSource.PlayOneShot(_clipLevelUp);
            _thisText.text = _localization.GetText(KEY_LEVEL) + " " + level.ToString();
            yield return pauseOneSecond;

            SetActive(false);
        }
        #endregion
    }

    private void SetActive(bool value) => gameObject.SetActive(value);

    private void OnDestroy()
    {
        if(DataGame.Instance != null)
            DataGame.Instance.EventChangeLevel -= LevelUp;
    }
}

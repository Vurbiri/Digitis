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

    private TMP_Text _thisText;

    private readonly WaitForSecondsRealtime pauseOneSecond = new(1f);
    private const string KEY_START = "Start";
    private const string KEY_LEVEL = "Level";

    private void Awake()
    {
        _thisText = GetComponent<TMP_Text>();
        _thisText.text = string.Empty;
        SetActive(false);

        _game.EventCountdown += () => { SetActive(true); StartCoroutine(CountdownCoroutine()); };
        DataGame.Instance.EventChangeLevel += LevelUp;

        #region Local Functions
        IEnumerator CountdownCoroutine()
        {
            _thisText.text = string.Empty;
            yield return pauseOneSecond;
            for (int i = _clipsCountdown.Length - 1; i >= 0; i--)
            {
                _audioSource.PlayOneShot(_clipsCountdown[i]);
                _thisText.text = i.ToString();
                yield return pauseOneSecond;
            }
            _audioSource.PlayOneShot(_clipStart);
            _thisText.text = Localization.Instance.GetText(KEY_START) + "!";

            yield return pauseOneSecond;

            SetActive(false);
        }
        #endregion
    }

    private void LevelUp(string level)
    {
        SetActive(true); 
        StartCoroutine(LevelUpCoroutine());

        #region Local Functions
        IEnumerator LevelUpCoroutine()
        {
            _audioSource.PlayOneShot(_clipLevelUp);
            _thisText.text = Localization.Instance.GetText(KEY_LEVEL) + " " + level;
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

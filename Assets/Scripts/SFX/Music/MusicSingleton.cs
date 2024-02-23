using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicSingleton : ASingleton<MusicSingleton>
{
    [Space]
    [SerializeField] private AudioClip _musicMenu;
    [SerializeField] private AudioClip _musicGame;
    [Space]
    [SerializeField] private float _volume = 1f;
    [Space]
    [SerializeField] private float _timeSwitching = 0.25f;

    public float Pitch { get => _thisAudio.pitch; set => _thisAudio.pitch = value; }

    private AudioSource _thisAudio;
    private Coroutine _coroutine;

    private void Start()
    {
        _thisAudio = GetComponent<AudioSource>();
        _thisAudio.volume = _volume;

        _musicMenu.LoadAudioData();
        _musicGame.LoadAudioData();
    }

    public void Switch(Music music, float pitch = 1f)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(SwitchCoroutine());

        #region Local Function
        IEnumerator SwitchCoroutine()
        {
            float volume = _thisAudio.volume;
            float speed = volume / _timeSwitching;

            while (_thisAudio.volume > 0) 
            { 
                yield return null;
                _thisAudio.volume -= speed * Time.unscaledDeltaTime;
            }
            _thisAudio.volume = 0f;

            speed = _volume / _timeSwitching;
            Play(music, pitch);

            while (_thisAudio.volume < _volume)
            {
                yield return null;
                _thisAudio.volume += speed * Time.unscaledDeltaTime;
            }
            _thisAudio.volume = _volume;

            _coroutine = null;
        }
        #endregion
    }

    public void Play(Music music, float pitch = 1f)
    {
        Stop();
        _thisAudio.clip = music switch { Music.Game => _musicGame, Music.Menu => _musicMenu, _=> null };
        _thisAudio.pitch = pitch;
        _thisAudio.Play();
    }

    public void Stop()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
            _thisAudio.volume = _volume;
        }
        _thisAudio.Stop();
    }
}

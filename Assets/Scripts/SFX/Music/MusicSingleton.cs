using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicSingleton : ASingleton<MusicSingleton>
{
    [Space]
    [SerializeField] private AudioClip _musicMenu;
    [SerializeField] private AudioClip _musicGame;

    public float Pitch { get => _thisAudio.pitch; set => _thisAudio.pitch = value; }

    protected AudioSource _thisAudio;

    protected override void Awake()
    {
        base.Awake();

        _thisAudio = GetComponent<AudioSource>();

        _musicMenu.LoadAudioData();
        _musicGame.LoadAudioData();
    }

    public void MenuPlay() => PlayClip(_musicMenu);
    public void GamePlay(float pitch) => PlayClip(_musicGame, pitch);

    public void Stop() => _thisAudio.Stop();

    protected void PlayClip(AudioClip clip, float pitch = 1f)
    {
        _thisAudio.Stop();
        _thisAudio.clip = clip;
        _thisAudio.pitch = pitch;
        _thisAudio.Play();
    }
}

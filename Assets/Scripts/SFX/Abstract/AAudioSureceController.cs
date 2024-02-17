using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AAudioSourceController : MonoBehaviour
{
    protected AudioSource _thisAudio;
    protected virtual void Awake()
    {
        _thisAudio = GetComponent<AudioSource>();
    }

    protected void Play() => _thisAudio.Play();
    protected void Stop() => _thisAudio.Stop();

    protected void PlayOneShot(AudioClip clip, float volume = 1f) => _thisAudio.PlayOneShot(clip, volume);
    protected void PlayClip(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        _thisAudio.Stop();
        _thisAudio.clip = clip;
        _thisAudio.volume = volume;
        _thisAudio.pitch = pitch;
        _thisAudio.Play();
    }
}

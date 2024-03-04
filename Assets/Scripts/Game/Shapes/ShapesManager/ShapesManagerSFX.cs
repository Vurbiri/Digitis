using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShapesManagerSFX : MonoBehaviour
{
    [SerializeField] private AudioClip _clipMove;
    [SerializeField] private AudioClip _clipRotate;
    [SerializeField] private AudioClip _clipDown;
    [SerializeField] private AudioClip _clipSpawn;
    [SerializeField] private AudioClip _clipToBomb;
    [SerializeField] private AudioClip _clipError;
    [SerializeField] private AudioClip _clipFixed;

    protected AudioSource _thisAudio;
    protected virtual void Awake()
    {
        _thisAudio = GetComponent<AudioSource>();
    }

    public void PlayMove() => _thisAudio.PlayOneShot(_clipMove);
    public void PlayRotate() => _thisAudio.PlayOneShot(_clipRotate);
    public void PlayDown() => _thisAudio.PlayOneShot(_clipDown);
    public void PlaySpawn() => _thisAudio.PlayOneShot(_clipSpawn);
    public void PlayError() => _thisAudio.PlayOneShot(_clipError);
    public void PlayToBomb() => _thisAudio.PlayOneShot(_clipToBomb);
    public void PlayFixed() => _thisAudio.PlayOneShot(_clipFixed);


}

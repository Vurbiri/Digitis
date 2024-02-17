using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShapesManagerSFX : AAudioSourceController
{
    [SerializeField] private AudioClip _clipMove;
    [SerializeField] private float _volumeMove = 1f;
    [SerializeField] private AudioClip _clipRotate;
    [SerializeField] private float _volumeRotate = 1f;
    [SerializeField] private AudioClip _clipDown;
    [SerializeField] private float _volumeDown = 1f;
    [SerializeField] private AudioClip _clipToBomb;
    [SerializeField] private float _volumeToBomb = 1f;
    [SerializeField] private AudioClip _clipError;
    [SerializeField] private float _volumeError = 1f;
    [Space]
    [SerializeField] private AudioClip _clipFixed;


    public void PlayMove() => PlayOneShot(_clipMove, _volumeMove);
    public void PlayRotate() => PlayOneShot(_clipRotate, _volumeRotate);
    public void PlayDown() => PlayOneShot(_clipDown, _volumeDown);
    public void PlayError() => PlayOneShot(_clipError, _volumeError);
    public void PlayToBomb() => PlayOneShot(_clipToBomb, _volumeToBomb);

    public void PlayFixed(bool isBomb)
    {
        if (!isBomb)
            PlayClip(_clipFixed);
    }


}

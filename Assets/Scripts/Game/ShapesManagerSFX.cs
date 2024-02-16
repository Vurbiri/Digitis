using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShapesManagerSFX : MonoBehaviour
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

    private AudioSource _thisAudio;
    
    private void Awake()
    {
        _thisAudio = GetComponent<AudioSource>();
    }

    public void PlayMove() => _thisAudio.PlayOneShot(_clipMove, _volumeMove);
    public void PlayRotate() => _thisAudio.PlayOneShot(_clipRotate, _volumeRotate);
    public void PlayDown() => _thisAudio.PlayOneShot(_clipDown, _volumeDown);
    public void PlayError() => _thisAudio.PlayOneShot(_clipError, _volumeError);
    public void PlayToBomb() => _thisAudio.PlayOneShot(_clipToBomb, _volumeToBomb);

    public void PlayFixed(bool isBomb)
    {
        if(!isBomb)
            PlayClip(_clipFixed);

        #region Local Functions
        void PlayClip(AudioClip clip)
        {
            _thisAudio.Stop();
            _thisAudio.clip = clip;
            _thisAudio.Play();
        }
        #endregion
    }


}

using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    private void Start()
    {
        MusicSingleton.Instance.Play(Music.Menu);
    }

    private void OnDestroy()
    {
        if(MusicSingleton.Instance != null)
            MusicSingleton.Instance.Stop();
    }
}

using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    private void Start()
    {
        MusicSingleton.Instance.MenuPlay();
    }

    private void OnDestroy()
    {
        if(MusicSingleton.Instance != null)
            MusicSingleton.Instance.Stop();
    }
}

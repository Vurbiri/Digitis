using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class MainSceneAutoloader
{
    private static readonly int startScene = 0;
    
    static MainSceneAutoloader()
    {
        EditorApplication.playModeStateChanged += OnModeStateChanged;

        if (EditorBuildSettings.scenes.Length <= startScene)
            return;

        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[startScene].path);
    }

    private static void OnModeStateChanged(PlayModeStateChange change)
    {
        if(!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorApplication.ExitPlaymode();
    }
}

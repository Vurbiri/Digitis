using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor
{
    internal class SceneAutoloaderWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Scene Autoloader", MENU = MENU_PATH + NAME;
        private const string LABEL_SCENE = "Start scene", LABEL_SAVE = "Save assets when playing";
        public const string SCENE_TYPE = "t:Scene";
        #endregion

        private static bool s_isSaveScene = true;
        private static readonly string s_key_save = Application.productName + "_MSA_SaveScene", s_key_path = Application.productName + "_MSA_PathScene";

        private readonly System.Type _typeSceneAsset = typeof(SceneAsset);
        private SceneAsset _sceneAsset;

        [MenuItem(MENU, false, 44)]
        private static void ShowWindow()
        {
            GetWindow<SceneAutoloaderWindow>(true, NAME);
        }

        private void OnEnable()
        {
            _sceneAsset = EditorSceneManager.playModeStartScene;
        }

        private void OnGUI()
        {
            if (Application.isPlaying)
                return;

            BeginWindows();
            {
                EditorGUILayout.Space(10f);
                EditorGUILayout.LabelField(NAME, STYLES.H1);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.Space();
                    _sceneAsset = (SceneAsset)EditorGUILayout.ObjectField(LABEL_SCENE, _sceneAsset, _typeSceneAsset, false);
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.Space();
                        s_isSaveScene = EditorGUILayout.ToggleLeft(LABEL_SAVE, s_isSaveScene);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndVertical();
            }
            EndWindows();

            if (_sceneAsset != EditorSceneManager.playModeStartScene)
            {
                EditorPrefs.SetString(s_key_path, _sceneAsset != null ? AssetDatabase.GetAssetPath(_sceneAsset) : string.Empty);
                EditorSceneManager.playModeStartScene = _sceneAsset;
            }
        }

        private void OnDisable()
        {
            EditorPrefs.SetBool(s_key_save, s_isSaveScene);
        }

        [InitializeOnLoadMethod]
        private static void OnProjectLoadedInEditor()
        {
            Load();

            EditorApplication.playModeStateChanged -= OnModeStateChanged;
            EditorApplication.playModeStateChanged += OnModeStateChanged;
        }

        private static void OnModeStateChanged(PlayModeStateChange change)
        {
            if (s_isSaveScene & change == PlayModeStateChange.ExitingEditMode)
            {
                AssetDatabase.SaveAssets();
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorApplication.ExitPlaymode();
            }
        }

        private static void Load()
        {
            if (EditorSceneManager.playModeStartScene != null)
            {
                Debug.Log($"<color=yellow>[SceneAutoloader] Start scene: <i>{EditorSceneManager.playModeStartScene.name}</i></color>");
                return;
            }

            if (EditorPrefs.HasKey(s_key_save))
                s_isSaveScene = EditorPrefs.GetBool(s_key_save);

            string path = null;
            if (EditorPrefs.HasKey(s_key_path))
                path = EditorPrefs.GetString(s_key_path);

            if (string.IsNullOrEmpty(path))
            {
                Debug.Log("<color=yellow>[SceneAutoloader] Start scene: <i>current</i></color>");
                return;
            }

            SceneAsset startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            if (startScene != null)
            {
                EditorSceneManager.playModeStartScene = startScene;
                Debug.Log($"<color=yellow>[SceneAutoloader] Set start scene: <i>{startScene.name}</i></color>");
            }
            else
            {
                Debug.LogWarning($"[SceneAutoloader] Could not find scene: <i>{path}</i>");
            }
        }
    }

}






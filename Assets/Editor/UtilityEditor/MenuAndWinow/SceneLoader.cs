using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Vurbiri;
using static VurbiriEditor.CONST_EDITOR;
using UScene = UnityEngine.SceneManagement.Scene;

namespace VurbiriEditor
{
    public class SceneLoader : EditorWindow
	{
        #region Consts
        private const string PATH_IMAGE = "ed_iconSceneLoader";
        private const string NAME = "Scenes Switch", MENU = MENU_PATH + NAME;
        #endregion

        private static SceneField s_sceneField;

        [MenuItem(MENU, false, 47)]
		private static void ShowWindow()
		{
            GetWindow<SceneLoader>();
		}
		
		private void OnEnable()
		{
            titleContent = new(NAME, Resources.Load<Texture>(PATH_IMAGE));

            s_sceneField = new(OpenScene);

            EditorSceneManager.activeSceneChangedInEditMode += ChangedActiveScene;
            SceneManager.activeSceneChanged += ChangedActiveScene;
        }

        public void CreateGUI()
        {
            rootVisualElement.Add(s_sceneField);
        }

        public void Update()
        {
            s_sceneField.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);
        }

        private void OnDisable()
        {
            EditorSceneManager.activeSceneChangedInEditMode -= ChangedActiveScene;
            SceneManager.activeSceneChanged -= ChangedActiveScene;

            s_sceneField = s_sceneField.Dispose(OpenScene);
        }

        private static void OpenScene(ChangeEvent<Scene> evt)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(evt.newValue);
        }

        private static void ChangedActiveScene(UScene current, UScene next)
        {
            s_sceneField.SetValueWithoutNotify(next);
        }

        #region Nested: SceneModificationProcessor, SceneField, Scene
        // ================== SceneModificationProcessor ==========================
        public class SceneModificationProcessor : AssetModificationProcessor
        {
            private const string SCENE_EXT = ".unity";

            public static void OnWillCreateAsset(string assetPath)
            {
                if (IsExecute(assetPath))
                    s_sceneField.AddItem(assetPath);
            }

            public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
            {
                if (IsExecute(assetPath))
                    s_sceneField.RemoveItem(assetPath);

                return AssetDeleteResult.DidNotDelete;
            }

            public static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
            {
                if (IsExecute(sourcePath))
                    s_sceneField.ReplaceItem(sourcePath, destinationPath);

                return AssetMoveResult.DidNotMove;
            }

            private static bool IsExecute(string assetPath) => s_sceneField != null && assetPath.EndsWith(SCENE_EXT);
        }
        // ================== SceneField ==========================
        private class SceneField : PopupField<Scene>
        {
            public SceneField(EventCallback<ChangeEvent<Scene>> callback)
            {
                choices = new();
                foreach (var guid in EUtility.FindGUIDAssets<SceneAsset>())
                    choices.Add(AssetDatabase.GUIDToAssetPath(guid));

                value = SceneManager.GetActiveScene();

                this.RegisterValueChangedCallback(callback);

                formatSelectedValueCallback = FormatItem;
                formatListItemCallback = FormatItem;
            }

            public SceneField Dispose(EventCallback<ChangeEvent<Scene>> callback)
            {
                choices.Clear();
                this.UnregisterCallback(callback);

                return null;
            }

            public void AddItem(string value)
            {
                if (IndexItem(value) < 0)
                    choices.Add(value);
            }

            public void RemoveItem(string value)
            {
                int index = IndexItem(value);
                if (index >= 0)
                    choices.RemoveAt(index);
            }

            public void ReplaceItem(string oldValue, string newValue)
            {
                int index = IndexItem(oldValue);
                if (index >= 0)
                    choices[index] = newValue;
            }

            public int IndexItem(string value)
            {
                int i = choices.Count;
                while (i --> 0 && !choices[i].Equals(value));
                return i;
            }

            private static string FormatItem(Scene scene) => scene.name;
        }
        // ================== Scene ==========================
        private class Scene : IEquatable<Scene>, IEquatable<string>, IEquatable<UScene>
        {
            public readonly string path;
            public readonly string name;

            public Scene(string value)
            {
                path = value;
                name = System.IO.Path.GetFileNameWithoutExtension(value);
            }
            public Scene(UScene scene)
            {
                path = scene.path;
                name = scene.name;
            }

            public static implicit operator Scene(string value) => new(value);
            public static implicit operator Scene(UScene value) => new(value);
            
            public static implicit operator string(Scene value) => value.path;

            public bool Equals(string other) => path.Equals(other);
            public bool Equals(Scene other) => other is not null && path.Equals(other.path);
            public bool Equals(UScene other) => path.Equals(other.path);

            public override string ToString() => name;
        }
        #endregion
    }
}

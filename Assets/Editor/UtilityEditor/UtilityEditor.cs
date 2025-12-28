#if UNITY_EDITOR

namespace Vurbiri
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UI;

    public static class EUtility
    {
        public const string TYPE_PREFAB = "t:Prefab";
        public readonly static string[] ASSET_FOLDERS = new string[] { "Assets" };

        public static T InstantiatePrefab<T>(T prefab, Transform parent) where T : Component
        {
            T temp = (T)PrefabUtility.InstantiatePrefab(prefab);
            temp.transform.SetParent(parent, false);
            return temp;
        }

        public static void DestroyGameObject<T>(ref T component) where T : Component
        {
            if (component != null)
                Object.DestroyImmediate(component.gameObject);
            component = null;
        }

        // ********************************************

        public static void SetGameObject(ref GameObject obj, string name)
        {
            if (obj != null && obj.name == name) 
                return;

            obj = GameObject.Find(name);
            if (obj == null)
                LogErrorFind<GameObject>("GameObject", name);
        }

        public static void SetObject<T>(ref T obj, string name = null) where T : Component
        {
            bool notName = string.IsNullOrEmpty(name);
            if (obj != null && (notName || obj.gameObject.name == name)) return;

            obj = notName ? Object.FindAnyObjectByType<T>(FindObjectsInactive.Include) : FindObjectByName<T>(name);
            if (obj == null)
                LogErrorFind<T>("object", notName ? typeof(T).Name : name);
        }
        public static void SetObjects<T>(ref T[] arr, int count = -1) where T : Object
        {
            if (arr != null && (arr.Length == count))
                return;

            arr = Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            if (arr == null || (count > 0 & arr.Length != count))
                LogErrorFind<T>("objects", typeof(T).Name);
        }

        public static void SetPrefab<T>(ref T obj, string name = null) where T : Component
        {
            bool notName = string.IsNullOrEmpty(name);
            if (obj != null) return;

            obj = notName ? FindAnyPrefab<T>() : FindAnyPrefab<T>(name);
            if (obj == null)
                LogErrorFind<T>("prefab", name);
        }

        public static void SetScriptable<T>(ref T obj, string name = null) where T : ScriptableObject
        {
            if (obj != null) return;
            obj = string.IsNullOrEmpty(name) ? FindAnyScriptable<T>() : FindAnyScriptable<T>(name);
            if (obj == null)
                LogErrorFind<T>("scriptable", name);
        }

        public static void SetAsset<T>(ref T obj, string name = null) where T : Object
        {
            if (obj != null) return;

            obj = string.IsNullOrEmpty(name) ? FindAnyAsset<T>() : FindAnyAsset<T>(name);
            if (obj == null)
                LogErrorFind<T>("asset", name);
        }

        public static void SetArray<T>(ref T[] arr, int length)
        {
            if (arr == null)
                arr = new T[length];
            else if (arr.Length != length)
                System.Array.Resize(ref arr, length);
        }

        public static bool SetComponent<T>(this Component self, ref T component) where T : Component
        {
            if (component == null) component = self.GetComponent<T>();
            if (component == null)
            {
                LogErrorFind<T>("component", self.gameObject.name);
                return false;
            }
            return true;
        }
        public static bool SetChildren<T>(this Component self, ref T component) where T : Component
        {
            if (component == null) component = self.GetComponentInChildren<T>(true);
            if (component == null)
            {
                LogErrorFind<T>("component", self.gameObject.name);
                return false;
            }
            return true;
        }
        public static void SetChildren<T>(this Component self, ref T component, string name, bool errorMsg = true) where T : Component
        {
            if (component == null || component.gameObject.name != name) 
                component = self.GetComponentInChildren<T>(name);
            if (errorMsg & component == null)
                LogErrorFind<T>("component", name);
        }
        public static void SetChildrens<T>(this Component self, ref T[] components, int length) where T : Component
        {
            if (components != null && components.Length == length) return;
            components = self.GetComponentsInChildren<T>(true);
            if (components == null || components.Length != length)
                LogErrorFind<T>("component", null);
        }

        // ********************************************
        public static void SetColorField<T>(T self, Color color, string field) where T : Object
        {
            SerializedObject so = new(self);
            so.FindProperty(field).colorValue = color;
            so.ApplyModifiedProperties();
        }
        public static void SetObjectField(this Object self, Object obj, string field)
        {
            SerializedObject so = new(self);
            so.FindProperty(field).objectReferenceValue = obj;
            so.ApplyModifiedProperties();
        }
        public static void SetColorField(this Graphic self, Color color) => SetColorField<Graphic>(self, color, "m_Color");
        public static void SetImageFields(this Image self, Color color, float pixelsPerUnit)
        {
            SerializedObject so = new(self);
            so.FindProperty("m_Color").colorValue = color;
            so.FindProperty("m_PixelsPerUnitMultiplier").floatValue = pixelsPerUnit;
            so.ApplyModifiedProperties();
        }
        // ********************************************

        public static T GetComponentInChildren<T>(this Component self, string name) where T : Component
        {
            foreach (var component in self.GetComponentsInChildren<T>(true))
                if (component.gameObject.name == name) return component;
            return null;
        }
        public static T GetComponentInChildren<T>(this GameObject self, string name) where T : Component
        {
            foreach (var component in self.GetComponentsInChildren<T>(true))
                if (component.gameObject.name == name) return component;
            return null;
        }

        // ********************************************

        public static T FindObjectByName<T>(string name) where T : Component
        {
            //return Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(t => t.gameObject.name == name).First();
            foreach (var component in Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                if (component.gameObject.name == name) return component;
            return null;
        }

        public static List<T> FindObjectsByName<T>(string name) where T : Component
        {
            List<T> components = new();
            foreach (var component in Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                if (component.gameObject.name == name) components.Add(component);
            return components;
        }

        public static T FindAnyPrefab<T>() where T : Component
        {
            foreach (var guid in FindGUIDPrefabs())
                if (LoadMainAssetAtGUID(guid).TryGetComponent<T>(out T component))
                    return component;

            return null;
        }
        public static T FindAnyPrefab<T>(string name) where T : Component
        {
            foreach (var guid in FindGUIDPrefabs(name))
                if (LoadMainAssetAtGUID(guid).TryGetComponent<T>(out T component))
                    return component;

            return null;
        }
        public static GameObject FindAnyPrefab(string name)
        {
            foreach (var guid in FindGUIDPrefabs(name))
                return LoadMainAssetAtGUID(guid);

            return null;
        }

        public static List<T> FindPrefabs<T>() where T : MonoBehaviour
        {
            List<T> list = new();
            foreach (var guid in FindGUIDPrefabs())
                if (LoadMainAssetAtGUID(guid).TryGetComponent<T>(out T component))
                    list.Add(component);

            return list;
        }

        public static List<T> FindComponentsPrefabs<T>() where T : Component
        {
            List<T> list = new(); T[] components;
            foreach (var guid in FindGUIDPrefabs())
                if ((components = LoadMainAssetAtGUID(guid).GetComponentsInChildren<T>()) != null)
                    list.AddRange(components);

            return list;
        }

        public static T FindAnyScriptable<T>() where T : ScriptableObject
        {
            foreach (var guid in FindGUIDAssets<T>())
                if (TryLoadAssetAtGUID<T>(guid, out T scriptable))
                    return scriptable;

            return null;
        }
        public static T FindAnyScriptable<T>(string name) where T : ScriptableObject
        {
            foreach (var guid in FindGUIDAssets<T>(name))
                if (TryLoadAssetAtGUID<T>(guid, out T scriptable))
                    return scriptable;

            return null;
        }

        public static List<T> FindScriptables<T>() where T : ScriptableObject
        {
            List<T> list = new();
            foreach (var guid in FindGUIDAssets<T>())
                if (TryLoadAssetAtGUID<T>(guid, out T scriptable))
                    list.Add(scriptable);

            return list;
        }

        public static T CreateScriptable<T>(string defaultName, string defaultPath) where T : ScriptableObject
        {
            string path = EditorUtility.SaveFilePanelInProject("Create Scriptable", defaultName, "asset", "", defaultPath);
            if (!string.IsNullOrEmpty(path))
            {
                T asset = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();

                return asset;
            }
            return null;
        }

        public static void CheckScriptable<T>(ref T scriptable, string defaultName, string defaultPath) where T : ScriptableObject
        {
            if (scriptable == null)
            {
                scriptable = FindAnyScriptable<T>();
                if (scriptable == null)
                    scriptable = CreateScriptable<T>(defaultName, defaultPath);
                else
                    Debug.LogWarning($"Set {typeof(T).Name}");
            }
        }

        public static T FindAnyAsset<T>() where T : Object
        {
            foreach (var guid in FindGUIDAssets<T>())
                if (TryLoadAssetAtGUID<T>(guid, out T asset))
                    return asset;

            return null;
        }
        public static T FindAnyAsset<T>(string name) where T : Object
        {
            foreach (var guid in FindGUIDAssets<T>(name))
                if (TryLoadAssetAtGUID<T>(guid, out T asset))
                    return asset;

            return null;
        }

        public static T[] FindAllAssets<T>(string name) where T : Object
        {
            foreach (var guid in FindGUIDAssets<T>(name))
                if (TryLoadAllAssetsAtGUID<T>(guid, out T[] assets))
                    return assets;

            return new T[0];
        }

        public static Sprite FindMultipleSprite(string name)
        {
            foreach (var sprite in FindAllAssets<Sprite>(name))
                if (sprite.name == name) return sprite;
            return null;
        }

        // ********************************************

        public static string[] FindGUIDPrefabs() => AssetDatabase.FindAssets(TYPE_PREFAB, ASSET_FOLDERS);
        public static string[] FindGUIDPrefabs(string name) => AssetDatabase.FindAssets($"{name} {TYPE_PREFAB}", ASSET_FOLDERS);
        public static string[] FindGUIDAssets<T>() where T : Object => AssetDatabase.FindAssets($"t:{typeof(T).Name}", ASSET_FOLDERS);
        public static string[] FindGUIDAssets<T>(string name) where T : Object => AssetDatabase.FindAssets($"{name} t:{typeof(T).Name}", ASSET_FOLDERS);
        public static GameObject LoadMainAssetAtGUID(string guid) => ((GameObject)AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guid)));
        public static bool TryLoadAssetAtGUID<T>(string guid, out T obj) where T : Object
        {
            obj = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
            return obj != null;
        }
        public static bool TryLoadAllAssetsAtGUID<T>(string guid, out T[] arr) where T : Object
        {
            arr = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(guid)).OfType<T>().ToArray();
            return arr != null && arr.Length > 0;
        }

        public static bool IsStartEditor => EditorApplication.timeSinceStartup < 120f;

        private static void LogErrorFind<T>(string type, string name)
        {
            Debug.LogError($"<color=orange> Unable to find the {type} <b>{typeof(T).Name}</b> {(string.IsNullOrEmpty(name) ? string.Empty : $"\"{name}\"")}</color>");
        }

    }
}
#endif

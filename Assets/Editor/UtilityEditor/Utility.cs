using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace VurbiriEditor
{
    public static class Utility
	{

        public static void CreateObjectFromResources(string path, string name, GameObject parent) 
            => Place(GameObject.Instantiate(Resources.Load(path)) as GameObject, parent, name);

        public static void CreateObjectFromPrefab(MonoBehaviour prefab, string name, GameObject parent)
            => Place(GameObject.Instantiate(prefab).gameObject, parent, name);

        public static GameObject CreateObject(string name, GameObject parent, params Type[] types)
        {
            GameObject newObject = ObjectFactory.CreateGameObject(name, types);
            newObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            Place(newObject, parent, name);

            return newObject;
        }

        private static void Place(GameObject gameObject, GameObject parent, string name)
        {
            gameObject.name = name;

            GameObjectUtility.SetParentAndAlign(gameObject, parent);

            //SceneView lastView = SceneView.lastActiveSceneView;
            //gameObject.transform.position = lastView ? lastView.pivot : Vector3.zero;

            StageUtility.PlaceGameObjectInCurrentStage(gameObject);
            GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create: {gameObject.name}");
            Selection.activeGameObject = gameObject;

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}

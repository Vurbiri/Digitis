using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using Vurbiri;
using Object = UnityEngine.Object;

namespace VurbiriEditor
{
    internal class SpriteAtlasUtility
    {
        private const string MENU = "Assets/Sprite Atlas/", ADD_MENU = MENU + "Add to Master", REMOVE_MENU = MENU + "Remove from Master";

        private static string s_path;
        private static SpriteAtlas s_atlas;
        private static SpriteAtlasAsset s_atlasAsset;

        [MenuItem(ADD_MENU, true)]
        private static bool AddValidate() => Validate(CanAdd);

        [MenuItem(ADD_MENU, false, 11)]
        private static void Add() => Action(CanAdd, s_atlasAsset.Add, "Added");

        [MenuItem(REMOVE_MENU, true)]
        private static bool RemoveValidate() => Validate(CanRemove);

        [MenuItem(REMOVE_MENU, false, 11)]
        private static void Remove() => Action(CanRemove, s_atlasAsset.Remove, "Removed");

        private static bool CanAdd(Object obj) => obj is Texture2D texture && s_atlas.GetSprite(texture.name) == null;
        private static bool CanRemove(Object obj) => obj is Texture2D texture && s_atlas.GetSprite(texture.name) != null;

        private static bool Validate(Func<Object, bool> canFunc)
        {
            if (!TryGetAtlasAsset())
                return false;

            foreach (var obj in Selection.objects)
                if (canFunc(obj))
                    return true;

            return false;
        }
        private static void Action(Func<Object, bool> canFunc, Action<Object[]> action, string actionName)
        {
            if (!TryGetAtlasAsset())
                return;

            List<Object> objects = new(Selection.objects.Length);
            foreach (var obj in Selection.objects)
                if (canFunc(obj))
                    objects.Add(obj);

            if (objects.Count > 0)
            {
                action(objects.ToArray());
                SpriteAtlasAsset.Save(s_atlasAsset, s_path);
                AssetDatabase.Refresh();
                Debug.Log($"[SpriteAtlasUtility] {actionName} {objects.Count} sprite(s)");
            }
        }

        private static bool TryGetAtlasAsset()
        {
            if (s_atlas != null & s_atlasAsset != null)
                return true;
            
            var guids = EUtility.FindGUIDAssets<SpriteAtlas>();
            if (guids == null || guids.Length != 1 || string.IsNullOrEmpty(guids[0])) return false;

            s_path = AssetDatabase.GUIDToAssetPath(guids[0]);
            if(string.IsNullOrEmpty(s_path)) return false;

            s_atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(s_path);
            s_atlasAsset = SpriteAtlasAsset.Load(s_path);

            return s_atlas != null & s_atlasAsset != null && !s_atlasAsset.isVariant;
        }
    }
}

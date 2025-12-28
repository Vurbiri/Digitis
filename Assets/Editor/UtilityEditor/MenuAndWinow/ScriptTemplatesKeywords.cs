using System.IO;
using UnityEditor;
using UnityEngine;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor
{
    public class ScriptTemplatesKeywords : AssetModificationProcessor
	{
        #region Consts
        private const string MENU_NAME = "Custom keywords/", MENU = MENU_PATH + MENU_NAME;
        private const string MENU_NAME_ENABLE = "Enable", MENU_COMMAND_ENABLE = MENU + MENU_NAME_ENABLE;
        private const string MENU_NAME_DISABLE = "Disable", MENU_COMMAND_DISABLE = MENU + MENU_NAME_DISABLE;
        private const string WINDOW = "Window", EDITOR = "Editor", DRAWER = "Drawer";
		#endregion

		private static bool s_enabled = true;
        private static readonly string s_key_save = Application.productName + "_CSTK_ENABLE";

        [MenuItem(MENU_COMMAND_ENABLE, false, 35)]
        private static void CommandEnable()
        {
            s_enabled = true;
			Save(); Log();
        }
        [MenuItem(MENU_COMMAND_ENABLE, true, 35)]
        private static bool CommandEnableValidate()
        {
            SetChecked();
            return !s_enabled;
        }

        [MenuItem(MENU_COMMAND_DISABLE, false, 36)]
        private static void CommandDisable()
        {
            s_enabled = false;
            Save(); Log();
        }
        [MenuItem(MENU_COMMAND_DISABLE, true, 36)]
        private static bool CommandDisableValidate() => !CommandEnableValidate();

        public static void OnWillCreateAsset(string assetName)
		{
            if (!s_enabled) return;
            
            if (!assetName.EndsWith(META_EXT)) return;
			assetName = assetName.Replace(META_EXT, string.Empty);
			if (!assetName.EndsWith(CS_EXT)) return;

			int index = Application.dataPath.LastIndexOf(ASSETS);
			string path = Path.Combine(Application.dataPath[..index], assetName);
			string file = File.ReadAllText(path, utf8WithoutBom);
			string name = Path.GetFileNameWithoutExtension(path);

			bool isReplace = false;
			isReplace |= Replace(ref file, @"#PROJECTNAME#", PlayerSettings.productName);
			isReplace |= Replace(ref file, @"#COMPANYNAME#", PlayerSettings.companyName);
            isReplace |= Replace(ref file, @"#NAMENOTWINDOW#", name.Replace(WINDOW, string.Empty));
            isReplace |= Replace(ref file, @"#NAMENOTEDITOR#", name.Replace(EDITOR, string.Empty));
			isReplace |= Replace(ref file, @"#NAMENOTDRAWER#", name.Replace(DRAWER, string.Empty));

			if (isReplace)
				File.WriteAllText(path, file, utf8WithoutBom);

			#region Local: Replace(..)
			//=======================================================
			static bool Replace(ref string file, string keyword, string replace)
			{
				if (file.IndexOf(keyword) < 0) 
					return false;

				file = file.Replace(keyword, replace);
				return true;
			}
			//=======================================================
			#endregion
		}

		private static void Save()
		{
            EditorPrefs.SetBool(s_key_save, s_enabled);

            SetChecked();
        }

        [InitializeOnLoadMethod]
        private static void Load()
		{
            if (EditorPrefs.HasKey(s_key_save))
                s_enabled = EditorPrefs.GetBool(s_key_save);
            
            SetChecked(); 
        }

        private static void SetChecked()
        {
            Menu.SetChecked(MENU_COMMAND_ENABLE, s_enabled);
            Menu.SetChecked(MENU_COMMAND_DISABLE, !s_enabled);

            //Menu.SetChecked(MENU, _enabled);
        }

        private static void Log()
        {
            string state = s_enabled ? MENU_NAME_ENABLE : MENU_NAME_DISABLE;
            Debug.Log($"[ScriptTemplatesKeywords] {state}");
        }
    }
}

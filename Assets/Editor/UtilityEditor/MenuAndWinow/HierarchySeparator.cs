using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public class HierarchySeparator
	{
        private const string NAME = "═══════════════════", TAG = "EditorOnly";
        private const string MENU = "GameObject/Create Separator";

        [MenuItem(MENU, false, 0)]
        public static void CreateFromMenu(MenuCommand command)
        {
            GameObject separator = Utility.CreateObject("Separator", command.context as GameObject);
            separator.name = NAME;
            separator.tag = TAG;
            separator.SetActive(false);
        }
    }
}

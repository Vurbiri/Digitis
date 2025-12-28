using System.Text;

namespace VurbiriEditor
{
    internal static class CONST_EDITOR
    {
        public const string MENU_PATH = "Vurbiri/";
        public const string ASSETS = "Assets";
        public const string META_EXT = ".meta", CS_EXT = ".cs";

        public static readonly Encoding utf8WithoutBom = new UTF8Encoding(false);
    }
}

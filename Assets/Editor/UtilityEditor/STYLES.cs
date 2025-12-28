using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public static class STYLES
	{
        private static readonly Texture2D s_borderTexture, s_borderLightTexture, s_borderDarkTexture, s_H1Texture;
        private static readonly Texture2D s_normalTexture, s_hoverTexture, s_focusedTexture, s_activeTexture;

        public static readonly GUIStyle H1;
        public static readonly GUIStyle H2;
        public static readonly GUIStyle H3;
        public static readonly GUIStyle border;
        public static readonly GUIStyle borderLight;
        public static readonly GUIStyle borderDark;
        public static readonly GUIStyle flatButton;

        public static Color32 defaultColor = new(56, 56, 56, 255), dimColor = new(51, 51, 51, 255), darkColor = new(33, 33, 33, 255), lightColor = new(80, 80, 80, 255);

        static STYLES()
		{
            s_H1Texture          = BackgroundColor(new(20, 40, 62, 255));
            s_borderTexture      = Border(darkColor);
            s_borderLightTexture = Border(lightColor, defaultColor);
            s_borderDarkTexture  = Border(darkColor, dimColor);
            s_normalTexture      = BackgroundColor(defaultColor);
            s_hoverTexture       = BackgroundColor(lightColor);
            s_focusedTexture     = BackgroundColor(new(118, 118, 118, 255));
            s_activeTexture      = BackgroundColor(dimColor);

            H1 = new()
            {
                name = "H1",
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 15,
            };
            H1.normal.textColor = new(0.75f, 0.75f, 0.8f);
            H1.normal.background = s_H1Texture;

            H2 = new()
            {
                name = "H2",
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 13,
            };
            H2.normal.textColor = new(0.6f, 0.7f, 0.8f);

            border = new()
            {
                name = "border",
                border = new(4, 4, 4, 4),
                padding = new(12, 6, 6, 6)

            };
            border.normal.background = s_borderTexture;

            borderLight = new(border)
            {
                name = "borderLight"
            };
            borderLight.normal.background = s_borderLightTexture;

            borderDark = new(border)
            {
                name = "borderDark"
            };
            borderDark.normal.background = s_borderDarkTexture;

            H3 = new(borderDark)
            {
                name = "H3",
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 13,
            };
            H3.normal.textColor = new(0.7f, 0.35f, 0.3f);

            flatButton = new()
            {
                name = "flatButton",
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = EditorGUIUtility.singleLineHeight,
                fixedWidth = EditorGUIUtility.singleLineHeight
            };
            flatButton.normal.background  = s_normalTexture;
            flatButton.hover.background   = s_hoverTexture;
            flatButton.focused.background = s_focusedTexture;
            flatButton.active.background  = s_activeTexture;
        }

        public static Texture2D BackgroundColor(Color32 color)
        {
            int size = 2, length = size * size;
            Color32[] pixels = new Color32[length];
            
            for (int i = 0; i < length; ++i)
                pixels[i] = color;

            return pixels.ToTexture(size);
        }

        public static Texture2D Border(Color32 colorBorder, int size = 8, int border = 1)
        {
            int borderMin = border, borderMax = size - border;
            Color32[] pixels = new Color32[size * size];

            for (int i = 0; i < borderMin; ++i)
                for (int j = 0; j < size; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMax; i < size; ++i)
                for (int j = 0; j < size; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMin; i < borderMax; ++i)
                for (int j = 0; j < borderMin; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMin; i < borderMax; ++i)
                for (int j = borderMax; j < size; ++j)
                    pixels[size * i + j] = colorBorder;

            return pixels.ToTexture(size);
        }

        public static Texture2D Border(Color32 colorBorder, Color32 colorMain, int size = 8, int border = 1)
        {
            int borderMin = border, borderMax = size - border;
            Color32[] pixels = new Color32[size * size];

            for (int i = 0; i < borderMin; ++i)
                for (int j = 0; j < size; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMax; i < size; ++i)
                for (int j = 0; j < size; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMin; i < borderMax; ++i)
                for (int j = 0; j < borderMin; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMin; i < borderMax; ++i)
                for (int j = borderMax; j < size; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMin; i < borderMax; ++i)
                for (int j = borderMin; j < borderMax; ++j)
                    pixels[size * i + j] = colorMain;
            
            return pixels.ToTexture(size);
        }

        private static Texture2D ToTexture(this Color32[] pixels, int size)
        {
            Texture2D texture = new(size, size);
            texture.SetPixels32(pixels);
            texture.Apply();

            return texture;
        }
    }
}

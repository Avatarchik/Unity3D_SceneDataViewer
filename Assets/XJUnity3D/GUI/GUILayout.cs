using UnityEngine;

namespace XJUnity3D.GUI
{
    /// <summary>
    /// GUI に関する拡張機能を提供します。
    /// </summary>
    public partial class GUILayout
    {
        #region Field

        /// <summary>
        /// XJUnity3D.GUI によって描画される TextField の幅。
        /// </summary>
        public static float TextFieldWidth = 50;

        #region Background & Color

        public static readonly Texture2D NormalBackgroundTexture;

        public static readonly Texture2D HoverBackgroundTexture;

        public static readonly Texture2D ActiveBackgroundTexture;

        public static readonly Texture2D FocusedBackgroundTexture;

        private static readonly Color NormalBackgroundColor
            = new Color() { a = 0.5f, r = 0f, g = 0f, b = 0f };

        private static readonly Color HoverBackgroundColor
            = new Color() { a = 0.5f, r = 0.8f, g = 0.8f, b = 0.8f };

        private static readonly Color ActiveBackgroundColor
            = new Color() { a = 0.4f, r = 0.8f, g = 0.8f, b = 0.8f };

        private static readonly Color FocusedBackgroundColor
            = new Color() { a = 0.5f, r = 1f, g = 1f, b = 1f };

        #endregion Background & Color

        #region Style

        public static readonly GUIStyle TabButtonStyle;

        public static readonly GUIStyle BoldLabelStyle;

        public static readonly GUIStyle FoldoutPanelStyle;

        public static readonly GUIStyle FoldoutPanelBoldStyle;

        #endregion Style

        #endregion Field

        #region Constructor

        static GUILayout()
        {
            GUILayout.NormalBackgroundTexture = 
                GUILayout.GenerateBackgroundTexture(GUILayout.NormalBackgroundColor);

            GUILayout.HoverBackgroundTexture =
                GUILayout.GenerateBackgroundTexture(GUILayout.HoverBackgroundColor);

            GUILayout.ActiveBackgroundTexture =
                GUILayout.GenerateBackgroundTexture(GUILayout.ActiveBackgroundColor);

            GUILayout.FocusedBackgroundTexture =
                GUILayout.GenerateBackgroundTexture(GUILayout.FocusedBackgroundColor);

            GUILayout.TabButtonStyle = new GUIStyle(UnityEngine.GUI.skin.button);
            GUILayout.TabButtonStyle.margin = new RectOffset(3, 3, 0, 0);
            GUILayout.TabButtonStyle.normal.background = GUILayout.NormalBackgroundTexture;
            GUILayout.TabButtonStyle.hover.background = GUILayout.HoverBackgroundTexture;
            GUILayout.TabButtonStyle.active.background = GUILayout.ActiveBackgroundTexture;
            GUILayout.TabButtonStyle.onNormal.background = GUILayout.ActiveBackgroundTexture;
            GUILayout.TabButtonStyle.onHover.background = GUILayout.HoverBackgroundTexture;
            GUILayout.TabButtonStyle.onActive.background = GUILayout.ActiveBackgroundTexture;

            GUILayout.BoldLabelStyle = new GUIStyle(UnityEngine.GUI.skin.label);
            GUILayout.BoldLabelStyle.fontStyle = FontStyle.Bold;

            GUILayout.FoldoutPanelStyle = new GUIStyle(UnityEngine.GUI.skin.label);
            GUILayout.FoldoutPanelStyle.hover.textColor = UnityEngine.GUI.skin.label.normal.textColor;
            GUILayout.FoldoutPanelStyle.hover.background = GUILayout.HoverBackgroundTexture;

            GUILayout.FoldoutPanelBoldStyle = new GUIStyle(GUILayout.FoldoutPanelStyle);
            GUILayout.FoldoutPanelBoldStyle.fontStyle = FontStyle.Bold;
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// ヘッダとなるラベルを描画します。
        /// </summary>
        /// <param name="text">
        /// ヘッダとして描画するテキスト。
        /// </param>
        public static void BoldLabel(string text)
        {
            UnityEngine.GUILayout.Label(text, BoldLabelStyle);
        }

        /// <summary>
        /// 任意の色を指定して背景用の Texture2D を生成します。
        /// </summary>
        /// <param name="color">
        /// 背景色。
        /// </param>
        /// <returns>
        /// 背景用の Texture2D.
        /// </returns>
        private static Texture2D GenerateBackgroundTexture(Color color)
        {
            int width = 1;
            int height = 1;

            Color[] pixels = new Color[width * height];

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D backgroundTexture = new Texture2D(width, height);
            backgroundTexture.SetPixels(pixels);
            backgroundTexture.Apply();

            return backgroundTexture;
        }

        #endregion Method
    }
}
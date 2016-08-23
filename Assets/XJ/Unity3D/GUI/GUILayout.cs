using UnityEngine;

namespace XJ.Unity3D.GUI
{
    /// <summary>
    /// GUI に関する拡張機能を提供します。
    /// </summary>
    public partial class GUILayout
    {
        #region Field

        /// <summary>
        /// XJ.Unity3D.GUI によって描画される TextField の幅。
        /// </summary>
        public static float TextFieldWidth = 50;

        /// <summary>
        /// XJ.Unity3D.GUI によって描画される float 型のコントロールの小数点以下の桁数。
        /// </summary>
        public static int NumberOfFloatValueDecimal = 2;

        #region Layout

        public static readonly RectOffset BaseMargin = new RectOffset(5, 5, 5, 5);

        public static readonly RectOffset BasePadding = new RectOffset(5, 5, 2, 2);

        #endregion Layout

        #region Background & Color

        public static readonly Color BaseTextColor
            = new Color() { a = 1f, r = 1f, g = 1f, b = 1f };

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
        public static readonly GUIStyle TabPanelStyle;

        public static readonly GUIStyle BoldLabelStyle;
        public static readonly GUIStyle MiddleRightAlignedLabelStyle;
        public static readonly GUIStyle LowerCenterAlignedLabelStyle;

        public static readonly GUIStyle NumericTextBoxStyle;

        public static readonly GUIStyle FoldoutPanelStyle;
        public static readonly GUIStyle FoldoutPanelBoldStyle;

        #endregion Style

        #endregion Field

        #region Constructor

        static GUILayout()
        {
            // 当初は必要なら new GUIStyle(UnityEngine.GUI.skin.button); のように、
            // 基底の skin を複製して、変更が必要なパラメータだけを変更していました。
            // 
            // しかしながら、static コンストラクタは OnGUI メソッド以外から実行される可能性があります。
            // UnityEngine.GUI は OnGUI メソッドの外から参照することができないので、
            // UnityEngine.GUI を使った実装は断念する必要がありました。

            GUILayout.NormalBackgroundTexture = 
                GUILayout.GenerateBackgroundTexture(GUILayout.NormalBackgroundColor);

            GUILayout.HoverBackgroundTexture =
                GUILayout.GenerateBackgroundTexture(GUILayout.HoverBackgroundColor);

            GUILayout.ActiveBackgroundTexture =
                GUILayout.GenerateBackgroundTexture(GUILayout.ActiveBackgroundColor);

            GUILayout.FocusedBackgroundTexture =
                GUILayout.GenerateBackgroundTexture(GUILayout.FocusedBackgroundColor);

            GUILayout.TabButtonStyle = new GUIStyle();
            GUILayout.TabButtonStyle.margin = new RectOffset(1, 1, BaseMargin.top, 0);
            GUILayout.TabButtonStyle.padding = GUILayout.BasePadding;
            GUILayout.TabButtonStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.TabButtonStyle.normal.textColor = GUILayout.BaseTextColor;
            GUILayout.TabButtonStyle.hover.textColor = GUILayout.BaseTextColor;
            GUILayout.TabButtonStyle.active.textColor = GUILayout.BaseTextColor;
            GUILayout.TabButtonStyle.onNormal.textColor = GUILayout.BaseTextColor;
            GUILayout.TabButtonStyle.onHover.textColor = GUILayout.BaseTextColor;
            GUILayout.TabButtonStyle.onActive.textColor = GUILayout.BaseTextColor;
            GUILayout.TabButtonStyle.normal.background = GUILayout.NormalBackgroundTexture;
            GUILayout.TabButtonStyle.hover.background = GUILayout.HoverBackgroundTexture;
            GUILayout.TabButtonStyle.active.background = GUILayout.ActiveBackgroundTexture;
            GUILayout.TabButtonStyle.onNormal.background = GUILayout.ActiveBackgroundTexture;
            GUILayout.TabButtonStyle.onHover.background = GUILayout.HoverBackgroundTexture;
            GUILayout.TabButtonStyle.onActive.background = GUILayout.ActiveBackgroundTexture;

            GUILayout.TabPanelStyle = new GUIStyle();
            GUILayout.TabPanelStyle.normal.background = GUILayout.ActiveBackgroundTexture;

            GUILayout.MiddleRightAlignedLabelStyle = new GUIStyle();
            GUILayout.MiddleRightAlignedLabelStyle.margin = GUILayout.BaseMargin;
            GUILayout.MiddleRightAlignedLabelStyle.normal.textColor = GUILayout.BaseTextColor;
            GUILayout.MiddleRightAlignedLabelStyle.alignment = TextAnchor.MiddleRight;

            GUILayout.LowerCenterAlignedLabelStyle = new GUIStyle(GUILayout.MiddleRightAlignedLabelStyle);
            GUILayout.LowerCenterAlignedLabelStyle.alignment = TextAnchor.LowerCenter;

            GUILayout.BoldLabelStyle = new GUIStyle(GUILayout.MiddleRightAlignedLabelStyle);
            GUILayout.BoldLabelStyle.fontStyle = FontStyle.Bold;
            GUILayout.BoldLabelStyle.alignment = TextAnchor.MiddleLeft;

            GUILayout.NumericTextBoxStyle = new GUIStyle();
            GUILayout.NumericTextBoxStyle.alignment = TextAnchor.MiddleRight;
            GUILayout.NumericTextBoxStyle.normal.background = GUILayout.NormalBackgroundTexture;
            GUILayout.NumericTextBoxStyle.normal.textColor = GUILayout.BaseTextColor;
            GUILayout.NumericTextBoxStyle.active.background = GUILayout.NormalBackgroundTexture;
            GUILayout.NumericTextBoxStyle.active.textColor = GUILayout.BaseTextColor;
            GUILayout.NumericTextBoxStyle.focused.background = GUILayout.NormalBackgroundTexture;
            GUILayout.NumericTextBoxStyle.focused.textColor = GUILayout.BaseTextColor;
            GUILayout.NumericTextBoxStyle.margin = GUILayout.BaseMargin;
            GUILayout.NumericTextBoxStyle.padding = GUILayout.BasePadding;
            
            GUILayout.FoldoutPanelStyle = new GUIStyle();
            GUILayout.FoldoutPanelStyle.margin = GUILayout.BaseMargin;
            GUILayout.FoldoutPanelStyle.normal.textColor = GUILayout.BaseTextColor;
            GUILayout.FoldoutPanelStyle.hover.textColor = GUILayout.BaseTextColor;
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
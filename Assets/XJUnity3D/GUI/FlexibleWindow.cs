using UnityEngine;

namespace XJUnity3D.GUI
{
    /// <summary>
    /// 描画内容に合わせて自動でサイズが可変する Window.
    /// </summary>
    public class FlexibleWindow
    {
        #region Field

        private float lastShowTime;

        private Rect windowRect;

        #endregion Field

        #region Property

        public int WindowID
        {
            get;
            private set;
        }

        public Rect WindowRect
        {
            get { return this.windowRect;  }
        }

        public float MinWidth
        {
            get;
            set;
        }

        public float MinHeight
        {
            get;
            set;
        }

        public float MaxWidth
        {
            get;
            set;
        }

        public float MaxHeight
        {
            get;
            set;
        }

        public bool EnableDrag
        {
            get;
            set;
        }

        #endregion Property

        #region Constructor

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="x">
        /// Window の左上の x 座標。
        /// </param>
        /// <param name="y">
        /// Window の左上の y 座標。
        /// </param>
        /// <param name="enableDrag">
        /// ドラッグ操作を有効にする。
        /// </param>
        public FlexibleWindow(float x, float y, bool enableDrag)
            :this(x, y, 0, 0, enableDrag)
        {
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="x">
        /// Window の左上の x 座標。
        /// </param>
        /// <param name="y">
        /// Window の左上の y 座標。
        /// </param>
        /// <param name="minWidth">
        /// 最小の幅。
        /// </param>
        /// <param name="minHeight">
        /// 最小の高さ。
        /// </param>
        /// <param name="enableDrag">
        /// ドラッグ操作を有効にする。
        /// </param>
        public FlexibleWindow(float x, float y, float minWidth, float minHeight, bool enableDrag)
            :this(x, y, minWidth, minHeight, float.MaxValue, float.MaxValue, enableDrag)
        {
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="x">
        /// Window の左上の x 座標。
        /// </param>
        /// <param name="y">
        /// Window の左上の y 座標。
        /// </param>
        /// <param name="minWidth">
        /// 最小の幅。
        /// </param>
        /// <param name="minHeight">
        /// 最小の高さ。
        /// </param>
        /// <param name="maxWidth">
        /// 最大の幅。
        /// </param>
        /// <param name="maxHeight">
        /// 最大の高さ。
        /// </param>
        /// <param name="enableDrag">
        /// ドラッグ操作を有効にする。
        /// </param>
        public FlexibleWindow(float x, float y, float minWidth, float minHeight,
                              float maxWidth, float maxHeight, bool enableDrag)
        {
            // この windowID は衝突する可能性が残っていますが、実用の面で問題になることは少ないでしょう。
            // https://blogs.msdn.microsoft.com/ericlippert/2010/03/22/socks-birthdays-and-hash-collisions/

            this.WindowID = System.Guid.NewGuid().GetHashCode();
            this.windowRect = new Rect(x, y, 0, 0);
            this.MinWidth = minWidth;
            this.MinHeight = MinHeight;
            this.MaxWidth = maxWidth;
            this.MaxHeight = MaxHeight;

            this.EnableDrag = enableDrag;
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// Window を表示します。
        /// </summary>
        /// <param name="title">
        /// Window のタイトル。
        /// </param>
        /// <param name="windowFunction">
        /// Window の内容を描画する関数。
        /// </param>
        public Rect Show(string title, UnityEngine.GUI.WindowFunction windowFunction)
        {
            if (this.EnableDrag)
            {
                windowFunction =
                    (UnityEngine.GUI.WindowFunction)UnityEngine.GUI.WindowFunction.Combine
                    (windowFunction,
                     new UnityEngine.GUI.WindowFunction((int windowId) =>
                     {
                         UnityEngine.GUI.DragWindow();
                     }));
            }

            // 常にサイズ 0 で描画するとき、ドラッグ中に Window が点滅する問題があります。
            // この問題は、必ずしも起こるわけではありません。
            // 比較的負荷の大きいアプリケーションに Window を描画するときに起こります。
            // (恐らく) OnGUI メソッドは1回の更新で少なくとも 2 回実行されるためです。
            // この問題を解決するため、更新のたびに 1 度だけサイズ 0 で描画し、
            // 同じ更新時間にもう一度呼び出されるときは、描画済みのサイズで描画します。

            if (this.lastShowTime != Time.timeSinceLevelLoad)
            {
                this.windowRect = new Rect(this.windowRect.x, this.windowRect.y, 0, 0);
                this.lastShowTime = Time.timeSinceLevelLoad;
            }

            this.windowRect = UnityEngine.GUILayout.Window
                (this.WindowID, this.windowRect, windowFunction, title,
                 UnityEngine.GUILayout.MinWidth(this.MinWidth),
                 UnityEngine.GUILayout.MinHeight(this.MinHeight),
                 UnityEngine.GUILayout.MaxWidth(this.MaxWidth),
                 UnityEngine.GUILayout.MaxHeight(this.MaxHeight));

            return this.windowRect;
        }

        #endregion Method
    }
}
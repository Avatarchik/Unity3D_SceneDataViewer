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

        #endregion Field

        #region Property

        public int WindowID
        {
            get;
            private set;
        }

        public Rect WindowRect
        {
            get;
            private set;
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
        {
            // 現状では WindowID が重複してしまう恐れがあります。
            // キャストした結果、少数点が切り捨てられるため、同じ値が出現することがあります。

            this.WindowID = (int)Time.time;
            this.WindowRect = new Rect(x, y, 0, 0);
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
            UnityEngine.GUI.WindowFunction function =
                (UnityEngine.GUI.WindowFunction)UnityEngine.GUI.WindowFunction.Combine
                (windowFunction, new UnityEngine.GUI.WindowFunction(Drag));

            // 常にサイズ 0 で描画するとき、ドラッグ中に Window が点滅する問題があります。
            // この問題は、必ずしも起こるわけではありません。
            // 比較的負荷の大きいアプリケーションに Window を描画するときに起こります。
            // (恐らく) OnGUI メソッドは1回の更新で少なくとも 2 回実行されるためです。
            // この問題を解決するため、更新のたびに 1 度だけサイズ 0 で描画し、
            // 同じ更新時間にもう一度呼び出されるときは、描画済みのサイズで描画します。

            if (this.lastShowTime != Time.timeSinceLevelLoad)
            {
                this.WindowRect = UnityEngine.GUILayout.Window
                    (this.WindowID, new Rect(this.WindowRect.x, this.WindowRect.y, 0, 0), function, title);

                this.lastShowTime = Time.timeSinceLevelLoad;
            }
            else
            {
                this.WindowRect = UnityEngine.GUILayout.Window
                    (this.WindowID, this.WindowRect, function, title);
            }

            return this.WindowRect;
        }

        private void Drag(int windowId)
        {
            UnityEngine.GUI.DragWindow();
        }

        #endregion Method
    }
}
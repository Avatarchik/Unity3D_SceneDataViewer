using UnityEngine;

namespace XJUnity3D.GUI
{
    /// <summary>
    /// 描画内容に合わせて自動でサイズが可変する Window.
    /// </summary>
    public class FlexibleWindow
    {
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
            this.WindowRect = new Rect(x, y, -1, -1);
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
            // 毎回 0 で描画すると、Window をドラッグして移動するとき、描画がちらつく問題があります。
            // この問題は必ずしも起こるものではありませんが、負荷が大きいアプリケーションで起こりました。
            // 
            // 満たすべき条件は次の通りです。
            // 
            // - あるサイズから小さくなる場合。
            // - あるサイズから大きくなる場合。
            // - ドラッグ中に描画が乱れない。
            // 
            // 現在この問題をスマートに解決する方法が見つかっていません。
            // 
            // (1) 一度サイズ 0 で描画した後、得られた Rect で再描画する。
            //     - 解決できませんでした。
            // (2) 以前のサイズで一度描画した後、再度 0 で描画する。
            //     - 解決できませんでした。
            // (3) サイズの更新レートを落とす。2 フレームに 1 度サイズを更新するなどが考えられる。
            // (4) ドラッグ操作を独自に実装する。

            UnityEngine.GUI.WindowFunction function =
                (UnityEngine.GUI.WindowFunction)UnityEngine.GUI.WindowFunction.Combine
                (windowFunction, new UnityEngine.GUI.WindowFunction(Drag));

            this.WindowRect = UnityEngine.GUILayout.Window
                (this.WindowID, new Rect(this.WindowRect.x, this.WindowRect.y, 0, 0), function, title);

            return this.WindowRect;
        }

        private void Drag(int windowId)
        {
            UnityEngine.GUI.DragWindow();
        }

        #endregion Method
    }
}
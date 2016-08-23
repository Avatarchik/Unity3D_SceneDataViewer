namespace XJ.Unity3D.GUI
{
    /// <summary>
    /// bool 型の値を編集するための GUI.
    /// </summary>
    public class BoolGUI
    {
        #region Property

        public bool Value
        {
            get;
            set;
        }

        #endregion Property

        #region Constructor

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public BoolGUI()
            :this(false)
        {
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="value">
        /// 初期値。
        /// </param>
        public BoolGUI(bool value)
        {
            this.Value = value;
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// bool 型の値を制御するコントローラを描画します。
        /// </summary>
        /// <param name="title">
        /// コントローラのタイトル。null を指定するとき描画しません。
        /// </param>
        /// <returns>
        /// コントローラによって変更された値。
        /// </returns>
        public bool Controller(string title)
        {
            UnityEngine.GUILayout.BeginHorizontal();

            if (title != null)
            {
                UnityEngine.GUILayout.Label(title);
            }

            UnityEngine.GUILayout.FlexibleSpace();

            this.Value = UnityEngine.GUILayout.Toggle(this.Value, "");

            UnityEngine.GUILayout.EndHorizontal();

            return this.Value;
        }

        #endregion Method
    }
}
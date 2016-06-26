using UnityEngine;

namespace XJUnity3D.GUI
{
    /// <summary>
    /// Vector2 型の値を編集するための GUI.
    /// </summary>
    public class Vector2GUI
    {
        #region Field

        private Vector2 value;
        private FloatGUI x;
        private FloatGUI y;

        #endregion Field

        #region Property

        public string X
        {
            get
            {
                return this.x.Text;
            }
            set
            {
                this.x.Text = value;
                this.value.Set(this.x.Value, this.y.Value);
            }
        }

        public string Y
        {
            get
            {
                return this.y.Text;
            }
            set
            {
                this.y.Text = value;
                this.value.Set(this.x.Value, this.y.Value);
            }
        }

        public Vector2 Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.x.Value = value.x;
                this.y.Value = value.y;
                this.value = value;
            }
        }

        #endregion Property

        #region Constructor

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="value">
        /// 初期値。
        /// </param>
        public Vector2GUI(Vector2 value)
        {
            this.x = new FloatGUI(value.x);
            this.y = new FloatGUI(value.y);
            this.value = value;
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// Vector2 型の値を制御するコントローラを描画します。
        /// </summary>
        /// <param name="title">
        /// コントローラのタイトル。null を指定するとき描画しません。
        /// </param>
        /// <param name="textFieldWidth">
        /// テキストフィールドの幅。
        /// </param>
        /// <returns>
        /// コントローラによって変更された値。
        /// </returns>
        public Vector2 Controller(string title)
        {
            const int EXPAND_WIDTH = 0;

            UnityEngine.GUILayout.BeginVertical();

            if (title != null)
            {
                UnityEngine.GUILayout.Label(title);
            }

            UnityEngine.GUILayout.BeginHorizontal();

            this.value.Set(this.x.Controller("X", EXPAND_WIDTH),
                           this.y.Controller("Y", EXPAND_WIDTH));

            UnityEngine.GUILayout.EndHorizontal();

            UnityEngine.GUILayout.EndVertical();

            return this.value;
        }

        /// <summary>
        /// Vector2 型の値を制御するコントローラをスライダ付きで描画します。
        /// </summary>
        /// <param name="title">
        /// コントローラのタイトル。null を指定するとき描画しません。
        /// </param>
        /// <param name="minValue">
        /// スライダの最小値。
        /// </param>
        /// <param name="maxValue">
        /// スライダの最大値。
        /// </param>
        /// <returns>
        /// コントローラによって変更された値。
        /// </returns>
        public Vector2 Controller(string title, float minValue, float maxValue)
        {
            return Controller(title, minValue, maxValue, GUILayout.TextFieldWidth);
        }

        /// <summary>
        /// Vector2 型の値を制御するコントローラをスライダ付きで描画します。
        /// </summary>
        /// <param name="title">
        /// コントローラのタイトル。null を指定するとき描画しません。
        /// </param>
        /// <param name="minValue">
        /// スライダの最小値。
        /// </param>
        /// <param name="maxValue">
        /// スライダの最大値。
        /// </param>
        /// <param name="textFieldWidth">
        /// テキストフィールドの幅。
        /// </param>
        /// <returns>
        /// コントローラによって変更された値。
        /// </returns>
        public Vector2 Controller
            (string title, float minValue, float maxValue, float textFieldWidth)
        {
            UnityEngine.GUILayout.BeginVertical();

            if (title != null)
            {
                UnityEngine.GUILayout.Label(title);
            }

            this.value.Set(this.x.Controller("X", minValue, maxValue, textFieldWidth),
                           this.y.Controller("Y", minValue, maxValue, textFieldWidth));

            UnityEngine.GUILayout.EndVertical();

            return this.value;
        }
    }
        #endregion Method
}
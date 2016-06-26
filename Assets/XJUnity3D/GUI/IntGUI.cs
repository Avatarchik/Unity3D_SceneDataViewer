namespace XJUnity3D.GUI
{
    /// <summary>
    /// int 型の値を編集するための GUI.
    /// </summary>
    public class IntGUI
    {
        #region Field

        private int value;
        private string text;
        private bool textIsValid;

        #endregion Field

        #region Property

        public int Value
        {
            set
            {
                this.value = value;
                this.text = value.ToString();
            }
            get
            {
                return this.value;
            }
        }

        public string Text
        {
            set
            {
                this.text = value;

                int newValue;
                this.textIsValid = int.TryParse(this.text, out newValue);

                if (this.textIsValid)
                {
                    this.value = newValue;
                }

            }
            get
            {
                if (this.text == null)
                {
                    this.text = this.value.ToString();
                }

                return this.text;
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
        public IntGUI(int value)
        {
            this.Value = value;
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// int 型の値を制御するコントローラを描画します。
        /// </summary>
        /// <param name="title">
        /// コントローラのタイトル。null を指定するとき描画しません。
        /// </param>
        /// <returns>
        /// コントローラによって変更された値。
        /// </returns>
        public int Controller(string title)
        {
            return Controller(title, GUILayout.TextFieldWidth);
        }

        /// <summary>
        /// int 型の値を制御するコントローラを描画します。
        /// </summary>
        /// <param name="title">
        /// コントローラのタイトル。null を指定するとき描画しません。
        /// </param>
        /// <param name="textFieldWidth">
        /// テキストフィールドの幅。
        /// 0 以下の値を指定するとき、幅は最大になります。
        /// </param>
        /// <returns>
        /// コントローラによって変更された値。
        /// </returns>
        public int Controller(string title, float textFieldWidth)
        {
            UnityEngine.GUILayout.BeginHorizontal();

            if (title != null)
            {
                if (textFieldWidth <= 0)
                {
                    UnityEngine.GUIStyle rightAlignStyle =
                        new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label)
                        {
                            alignment = UnityEngine.TextAnchor.MiddleRight
                        };

                    UnityEngine.GUILayout.Label
                        (title, rightAlignStyle, UnityEngine.GUILayout.ExpandWidth(false));
                }
                else
                {
                    UnityEngine.GUILayout.Label(title);
                }
            }

            UnityEngine.GUILayoutOption layoutOption = null;

            if (textFieldWidth <= 0)
            {
                layoutOption = UnityEngine.GUILayout.ExpandWidth(true);
            }
            else
            {
                layoutOption = UnityEngine.GUILayout.Width(textFieldWidth);
            }

            this.Text = UnityEngine.GUILayout.TextField(this.Text, layoutOption);

            UnityEngine.GUILayout.EndHorizontal();

            return this.Value;
        }

        /// <summary>
        /// int 型の値を制御するコントローラをスライダ付きで描画します。
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
        public int Controller(string title, int minValue, int maxValue)
        {
            return Controller(title, minValue, maxValue, GUILayout.TextFieldWidth);
        }

        /// <summary>
        /// int 型の値を制御するコントローラをスライダ付きで描画します。
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
        /// 0 以下の値を指定するとき、幅は最大になります。
        /// </param>
        /// <returns>
        /// コントローラによって変更された値。
        /// </returns>
        public int Controller
            (string title, int minValue, int maxValue, float textFieldWidth)
        {
            UnityEngine.GUILayout.BeginVertical();

            UnityEngine.GUILayout.BeginHorizontal();

            if (title != null)
            {
                UnityEngine.GUILayout.Label(title);
            }

            UnityEngine.GUILayoutOption layoutOption = null;

            if (textFieldWidth <= 0)
            {
                layoutOption = UnityEngine.GUILayout.ExpandWidth(true);
            }
            else
            {
                layoutOption = UnityEngine.GUILayout.Width(textFieldWidth);
            }

            this.Text = UnityEngine.GUILayout.TextField(this.Text, layoutOption);

            UnityEngine.GUILayout.EndHorizontal();

            // 無効な数値が TextField に与えられているときは、
            // スライダの値が変更されたときのみ Value を更新する。

            int newValue = (int)UnityEngine.GUILayout.HorizontalSlider(this.Value, minValue, maxValue);

            if (this.textIsValid)
            {
                this.Value = newValue;
            }
            else
            {
                if (newValue != this.Value)
                {
                    this.Value = newValue;
                }
            }

            UnityEngine.GUILayout.EndVertical();

            return this.Value;
        }

        #endregion Method
    }
}
using UnityEngine;

namespace XJ.Unity3D.GUI
{
    /// <summary>
    /// IPAddress を編集するための GUI.
    /// </summary>
    public class IPv4GUI
    {
        #region Field

        private string value;
        private IntGUI x;
        private IntGUI y;
        private IntGUI z;
        private IntGUI w;

        #endregion Field

        #region Property

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                int[] values = ParseIPAddressText(value);
                this.x.Value = values[0];
                this.y.Value = values[1];
                this.z.Value = values[2];
                this.w.Value = values[3];
                this.value = values[0] + "." + values[1] + "." + values[2] + "." + values[3];
            }
        }

        #endregion Property

        #region Constructor

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public IPv4GUI()
            :this("0.0.0.0")
        {
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="value">
        /// 初期値。
        /// </param>
        public IPv4GUI(string value)
        {
            int[] values = ParseIPAddressText(value);

            this.x = new IntGUI(values[0]);
            this.y = new IntGUI(values[1]);
            this.z = new IntGUI(values[2]);
            this.w = new IntGUI(values[3]);

            this.value = x.Value + "." + y.Value + "." + z.Value + "." + w.Value;
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// IPAddress を示すテキストをパースします。
        /// </summary>
        /// <param name="ipAddressText">
        /// IPAddress.
        /// </param>
        /// <returns>
        /// IPAddress をパースした結果。
        /// 不正な値が与えられる IPAddress の領域には 0 が入ります。
        /// </returns>
        protected virtual int[] ParseIPAddressText(string ipAddressText)
        {
            string[] values = ipAddressText.Split('.');
            int[] intValues = new int[4];

            for (int i = 0; i < 4; i++)
            {
                intValues[i] = 0;    
            }

            for(int i = 0; i < values.Length && i < 4; i++)
            {
                int intValue = 0;
                int.TryParse(values[i], out intValue);
                intValues[i] = intValue;
            }

            return intValues;
        }

        /// <summary>
        /// IPv4 を制御するコントローラを描画します。
        /// </summary>
        /// <param name="title">
        /// コントローラのタイトル。null を指定するとき描画しません。
        /// </param>
        /// <returns>
        /// コントローラによって変更された値。
        /// </returns>
        public string Controller(string title)
        {
            const int EXPAND_WIDTH = 0;

            UnityEngine.GUILayout.BeginVertical();

            if (title != null)
            {
                UnityEngine.GUILayout.Label(title);
            }

            UnityEngine.GUILayout.BeginHorizontal();

            int x = this.x.Controller(null, EXPAND_WIDTH);
            UnityEngine.GUILayout.Label(".", GUILayout.LowerCenterAlignedLabelStyle);
            int y = this.y.Controller(null, EXPAND_WIDTH);
            UnityEngine.GUILayout.Label(".", GUILayout.LowerCenterAlignedLabelStyle);
            int z = this.z.Controller(null, EXPAND_WIDTH);
            UnityEngine.GUILayout.Label(".", GUILayout.LowerCenterAlignedLabelStyle);
            int w = this.w.Controller(null, EXPAND_WIDTH);

            this.value = x + "." + y + "." + z + "." + w;

            UnityEngine.GUILayout.EndHorizontal();

            UnityEngine.GUILayout.EndVertical();

            return this.value;
        }
    }
        #endregion Method
}
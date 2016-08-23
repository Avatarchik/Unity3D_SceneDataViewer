using System;
using System.Linq;

namespace XJ.Unity3D.GUI
{
    /// <summary>
    /// ツールバー。
    /// </summary>
    public class Toolbar
    {
        #region Field

        private int value;

        #endregion Field

        #region Property

        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        #endregion Property

        #region Constructor

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public Toolbar()
            :this(0)
        {
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="selectedToolbarIndex">
        /// 初期値。
        /// </param>
        public Toolbar(int selectedToolbarIndex)
        {
            this.value = selectedToolbarIndex;
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// ツールバー型の UILayout を描画します。
        /// </summary>
        /// <param name="title">
        /// コントローラのタイトル。null を指定するとき描画しません。
        /// </param>
        /// <param name="enumTypeToolNames">
        /// 各ツールの名前。
        /// </param>
        /// <returns>
        /// 選択されたツールの index. エラーが起こるとき -1.
        /// </returns>
        public int Controller(string title, Type enumTypeToolNames)
        {
            if (!enumTypeToolNames.IsEnum)
            {
                UnityEngine.GUILayout.Label("Type of \"enumTypeToolNames\" is not Enum.");

                return -1;
            }

            return Controller(title,
                              Enum.GetNames(enumTypeToolNames));
        }

        /// <summary>
        /// ツールバー型の UILayout を描画します。
        /// </summary>
        /// <param name="title">
        /// コントローラのタイトル。null を指定するとき描画しません。
        /// </param>
        /// <param name="toolNames">
        /// 各ツールの名前。
        /// </param>
        /// <returns>
        /// 選択されたツールの index. エラーが起こるとき -1.
        /// </returns>
        public int Controller(string title, int[] toolNames)
        {
            return Controller(title,
                              toolNames.Select(x => x.ToString()).ToArray());
        }

        /// <summary>
        /// ツールバー型の UILayout を描画します。
        /// </summary>
        /// <param name="title">
        /// コントローラのタイトル。null を指定するとき描画しません。
        /// </param>
        /// <param name="toolNames">
        /// 各ツールの名前。
        /// </param>
        /// <returns>
        /// 選択されたツールの index. エラーが起こるとき -1.
        /// </returns>
        public int Controller(string title, string[] toolNames)
        {
            if (toolNames == null || toolNames.Length == 0)
            {
                UnityEngine.GUILayout.Label("\"SelectedToolIndex\" is larger than length of \"toolNames\".");

                return -1;
            }

            UnityEngine.GUILayout.BeginVertical();

            if (title != null)
            {
                UnityEngine.GUILayout.Label(title);
            }

            UnityEngine.GUILayout.BeginHorizontal();

            this.value = UnityEngine.GUILayout.Toolbar(this.value, toolNames);

            UnityEngine.GUILayout.EndHorizontal();

            UnityEngine.GUILayout.EndVertical();

            return this.value;
        }

        #endregion Method
    }
}
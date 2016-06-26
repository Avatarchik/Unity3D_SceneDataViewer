using System;
using System.Linq;

namespace XJUnity3D.GUI
{
    /// <summary>
    /// ツールバー。
    /// </summary>
    public class Toolbar
    {
        #region Field

        private int selectedToolIndex;

        #endregion Field

        #region Property

        /// <summary>
        /// 現在選択されているツールのインデックスを取得、設定します。
        /// </summary>
        public int SelectedToolIndex
        {
            get
            {
                return this.selectedToolIndex;
            }
            set
            {
                this.selectedToolIndex = value;
            }
        }

        #endregion Property

        #region Constructor

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="selectedToolbarIndex0">
        /// 初期値。指定しないとき 0.
        /// </param>
        public Toolbar(int selectedToolbarIndex = 0)
        {
            this.selectedToolIndex = selectedToolbarIndex;
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

            this.selectedToolIndex = UnityEngine.GUILayout.Toolbar(this.selectedToolIndex, toolNames);

            UnityEngine.GUILayout.EndHorizontal();

            UnityEngine.GUILayout.EndVertical();

            return this.selectedToolIndex;
        }

        #endregion Method
    }
}
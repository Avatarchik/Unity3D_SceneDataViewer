using System;
using System.Linq;
using UnityEngine;

namespace XJUnity3D.GUI
{
    /// <summary>
    /// タブパネル。
    /// </summary>
    public class TabPanel
    {
        #region Field

        private int selectedTabIndex;

        #endregion Field

        #region Property

        /// <summary>
        /// 現在選択されているタブのインデックスを取得、設定します。
        /// </summary>
        public int SelectedTabIndex
        {
            get
            {
                return this.selectedTabIndex;
            }
            set
            {
                this.selectedTabIndex = value;
            }
        }

        #endregion Property

        #region Constructor

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="selectedTabIndex">
        /// 初期値。
        /// </param>
        public TabPanel(int selectedTabIndex)
        {
            this.selectedTabIndex = selectedTabIndex;
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// タブ型の UILayout を描画します。
        /// </summary>
        /// <param name="guiLayoutFunc">
        /// 各タブの内容を描画する関数。
        /// </param>
        /// <param name="tabNames">
        /// 各タブの名前。
        /// </param>
        /// <returns>
        /// 選択されたタブの index. エラーが起こるとき -1.
        /// </returns>
        public int Controller(Action[] guiLayoutFunc, Type enumTypeTabNames)
        {
            if (!enumTypeTabNames.IsEnum)
            {
                UnityEngine.GUILayout.Label("Type of \"enumTypeTabNames\" is not Enum.");

                return -1;
            }

            return Controller(guiLayoutFunc,
                              Enum.GetNames(enumTypeTabNames.GetType()));
        }

        /// <summary>
        /// タブ型の UILayout を描画します。
        /// </summary>
        /// <param name="guiLayoutFunc">
        /// 各タブの内容を描画する関数。
        /// </param>
        /// <param name="tabNames">
        /// 各タブの名前。
        /// </param>
        /// <returns>
        /// 選択されたタブの index. エラーが起こるとき -1.
        /// </returns>
        public int Controller(Action[] guiLayoutFunc, int[] tabNames)
        {
            return Controller(guiLayoutFunc,
                              tabNames.Select(x => x.ToString()).ToArray());
        }

        /// <summary>
        /// タブ型の UILayout を描画します。
        /// </summary>
        /// <param name="guiLayoutFunc">
        /// 各タブの内容を描画する関数。
        /// </param>
        /// <param name="tabNames">
        /// 各タブの名前。
        /// </param>
        /// <returns>
        /// 選択されたタブの index. エラーが起こるとき -1.
        /// </returns>
        public int Controller(Action[] guiLayoutFunc, string[] tabNames)
        {
            if (guiLayoutFunc.Length != tabNames.Length)
            {
                UnityEngine.GUILayout.Label("Length of guiLayoutFunc not equal to tabNames.");

                return -1;
            }
            if (guiLayoutFunc.Length <= this.selectedTabIndex)
            {
                UnityEngine.GUILayout.Label("SelectedTabIndex is larger than length of guiLayoutFunc.");

                return -1;
            }

            UnityEngine.GUILayout.BeginHorizontal();

            this.selectedTabIndex = UnityEngine.GUILayout.Toolbar
                (this.selectedTabIndex, tabNames, GUILayout.TabButtonStyle);

            UnityEngine.GUILayout.EndHorizontal();

            guiLayoutFunc[this.selectedTabIndex]();

            return this.selectedTabIndex;
        }

        private void TabPanelStyle()
        {
            Color previousBackgroundColor = UnityEngine.GUI.backgroundColor;
        }

        #endregion Method
    }
}
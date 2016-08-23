using System;
using System.Linq;
using System.Collections.Generic;

namespace XJ.Unity3D.GUI
{
    /// <summary>
    /// タブパネル。
    /// </summary>
    public class TabPanel
    {
        #region Field

        protected int value;

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
        public TabPanel()
            :this(0)
        {
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="selectedTabIndex">
        /// 初期値。
        /// </param>
        public TabPanel(int selectedTabIndex)
        {
            this.value = selectedTabIndex;
        }

        #endregion Constructor

        #region Method

        public int Controller(ICollection<Action> guiLayoutFunc, Type enumTypeTabNames)
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
        public int Controller(ICollection<Action> guiLayoutFunc, int[] tabNames)
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
        public int Controller(ICollection<Action> guiLayoutFunc, ICollection<string> tabNames)
        {
            if (guiLayoutFunc.Count != tabNames.Count)
            {
                UnityEngine.GUILayout.Label("Length of guiLayoutFunc not equal to tabNames.");

                return -1;
            }
            if (guiLayoutFunc.Count <= this.value)
            {
                UnityEngine.GUILayout.Label("SelectedTabIndex is larger than length of guiLayoutFunc.");

                return -1;
            }

            UnityEngine.GUILayout.BeginHorizontal();

            // InvalidCastException: Cannot cast from source type to destination type.	

            this.value = UnityEngine.GUILayout.Toolbar
                (this.value, (string[])tabNames, GUILayout.TabButtonStyle);

            UnityEngine.GUILayout.EndHorizontal();

            UnityEngine.GUILayout.BeginVertical(GUILayout.TabPanelStyle);

            guiLayoutFunc.ElementAt<Action>(this.value)();

            UnityEngine.GUILayout.EndVertical();

            return this.value;
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
        public int Controller(ICollection<Action> guiLayoutFunc, string[] tabNames)
        {
            if (guiLayoutFunc.Count != tabNames.Length)
            {
                UnityEngine.GUILayout.Label("Length of guiLayoutFunc not equal to tabNames.");

                return -1;
            }
            if (guiLayoutFunc.Count <= this.value)
            {
                UnityEngine.GUILayout.Label("SelectedTabIndex is larger than length of guiLayoutFunc.");

                return -1;
            }

            UnityEngine.GUILayout.BeginHorizontal();

            this.value = UnityEngine.GUILayout.Toolbar
                (this.value, tabNames, GUILayout.TabButtonStyle);

            UnityEngine.GUILayout.EndHorizontal();

            UnityEngine.GUILayout.BeginVertical(GUILayout.TabPanelStyle);

            guiLayoutFunc.ElementAt<Action>(this.value)();

            UnityEngine.GUILayout.EndVertical();

            return this.value;
        }

        #endregion Method
    }
}
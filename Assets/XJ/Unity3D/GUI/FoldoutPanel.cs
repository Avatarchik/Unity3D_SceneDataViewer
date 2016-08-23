using System;
using UnityEngine;

namespace XJ.Unity3D.GUI
{
    /// <summary>
    /// 開閉するパネル。
    /// </summary>
    public class FoldoutPanel
    {
        #region Field

        private bool value;

        #endregion Field

        #region Property

        public bool Value
        {
            set
            {
                this.value = value;
            }
            get
            {
                return this.value;
            }
        }

        #endregion Property

        #region Constructor

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public FoldoutPanel()
            :this(false)
        {
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="isOpen">
        /// 初期値。
        /// </param>
        public FoldoutPanel(bool isOpen)
        {
            this.value = isOpen;
        }

        #endregion Constructor

        #region Method

        // 一致するときに false を返し、一致しないときは true を返す。
        // 
        // isOpen = true  && push = true  => false
        // isOpen = true  && push = false => true
        // isOpen = false && push = true  => true
        // isOepn = false && push = false => false

        /// <summary>
        /// 開閉するパネル (Foldout) を描画します。
        /// </summary>
        /// <param name="title">
        /// パネルのタイトル。
        /// </param>
        /// <param name="guiLayoutFunc">
        /// パネルの内容を描画する関数。
        /// </param>
        /// <returns>
        /// パネルが開かれているかどうか。開かれているとき true.
        /// </returns>
        public bool Controller(string title, Action guiLayoutFunc)
        {
            return Controller(title, false, guiLayoutFunc);
        }

        /// <summary>
        /// 開閉するパネル (Foldout) を描画します。
        /// </summary>
        /// <param name="title">
        /// パネルのタイトル。
        /// </param>
        /// <param name="boldTitle">
        /// タイトルを太字にする。
        /// </param>
        /// <param name="guiLayoutFunc">
        /// パネルの内容を描画する関数。
        /// </param>
        /// <returns>
        /// パネルが開かれているかどうか。開かれているとき true.
        /// </returns>
        public bool Controller(string title, bool boldTitle, Action guiLayoutFunc)
        {
            title = (this.value ? "\u25BC " : "\u25BA ") + title;

            GUIStyle style = GUILayout.FoldoutPanelStyle;

            if (boldTitle)
            {
                style = GUILayout.FoldoutPanelBoldStyle;
            }

            bool push = UnityEngine.GUILayout.Button(title, style);

            this.value = !(value == push);

            if (this.value)
            {
                guiLayoutFunc();
            }

            return this.value;
        }

        #endregion Method
    }
}
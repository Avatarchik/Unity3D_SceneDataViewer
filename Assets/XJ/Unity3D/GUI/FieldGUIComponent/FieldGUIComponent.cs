using System;
using System.Reflection;

namespace XJ.Unity3D.GUI.FieldGUIComponents
{
    public class GUIAttribute : Attribute
    {
        public string Title { get; set; }
        public string TabTitle { get; set; }
        public string FoldoutTitle { get; set; }

        public float MaxValue { get; set; }
        public float MinValue { get; set; }
        public string[] EnumNames { get; set; }
        public bool IPv4 { get; set; }

        public bool TabClear { get; set; }
        public bool FoldoutClear { get; set; }

        public GUIAttribute()
        {
            this.Title = null;
            this.TabTitle = null;
            this.FoldoutTitle = null;

            this.MaxValue = float.NaN;
            this.MinValue = float.NaN;
            this.EnumNames = null;
        }
    }

    public abstract class FieldGUIComponent
    {
        #region Field

        public System.Object data
        {
            get;
            private set;
        }

        public FieldInfo fieldInfo
        {
            get;
            private set;
        }

        public GUIAttribute guiAttribute
        {
            get;
            private set;
        }

        #endregion Field

        #region Constructor

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="data">
        /// データ。
        /// </param>
        /// <param name="fieldInfo">
        /// データに含まれるフィールドの情報。
        /// </param>
        /// <param name="guiAttribute">
        /// フィールドに与えられた属性情報。
        /// </param>
        public FieldGUIComponent(System.Object data, FieldInfo fieldInfo)
            :this(data,
                  fieldInfo,
                  Attribute.GetCustomAttribute(fieldInfo, typeof(GUIAttribute)) as GUIAttribute)
        {
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="data">
        /// データ。
        /// </param>
        /// <param name="fieldInfo">
        /// データに含まれるフィールドの情報。
        /// </param>
        /// <param name="guiAttribute">
        /// フィールドに与えられた属性情報。
        /// </param>
        public FieldGUIComponent(System.Object data, FieldInfo fieldInfo, GUIAttribute guiAttribute)
        {
            this.data = data;
            this.fieldInfo = fieldInfo;
            this.guiAttribute = guiAttribute;

            // 派生の class はすべて Load メソッドを実行しますが、
            // 継承元の class で Load メソッドを実行するようにはできません。
            // Load メソッドは各 class に固有の GUI コンポーネントが
            // 初期化された状態で実行する必要があります。
        }

        #endregion Constrcutor

        #region Method

        public abstract void Controller();

        public abstract void Save();

        public abstract void Load();

        public string ToTitleCase(string text)
        {
            return char.ToUpper(text[0]) + text.Substring(1);

            // 先頭の文字が大文字になり、それ以外の文字が小文字になってしまうので注意する。
            //return System.Globalization.CultureInfo
            //        .CurrentCulture.TextInfo.ToTitleCase(text);
        }

        #endregion Method
    }
}
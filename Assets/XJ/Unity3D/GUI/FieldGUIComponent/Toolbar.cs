using System;
using System.Reflection;

namespace XJ.Unity3D.GUI.FieldGUIComponents
{
    public class Toolbar : FieldGUIComponent
    {
        #region Field

        private XJ.Unity3D.GUI.Toolbar gui;

        #endregion Field

        #region Constructor

        public Toolbar(System.Object data, FieldInfo fieldInfo, GUIAttribute guiAttribute)
            :base(data, fieldInfo, guiAttribute)
        {
            this.gui = new XJ.Unity3D.GUI.Toolbar();
            Load();
        }

        #endregion Constructor

        #region Method

        public override void Controller()
        {
            if (base.guiAttribute != null)
            {
                if (base.guiAttribute.EnumNames != null)
                {
                    this.gui.Controller(base.guiAttribute.Title, base.guiAttribute.EnumNames);
                }
                else
                {
                    this.gui.Controller(base.guiAttribute.Title, base.fieldInfo.FieldType);
                }
            }
            else
            {
                this.gui.Controller(ToTitleCase(base.fieldInfo.Name), base.fieldInfo.FieldType);
            }

            Save();
        }

        public override void Save()
        {
            base.fieldInfo.SetValue(base.data, this.gui.Value);
        }

        public override void Load()
        {
            this.gui.Value = (int)base.fieldInfo.GetValue(base.data);
        }

        #endregion Method
    }
}
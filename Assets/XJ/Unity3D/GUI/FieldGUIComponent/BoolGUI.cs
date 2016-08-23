using System;
using System.Reflection;

namespace XJ.Unity3D.GUI.FieldGUIComponents
{
    public class BoolGUI : FieldGUIComponent
    {
        #region Field

        private XJ.Unity3D.GUI.BoolGUI gui;

        #endregion Field

        #region Constructor

        public BoolGUI(System.Object data, FieldInfo fieldInfo, GUIAttribute guiAttribute)
            :base(data, fieldInfo, guiAttribute)
        {
            this.gui = new GUI.BoolGUI();
            Load();
        }

        #endregion Constructor

        #region Method

        public override void Controller()
        {
            if (base.guiAttribute != null)
            {
                this.gui.Controller(base.guiAttribute.Title);
            }
            else
            {
                this.gui.Controller(ToTitleCase(base.fieldInfo.Name));
            }

            Save();
        }

        public override void Save()
        {
            this.fieldInfo.SetValue(this.data, this.gui.Value);
        }

        public override void Load()
        {
            this.gui.Value = (bool)this.fieldInfo.GetValue(this.data);
        }

        #endregion Method
    }
}
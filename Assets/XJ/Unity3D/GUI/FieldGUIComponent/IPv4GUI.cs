using System.Reflection;
using UnityEngine;

namespace XJ.Unity3D.GUI.FieldGUIComponents
{
    public class IPv4GUI : FieldGUIComponent
    {
        #region Field

        private XJ.Unity3D.GUI.IPv4GUI gui;

        #endregion Field

        #region Constructor

        public IPv4GUI(System.Object data, FieldInfo fieldInfo)
            :base(data, fieldInfo)
        {
            this.gui = new GUI.IPv4GUI();
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
            base.fieldInfo.SetValue(base.data, this.gui.Value);
        }

        public override void Load()
        {
            this.gui.Value = (string)base.fieldInfo.GetValue(base.data);
        }

        #endregion Method
    }
}
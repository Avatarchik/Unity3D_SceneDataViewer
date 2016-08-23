using System.Reflection;

namespace XJ.Unity3D.GUI.FieldGUIComponents
{
    public class UnsupportedGUI : FieldGUIComponent
    {
        #region Constructor

        public UnsupportedGUI(System.Object data, FieldInfo fieldInfo, GUIAttribute guiAttribute)
            :base(data, fieldInfo, guiAttribute)
        {
        }

        #endregion Constructor

        #region Method

        public override void Controller()
        {
            UnityEngine.GUILayout.BeginHorizontal();
            UnityEngine.GUILayout.Label("Unsupported Field : " + ToTitleCase(base.fieldInfo.Name));
            UnityEngine.GUILayout.EndHorizontal();
        }

        public override void Save()
        {
            // Nothing to do.
        }

        public override void Load()
        {
            // Nothing to do.
        }

        #endregion Method
    }
}
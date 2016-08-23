using UnityEngine;
using XJGUI = XJ.Unity3D.GUI;

namespace SceneDataViewer
{
    public class SceneDataViewer : MonoBehaviour
    {
        #region Field

        public KeyCode toggleVisibilityKey = KeyCode.D;
        public bool visibility = true;

        public Color textColor = Color.white;
        public Color backgroundColor = Color.black;

        public TimeDataPanel timeDataPanel = new TimeDataPanel();
        #if DEVELOPMENT_BUILD || UNITY_EDITOR
        public MemoryDataPanel memoryDataPanel = new MemoryDataPanel();
        #endif
        public ObjectDataPanel objectDataPanel = new ObjectDataPanel();

        private XJGUI.FlexibleWindow window;

        #endregion Field

        #region Method

        void Awake()
        {
            this.window = new XJGUI.FlexibleWindow(10, 10, 350, 350, true);
            this.objectDataPanel.Initialize();
        }

        void Update()
        {
            if (Input.GetKeyDown(this.toggleVisibilityKey))
            {
                this.visibility = !this.visibility;
            }

            this.timeDataPanel.Update();

            if (this.visibility)
            {
                #if DEVELOPMENT_BUILD || UNITY_EDITOR
                this.memoryDataPanel.Update();
                #endif
                this.objectDataPanel.Update();
            }
        }

        void OnGUI()
        {
            if (!this.visibility)
            {
                return;
            }

            Color previousColor = GUI.color;
            GUI.color = this.textColor;

            this.window.Show("Scene Data Viewer", delegate (int windowId)
            {
                this.timeDataPanel.OnGUI();
                #if DEVELOPMENT_BUILD || UNITY_EDITOR
                this.memoryDataPanel.OnGUI();
                #endif
                this.objectDataPanel.OnGUI();
            });

            GUI.color = previousColor;
        }

        #endregion Method
    }
}
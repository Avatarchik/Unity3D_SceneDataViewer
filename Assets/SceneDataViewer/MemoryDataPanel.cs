using UnityEngine;
using XJGUI = XJ.Unity3D.GUI;
using XJ.NET;

namespace SceneDataViewer
{
    [System.Serializable]
    public class MemoryDataPanel : IDataPanel
    {
        #region Field

        // Memory に関連するデータを更新する時間を設定します。
        // 0.5 を指定するとき、0.5 sec おきに更新します。
        // 設定値が 0 より大きいときだけ関連するデータが表示されます。

        public float dataUpdateTimeInSec = 0;
        private float elapsedTimeToUpdateDataInSec = 0;

        private uint currentMemoryUsage = 0;
        private uint maxMemoryUsage = 0;

        private XJGUI.FoldoutPanel foldoutPanelMemoryUsage;

        #endregion Field

        #region Constructor

        public MemoryDataPanel()
        {
            Initialize();
        }

        #endregion Constructor

        #region Method

        #region Implement IDataPanel

        public void Initialize()
        {
            this.foldoutPanelMemoryUsage = new XJGUI.FoldoutPanel(false);
        }

        public void Update()
        {
            if (this.elapsedTimeToUpdateDataInSec < this.dataUpdateTimeInSec)
            {
                this.elapsedTimeToUpdateDataInSec += Time.deltaTime;
                return;
            }

            this.elapsedTimeToUpdateDataInSec = 0;

            this.currentMemoryUsage = Profiler.GetTotalReservedMemory();

            this.maxMemoryUsage =
                this.currentMemoryUsage > this.maxMemoryUsage ?
                this.currentMemoryUsage : this.maxMemoryUsage;
        }

        public void OnGUI()
        {
            if (this.dataUpdateTimeInSec == 0)
            {
                return;
            }

            string currentMemoryUsage = string.Format("Memory Usage : {0:f2} MB",
                                                      ByteToMByte(this.currentMemoryUsage));

            this.foldoutPanelMemoryUsage.Controller(currentMemoryUsage, () =>
            {
                GUILayout.Label(string.Format("― Max : {0:f2} MB", ByteToMByte(this.maxMemoryUsage)));
            });
        }

        #endregion IDataPanel

        public static float ByteToMByte(uint byteValue)
        {
            return byteValue / 1048576f;
        }

        #endregion Method
    }
}
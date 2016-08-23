using System;
using UnityEngine;
using XJGUI = XJ.Unity3D.GUI;

namespace SceneDataViewer
{
    [System.Serializable]
    public class TimeDataPanel : IDataPanel
    {
        #region Field

        // ある処理が繰り返されるようなシーンで活用します。
        // その処理 1 回あたりの実行時間を指定します。
        // 設定値が 0 より大きいときだけ関連するデータが表示されます。

        public int loopTimeSec = 0;

        // fps を更新する時間を設定します。
        // 0.5 を指定するとき、0.5 sec おきに更新します。
        // 設定値が 0 より大きいときだけ関連するデータが表示されます。

        public float fpsUpdateTimeInSec = 0;

        #region Data

        private DateTime startDateTime;

        private float timeSinceLoadInSec;

        private int timeSinceLoadDay;
        private int timeSinceLoadHour;
        private int timeSinceLoadMinute;
        private int timeSinceLoadSecond;

        private float timeSinceLoopInSec;
        private int loopCount;

        private int timeSinceLoopDay;
        private int timeSinceLoopHour;
        private int timeSinceLoopMinute;
        private int timeSinceLoopSecond;

        private float fps;
        private float elapsedTimeToUpdateFpsInSec;
        private float fpsFrameCount;

        private float maxFps;
        private float minFps;

        #endregion Data

        #region GUI

        private XJGUI.FoldoutPanel foldoutPanelFps;
        private XJGUI.FoldoutPanel foldoutPanelTimeSinceLoad;
        private XJGUI.FoldoutPanel foldoutPanelTimeSinceLoop;

        #endregion GUI

        #endregion Field

        #region Constructor

        public TimeDataPanel()
        {
            Initialize();
        }

        #endregion Constructor

        #region Implement IDataPanel

        public void Initialize()
        {
            this.startDateTime = DateTime.Now;
            InitializeGUI();
        }

        public void Update()
        {
            UpdateFpsData();
            UpdateTimeSinceLoadData();
            UpdateTimeSinceLoopData();
        }

        public void OnGUI()
        {
            ShowDateTimeData();
            ShowTimeSinceLoadData();
            ShowTimeSinceLoopData();
            ShowFpsData();
        }

        #endregion Implement IDataPanel

        private void InitializeGUI()
        {
            this.foldoutPanelFps = new XJGUI.FoldoutPanel(false);
            this.foldoutPanelTimeSinceLoad = new XJGUI.FoldoutPanel(false);
            this.foldoutPanelTimeSinceLoop = new XJGUI.FoldoutPanel(false);
        }

        #region Update Time Data

        private void UpdateFpsData()
        {
            if (this.fpsUpdateTimeInSec <= 0)
            {
                return;
            }

            this.fpsFrameCount += 1;

            if (this.elapsedTimeToUpdateFpsInSec < this.fpsUpdateTimeInSec)
            {
                this.elapsedTimeToUpdateFpsInSec += Time.deltaTime;

                return;
            }

            CalculateFps();

            this.elapsedTimeToUpdateFpsInSec = 0;
            this.fpsFrameCount = 0;
        }

        private void CalculateFps()
        {
            this.fps = this.fpsFrameCount / this.elapsedTimeToUpdateFpsInSec;

            if (this.fps > this.maxFps)
            {
                this.maxFps = this.fps;
            }

            if (this.minFps == 0)
            {
                this.minFps = this.fps;
            }

            if (this.fps < this.minFps)
            {
                this.minFps = this.fps;
            }
        }

        private void UpdateTimeSinceLoadData()
        {
            this.timeSinceLoadInSec = Time.timeSinceLevelLoad;

            ConvertTimeInSecToDayHourMinuteSec
                (this.timeSinceLoadInSec,
                 out this.timeSinceLoadDay,
                 out this.timeSinceLoadHour,
                 out this.timeSinceLoadMinute,
                 out this.timeSinceLoadSecond);
        }

        private void UpdateTimeSinceLoopData()
        {
            if (this.loopTimeSec <= 0)
            {
                return;
            }

            this.timeSinceLoopInSec = this.timeSinceLoadInSec % this.loopTimeSec;
            this.loopCount = (int)(this.timeSinceLoadInSec / this.loopTimeSec);

            ConvertTimeInSecToDayHourMinuteSec
                (timeSinceLoopInSec,
                 out this.timeSinceLoopDay,
                 out this.timeSinceLoopHour,
                 out this.timeSinceLoopMinute,
                 out this.timeSinceLoopSecond);
        }

        private void ConvertTimeInSecToDayHourMinuteSec
        (float timeInSec, out int day, out int hour, out int minute, out int sec)
        {
            const int MinuteInSec = 60;
            const int HourInSec = MinuteInSec * 60;
            const int DayInSec = HourInSec * 24;

            day = (int)Math.Floor(timeInSec / DayInSec);
            hour = (int)Math.Floor(timeInSec % DayInSec / HourInSec);
            minute = (int)Math.Floor(timeInSec % HourInSec / MinuteInSec);
            sec = (int)(timeInSec % MinuteInSec);
        }

        #endregion Update Time Data

        #region Show Time Data

        private void ShowDateTimeData()
        {
            GUILayout.Label("Start Date Time : " + this.startDateTime);
            GUILayout.Label("Current Date Time : " + DateTime.Now);
        }

        private void ShowFpsData()
        {
            if (this.fpsUpdateTimeInSec <= 0)
            {
                return;
            }

            this.foldoutPanelFps.Controller(string.Format("FPS : {0:f1}", this.fps), () =>
            {
                GUILayout.Label(string.Format("― Max FPS : {0:f1}", this.maxFps));
                GUILayout.Label(string.Format("― Min FPS : {0:f1}", this.minFps));
            });
        }

        private void ShowTimeSinceLoadData()
        {
            string timeSinceLoadTitle = "Time Since Load : " + this.timeSinceLoadDay + " d "
                                                             + this.timeSinceLoadHour + " h "
                                                             + this.timeSinceLoadMinute + " m "
                                                             + this.timeSinceLoadSecond + " s ";

            this.foldoutPanelTimeSinceLoad.Controller(timeSinceLoadTitle, () =>
            {
                GUILayout.Label(string.Format("― In Seconds : {0:f1} s", this.timeSinceLoadInSec));
            });
        }

        private void ShowTimeSinceLoopData()
        {
            if (this.loopTimeSec <= 0)
            {
                return;
            }

            string timeSinceLoopTitle = "Tiem Since Loop : " + this.timeSinceLoopDay + " d "
                                                             + this.timeSinceLoopHour + " h "
                                                             + this.timeSinceLoopMinute + " m "
                                                             + this.timeSinceLoopSecond + " s ";

            this.foldoutPanelTimeSinceLoop.Controller(timeSinceLoopTitle, () =>
            {
                GUILayout.Label(string.Format("― In Seconds : {0:f1} s", this.timeSinceLoopInSec));
                GUILayout.Label("― Loop Count : " + this.loopCount);
            });
        }

        #endregion Show Time Data
    }
}
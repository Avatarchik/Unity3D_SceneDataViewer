using System;
using UnityEngine;

public class SceneDataViewer : MonoBehaviour
{
    public Color textColor = Color.white;
    public Color backgroundColor = Color.black;

    // loopTimeSec は繰り返し実行するようなシーンで活用します。
    // loopTimeSec には、繰り返し1回あたりの実行時間を sec で指定します。
    // loopTimeSec が 0 より大きいときだけ関連するデータが表示されます。
    public int loopTimeSec = 0;

    // fpsRefreshTimeSec は fps を更新する時間を設定します。
    // 0.5 を指定するとき、0.5 sec おきに fps が更新されます。
    // fpsRefreshTimeSec が 0 より大きい時だけ関連するデータが表示されます。
    public float fpsRefreshTimeSec = 0;

    #region Time Data

    private DateTime startDateTime;

    private float timeSinceLoadInSec;

    private int timeSinceLoadDay;
    private int timeSinceLoadHour;
    private int timeSinceLoadMinute;
    private int timeSinceLoadSecond;

    private int timeSinceLoopInSec;
    private int loopCount;

    private int timeSinceLoopDay;
    private int timeSinceLoopHour;
    private int timeSinceLoopMinute;
    private int timeSinceLoopSecond;

    private float fps;
    private float fpsElapsedTime;
    private float fpsFrameCount;

    private float maxFps;
    private float minFps;

    #endregion Time Data

    #region Object Data

    private UnityEngine.Object[] currentAllObjects;
    private UnityEngine.Object[] currentTextures;
    private UnityEngine.Object[] currentAudioClips;
    private UnityEngine.Object[] currentMeshes;
    private UnityEngine.Object[] currentMaterials;
    private UnityEngine.Object[] currentGameObjects;
    private UnityEngine.Object[] currentComponents;

    private int currentAllObjectCount;
    private int currentTextureCount;
    private int currentAudioClipCount;
    private int currentMeshCount;
    private int currentMaterialCount;
    private int currentGameObjectCount;
    private int currentComponentCount;

    private int maxAllObjectCount;
    private int maxTextureCount;
    private int maxAudioClipCount;
    private int maxMeshCount;
    private int maxMaterialCount;
    private int maxGameObjectCount;
    private int maxComponentCount;

    private int minAllObjectCount;
    private int minTextureCount;
    private int minAudioClipCount;
    private int minMeshCount;
    private int minMaterialCount;
    private int minGameObjectCount;
    private int minComponentCount;

    #endregion Object data

    void Awake()
    {
        this.startDateTime = DateTime.Now;
    }

    void OnGUI()
    {
        Color previousColor = GUI.color;
        GUI.color = this.textColor;

        GUILayout.BeginArea(new Rect(10, 10, 1000, 1000));

        ShowTimeData();
        ShowObjectData();

        GUILayout.EndArea();

        GUI.color = previousColor;
    }

    private void ShowTimeData()
    {
        GUILayout.Label("Start Date Time : " + this.startDateTime);
        GUILayout.Label("Current Date Time : " + DateTime.Now);

        if (this.fpsRefreshTimeSec > 0)
        {
            CalculateFps();
            GUILayout.Label("FPS : " + this.fps);
            GUILayout.Label("― Max FPS : " + this.maxFps);
            GUILayout.Label("― Min FPS : " + this.minFps);
        }

        this.timeSinceLoadInSec = Time.timeSinceLevelLoad;

        ConvertTimeInSecToDayHourMinuteSec
            (this.timeSinceLoadInSec,
             out this.timeSinceLoadDay,
             out this.timeSinceLoadHour,
             out this.timeSinceLoadMinute,
             out this.timeSinceLoadSecond);

        GUILayout.Label("Tiem Since Load : " + this.timeSinceLoadDay + " d "
                                             + this.timeSinceLoadHour + " h "
                                             + this.timeSinceLoadMinute + " m "
                                             + this.timeSinceLoadSecond + " s ");
        GUILayout.Label("― Total : " + this.timeSinceLoadInSec + " s ");

        if (this.loopTimeSec > 0)
        {
            this.timeSinceLoopInSec = (int)(this.timeSinceLoadInSec % this.loopTimeSec);
            this.loopCount = (int)(this.timeSinceLoadInSec / this.loopTimeSec);

            ConvertTimeInSecToDayHourMinuteSec
                (timeSinceLoopInSec,
                 out this.timeSinceLoopDay,
                 out this.timeSinceLoopHour,
                 out this.timeSinceLoopMinute,
                 out this.timeSinceLoopSecond);

            GUILayout.Label("Tiem Since Loop : " + this.timeSinceLoopDay + " d "
                                                 + this.timeSinceLoopHour + " h "
                                                 + this.timeSinceLoopMinute + " m "
                                                 + this.timeSinceLoopSecond + " s ");
            GUILayout.Label("― Total : " + this.timeSinceLoopInSec + " s ");
            GUILayout.Label("― Loop Count : " + this.loopCount);
        }
    }

    private void CalculateFps()
    {
        if (this.fpsElapsedTime < this.fpsRefreshTimeSec)
        {
            this.fpsElapsedTime += Time.deltaTime;
            this.fpsFrameCount += 1;
        }
        else
        {
            this.fps = this.fpsFrameCount / this.fpsElapsedTime;
            this.fpsElapsedTime = 0;
            this.fpsFrameCount = 0;

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
    }

    private void ConvertTimeInSecToDayHourMinuteSec
        (float timeInSec, out int day, out int hour, out int minute, out int sec)
    {
        const int MinuteInSec = 60;
        const int HourInSec = MinuteInSec * 60;
        const int DayInSec = HourInSec * 24;

        day =    (int)Math.Floor(timeInSec / DayInSec);
        hour =   (int)Math.Floor(timeInSec % DayInSec / HourInSec);
        minute = (int)Math.Floor(timeInSec % HourInSec / MinuteInSec);
        sec =    (int)(timeInSec % MinuteInSec);
    }

    private void ShowObjectData()
    {
        this.currentAllObjects  = Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object));
        this.currentTextures    = Resources.FindObjectsOfTypeAll(typeof(Texture));
        this.currentAudioClips  = Resources.FindObjectsOfTypeAll(typeof(AudioClip));
        this.currentMeshes      = Resources.FindObjectsOfTypeAll(typeof(Mesh));
        this.currentMaterials   = Resources.FindObjectsOfTypeAll(typeof(Material));
        this.currentGameObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        this.currentComponents  = Resources.FindObjectsOfTypeAll(typeof(Component));

        this.currentAllObjectCount  = this.currentAllObjects.Length;
        this.currentTextureCount    = this.currentTextures.Length;
        this.currentAudioClipCount  = this.currentAudioClips.Length;
        this.currentMeshCount       = this.currentMeshes.Length;
        this.currentMaterialCount   = this.currentMaterials.Length;
        this.currentGameObjectCount = this.currentGameObjects.Length;
        this.currentComponentCount  = this.currentComponents.Length;

        GUILayout.Label("All : "        + this.currentAllObjectCount);
        GUILayout.Label("Texture : "    + this.currentTextureCount);
        GUILayout.Label("AudioClip : "  + this.currentAudioClipCount);
        GUILayout.Label("Mesh : "       + this.currentMeshCount);
        GUILayout.Label("Material : "   + this.currentMaterialCount);
        GUILayout.Label("GameObject : " + this.currentGameObjectCount);
        GUILayout.Label("Component : "  + this.currentComponentCount);
    }
}
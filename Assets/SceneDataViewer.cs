using System;
using UnityEngine;
using XJGUI = XJUnity3D.GUI;

public class SceneDataViewer : MonoBehaviour
{
    public bool visibility = true;

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

    private float timeSinceLoopInSec;
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

    #region GUI

    private XJGUI.FlexibleWindow window;

    private XJGUI.FoldoutPanel foldoutPanelFps;
    private XJGUI.FoldoutPanel foldoutPanelTimeSinceLoad;
    private XJGUI.FoldoutPanel foldoutPanelTimeSinceLoop;
    private XJGUI.FoldoutPanel foldoutPanelAllObject;
    private XJGUI.FoldoutPanel foldoutPanelTexture;

    private XJGUI.FoldoutPanel foldoutPanelAudioClip;
    private XJGUI.FoldoutPanel foldoutPanelMesh;
    private XJGUI.FoldoutPanel foldoutPanelMaterial;
    private XJGUI.FoldoutPanel foldoutPanelGameObject;
    private XJGUI.FoldoutPanel foldoutPanelComponent;

    #endregion GUI

    #region Method

    void Awake()
    {
        InitializeGUI();

        this.startDateTime = DateTime.Now;

        // 実行時に一度だけ、すべての最小リソース数を現在のリソース数で更新しておきます。
        // そうしなければ、常に 0 が表示されてしまうためです。

        UpdateSceneObjectsReferences();
        UpdateCurrentObjectCount();
        InitializeMinObjectCount();
    }

    void InitializeGUI()
    {
        this.window = new XJGUI.FlexibleWindow(10, 10, 350, 350, true);

        this.foldoutPanelFps = new XJGUI.FoldoutPanel(false);
        this.foldoutPanelTimeSinceLoad = new XJGUI.FoldoutPanel(false);
        this.foldoutPanelTimeSinceLoop = new XJGUI.FoldoutPanel(false);
        this.foldoutPanelAllObject = new XJGUI.FoldoutPanel(false);
        this.foldoutPanelTexture = new XJGUI.FoldoutPanel(false);

        this.foldoutPanelAudioClip = new XJGUI.FoldoutPanel(false);
        this.foldoutPanelMesh = new XJGUI.FoldoutPanel(false);
        this.foldoutPanelMaterial = new XJGUI.FoldoutPanel(false);
        this.foldoutPanelGameObject = new XJGUI.FoldoutPanel(false);
        this.foldoutPanelComponent = new XJGUI.FoldoutPanel(false);
    }

    void OnGUI()
    {
        if (!this.visibility)
        {
            return;
        }

        Color previousColor = GUI.color;
        GUI.color = this.textColor;

        this.window.Show("Scene Data Viewer", delegate(int windowId)
        {
            ShowTimeData();
            ShowObjectData();
        });

        GUI.color = previousColor;
    }

    private void ShowTimeData()
    {
        ShowDateTimeData();
        ShowFpsData();
        ShowTimeSinceLoadData();
        ShowTimeSinceLoopData();
    }

    private void ShowDateTimeData()
    {
        GUILayout.Label("Start Date Time : " + this.startDateTime);
        GUILayout.Label("Current Date Time : " + DateTime.Now);
    }

    private void ShowFpsData()
    {
        if (this.fpsRefreshTimeSec <= 0)
        {
            return;
        }

        CalculateFps();

        this.foldoutPanelFps.Controller(string.Format("FPS : {0:f1}", this.fps), () =>
        {
            GUILayout.Label(string.Format("― Max FPS : {0:f1}", this.maxFps));
            GUILayout.Label(string.Format("― Min FPS : {0:f1}", this.minFps));
        });
    }

    private void ShowTimeSinceLoadData()
    {
        this.timeSinceLoadInSec = Time.timeSinceLevelLoad;

        ConvertTimeInSecToDayHourMinuteSec
            (this.timeSinceLoadInSec,
             out this.timeSinceLoadDay,
             out this.timeSinceLoadHour,
             out this.timeSinceLoadMinute,
             out this.timeSinceLoadSecond);

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

        this.timeSinceLoopInSec = this.timeSinceLoadInSec % this.loopTimeSec;
        this.loopCount = (int)(this.timeSinceLoadInSec / this.loopTimeSec);

        ConvertTimeInSecToDayHourMinuteSec
            (timeSinceLoopInSec,
                out this.timeSinceLoopDay,
                out this.timeSinceLoopHour,
                out this.timeSinceLoopMinute,
                out this.timeSinceLoopSecond);

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
        UpdateSceneObjectsReferences();
        UpdateCurrentObjectCount();
        UpdateMinObjectCount();
        UpdateMaxObjectCount();

        this.foldoutPanelAllObject.Controller("All : " + this.currentAllObjectCount, () =>
        {
            GUILayout.Label("― Max : " + this.maxAllObjectCount);
            GUILayout.Label("― Min : " + this.minAllObjectCount);
        });

        this.foldoutPanelTexture.Controller("Texture : " + this.currentTextureCount, () =>
        {
            GUILayout.Label("― Max : " + this.maxTextureCount);
            GUILayout.Label("― Min : " + this.minTextureCount);
        });

        this.foldoutPanelAudioClip.Controller("AudioClip : " + this.currentAudioClipCount, () =>
        {
            GUILayout.Label("― Max : " + this.maxAudioClipCount);
            GUILayout.Label("― Min : " + this.minAudioClipCount);
        });

        this.foldoutPanelMesh.Controller("Mesh : " + this.currentMeshCount, () =>
        {
            GUILayout.Label("― Max : " + this.maxMeshCount);
            GUILayout.Label("― Min : " + this.minMeshCount);
        });

        this.foldoutPanelMaterial.Controller("Material : " + this.currentMaterialCount, () =>
        {
            GUILayout.Label("― Max : " + this.maxMaterialCount);
            GUILayout.Label("― Min : " + this.minMaterialCount);
        });

        this.foldoutPanelGameObject.Controller("GameObject : " + this.currentGameObjectCount, () =>
        {
            GUILayout.Label("― Max : " + this.maxGameObjectCount);
            GUILayout.Label("― Min : " + this.minGameObjectCount);
        });

        this.foldoutPanelComponent.Controller("Component : " + this.currentComponentCount, () =>
        {
            GUILayout.Label("― Max : " + this.maxComponentCount);
            GUILayout.Label("― Min : " + this.minComponentCount);
        });
    }

    private void UpdateSceneObjectsReferences()
    {
        this.currentAllObjects = Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object));
        this.currentTextures = Resources.FindObjectsOfTypeAll(typeof(Texture));
        this.currentAudioClips = Resources.FindObjectsOfTypeAll(typeof(AudioClip));
        this.currentMeshes = Resources.FindObjectsOfTypeAll(typeof(Mesh));
        this.currentMaterials = Resources.FindObjectsOfTypeAll(typeof(Material));
        this.currentGameObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        this.currentComponents = Resources.FindObjectsOfTypeAll(typeof(Component));
    }

    private void UpdateCurrentObjectCount()
    {
        this.currentAllObjectCount = this.currentAllObjects.Length;
        this.currentTextureCount = this.currentTextures.Length;
        this.currentAudioClipCount = this.currentAudioClips.Length;
        this.currentMeshCount = this.currentMeshes.Length;
        this.currentMaterialCount = this.currentMaterials.Length;
        this.currentGameObjectCount = this.currentGameObjects.Length;
        this.currentComponentCount = this.currentComponents.Length;
    }

    private void UpdateMinObjectCount()
    {
        if (this.currentAllObjectCount < this.minAllObjectCount)
        {
            this.minAllObjectCount = this.currentAllObjectCount;
        }
        if (this.currentTextureCount < this.minTextureCount)
        {
            this.minTextureCount = this.currentTextureCount;
        }
        if (this.currentAudioClipCount < this.minAudioClipCount)
        {
            this.minAudioClipCount = this.currentAudioClipCount;
        }
        if (this.currentMeshCount < this.minMeshCount)
        {
            this.minMeshCount = this.currentMeshCount;
        }
        if (this.currentMaterialCount < this.minMaterialCount)
        {
            this.minMaterialCount = this.currentMaterialCount;
        }
        if (this.currentGameObjectCount < this.minGameObjectCount)
        {
            this.minGameObjectCount = this.currentGameObjectCount;
        }
        if (this.currentComponentCount < this.minComponentCount)
        {
            this.minComponentCount = this.currentComponentCount;
        }
    }

    private void UpdateMaxObjectCount()
    {
        if (this.currentAllObjectCount > this.maxAllObjectCount)
        {
            this.maxAllObjectCount = this.currentAllObjectCount;
        }
        if (this.currentTextureCount > this.maxTextureCount)
        {
            this.maxTextureCount = this.currentTextureCount;
        }
        if (this.currentAudioClipCount > this.maxAudioClipCount)
        {
            this.maxAudioClipCount = this.currentAudioClipCount;
        }
        if (this.currentMeshCount > this.maxMeshCount)
        {
            this.maxMeshCount = this.currentMeshCount;
        }
        if (this.currentMaterialCount > this.maxMaterialCount)
        {
            this.maxMaterialCount = this.currentMaterialCount;
        }
        if (this.currentGameObjectCount > this.maxGameObjectCount)
        {
            this.maxGameObjectCount = this.currentGameObjectCount;
        }
        if (this.currentComponentCount > this.maxComponentCount)
        {
            this.maxComponentCount = this.currentComponentCount;
        }
    }

    private void InitializeMinObjectCount()
    {
        this.minAllObjectCount = this.currentAllObjectCount;
        this.minTextureCount = this.currentTextureCount;
        this.minAudioClipCount = this.currentAudioClipCount;
        this.minMeshCount = this.currentMeshCount;
        this.minMaterialCount = this.currentMaterialCount;
        this.minGameObjectCount = this.currentGameObjectCount;
        this.minComponentCount = this.currentComponentCount;
    }

    #endregion Method
}
using UnityEngine;
using System.Collections;

public class SceneDataViewer : MonoBehaviour
{
    public Color TextColor = Color.white;
    public int loopSec = 0;

    void OnGUI()
    {
        float timeLoad = Time.timeSinceLevelLoad;
        int timeLoadMinute = (int)(timeLoad / 60f);
        int timeLoadSecond = (int)(timeLoad % 60f);

        float timeLoop = 0;
        int loopCount = 0;
        int timeLoopMinute = 0;
        int timeLoopSecond = 0;

        if (this.loopSec != 0)
        {
            timeLoop = timeLoad % this.loopSec;
            loopCount = (int)(timeLoad / this.loopSec);
            timeLoopMinute = (int)(timeLoop / 60f);
            timeLoopSecond = (int)(timeLoop % 60f);
        }

        Color previousColor = GUI.color;
        GUI.color = this.TextColor;

        GUILayout.BeginArea(new Rect(10, 10, 1000, 1000));
        GUILayout.Label("Time(Load) : " + timeLoadMinute + "m" + timeLoadSecond + "s");
        if (this.loopSec != 0)
        {
            GUILayout.Label("Time(Loop) : " + timeLoopMinute + "m" + timeLoopSecond + "s");
            GUILayout.Label("LoopCount : " + loopCount);
        }
        GUILayout.Label("All : " + Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)).Length);
        GUILayout.Label("Texture : " + Resources.FindObjectsOfTypeAll(typeof(Texture)).Length);
        GUILayout.Label("AudioClip : " + Resources.FindObjectsOfTypeAll(typeof(AudioClip)).Length);
        GUILayout.Label("Meshe : " + Resources.FindObjectsOfTypeAll(typeof(Mesh)).Length);
        GUILayout.Label("Material : " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
        GUILayout.Label("GameObject : " + Resources.FindObjectsOfTypeAll(typeof(GameObject)).Length);
        GUILayout.Label("Component : " + Resources.FindObjectsOfTypeAll(typeof(Component)).Length);
        GUILayout.EndArea();

        GUI.color = previousColor;
    }
}
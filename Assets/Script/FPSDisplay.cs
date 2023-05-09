using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        int width = Screen.width, height = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, width, height * 2 / 100);
        style.fontSize = height * 2 / 100;
        style.normal.textColor = new Color(1f, 1f, 1f, 1.0f);
        float milliSec = deltaTime * 1000f;
        float fps = 1f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", milliSec, fps);
        GUI.Label(rect, text, style);
    }
}

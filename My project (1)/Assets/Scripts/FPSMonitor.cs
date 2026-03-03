using UnityEngine;

public class FPSMonitor : MonoBehaviour
{
    GUIStyle fpsGuistyle;
    float deltaTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fpsGuistyle = new GUIStyle();
        fpsGuistyle.fontSize = 20;
        fpsGuistyle.normal.textColor = Color.white;
    }

    // Update is called once per frame
    void OnGUI()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        GUI.Label(new Rect(10, 10, 200, 80), fps.ToString("0.") + " FPS", fpsGuistyle);
    }
}
using UnityEngine;
using System.Collections;

public class FPSManager : MonoBehaviour {
    // TIME
    public TextMesh text;
    private float time = 0;

    public bool check = false;

    // FPS 
    private float timeLeft = 0.0f;
    private float accum = 0.0f;
    private int frames = 0;
    private string strFPS = null;
    private float updateInterval = 0.5f;

    void Update()
    {
        text.text = strFPS;
        time += Time.deltaTime;
        UpdateFPS();
    }


    void UpdateFPS()
    {
        timeLeft -= Time.unscaledDeltaTime;
        accum += Time.unscaledDeltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeLeft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = frames / accum;
            if(fps < 40)
            {
                Debug.LogError("Low FPS");                
            }
                
            strFPS = System.String.Format("FPS: {0:F2}", fps);

            timeLeft += updateInterval;
            accum = 0.0f;
            frames = 0;
        }        
    }
}

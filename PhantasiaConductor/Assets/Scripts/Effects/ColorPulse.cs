using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// changes color with beats
[RequireComponent(typeof(BeatInfo), typeof(Renderer))]
public class ColorPulse : MonoBehaviour
{
    public float alpha = 1.0f;

    static float numBeats = 16;
    static float timeBetweenPulse = MasterLoop.loopTime / numBeats;

    private BeatInfo beatInfo;
    private int beatIndex;
    private int colorIndex;
    private Color[] colors;

    private void Awake()
    {
        beatIndex = 0;
        colorIndex = 0;
        beatInfo = GetComponent<BeatInfo>();
        colors = new Color[6] { Color.cyan, Color.red, Color.yellow, Color.green, Color.blue, Color.magenta };
    }

    void Start() {
        ChangeColor();
    }

    void BeatTick() {
        if (beatInfo.beats[beatIndex % beatInfo.beats.Length])
        {
            ChangeColor();
        }
        beatIndex++;
        Invoke("BeatTick", beatInfo.beatTime);
    }

    void ChangeColor() {
        // Color c = ColorGenerator.GenerateColor();
        Color c = colors[colorIndex % colors.Length];
        c.a = alpha;
        colorIndex++;

        Renderer renderer = GetComponent<Renderer>();
        
        renderer.material.color = c;
    }

    public void NewLoop() {
        CancelInvoke();
        // beatIndex = 0;
        ChangeColor();
        Invoke("BeatTick", timeBetweenPulse);
    }
}

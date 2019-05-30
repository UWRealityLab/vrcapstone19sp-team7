using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FantasiaController : MonoBehaviour
{
    public UnityEvent BattonTrails;
    public UnityEvent ObjectTracking;
    public UnityEvent Confetti;
    public UnityEvent Walls;
    public UnityEvent WallsOff;
    public UnityEvent ObjectTrackingOff;

    private MasterLoop masterLoop;
    private float normal = 63.0f;
    private float measureTIme;
    private float beatTIme;

    private int measureCount;

    private void Awake()
    {
        masterLoop = GetComponent<MasterLoop>();
    }

    public void StartFantasia()
    {
        measureCount = 0;
        measureTIme = normal / 29.0f;
        beatTIme = measureTIme / 4.0f;
        MasterLoop.loopTime = beatTIme * 8;
        GetComponent<AudioSource>().Play();
        MeasureLoop();
    }

    private void MeasureLoop()
    {
        if (measureCount % 2 == 0)
        {
            masterLoop.NewLoop();
        }
        measureCount++;

        if (measureCount == 1)
        {
            BattonTrails.Invoke();
        } else if (measureCount == 5)
        {
            ObjectTracking.Invoke();
        } else if (measureCount == 17)
        {
            ObjectTrackingOff.Invoke();
            Walls.Invoke();
        } else if (measureCount == 20)
        {
            ObjectTracking.Invoke();
        } else if (measureCount == 24)
        {
            Confetti.Invoke();
        } else if (measureCount == 26)
        {
            WallsOff.Invoke();
        }

        Invoke("MeasureLoop", measureTIme);
    }
}

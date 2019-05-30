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

    private MasterLoop masterLoop;
    private float normal = 63.0f;
    private float measureTIme;
    private float beatTIme;

    private int measureCount;

    private void Awake()
    {
        masterLoop = GetComponent<MasterLoop>();
        measureCount = 0;
        measureTIme = normal / 29.0f;
        beatTIme = measureTIme / 4.0f;
        MasterLoop.loopTime = beatTIme * 8; 
    }

    public void StartFantasia()
    {
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
        } else if (measureCount == 14)
        {
            Walls.Invoke();
        } else if (measureCount == 24)
        {
            Confetti.Invoke();
        }

        Invoke("MeasureLoop", measureTIme);
    }
}

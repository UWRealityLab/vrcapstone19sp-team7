using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FantasiaController : MonoBehaviour
{
    public GameObject tempoController;

    // Pause puzzle effects
    public UnityEvent PausePuzzles;

    // Resume puzzle effects
    public UnityEvent ResumePuzzles;

    // Turn effects on
    public UnityEvent BattonTrails;
    public UnityEvent ObjectTracking;
    public UnityEvent Confetti;
    public UnityEvent WallsOn;
    public UnityEvent WallsPulseResume;

    // Turn effects off
    public UnityEvent TrailOff;
    public UnityEvent WallsOff;
    public UnityEvent WallsPulseOff; // Stops pusling sequence and sets to random colors
    public UnityEvent WallsPulsePause;
    public UnityEvent ObjectTrackingOff;
    public UnityEvent ConfettiOff;


    // End of Fantasia
    public UnityEvent onFinish;

    private MasterLoop masterLoop;
    private TempoController tempo;
    private float normal = 56.0f; // song length
    private float measureTime;
    private float beatTIme;
    private float prevMeasureTime;

    // For pausing and restarting
    private float delay;

    // For timing effects
    private int measureCount;

    // Keep track of which effects are on
    private bool trail = false;
    public bool tracking = false;
    private bool wallsPulse = false;
    private bool walls = false; 
    private bool confetti = false;

    // Start wall pulse on measure
    private bool resumePulse = false;

    private void Awake()
    {
        masterLoop = GetComponent<MasterLoop>();
        tempo = tempoController.GetComponent<TempoController>();
    }

    public void StartFantasia()
    {
        // Set tempo/timing metrics
        measureCount = 0;
        measureTime = normal / 28.0f;
        beatTIme = measureTime / 4.0f;
        // MasterLoop.loopTime = beatTIme * 8;

        GetComponent<AudioSource>().Play();
        prevMeasureTime = Time.time;

        tempoController.SetActive(true);

        MeasureLoop();
    }

    private void MeasureLoop()
    {
        // resume wall pulse at beginning of measure if stopped
        if (resumePulse)
        {
            resumePulse = false;
            WallsPulseResume.Invoke();
        }
      
        if (measureCount % 2 == 0)
        {
            masterLoop.NewLoop();
        }
        measureCount++;

        if (measureCount == 1)
        {
            BattonTrails.Invoke();
            trail = true;
        } else if (measureCount == 7)
        {
            ObjectTracking.Invoke();
            tracking = true;
        } else if (measureCount == 19)
        {
            ObjectTrackingOff.Invoke();
            WallsOn.Invoke();

            tracking = false;
            wallsPulse = true;
            walls = true;
        } else if (measureCount == 21)
        {
            ObjectTracking.Invoke();
            tracking = true;
        } else if (measureCount == 24)
        {
            Confetti.Invoke();
            confetti = true;
        } else if (measureCount == 26)
        {
            WallsPulseOff.Invoke();
            wallsPulse = false;
        } else if (measureCount == 30)
        {
            ObjectTrackingOff.Invoke();
            onFinish.Invoke();
        }

        prevMeasureTime = Time.time;
        Invoke("MeasureLoop", measureTime);
    }

    // Pause effects and stop music
    public void PauseFantasia()
    {
        CancelInvoke();

        // To keep track of when to call MeasureLoop next
        delay = measureTime - (Time.time - prevMeasureTime);
        if (delay < 0) { delay = 0; }

        tempo.StopPlaying();
        PausePuzzles.Invoke();
        EffectsOff();
    }

    // Resume effects and music
    public void ResumeFantasia()
    {
        Invoke("MeasureLoop", delay);
        tempo.StartPlaying();
        ResumePuzzles.Invoke();
        EffectsOn();
    }

    private void EffectsOff()
    {
        if (trail)
        {
            TrailOff.Invoke();
        }
        if (tracking)
        {
            ObjectTrackingOff.Invoke();
        }
        if (wallsPulse)
        {
            WallsPulsePause.Invoke();
        }
        if (walls)
        {
            WallsOff.Invoke();
        }
        if (confetti)
        {
            ConfettiOff.Invoke();
        }
    }

    private void EffectsOn()
    {
        if (trail)
        {
            BattonTrails.Invoke();
        }
        if (tracking)
        {
            ObjectTracking.Invoke();
        }
        if (wallsPulse)
        {
            resumePulse = true; // so turn on pulse at beginning of measure
        }
        if (walls)
        {
            WallsOn.Invoke();
        }
        if (confetti)
        {
            Confetti.Invoke();
        }
    }
}

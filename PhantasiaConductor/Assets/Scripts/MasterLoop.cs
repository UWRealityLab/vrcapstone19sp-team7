using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MasterLoop : MonoBehaviour
{
    public UnityEvent onNewLoop;
    public static float loopTime = 4f;
    public static float standardLoopTime = 4f; //The 'default' time for one loop (audio rendered at this tempo)
    public static float delay = 0;

    public static int loopCount = 0;

    public bool fantasiaOn = false;

    private bool startFantasia = true;
    //Alternate idea - store map of points in time to events, check every update (would allow arbitrary tempo changes mid loop)

    private void OnEnable()
    {
        Invoke("NewLoop", delay);
    }

    public void NewLoop()
    {
        onNewLoop.Invoke();
        loopCount++;
        if (!fantasiaOn)
        {
            Invoke("NewLoop", loopTime);
        } else if (startFantasia)
        {
            startFantasia = false;
            Invoke("StartFantasia", loopTime);
        }
    }

    private void StartFantasia()
    {
        StartCoroutine(GetComponent<StartFantasia>().StartFantasiaSection());
    }

    public bool FantasiaOn
    {
        set
        {
            fantasiaOn = value;
        }
        get
        {
            return fantasiaOn;
        }
    }
}

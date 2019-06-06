using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyObject : MonoBehaviour
{
    public Material windowOnMat;
    public Material windowOffMat;

    // mat to use while successfully following
    public Material trackingMat;

    // mat to use after failed
    public Material failMat;

    public Material unlockedMat;

    public PathBeat pathBeat;
    
    public float windowLength = 1f;
    public bool unlocked = false;

    public float beatOffset = 0;
    public bool fantasiaOn;

    private AudioSource loopSource;
    private Collider coll;
    private MeshRenderer rend;

    private Hittable hittable;

    private bool windowStatus = false;


    void Awake()
    {
        coll = GetComponent<Collider>();
        rend = GetComponent<MeshRenderer>();
        loopSource = GetComponent<AudioSource>();
        rend.material = windowOffMat;

        hittable = GetComponent<Hittable>();
    }

    void Start()
    {
        hittable.canInteract = false;
        if (gameObject.activeInHierarchy)
        {
            rend.enabled = true;
        }

        // right now just hardcode the default masterloop time of 4.0f
        // float delta = 4.0f / beatInfo.numBeats;
        // int i = 0;

        // find the first beat and use that as the offset
        // foreach (var b in beatInfo.beats) {
        //     if (b) {
        //         break;
        //     }
        //     i++;
        // }

        // when using beat infos
        // beatOffset = i * delta;
    }

    public void NewLoop()
    {
        if (gameObject.activeInHierarchy)
        {
            // just keep looping if unlocked
            if (unlocked)
            {
                pathBeat.Invoke("ResetPosition", beatOffset);
                if (!fantasiaOn)
                {
                    loopSource.Play();
                }
            }
            
            // if still locked and not moving then handle the window indicator
            Invoke("WindowOn", beatOffset);
            Invoke("WindowOff", windowLength + beatOffset);
        }
    }

    public void WindowOn()
    {
        // we need to keep the window status up to date incase the player fails
        windowStatus = true;
        if (!pathBeat.moving)
        {
            rend.material = windowOnMat;
            hittable.canInteract = true;
        }

    }

    public void WindowOff()
    {
        windowStatus = false;

        // only if  not moving
        if (!pathBeat.moving)
        {
            rend.material = windowOffMat;
            hittable.canInteract = false;
        }
    }

    public Material GetWindowMaterial()
    {
        return windowStatus ? windowOnMat : windowOffMat;
    }

    public void UnlockObject()
    {
        unlocked = true;
        rend.material = unlockedMat;
    }

    public void ObjectFailed()
    {
        pathBeat.Reset();
        rend.material = GetWindowMaterial();
    }

    public void SetTrackingMat()
    {
        rend.material = trackingMat;
    }

    public void SetFailedMat()
    {
        rend.material = failMat;
    }

    public void ResetMaterial()
    {
        rend.material = GetWindowMaterial();
    }
}
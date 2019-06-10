using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hittable : MonoBehaviour
{
    public uint hitsToUnlock;

    public UnityEvent onHitOnce;
    public UnityEvent onMiss;
    public UnityEvent onUnlock;
    public UnityEvent onPinched;
    public UnityEvent onTracked;

    public GameObject haptics;
    public CountDown counter;
    public bool inContact;

    public bool canHit;
    public bool canInteract;
    public bool preventRepeated = true;

    private bool isUnlocked = false;

    // keep track of hit counts
    [HideInInspector]
    public uint hitCount;

    // use hit flag to keep track of hits
    private bool hitFlag;



    void Start()
    {
        haptics = GameObject.Find("/Haptics");
        counter = FindObjectOfType<CountDown>();
    }

    void Update()
    {
        if (counter.trig && counter.cSwitch == 1)
        {
            if (!isUnlocked)
            {
                Unlock();
                counter.trig = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (canHit)
        {
            if (preventRepeated)
            {
                canHit = false;
            }
            if (other.gameObject.tag == "DrumstickLeft") {

                haptics.GetComponent<Haptics>().PulseLeft();
            } else if (other.gameObject.tag == "DrumstickRight")
            {
                haptics.GetComponent<Haptics>().PulseRight();
            }
            onHitOnce.Invoke();
            hitCount++;
            
            if (hitCount == hitsToUnlock)
            {
                Unlock();
            }


            HitFlag = true;
        }
    }

    public void Unlock()
    {
        onUnlock.Invoke();
        isUnlocked = true;
    }

    void OnPinched()
    {
        if (canInteract)
        {
            onPinched.Invoke();
        }
    }

    void OnTracked(int hand) // 0 = right , 1 = left, andything else = none 
    {
        if (canInteract)
        {
            onTracked.Invoke();
            if (hand == 0)
            {
                haptics.GetComponent<Haptics>().PulseRight();
            } 
            if (hand == 1)
            {
                haptics.GetComponent<Haptics>().PulseLeft();
            }
        }
    }



    public void StopHit()
    {
        canHit = false;
    }

    public bool CanHit
    {
        get
        {
            return canHit;
        }
        set
        {
            canHit = value;
        }
    }

    // Resets the hit count if the hitflag is not set
    public void ResetIfHitFlagNotSet()
    {
        if (hitCount != 0)
        {
            onMiss.Invoke();
            //Play miss sound quietly?
        }
        if (!HitFlag)
        {
            HitCount = 0;
        }
    }

    public bool HitFlag
    {
        get
        {
            return hitFlag;
        }

        set
        {
            hitFlag = value;
        }
    }

    public uint HitCount
    {
        get
        {
            return hitCount;
        }

        set
        {
            hitCount = value;
        }
    }

    public float completion
    {
        get
        {
            //return (float) hitCount / hitsToUnlock;
            return 0;
        }
    }
}

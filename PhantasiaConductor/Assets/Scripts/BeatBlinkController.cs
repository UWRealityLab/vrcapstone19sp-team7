using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Blink))]
public class BeatBlinkController : MonoBehaviour
{

    public UnityEvent onHitUnlocked; // A 'hit' that plays  once a track is unlocked
     public UnityEvent onHitLocked;
    private Blink blink;
    public BeatInfo beatInfo;
    public bool unlocked = false;
    private int hitCount = -1;
    private Vector3 originalPos;

    private float prevTime;
    private float delay;
    private int beatIndex;

    void Awake()
    {
        blink = GetComponent<Blink>();
        originalPos = transform.localPosition;

        updateOffset();
    }

    public void Unlock()
    {
        unlocked = true;
    }

    public void NewLoop()
    {
        prevTime = Time.time;
        StartCoroutine(RunBeat(0));
    }
 
    IEnumerator RunBeat(int beatCount)
    {
        beatIndex = beatCount;
        bool isHit = beatInfo.beats[beatCount];
        bool isNextHit = beatInfo.beats[(beatCount + 1) % beatInfo.beats.Length];
        

        if (unlocked)
        {
            if (isHit)
            {
                updateOffset();
                onHitUnlocked.Invoke();
            }
        }
        else
        {
            if (isNextHit)
            {
                Invoke("BlinkOn", beatInfo.beatTime - (beatInfo.beatTime * beatInfo.hittableBefore));
            }
            if (isHit)
            {
                onHitLocked.Invoke();
                Invoke("BlinkOff", beatInfo.beatTime * beatInfo.hittableAfter);
            }
        }

        if (beatCount < beatInfo.beats.Length - 1)
        {
            prevTime = Time.time;
            yield return new WaitForSeconds(beatInfo.beatTime);
            StartCoroutine(RunBeat(beatCount + 1));
        }
    }
    
    void BlinkOn()
    {
        blink.BlinkOnOnce();
        updateOffset();
    }

    void BlinkOff()
    {
        blink.BlinkOffOnce();
        
    }

    private void updateOffset()
    {
        if (beatInfo.offsets.Length != 0)
        {
            hitCount++;
            if (hitCount == beatInfo.offsets.Length)
            {
                hitCount = 0;
            }
            transform.localPosition = originalPos + beatInfo.offsets[hitCount];
            // Debug.Log(beatInfo.offsets[hitCount]);
        }
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    public void PauseFantasia()
    {
        delay = beatInfo.beatTime - (Time.time - prevTime);
        if (delay < 0) { delay = 0; }
        StopAllCoroutines();
    }

    public void ResumeFantasia()
    {
        StartCoroutine(DelayedResume());
    }

    private IEnumerator DelayedResume()
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(RunBeat((beatIndex + 1) % beatInfo.beats.Length));
    }

}
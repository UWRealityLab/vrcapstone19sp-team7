using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatInfo : MonoBehaviour
{

    public int numBeats;
    // Percentage of timesPerBeat
    public float hittableBefore;
    public float hittableAfter;

    
    public bool[] beats = {true, false, true, false, false, false, true, false};
    public Vector3[] offsets;

    public float beatTime;

    // For melody and other ntoes
    // noteArray[i] indicates for how many beats note i will be playing.
    // Example:
    //  bitArray = {true, true, ...}
    //  noteArray = {2, 0, ...}
    //  Means the first note will play for 2 beats 
    public int[] notes = { 1, 0, 1, 0, 0, 0, 1, 0 };
    private void Awake()
    {
        numBeats = beats.Length;
        beatTime = MasterLoop.loopTime / beats.Length;

    }

}

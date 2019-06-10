using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTracker : MonoBehaviour
{
    public void RhythmSkip()
    {
        FindObjectOfType<CountDown>().cSwitch = 1;
    }
    public void BassSkip()
    {
        FindObjectOfType<CountDown>().cSwitch = 2;
    }
    public void MelodySkip()
    {
        FindObjectOfType<CountDown>().cSwitch = 3;
    }
    public void HarmonySkip()
    {
        FindObjectOfType<CountDown>().cSwitch = 4;
    }

}

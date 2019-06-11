using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTracker : MonoBehaviour
{
    public void RhythmSkip()
    {
        FindObjectOfType<CountDown>().cSwitch = 1;
        Debug.Log("rhythm skip");
    }
    public void BassSkip()
    {
        FindObjectOfType<CountDown>().cSwitch = 2;
        Debug.Log("bass skip");
    }
    public void MelodySkip()
    {
        FindObjectOfType<CountDown>().cSwitch = 3;
        Debug.Log("melody skip");
    }
    public void HarmonySkip()
    {
        FindObjectOfType<CountDown>().cSwitch = 4;
        Debug.Log("harmony skip");
    }

}

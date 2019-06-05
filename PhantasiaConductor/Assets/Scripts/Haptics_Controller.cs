using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haptics_Controller : MonoBehaviour
{

    public static bool hapticsOn = false;
    public GameObject haptics;

    // Start is called before the first frame update
    void Start()
    {
        haptics = GameObject.Find("/Haptics");
    }

    // Update is called once per frame
    void Update()
    {
        if (hapticsOn)
        {
            Invoke("goodVibes", 0);
        }
    }

    public void HapticsOn()
    {
        hapticsOn = true;
    }

    public void HapticsOff()
    {
        hapticsOn = false;
    }

    void goodVibes()
    {
        if (GetComponentInParent<BeatInfo>().isActiveAndEnabled)
        {
            haptics.GetComponent<Haptics>().sweepHandRight();
            haptics.GetComponent<Haptics>().sweepHandLeft();
        }
        
    }
}

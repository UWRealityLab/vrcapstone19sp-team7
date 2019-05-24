using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Haptics : MonoBehaviour
{
    public SteamVR_Action_Vibration hapticAction;

    public void PulseLeft()
    {
        hapticAction.Execute(0, 1, 150, 75, SteamVR_Input_Sources.LeftHand);
        
    }

    public void PulseRight()
    {
        hapticAction.Execute(0, 1, 150, 75, SteamVR_Input_Sources.RightHand);

    }
}

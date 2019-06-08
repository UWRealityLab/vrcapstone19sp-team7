using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Haptics : MonoBehaviour
{
    #if UNITY_EDITOR
    public SteamVR_Action_Vibration hapticAction;
    void Start() {
        Debug.Log("Hello Windows Haptics");
    }

    public void PulseLeft()
    {
        hapticAction.Execute(0, 1, 150, 75, SteamVR_Input_Sources.LeftHand);
        
    }

    public void PulseRight()
    {
        hapticAction.Execute(0, 1, 150, 75, SteamVR_Input_Sources.RightHand);

    }

    public void sweepHandRight() {
        //hapticAction.Execute(0, 1, 75, 75, SteamVR_Input_Sources.RightHand);
        //hapticAction.Execute(0, 0.5f, 100, 75, SteamVR_Input_Sources.RightHand);
        hapticAction.Execute(0, 1, 125, 60, SteamVR_Input_Sources.RightHand);
        hapticAction.Execute(0, 1, 150, 75, SteamVR_Input_Sources.RightHand);


    }
    public void sweepHandLeft() {
        //hapticAction.Execute(0, 1, 75, 20, SteamVR_Input_Sources.LeftHand);
        //hapticAction.Execute(0, 0.5f, 100, 40, SteamVR_Input_Sources.LeftHand);
        hapticAction.Execute(0, 1, 125, 60, SteamVR_Input_Sources.LeftHand);
        hapticAction.Execute(0, 1, 150, 75, SteamVR_Input_Sources.LeftHand);

    }
    #elif UNITY_ANDROID
    void Start() {
        Debug.Log("Hello Android");
    }

    public void PulseLeft() {

    }

    public void PulseRight() {

    }

    public void sweepHandRight() {

    }

    public void sweepHandLeft() {
        
    }
    #else
    void Start() {
        Debug.Log("Unrecognized platform, check Haptics.cs");
    }
    #endif
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSource : MonoBehaviour
{
    public GameObject steamHandRight;

    public GameObject steamHandLeft;

    public GameObject oculusHandRight;

    public GameObject oculusHandLeft;

    private static GameObject oHandRight;

    private static GameObject oHandLeft;

    private static GameObject sHandRight;

    private static GameObject sHandLeft;


    private static bool useOculus = false;


    public static GameObject GetRightHand() {
        if (useOculus) {
            return oHandRight;
        } else {
            return sHandRight;
        }
    }

    public static GameObject GetLeftHand() {
        if (useOculus) {
            return oHandLeft;
        } else {
            return sHandLeft;
        }
    }
}

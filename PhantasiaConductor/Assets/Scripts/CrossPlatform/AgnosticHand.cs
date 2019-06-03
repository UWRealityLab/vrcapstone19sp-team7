using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgnosticHand : MonoBehaviour
{
    public GameObject steamHand;

    public GameObject oculusHand;


    public bool useOculus = false;

    void Awake()
    {
        GameObject src = useOculus ? oculusHand : steamHand;

        transform.parent = src.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}

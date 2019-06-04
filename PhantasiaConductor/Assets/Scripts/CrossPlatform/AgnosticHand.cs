﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgnosticHand : MonoBehaviour
{
    public GameObject steamHand;

    public GameObject oculusHand;


    public bool useOculus = false;

    public bool isRight;

    private Baton baton;

    static Baton rightBaton;
    static Baton leftBaton;


    void Awake()
    {
        GameObject src = useOculus ? oculusHand : steamHand;

        transform.parent = src.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        baton = transform.Find("baton").gameObject.GetComponent<Baton>();
        if (baton == null)
        {
            Debug.Log("Could not find baton");
        }
        else
        {
            if (isRight)
            {
                rightBaton = baton;
            }
            else
            {
                leftBaton = baton;
          }
        }
    }

    public Baton GetBaton()
    {
        return baton;
    }

    public static Baton GetRightBaton() {
        return rightBaton;
    }

    public static Baton GetLeftBaton() {
        return leftBaton;
    }


}
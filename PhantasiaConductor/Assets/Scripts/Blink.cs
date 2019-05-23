using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Blink : MonoBehaviour
{
    public UnityEvent onBlinkOn;
    public UnityEvent onBlinkOff;
    
    // when blink change change state on/off
    public UnityEvent onBlinkOnToOff;
    public UnityEvent onBlinkOffToOn;

    public bool isPiano = false;
    public float transparency;
    
    // true is on, false is off
    private bool blinkState = true;
    private bool unlocked = false;

    private void Start()
    {
        if (isPiano) { 
            GetComponent<Collider>().enabled = true;
        }
        Color color = this.GetComponent<MeshRenderer>().material.color;
        color.a = .2f;
        this.GetComponent<MeshRenderer>().material.color = color;
    }


    public void BlinkOnOnce()
    {
        if (!unlocked)
        {
            if (!blinkState)
            {
                onBlinkOffToOn.Invoke();
            }
            blinkState = true;


            Color color = this.GetComponent<MeshRenderer>().material.color;
            color.a = 1;
            this.GetComponent<MeshRenderer>().material.color = color;
            if (isPiano)
            {
                GetComponent<Collider>().enabled = true;
            }
        
            onBlinkOn.Invoke();
        }
    }

    public void BlinkOffOnce()
    {
        if (blinkState)
        {
            onBlinkOnToOff.Invoke();
        }
        blinkState = false;

        Color color = this.GetComponent<MeshRenderer>().material.color;
        color.a = .25f;
        this.GetComponent<MeshRenderer>().material.color = color;
        if (isPiano)
        {
            GetComponent<Collider>().enabled = false;
        }

        onBlinkOff.Invoke();
    }



}

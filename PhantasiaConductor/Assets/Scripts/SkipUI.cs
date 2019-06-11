using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipUI : MonoBehaviour
{
    public float holdTime = 3f;
    public CountDown counter;
    private float holdTimer = 0f;
    public AgnosticHand leftHand;
    public AgnosticHand righttHand;

    private void Awake()
    {
        counter = FindObjectOfType<CountDown>();
    }
    // Update is called once per frame
    void Update()
    {

        if (leftHand.IsTriggerDown() || righttHand.IsTriggerDown())
        {
            holdTimer += Time.deltaTime;

            if (holdTimer > holdTime)
            {
                Debug.Log(holdTimer);
                counter.enabled = true;
                holdTimer = 0f;
            }
        }
        if (!(leftHand.IsTriggerDown() || righttHand.IsTriggerDown()))
        {
            holdTimer = 0f;
        }
    }
}

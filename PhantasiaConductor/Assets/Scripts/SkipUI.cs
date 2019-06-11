using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipUI : MonoBehaviour
{
    public float holdTime = 3f;
    public CountDown counter;
    private float holdTimer = 0f;

    private void Awake()
    {
        counter = FindObjectOfType<CountDown>();
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            holdTimer += Time.deltaTime;

            if (holdTimer > holdTime)
            {
                Debug.Log(holdTimer);
                counter.enabled = true;
                holdTimer = 0f;
            }
        }
        if (!Input.GetKey(KeyCode.Space))
        {
            holdTimer = 0f;
        }
    }
}
